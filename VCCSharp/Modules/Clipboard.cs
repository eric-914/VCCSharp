using System.Diagnostics;
using System.Windows;
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
            '@','a','b','c','d','e','f','g',   'h','i','j','k','l','m','n','o',  'p','q','r','s','t','u','v','w',   'x','y','z','[','\\',']',' ',' ',
            ' ','!','\"','#','$','%','&','\'', '(',')','*','+',',','-','.','/',  '0','1','2','3','4','5','6','7',   '8','9',':',';','<','=','>','?',
            '@','A','B','C','D','E','F','G',   'H','I','J','K','L','M','N','O',  'P','Q','R','S','T','U','V','W',   'X','Y','Z','[','\\',']',' ',' ',
            ' ','!','\"','#','$','%','&','\'', '(',')','*','+',',','-','.','/',  '0','1','2','3','4','5','6','7',   '8','9',':',';','<','=','>','?',
            '@','a','b','c','d','e','f','g',   'h','i','j','k','l','m','n','o',  'p','q','r','s','t','u','v','w',   'x','y','z','[','\\',']',' ',' ',
            ' ','!','\"','#','$','%','&','\'', '(',')','*','+',',','-','.','/',  '0','1','2','3','4','5','6','7',   '8','9',':',';','<','=','>','?'
        };

        private char[] pcchars40 =
        {
            ' ','!','\"','#','$','%','&','\'', '(',')','*','+',',','-','.','/',  '0','1','2','3','4','5','6','7',   '8','9',':',';','<','=','>','?',
            '@','A','B','C','D','E','F','G',   'H','I','J','K','L','M','N','O',  'P','Q','R','S','T','U','V','W',   'X','Y','Z','[','\\',']',' ',' ',
            '^','a','b','c','d','e','f','g',   'h','i','j','k','l','m','n','o',  'p','q','r','s','t','u','v','w',   'x','y','z','{','|','}','~','_',
            (char)'Ç',(char)'ü',(char)'é',(char)'â',(char)'ä',(char)'à',(char)'å',(char)'ç',
            (char)'ê',(char)'ë',(char)'è',(char)'ï',(char)'î',(char)'ß',(char)'Ä',(char)'Â',
            (char)'Ó',(char)'æ',(char)'Æ',(char)'ô',(char)'ö',(char)'ø',(char)'û',(char)'ù',
            (char)'Ø',(char)'Ö',(char)'Ü',(char)'§',(char)'£',(char)'±',(char)'º',(char)'¥',
            ' ',' ','!','\"','#','$','%','&',  '\'','(',')','*','+',',','-','.', '/','0','1','2','3','4','5','6',   '7','8','9',':',';','<','=','>',
            '?','@','A','B','C','D','E','F',   'G','H','I','J','K','L','M','N',  'O','P','Q','R','S','T','U','V',   'W','X','Y','Z','[','\\',']',' ',
            ' ','^','a','b','c','d','e','f',   'g','h','i','j','k','l','m','n',  'o','p','q','r','s','t','u','v',   'w','x','y','z','{','|','}','~','_'
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
            const string warning = "Warning: This operation will erase the Coco's BASIC memory before pasting. Continue?";

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

                _modules.Keyboard.vccKeyboardBuildRuntimeTable(1);

                var text = System.Windows.Clipboard.GetText();

                if (string.IsNullOrEmpty(text))
                {
                    MessageBox.Show("No text found in clipboard.", "Clipboard");

                    return;
                }

                PasteText(text);
            }
        }

        public void PasteText(string text)
        {
            Library.Clipboard.PasteText(text);
        }
    }
}
