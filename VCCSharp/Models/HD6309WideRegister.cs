using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct HD6309WideRegister
    {
        [FieldOffset(0)]
        public uint Reg;

        [FieldOffset(0)]
        public ushort msw;

        [FieldOffset(2)]
        public ushort lsw;

        //struct
        //{
        //    unsigned short msw, lsw;
        //} Word;

        [FieldOffset(0)]
        public byte mswlsb;

        [FieldOffset(1)]
        public byte mswmsb;

        [FieldOffset(2)]
        public byte lswlsb;

        [FieldOffset(3)]
        public byte lswmsb;	//Might be backwards

        //struct
        //{
        //    unsigned char mswlsb, mswmsb, lswlsb, lswmsb;	//Might be backwards
        //} Byte;
    }
}
