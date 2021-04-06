using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.HD6309
{
    [StructLayout(LayoutKind.Explicit)]
    public class HD6309CpuRegister
    {
        [FieldOffset(0)]
        public ushort Reg;

        //--------------------------------------

        [FieldOffset(0)]
        public byte lsb;

        [FieldOffset(1)]
        public byte msb;
    }
}
