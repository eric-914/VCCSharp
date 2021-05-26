using HANDLE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct CassetteState
    {
        public byte FileType;

        public byte Byte;
        public byte LastSample;
        public byte Mask;

        public uint BytesMoved;
        public uint TapeOffset;
        public uint TotalSize;

        public HANDLE TapeHandle;

        public int LastTrans;
        public uint TempIndex;

        public unsafe fixed byte TempBuffer[8192];

        public unsafe byte* CasBuffer;
    }
}
