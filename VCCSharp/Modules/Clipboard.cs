using System;
using System.Diagnostics;
using System.Text;
using System.Windows;
using VCCSharp.IoC;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        bool ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteClipboard();
        void PasteBasic();
        void PasteBasicWithNew();
        int CurrentKeyMap { get; set; }
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
        private IGraphics _graphics => _modules.Graphics;

        public bool CodePaste;
        public bool PasteWithNew;
        public int CurrentKeyMap { get; set; }

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
            int graphicsMode = _graphics.GraphicsMode;

            if (graphicsMode != 0)
            {
                const string warning = "Warning: You are not in text mode. Continue Pasting?";

                var result = MessageBox.Show(warning, "Clipboard", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.No)
                {
                    return;
                }
            }

            _modules.Keyboard.SetPaste(true);

            //This sets the keyboard to Natural,
            //but we need to read it first so we can set it back
            CurrentKeyMap = _modules.Config.GetCurrentKeyboardLayout();

            _modules.Keyboard.KeyboardBuildRuntimeTable(1); //Natural (OS9)

            var text = System.Windows.Clipboard.GetText();

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("No text found in clipboard.", "Clipboard");

                return;
            }

            PasteText(text);

            //--This process is asynchronous.  The text will finish pasting long after here.
            //_modules.Keyboard.KeyboardBuildRuntimeTable((byte)clipboardState->CurrentKeyMap);
        }

        public void PasteText(string text)
        {
            if (PasteWithNew)
            {
                text = $"NEW\n{text}";
            }

            text = ParseText(text, CodePaste);

            string codes = ConvertScanCodes(text);

            SetClipboardText(codes);
        }

        private static string ParseText(string text, bool codePaste)
        {
            const char result = '\n';

            string line = "";
            string parsed = "";

            foreach (var c in text)
            {
                if (c != result)
                {
                    line += c;
                }
                else if (line.Length <= 249 || !codePaste) //...the character is a <CR>
                {
                    // Just a regular line.
                    parsed += line + result;
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

            return parsed;
        }

        private string ConvertScanCodes(string text)
        {
            var result = new StringBuilder();

            foreach (var letter in text)
            {
                char sc = GetScanCode(letter);
                // ReSharper disable once InconsistentNaming
                bool CSHIFT = GetCSHIFT(letter);
                // ReSharper disable once InconsistentNaming
                bool LCNTRL = GetLCNTRL(letter);

                if (CSHIFT) { result.Append((char)0x36); }
                if (LCNTRL) { result.Append((char)0x1D); }

                result.Append(sc);
            }

            return result.ToString();
        }

        public char GetScanCode(char letter)
        {
            return letter switch
            {
                '@' => (char)0x03,
                'A' => (char)0x1E,
                'B' => (char)0x30,
                'C' => (char)0x2E,
                'D' => (char)0x20,
                'E' => (char)0x12,
                'F' => (char)0x21,
                'G' => (char)0x22,
                'H' => (char)0x23,
                'I' => (char)0x17,
                'J' => (char)0x24,
                'K' => (char)0x25,
                'L' => (char)0x26,
                'M' => (char)0x32,
                'N' => (char)0x31,
                'O' => (char)0x18,
                'P' => (char)0x19,
                'Q' => (char)0x10,
                'R' => (char)0x13,
                'S' => (char)0x1F,
                'T' => (char)0x14,
                'U' => (char)0x16,
                'V' => (char)0x2F,
                'W' => (char)0x11,
                'X' => (char)0x2D,
                'Y' => (char)0x15,
                'Z' => (char)0x2C,
                ' ' => (char)0x39,
                'a' => (char)0x1E,
                'b' => (char)0x30,
                'c' => (char)0x2E,
                'd' => (char)0x20,
                'e' => (char)0x12,
                'f' => (char)0x21,
                'g' => (char)0x22,
                'h' => (char)0x23,
                'i' => (char)0x17,
                'j' => (char)0x24,
                'k' => (char)0x25,
                'l' => (char)0x26,
                'm' => (char)0x32,
                'n' => (char)0x31,
                'o' => (char)0x18,
                'p' => (char)0x19,
                'q' => (char)0x10,
                'r' => (char)0x13,
                's' => (char)0x1F,
                't' => (char)0x14,
                'u' => (char)0x16,
                'v' => (char)0x2F,
                'w' => (char)0x11,
                'x' => (char)0x2D,
                'y' => (char)0x15,
                'z' => (char)0x2C,
                '0' => (char)0x0B,
                '1' => (char)0x02,
                '2' => (char)0x03,
                '3' => (char)0x04,
                '4' => (char)0x05,
                '5' => (char)0x06,
                '6' => (char)0x07,
                '7' => (char)0x08,
                '8' => (char)0x09,
                '9' => (char)0x0A,
                '!' => (char)0x02,
                '#' => (char)0x04,
                '$' => (char)0x05,
                '%' => (char)0x06,
                '^' => (char)0x07,
                '&' => (char)0x08,
                '*' => (char)0x09,
                '(' => (char)0x0A,
                ')' => (char)0x0B,
                '-' => (char)0x0C,
                '=' => (char)0x0D,
                ';' => (char)0x27,
                '\'' => (char)0x28,
                '/' => (char)0x35,
                '.' => (char)0x34,
                ',' => (char)0x33,
                '\n' => (char)0x1C,
                '+' => (char)0x0D,
                ':' => (char)0x27,
                '\"' => (char)0x28,
                '?' => (char)0x35,
                '<' => (char)0x33,
                '>' => (char)0x34,
                '[' => (char)0x1A,
                ']' => (char)0x1B,
                '{' => (char)0x1A,
                '}' => (char)0x1B,
                '\\' => (char)0x2B,
                '|' => (char)0x2B,
                '`' => (char)0x29,
                '~' => (char)0x29,
                '_' => (char)0x0C,
                '\t' => (char)0x39 // TAB
                ,
                _ => (char)0xFF
            };
        }

        // ReSharper disable once InconsistentNaming
        public bool GetCSHIFT(char letter)
        {
            return letter switch
            {
                '@' => true,
                'A' => true,
                'B' => true,
                'C' => true,
                'D' => true,
                'E' => true,
                'F' => true,
                'G' => true,
                'H' => true,
                'I' => true,
                'J' => true,
                'K' => true,
                'L' => true,
                'M' => true,
                'N' => true,
                'O' => true,
                'P' => true,
                'Q' => true,
                'R' => true,
                'S' => true,
                'T' => true,
                'U' => true,
                'V' => true,
                'W' => true,
                'X' => true,
                'Y' => true,
                'Z' => true,
                ' ' => false,
                'a' => false,
                'b' => false,
                'c' => false,
                'd' => false,
                'e' => false,
                'f' => false,
                'g' => false,
                'h' => false,
                'i' => false,
                'j' => false,
                'k' => false,
                'l' => false,
                'm' => false,
                'n' => false,
                'o' => false,
                'p' => false,
                'q' => false,
                'r' => false,
                's' => false,
                't' => false,
                'u' => false,
                'v' => false,
                'w' => false,
                'x' => false,
                'y' => false,
                'z' => false,
                '0' => false,
                '1' => false,
                '2' => false,
                '3' => false,
                '4' => false,
                '5' => false,
                '6' => false,
                '7' => false,
                '8' => false,
                '9' => false,
                '!' => true,
                '#' => true,
                '$' => true,
                '%' => true,
                '^' => true,
                '&' => true,
                '*' => true,
                '(' => true,
                ')' => true,
                '-' => false,
                '=' => false,
                ';' => false,
                '\'' => false,
                '/' => false,
                '.' => false,
                ',' => false,
                '\n' => false,
                '+' => true,
                ':' => true,
                '\"' => true,
                '?' => true,
                '<' => true,
                '>' => true,
                '[' => false,
                ']' => false,
                '{' => true,
                '}' => true,
                '\\' => false,
                '|' => true,
                '`' => false,
                '~' => true,
                '_' => true,
                '\t' => false // TAB
                ,
                _ => false
            };
        }

        // ReSharper disable once InconsistentNaming
        public bool GetLCNTRL(char letter)
        {
            return letter switch
            {
                '@' => false,
                'A' => false,
                'B' => false,
                'C' => false,
                'D' => false,
                'E' => false,
                'F' => false,
                'G' => false,
                'H' => false,
                'I' => false,
                'J' => false,
                'K' => false,
                'L' => false,
                'M' => false,
                'N' => false,
                'O' => false,
                'P' => false,
                'Q' => false,
                'R' => false,
                'S' => false,
                'T' => false,
                'U' => false,
                'V' => false,
                'W' => false,
                'X' => false,
                'Y' => false,
                'Z' => false,
                ' ' => false,
                'a' => false,
                'b' => false,
                'c' => false,
                'd' => false,
                'e' => false,
                'f' => false,
                'g' => false,
                'h' => false,
                'i' => false,
                'j' => false,
                'k' => false,
                'l' => false,
                'm' => false,
                'n' => false,
                'o' => false,
                'p' => false,
                'q' => false,
                'r' => false,
                's' => false,
                't' => false,
                'u' => false,
                'v' => false,
                'w' => false,
                'x' => false,
                'y' => false,
                'z' => false,
                '0' => false,
                '1' => false,
                '2' => false,
                '3' => false,
                '4' => false,
                '5' => false,
                '6' => false,
                '7' => false,
                '8' => false,
                '9' => false,
                '!' => false,
                '#' => false,
                '$' => false,
                '%' => false,
                '^' => false,
                '&' => false,
                '*' => false,
                '(' => false,
                ')' => false,
                '-' => false,
                '=' => false,
                ';' => false,
                '\'' => false,
                '/' => false,
                '.' => false,
                ',' => false,
                '\n' => false,
                '+' => false,
                ':' => false,
                '\"' => false,
                '?' => false,
                '<' => false,
                '>' => false,
                '[' => true,
                ']' => true,
                '{' => false,
                '}' => false,
                '\\' => true,
                '|' => false,
                '`' => false,
                '~' => false,
                '_' => false,
                '\t' => false // TAB
                ,
                _ => false
            };
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
            byte bytesPerRow = _graphics.BytesPerRow;
            byte graphicsMode = _graphics.GraphicsMode;
            uint startOfVidRam = _graphics.StartOfVidRam;

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