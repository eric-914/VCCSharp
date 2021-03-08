using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        int ClipboardEmpty();
        char PeekClipboard();
        void PopClipboard();
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
    }
}
