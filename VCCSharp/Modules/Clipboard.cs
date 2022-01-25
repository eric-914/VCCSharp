using System;
using System.Diagnostics;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        void PasteText(string text);
        bool ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteClipboard();
        void PasteBasic();
        void PasteBasicWithNew();
        void SetClipboardText(string text);

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

        private string _clipboardText;

        private readonly IModules _modules;
        private readonly IKeyboardScanCodes _keyboardScanCodes;
        private IGraphics Graphics => _modules.Graphics;

        public bool CodePaste;
        public bool PasteWithNew;

        public bool Abort { get; set; }

        public Clipboard(IModules modules, IKeyboardScanCodes keyboardScanCodes)
        {
            _modules = modules;
            _keyboardScanCodes = keyboardScanCodes;
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

            //This sets the keyboard to Natural (keyboard stores previous to retrieve later)
            _modules.Keyboard.SwapKeyboardLayout(KeyboardLayouts.kKBLayoutNatural); //Natural (OS9)

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

            string codes = _keyboardScanCodes.ConvertScanCodes(text);

            SetClipboardText(codes);
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

            if (!string.IsNullOrEmpty(parsed)) return parsed;
            return line;
        }


        public void SetClipboardText(string text)
        {
            _clipboardText = text;
        }

        public bool ClipboardEmpty()
        {
            return string.IsNullOrEmpty(_clipboardText);
        }

        public char PeekClipboard()
        {
            return _clipboardText[0]; // get the next key in the string
        }

        public void PopClipboard()
        {
            _clipboardText = _clipboardText[1..];
        }

        public void CopyText()
        {
            byte bytesPerRow = Graphics.BytesPerRow;
            byte graphicsMode = Graphics.GraphicsMode;
            uint startOfVidRam = Graphics.StartOfVidRam;

            if (graphicsMode != 0)
            {
                const string error = "ERROR: Graphics screen can not be copied.\nCopy can ONLY use a hardware text screen.";
                MessageBox.Show(error, "Clipboard");

                return;
            }

            Debug.WriteLine($"StartOfVidRam is: {startOfVidRam}\nGraphicsMode is: {graphicsMode}");

            int lines = bytesPerRow == 32 ? 15 : 23;
            string clipOut = "";

            // Read the lo-res text screen...
            if (bytesPerRow == 32)
            {
                for (int y = 0; y <= lines; y++)
                {
                    int lastChar = 0;
                    string tmpLine = "";

                    for (int idx = 0; idx < bytesPerRow; idx++)
                    {
                        ushort address = (ushort)(0x0400 + y * bytesPerRow + idx);
                        byte tmp = _modules.TC1014.MemRead8(address);

                        if (tmp != 32 && tmp != 64 && tmp != 96)
                        {
                            lastChar = idx + 1;
                        }

                        if (tmp < 128) //--Ignore anything beyond ASCII 
                        {
                            tmpLine += _pcChars32[tmp];
                        }
                    }

                    tmpLine = tmpLine[..lastChar];

                    if (lastChar != 0)
                    {
                        clipOut += tmpLine + "\n";
                    }
                }

                if (string.IsNullOrEmpty(clipOut))
                {
                    MessageBox.Show("No text found on screen.", "Clipboard");
                }

            }
            //TODO: This is untested since C# conversion
            else if (bytesPerRow == 40 || bytesPerRow == 80)
            {
                int offset = 32;

                for (int y = 0; y <= lines; y++)
                {
                    int lastChar = 0;
                    string tmpLine = "";

                    for (int idx = 0; idx < bytesPerRow * 2; idx += 2)
                    {
                        int address = (int)(startOfVidRam + y * (bytesPerRow * 2) + idx);
                        int tmp = _modules.TC1014.GetMem(address);

                        if (tmp == 32 || tmp == 64 || tmp == 96)
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
                        clipOut += tmpLine; clipOut += "\n";
                    }
                }
            }

            System.Windows.Clipboard.SetText(clipOut);
        }
    }
}
