using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.Registers
{
    /// <summary>
    /// Most registers are 16-bit internally.
    /// Some like the X register can be accessed as two 8-bit registers: A register, B register
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public class Register16
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
