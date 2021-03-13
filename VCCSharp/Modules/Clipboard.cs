using System.Diagnostics;
using System.Text;
using System.Windows;
using Microsoft.VisualBasic;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        unsafe ClipboardState* GetClipboardState();
        bool ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteClipboard();
        void PasteBASIC();
        void PasteBASICWithNew();
    }

    public class Clipboard : IClipboard
    {
        #region pcchars

        private char[] pcchars32 =
        {
            '@', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z', '[', '\\', ']', ' ', ' ',
            ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
            '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', ' ', ' ',
            ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
            '@', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z', '[', '\\', ']', ' ', ' ',
            ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?'
        };

        private char[] pcchars40 =
        {
            ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9', ':', ';', '<', '=', '>', '?',
            '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T',
            'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', ' ', ' ',
            '^', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
            'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', '_',
            (char) 'Ç', (char) 'ü', (char) 'é', (char) 'â', (char) 'ä', (char) 'à', (char) 'å', (char) 'ç',
            (char) 'ê', (char) 'ë', (char) 'è', (char) 'ï', (char) 'î', (char) 'ß', (char) 'Ä', (char) 'Â',
            (char) 'Ó', (char) 'æ', (char) 'Æ', (char) 'ô', (char) 'ö', (char) 'ø', (char) 'û', (char) 'ù',
            (char) 'Ø', (char) 'Ö', (char) 'Ü', (char) '§', (char) '£', (char) '±', (char) 'º', (char) '¥',
            ' ', ' ', '!', '\"', '#', '$', '%', '&', '\'', '(', ')', '*', '+', ',', '-', '.', '/', '0', '1', '2', '3',
            '4', '5', '6', '7', '8', '9', ':', ';', '<', '=', '>',
            '?', '@', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S',
            'T', 'U', 'V', 'W', 'X', 'Y', 'Z', '[', '\\', ']', ' ',
            ' ', '^', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's',
            't', 'u', 'v', 'w', 'x', 'y', 'z', '{', '|', '}', '~', '_'
        };

        #endregion

        private readonly IModules _modules;

        public Clipboard(IModules modules)
        {
            _modules = modules;
        }

        public unsafe ClipboardState* GetClipboardState()
        {
            return Library.Clipboard.GetClipboardState();
        }

        public bool ClipboardEmpty()
        {
            return Library.Clipboard.ClipboardEmpty() == Define.TRUE;
        }

        public char PeekClipboard()
        {
            return Library.Clipboard.PeekClipboard();
        }

        public void PopClipboard()
        {
            Library.Clipboard.PopClipboard();
        }

        public void CopyText()
        {
            Library.Clipboard.CopyText();
        }

        public void PasteBASIC()
        {
            unsafe
            {
                ClipboardState* instance = GetClipboardState();

                instance->CodePaste = Define.TRUE;

                PasteClipboard();

                instance->CodePaste = Define.FALSE;
            }
        }

        public void PasteBASICWithNew()
        {
            const string warning =
                "Warning: This operation will erase the Coco's BASIC memory before pasting. Continue?";

            var result = MessageBox.Show(warning, "Clipboard", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.No)
            {
                return;
            }

            unsafe
            {
                ClipboardState* instance = GetClipboardState();

                instance->CodePaste = Define.TRUE;
                instance->PasteWithNew = Define.TRUE;

                PasteClipboard();

                instance->CodePaste = Define.FALSE;
                instance->PasteWithNew = Define.FALSE;
            }
        }

        public void PasteClipboard()
        {
            unsafe
            {
                ClipboardState* clipboardState = GetClipboardState();

                GraphicsState* graphicsState = _modules.Graphics.GetGraphicsState();

                int graphicsMode = graphicsState->GraphicsMode;

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
                clipboardState->CurrentKeyMap = _modules.Config.GetCurrentKeyboardLayout();

                _modules.Keyboard.vccKeyboardBuildRuntimeTable(1); //Natural (OS9)

                var text = System.Windows.Clipboard.GetText();

                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("No text found in clipboard.", "Clipboard");

                    return;
                }

                PasteText(text);

                //--This process is asynchronous.  The text will finish pasting long after here.
                //_modules.Keyboard.vccKeyboardBuildRuntimeTable((byte)clipboardState->CurrentKeyMap);
            }
        }

        public void PasteText(string text)
        {
            unsafe
            {
                ClipboardState* clipboardState = GetClipboardState();

                if (clipboardState->PasteWithNew == Define.TRUE)
                {
                    text = $"NEW\n{text}";
                }

                text = ParseText(text, clipboardState->CodePaste == Define.TRUE);

                string codes = ConvertScanCodes(text);

                SetClipboardText(codes);
            }
        }

        private static string ParseText(string text, bool codePaste)
        {
            const char Return = '\n';

            string line = "";
            string parsed = "";

            foreach (var c in text)
            {
                if (c != Return)
                {
                    line += c;
                }
                else if (line.Length <= 249 || !codePaste) //...the character is a <CR>
                {
                    // Just a regular line.
                    parsed += line + Return;
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
                    int blank = line.IndexOf(" ");
                    string main = line.Substring(0, 249);
                    string extra = line.Substring(249, line.Length - 249);
                    string spaces = "";

                    for (int p = 1; p < 249; p++)
                    {
                        spaces += " ";
                    }

                    string linestr = line.Substring(0, blank);

                    line = main + "\n\nEDIT " + linestr + "\n" + spaces + "I" + extra + "\n";
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
                bool CSHIFT = GetCSHIFT(letter);
                bool LCNTRL = GetLCNTRL(letter);

                if (CSHIFT) { result.Append((char)0x36); }
                if (LCNTRL) { result.Append((char)0x1D); }

                result.Append(sc);
            }

            return result.ToString();
        }

        public char GetScanCode(char letter)
        {
            switch (letter)
            {
                case '@': return (char)0x03;
                case 'A': return (char)0x1E;
                case 'B': return (char)0x30;
                case 'C': return (char)0x2E;
                case 'D': return (char)0x20;
                case 'E': return (char)0x12;
                case 'F': return (char)0x21;
                case 'G': return (char)0x22;
                case 'H': return (char)0x23;
                case 'I': return (char)0x17;
                case 'J': return (char)0x24;
                case 'K': return (char)0x25;
                case 'L': return (char)0x26;
                case 'M': return (char)0x32;
                case 'N': return (char)0x31;
                case 'O': return (char)0x18;
                case 'P': return (char)0x19;
                case 'Q': return (char)0x10;
                case 'R': return (char)0x13;
                case 'S': return (char)0x1F;
                case 'T': return (char)0x14;
                case 'U': return (char)0x16;
                case 'V': return (char)0x2F;
                case 'W': return (char)0x11;
                case 'X': return (char)0x2D;
                case 'Y': return (char)0x15;
                case 'Z': return (char)0x2C;
                case ' ': return (char)0x39;
                case 'a': return (char)0x1E;
                case 'b': return (char)0x30;
                case 'c': return (char)0x2E;
                case 'd': return (char)0x20;
                case 'e': return (char)0x12;
                case 'f': return (char)0x21;
                case 'g': return (char)0x22;
                case 'h': return (char)0x23;
                case 'i': return (char)0x17;
                case 'j': return (char)0x24;
                case 'k': return (char)0x25;
                case 'l': return (char)0x26;
                case 'm': return (char)0x32;
                case 'n': return (char)0x31;
                case 'o': return (char)0x18;
                case 'p': return (char)0x19;
                case 'q': return (char)0x10;
                case 'r': return (char)0x13;
                case 's': return (char)0x1F;
                case 't': return (char)0x14;
                case 'u': return (char)0x16;
                case 'v': return (char)0x2F;
                case 'w': return (char)0x11;
                case 'x': return (char)0x2D;
                case 'y': return (char)0x15;
                case 'z': return (char)0x2C;
                case '0': return (char)0x0B;
                case '1': return (char)0x02;
                case '2': return (char)0x03;
                case '3': return (char)0x04;
                case '4': return (char)0x05;
                case '5': return (char)0x06;
                case '6': return (char)0x07;
                case '7': return (char)0x08;
                case '8': return (char)0x09;
                case '9': return (char)0x0A;
                case '!': return (char)0x02;
                case '#': return (char)0x04;
                case '$': return (char)0x05;
                case '%': return (char)0x06;
                case '^': return (char)0x07;
                case '&': return (char)0x08;
                case '*': return (char)0x09;
                case '(': return (char)0x0A;
                case ')': return (char)0x0B;
                case '-': return (char)0x0C;
                case '=': return (char)0x0D;
                case ';': return (char)0x27;
                case '\'': return (char)0x28;
                case '/': return (char)0x35;
                case '.': return (char)0x34;
                case ',': return (char)0x33;
                case '\n': return (char)0x1C;
                case '+': return (char)0x0D;
                case ':': return (char)0x27;
                case '\"': return (char)0x28;
                case '?': return (char)0x35;
                case '<': return (char)0x33;
                case '>': return (char)0x34;
                case '[': return (char)0x1A;
                case ']': return (char)0x1B;
                case '{': return (char)0x1A;
                case '}': return (char)0x1B;
                case '\\': return (char)0x2B;
                case '|': return (char)0x2B;
                case '`': return (char)0x29;
                case '~': return (char)0x29;
                case '_': return (char)0x0C;
                case '\t': return (char)0x39;  // TAB
                default: return (char)0xFF;
            }
        }

        public bool GetCSHIFT(char letter)
        {
            switch (letter)
            {
                case '@': return true;
                case 'A': return true;
                case 'B': return true;
                case 'C': return true;
                case 'D': return true;
                case 'E': return true;
                case 'F': return true;
                case 'G': return true;
                case 'H': return true;
                case 'I': return true;
                case 'J': return true;
                case 'K': return true;
                case 'L': return true;
                case 'M': return true;
                case 'N': return true;
                case 'O': return true;
                case 'P': return true;
                case 'Q': return true;
                case 'R': return true;
                case 'S': return true;
                case 'T': return true;
                case 'U': return true;
                case 'V': return true;
                case 'W': return true;
                case 'X': return true;
                case 'Y': return true;
                case 'Z': return true;
                case ' ': return false;
                case 'a': return false;
                case 'b': return false;
                case 'c': return false;
                case 'd': return false;
                case 'e': return false;
                case 'f': return false;
                case 'g': return false;
                case 'h': return false;
                case 'i': return false;
                case 'j': return false;
                case 'k': return false;
                case 'l': return false;
                case 'm': return false;
                case 'n': return false;
                case 'o': return false;
                case 'p': return false;
                case 'q': return false;
                case 'r': return false;
                case 's': return false;
                case 't': return false;
                case 'u': return false;
                case 'v': return false;
                case 'w': return false;
                case 'x': return false;
                case 'y': return false;
                case 'z': return false;
                case '0': return false;
                case '1': return false;
                case '2': return false;
                case '3': return false;
                case '4': return false;
                case '5': return false;
                case '6': return false;
                case '7': return false;
                case '8': return false;
                case '9': return false;
                case '!': return true;
                case '#': return true;
                case '$': return true;
                case '%': return true;
                case '^': return true;
                case '&': return true;
                case '*': return true;
                case '(': return true;
                case ')': return true;
                case '-': return false;
                case '=': return false;
                case ';': return false;
                case '\'': return false;
                case '/': return false;
                case '.': return false;
                case ',': return false;
                case '\n': return false;
                case '+': return true;
                case ':': return true;
                case '\"': return true;
                case '?': return true;
                case '<': return true;
                case '>': return true;
                case '[': return false;
                case ']': return false;
                case '{': return true;
                case '}': return true;
                case '\\': return false;
                case '|': return true;
                case '`': return false;
                case '~': return true;
                case '_': return true;
                case '\t': return false; // TAB
                default: return false;
            }
        }

        public bool GetLCNTRL(char letter)
        {
            switch (letter)
            {
                case '@': return false;
                case 'A': return false;
                case 'B': return false;
                case 'C': return false;
                case 'D': return false;
                case 'E': return false;
                case 'F': return false;
                case 'G': return false;
                case 'H': return false;
                case 'I': return false;
                case 'J': return false;
                case 'K': return false;
                case 'L': return false;
                case 'M': return false;
                case 'N': return false;
                case 'O': return false;
                case 'P': return false;
                case 'Q': return false;
                case 'R': return false;
                case 'S': return false;
                case 'T': return false;
                case 'U': return false;
                case 'V': return false;
                case 'W': return false;
                case 'X': return false;
                case 'Y': return false;
                case 'Z': return false;
                case ' ': return false;
                case 'a': return false;
                case 'b': return false;
                case 'c': return false;
                case 'd': return false;
                case 'e': return false;
                case 'f': return false;
                case 'g': return false;
                case 'h': return false;
                case 'i': return false;
                case 'j': return false;
                case 'k': return false;
                case 'l': return false;
                case 'm': return false;
                case 'n': return false;
                case 'o': return false;
                case 'p': return false;
                case 'q': return false;
                case 'r': return false;
                case 's': return false;
                case 't': return false;
                case 'u': return false;
                case 'v': return false;
                case 'w': return false;
                case 'x': return false;
                case 'y': return false;
                case 'z': return false;
                case '0': return false;
                case '1': return false;
                case '2': return false;
                case '3': return false;
                case '4': return false;
                case '5': return false;
                case '6': return false;
                case '7': return false;
                case '8': return false;
                case '9': return false;
                case '!': return false;
                case '#': return false;
                case '$': return false;
                case '%': return false;
                case '^': return false;
                case '&': return false;
                case '*': return false;
                case '(': return false;
                case ')': return false;
                case '-': return false;
                case '=': return false;
                case ';': return false;
                case '\'': return false;
                case '/': return false;
                case '.': return false;
                case ',': return false;
                case '\n': return false;
                case '+': return false;
                case ':': return false;
                case '\"': return false;
                case '?': return false;
                case '<': return false;
                case '>': return false;
                case '[': return true;
                case ']': return true;
                case '{': return false;
                case '}': return false;
                case '\\': return true;
                case '|': return false;
                case '`': return false;
                case '~': return false;
                case '_': return false;
                case '\t': return false; // TAB
                default: return false;
            }
        }

        public void SetClipboardText(string text)
        {
            Library.Clipboard.SetClipboardText(text);
        }
    }
}