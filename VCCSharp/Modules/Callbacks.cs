using VCCSharp.Libraries;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface ICallbacks
    {
        void RefreshDynamicMenu(HWND hWnd);
    }

    public class Callbacks : ICallbacks
    {
        public void RefreshDynamicMenu(HWND hWnd)
        {
            Library.Callbacks.RefreshDynamicMenu(hWnd);
        }
    }
}
