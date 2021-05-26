using System.Drawing;
using System.Runtime.InteropServices;
using HWND = System.IntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EmuState
    {
        public HWND WindowHandle;

        public byte EmulationRunning;
        public byte ResetPending;
    }
}
