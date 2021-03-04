using System;
using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    public struct EmuState
    {
        public IntPtr Resources;

        public IntPtr WindowHandle;
        public IntPtr ConfigDialog;

        public POINT WindowSize;

        public byte RamSize;

        public double CPUCurrentSpeed;

        public byte DoubleSpeedMultiplier;
        public byte DoubleSpeedFlag;
        public byte TurboSpeedFlag;
        public byte CpuType;
        public byte FrameSkip;
        public byte BitDepth;

        public long SurfacePitch;

        public short LineCounter;

        public byte ScanLines;
        public byte EmulationRunning;
        public byte ResetPending;

        public byte FullScreen;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
        public string StatusLine;
    }
}
