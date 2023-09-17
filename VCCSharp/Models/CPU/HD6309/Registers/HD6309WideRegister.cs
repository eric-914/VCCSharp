using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.HD6309.Registers;

[StructLayout(LayoutKind.Explicit)]
internal class HD6309WideRegister
{
    [FieldOffset(0)]
    internal uint Reg;

    //--------------------------------------

    [FieldOffset(0)]
    internal ushort msw;

    [FieldOffset(2)]
    internal ushort lsw;

    //--------------------------------------

    [FieldOffset(0)]
    internal byte mswlsb;

    [FieldOffset(1)]
    internal byte mswmsb;

    [FieldOffset(2)]
    internal byte lswlsb;

    [FieldOffset(3)]
    internal byte lswmsb;
}
