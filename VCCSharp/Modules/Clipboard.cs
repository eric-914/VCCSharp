using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        bool ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteText();
        void PasteBASIC();
        void PasteBASICWithNew();
    }

    public class Clipboard : IClipboard
    {
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

        public void PasteText()
        {
            Library.Clipboard.PasteText();
        }

        public void PasteBASIC()
        {
            Library.Clipboard.PasteBASIC();
        }

        public void PasteBASICWithNew()
        {
            Library.Clipboard.PasteBASICWithNew();
        }
    }
}
