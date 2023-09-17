using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.Registers
{
    /// <summary>
    /// Most registers are 16-bit internally.
    /// Some like the X register can be accessed as two 8-bit registers: A register, B register
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    internal class Register16
    {
        [FieldOffset(0)]
        internal ushort Reg;

        //--------------------------------------

        [FieldOffset(0)]
        internal byte lsb;

        [FieldOffset(1)]
        internal byte msb;
    }
}
