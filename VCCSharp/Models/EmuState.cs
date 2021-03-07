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

        public byte BitDepth;
        public byte CpuType;
        public byte DoubleSpeedFlag;
        public byte DoubleSpeedMultiplier;
        public byte EmulationRunning;
        public byte FrameSkip;
        public byte FullScreen;
        public byte RamSize;
        public byte ResetPending;
        public byte ScanLines;
        public byte TurboSpeedFlag;

        public short FrameCounter;
        public short LineCounter;

        public double CPUCurrentSpeed;
  
        public long SurfacePitch;

        public unsafe fixed byte StatusLine[256];
    }
}
