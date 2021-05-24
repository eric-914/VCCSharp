using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMenuCallbacks
    {
        unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, MenuActions menuId, int type);
        unsafe void DynamicMenuActivated(EmuState* emuState, int menuItem);
    }

    public class MenuCallbacks : IMenuCallbacks
    {
        public unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, MenuActions menuId, int type)
        {
            Library.MenuCallbacks.DynamicMenuCallback(emuState, menuName, (int)menuId, type);
        }

        public unsafe void DynamicMenuActivated(EmuState* emuState, int menuItem)
        {
            Library.MenuCallbacks.DynamicMenuActivated(emuState, menuItem);
        }
    }
}
