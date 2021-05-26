using HANDLE = System.IntPtr;

namespace VCCSharp.Models
{
    // ReSharper disable once InconsistentNaming
    public struct MC6821State
    {
        public byte Asample;
        public byte Ssample;
        public byte Csample;
        public byte CartInserted;
        public byte CartAutoStart;

        public unsafe fixed byte rega[4];
        public unsafe fixed byte regb[4];
    }
}
