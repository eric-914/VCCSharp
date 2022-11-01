using System.Diagnostics;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Keyboard.Mappings;

namespace VCCSharp.Modules;

public interface IClipboard : IModule
{
    bool ClipboardEmpty();
    IKey PeekClipboard();
    void PopClipboard();
    void CopyText();
    void PasteClipboard();
    void PasteBasic();
    void PasteBasicWithNew();
    void ClearClipboard();

    bool Abort { get; set; }
}

public class Clipboard : IClipboard
{
    #region pcchars

    private readonly char[] _pcChars32 =
    {
        '@','a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z','[','\\',']',' ',' ',
        ' ','!','\"','#','$','%','&','\'','(',')','*','+',',','-','.','/','0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?',
        '@','A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z','[','\\',']',' ',' ',
        ' ','!','\"','#','$','%','&','\'','(',')','*','+',',','-','.','/','0','1','2','3','4','5','6','7','8','9',':',';','<','=','>','?'
    };

    private readonly char[] _pcChars40 =
    {
        ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4',
        '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
        '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
        'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', ' ', ' ',
        '^', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
        'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', '_',
        'Ç', 'ü', 'é', 'â', 'ä', 'à', 'å', 'ç',
        'ê', 'ë', 'è', 'ï', 'î', 'ß', 'Ä', 'Â',
        'Ó', 'æ', 'Æ', 'ô', 'ö', 'ø', 'û', 'ù',
        'Ø', 'Ö', 'Ü', '§', '£', '±', 'º', '¥',
        ' ', ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3',
        '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>',
        '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
        'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', ' ',
        ' ', '^', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
        't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', '_'
    };

    #endregion

    private List<IKey> _clipboardText = new();

    private readonly IModules _modules;
    private IGraphics Graphics => _modules.Graphics;

    public bool CodePaste;
    public bool PasteWithNew;

    public bool Abort { get; set; }

    public Clipboard(IModules modules)
    {
        _modules = modules;
    }

    public void PasteBasic()
    {
        CodePaste = true;

        PasteClipboard();

        CodePaste = false;
    }

    public void PasteBasicWithNew()
    {
        const string warning =
            "Warning: This operation will erase the Coco's BASIC memory before pasting. Continue?";

        var result = MessageBox.Show(warning, "Clipboard", MessageBoxButton.YesNo);

        if (result == MessageBoxResult.No)
        {
            return;
        }

        CodePaste = true;
        PasteWithNew = true;

        PasteClipboard();

        CodePaste = false;
        PasteWithNew = false;
    }

    public void PasteClipboard()
    {
        var text = System.Windows.Clipboard.GetText();

        if (string.IsNullOrEmpty(text))
        {
            MessageBox.Show("No text found in clipboard.", "Clipboard");

            return;
        }

        PasteClipboard(text);
    }

    public void PasteClipboard(string text)
    {
        if (!Graphics.InTextMode())
        {
            return;
        }

        //This sets the keyboard to PC (keyboard stores previous to retrieve later)
        _modules.Keyboard.SwapKeyboardLayout(KeyboardLayouts.PC);

        //--This process is asynchronous.  The text will finish pasting long after here.
        PasteText(text);
    }

    public void PasteText(string text)
    {
        Abort = false;

        if (PasteWithNew)
        {
            text = $"NEW\n{text}";
        }

        text = ParseText(text, CodePaste);

        var data = text.Select(x => KeyDefinitions.Instance.ByCharacter(x));

        _clipboardText = data.ToList();
    }

    private static string ParseText(string text, bool codePaste)
    {
        const char cr = '\n';

        string line = "";
        string parsed = "";

        foreach (var c in text)
        {
            if (c != cr)
            {
                line += c;
            }
            else if (line.Length <= 249 || !codePaste) //...the character is a <CR>
            {
                // Just a regular line.
                parsed += line + cr;
                line = "";
            }
            //TODO: Look at this later.  Some of the 256 is used by line number.
            else if (line.Length >= 257)    // Line is too long to handle. Truncate.
            {
                string warning = $"Warning! Line {line} is too long for BASIC and will be truncated.";

                MessageBox.Show(warning, "Clipboard");

                line = line.Substring(0, 249);
            }
            //TODO: Look at this later.  I think it's using some trick with EDIT to squeeze more in.
            else
            {
                int blank = line.IndexOf(" ", StringComparison.Ordinal);
                string main = line[..249];
                string extra = line[249..^249];  //Translation: from '249' to 'all but 249'
                string spaces = "";

                for (int p = 1; p < 249; p++)
                {
                    spaces += " ";
                }

                string substring = line[..blank];

                line = $"{main}\n\nEDIT {substring}\n{spaces}I{extra}\n";
                parsed += line;
                line = "";
            }
        }

        //TODO: Might be a problem here if multiline, but last line not CR-terminated
        if (!string.IsNullOrEmpty(parsed)) return parsed;
        return line;
    }


    public void ClearClipboard()
    {
        _clipboardText = new List<IKey>();
    }

    public bool ClipboardEmpty()
    {
        return !_clipboardText.Any();
    }

    public IKey PeekClipboard()
    {
        return _clipboardText.First(); // get the next key in the string
    }

    public void PopClipboard()
    {
        _clipboardText = _clipboardText.Skip(1).ToList();
    }

    public void CopyText()
    {
        var text = GetText();

        if (text != null)
        {
            System.Windows.Clipboard.SetText(text);
        }
    }

    private string? GetText()
    {
        if (Graphics.GraphicsMode != 0)
        {
            const string error = "ERROR: Graphics screen can not be copied.\nCopy can ONLY use a hardware text screen.";
            MessageBox.Show(error, "Clipboard");

            return null;
        }

        Debug.WriteLine($"GraphicsMode is: {Graphics.GraphicsMode}");

        switch (Graphics.BytesPerRow)
        {
            // Read the lo-res text screen...
            case 32:
                return ParseCoCoScreen();
            case 40:
            case 80:
                return ParseCoCo3Screen();
        }

        return null;
    }

    //TODO: This is untested since C# conversion
    private string ParseCoCo3Screen()
    {
        const int lines = 23;
        byte bytesPerRow = Graphics.BytesPerRow;
        int offset = 32;
        string clipOut = "";

        uint startOfVidRam = Graphics.StartOfVidRam;

        Debug.WriteLine($"StartOfVidRam is: {startOfVidRam}");

        for (int y = 0; y <= lines; y++)
        {
            int lastChar = 0;
            string tmpLine = "";

            for (int idx = 0; idx < bytesPerRow * 2; idx += 2)
            {
                ushort address = (ushort)(startOfVidRam + y * (bytesPerRow * 2) + idx);
                int tmp = _modules.TCC1014.MemRead8(address);

                if (tmp is 32 or 64 or 96)
                {
                    tmp = offset;
                }
                else
                {
                    lastChar = idx / 2 + 1;
                }

                tmpLine += _pcChars40[tmp - offset];
            }

            tmpLine = tmpLine[..lastChar];

            if (lastChar != 0)
            {
                clipOut += tmpLine;
                clipOut += "\n";
            }
        }

        return clipOut;
    }

    private string ParseCoCoScreen()
    {
        const uint startAddress = 0x0400;
        const int rows = 16;
        byte cols = Graphics.BytesPerRow;

        char Ascii(byte value) => value < 127 ? _pcChars32[value] : ' ';
        string AsString(IEnumerable<char> line) => string.Concat(line).Trim();

        var lines =
            ReadMemoryBlock(startAddress, cols * rows)
                .Chunk(cols)
                .Select(line => line.Select(Ascii))
                .Select(AsString);

        var text = string.Join(Environment.NewLine, lines).Trim();

        if (string.IsNullOrEmpty(text))
        {
            MessageBox.Show("No text found on screen.", "Clipboard");
        }

        return $"{text}{Environment.NewLine}";
    }

    public IEnumerable<byte> ReadMemoryBlock(uint startAddress, int range)
    {
        return from index in Enumerable.Range(0, range - 1)
               let address = (ushort)(startAddress + index)
               select _modules.TCC1014.MemRead8(address);
    }

    public void ModuleReset()
    {
        CodePaste = false;
        PasteWithNew = false;
        Abort = false;
    }
}