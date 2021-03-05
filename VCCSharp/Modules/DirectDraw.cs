using VCCSharp.Libraries;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Modules
{
    public class DirectDraw
    {
        public bool InitDirectDraw(HINSTANCE hInstance, HINSTANCE resources)
        {
            return Library.DirectDraw.InitDirectDraw(hInstance, resources);
        }

        public void ClearScreen()
        {
            Library.DirectDraw.ClearScreen();
        }
    }
}
