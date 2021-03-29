using System.Drawing;
using HWND = System.IntPtr;
using HINSTANCE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct DirectDrawState
    {
        public HWND hWndStatusBar;
        public HINSTANCE hInstance;
        public Point WindowSize;

        public byte InfoBand;
        public byte ForceAspect;

        public uint StatusBarHeight;
        public uint Color;

        public unsafe fixed byte AppNameText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte TitleBarText[Define.MAX_LOADSTRING];	// The title bar text
        public unsafe fixed byte StatusText[255];
    }
}
