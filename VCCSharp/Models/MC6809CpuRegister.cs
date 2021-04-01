using System.Runtime.InteropServices;

namespace VCCSharp.Models
{
    [StructLayout(LayoutKind.Explicit)]
    public struct MC6809CpuRegister
    {
        [FieldOffset(0)]
        public ushort Reg;

        [FieldOffset(0)]
        public byte lsb;

        [FieldOffset(1)]
        public byte msb;

        //public struct B
        //{
        //}
    }
}
