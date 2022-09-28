#pragma warning disable IDE1006


namespace VCCSharp.Models.CPU.MC6809.Registers;

// ReSharper disable once InconsistentNaming
public class MC6809CpuRegisters
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo

    public MC6809CpuRegister pc { get; } = new();
    public MC6809CpuRegister d { get; } = new();
    public MC6809CpuRegister x { get; } = new();
    public MC6809CpuRegister y { get; } = new();
    public MC6809CpuRegister u { get; } = new();
    public MC6809CpuRegister s { get; } = new();
    public MC6809CpuRegister dp { get; } = new();

    public byte ccbits;

    public bool[] cc = new bool[8];

    public Reg8 ureg8 { get; }
    public Reg16 xfreg16 { get; }

    public MC6809CpuRegisters()
    {
        ureg8 = new Reg8(this);
        xfreg16 = new Reg16(this);
    }
}
