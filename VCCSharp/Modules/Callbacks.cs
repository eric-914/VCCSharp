using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICallbacks
    {
        unsafe void RefreshDynamicMenu(EmuState* emuState);
    }

    public class Callbacks : ICallbacks
    {
        public unsafe void RefreshDynamicMenu(EmuState* emuState)
        {
            Library.Callbacks.RefreshDynamicMenu(emuState);;
        }
    }
}
