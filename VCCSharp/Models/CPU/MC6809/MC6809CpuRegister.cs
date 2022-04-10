using System.Runtime.InteropServices;

namespace VCCSharp.Models.CPU.MC6809;

[StructLayout(LayoutKind.Explicit)]
// ReSharper disable once InconsistentNaming
public class MC6809CpuRegister
{
    [FieldOffset(0)]
    public ushort Reg;

    //--------------------------------------

    [FieldOffset(0)]
    public byte lsb;

    [FieldOffset(1)]
    public byte msb;
}
