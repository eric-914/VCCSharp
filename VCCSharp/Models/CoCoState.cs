using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct CoCoState
    {
        public double SoundInterrupt;
        public double PicosToSoundSample;
        public double CycleDrift;
        public double CyclesThisLine;
        public double PicosToInterrupt;
        public double OldMaster;
        public double MasterTickCounter;
        public double UnxlatedTickCounter;
        public double PicosThisLine;
        public double CyclesPerSecord;
        public double LinesPerSecond;
        public double PicosPerLine;
        public double CyclesPerLine;

        public byte SoundOutputMode;
        public byte HorzInterruptEnabled;
        public byte VertInterruptEnabled;
        public byte TopBorder;
        public byte BottomBorder;
        public byte LinesperScreen;
        public byte TimerInterruptEnabled;
        public byte BlinkPhase;

        public ushort TimerClockRate;
        public ushort SoundRate;
        public ushort AudioIndex;

        public uint StateSwitch;

        public int MasterTimer;
        public int TimerCycleCount;
        public int ClipCycle;
        public int WaitCycle;
        public int IntEnable;
        public int SndEnable;
        public int OverClock;

        public byte Throttle;

        public unsafe fixed uint AudioBuffer[16384];
        public unsafe fixed byte CassBuffer[8192];
    }
}
