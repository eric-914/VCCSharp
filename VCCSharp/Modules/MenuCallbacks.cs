using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public class MenuCallbacks
    {
        public unsafe void DynamicMenuCallback(EmuState* emuState, string menuName, int menuId, int type)
        {
            Library.MenuCallbacks.DynamicMenuCallback(emuState, menuName, menuId, type);
        }
    }
}
