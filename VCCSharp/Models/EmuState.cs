using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct EmuState
    {
        public IntPtr Resources;

        public IntPtr WindowHandle;
        public IntPtr ConfigDialog;

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

        public short LineCounter;

        public double CPUCurrentSpeed;
  
        public long SurfacePitch;

        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        //public string StatusLine;
        public unsafe fixed byte StatusLine[256];
    }
}
