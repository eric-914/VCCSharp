using System.Drawing;
using System.Runtime.InteropServices;
using HINSTANCE = System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EmuState
    {
        public HINSTANCE Resources;

        public HWND WindowHandle;
        public HWND ConfigDialog;

        public Point WindowSize;

        public byte DoubleSpeedFlag;
        public byte DoubleSpeedMultiplier;
        public byte EmulationRunning;
        public byte ResetPending;
        public byte TurboSpeedFlag;

        public double CPUCurrentSpeed;
 
    }
}
