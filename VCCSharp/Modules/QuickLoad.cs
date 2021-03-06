using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IQuickLoad
    {
        unsafe int QuickStart(EmuState* emuState, string binFileName);
    }

    public class QuickLoad : IQuickLoad
    {
        public unsafe int QuickStart(EmuState* emuState, string binFileName)
        {
            return Library.QuickLoad.QuickStart(emuState, binFileName);
        }
    }
}
