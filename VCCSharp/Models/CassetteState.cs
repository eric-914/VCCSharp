using HANDLE = System.IntPtr;

namespace VCCSharp.Models
{
    public struct CassetteState
    {
        public byte FileType;

        public byte Byte;
        public byte LastSample;
        public byte Mask;
        public byte MotorState;
        public byte Quiet;
        public byte TapeMode;
        public byte WriteProtect;

        public uint BytesMoved;
        public uint TapeOffset;
        public uint TotalSize;

        public short LastTrans;
        public ushort TempIndex;

        public HANDLE TapeHandle;

        public unsafe fixed byte CassPath[Define.MAX_PATH];
        public unsafe fixed byte TapeFileName[Define.MAX_PATH];

        public unsafe fixed byte One[21];
        public unsafe fixed byte Zero[40];
        public unsafe fixed byte TempBuffer[8192];

        public unsafe byte* CasBuffer;
    }
}
