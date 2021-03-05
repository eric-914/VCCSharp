using System.Drawing;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;
using LPARAM = System.IntPtr;
using WPARAM = System.UIntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct MSG
    {
        public HWND hwnd;
        public ushort message;  //UINT
        public WPARAM wParam;
        public LPARAM lParam;
        public uint time;       //DWORD
        public Point pt;
    }
}
