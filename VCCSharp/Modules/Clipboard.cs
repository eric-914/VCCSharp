using VCCSharp.Libraries;

namespace VCCSharp.Modules
{
    public interface IClipboard
    {
        int ClipboardEmpty();
    }

    public class Clipboard : IClipboard
    {
        public int ClipboardEmpty()
        {
            return Library.Clipboard.ClipboardEmpty();
        }
    }
}
