using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public class QuickLoad
    {
        public unsafe int QuickStart(EmuState* emuState, string binFileName)
        {
            return Library.QuickLoad.QuickStart(emuState, binFileName);
        }
    }
}
