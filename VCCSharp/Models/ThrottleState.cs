namespace VCCSharp.Models
{
    public struct ThrottleState
    {
        public LARGE_INTEGER StartTime;
        public LARGE_INTEGER EndTime;
        public LARGE_INTEGER OneFrame;
        public LARGE_INTEGER CurrentTime;
        public LARGE_INTEGER SleepRes;
        public LARGE_INTEGER TargetTime, OneMs;
        public LARGE_INTEGER MasterClock;
        public LARGE_INTEGER Now;

        public byte FrameSkip;
        public float fMasterClock;
    }
}
