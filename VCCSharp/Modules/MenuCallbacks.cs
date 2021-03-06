using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type);
    }

    public class MenuCallbacks : IMenuCallbacks
    {
        public unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type)
        {
            Library.MenuCallbacks.DynamicMenuCallback(emuState, menuName, menuId, type);
        }
    }
}
