#pragma warning disable IDE1006


using VCCSharp.Models.CPU.Registers;

namespace VCCSharp.Models.CPU.MC6809.Registers;

// ReSharper disable once InconsistentNaming
public class MC6809CpuRegisters
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo

    public Register16 pc { get; } = new();
    public Register16 d { get; } = new();
    public Register16 x { get; } = new();
    public Register16 y { get; } = new();
    public Register16 u { get; } = new();
    public Register16 s { get; } = new();
    public Register16 dp { get; } = new();

    public RegisterCC cc { get; } = new();

    public Reg8 ureg8 { get; }
    public Reg16 xfreg16 { get; }

    public MC6809CpuRegisters()
    {
        ureg8 = new Reg8(this);
        xfreg16 = new Reg16(this);
    }
}
