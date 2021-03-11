using HWND = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct DirectDrawState
    {
        public byte InfoBand;
        public byte ForceAspect;

        public ushort StatusBarHeight;
        public ushort Color;

        public unsafe fixed byte TitleBarText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte AppNameText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte StatusText[255];

        public HWND hWndStatusBar;
        public HINSTANCE hInstance;
    }
}
