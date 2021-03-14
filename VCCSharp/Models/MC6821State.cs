using HANDLE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct MC6821State
    {
        public byte LeftChannel;
        public byte RightChannel;
        public byte Asample;
        public byte Ssample;
        public byte Csample;
        public byte CartInserted;
        public byte CartAutoStart;
        public byte AddLF;

        public unsafe fixed byte rega[4];
        public unsafe fixed byte regb[4];
        public unsafe fixed byte rega_dd[4];
        public unsafe fixed byte regb_dd[4];

        public HANDLE hPrintFile;
        public HANDLE hOut;
        public int MonState;
    }
}
