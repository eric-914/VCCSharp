using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        int ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
        void CopyText();
        void PasteText();
        void PasteBASIC();
        void PasteBASICWithNew();
    }

    public class Clipboard : IClipboard
    {
        public int ClipboardEmpty()
        {
            return Library.Clipboard.ClipboardEmpty();
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
