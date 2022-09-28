#pragma warning disable IDE1006


namespace VCCSharp.Models.CPU.HD6309.Registers;

// ReSharper disable once InconsistentNaming
public class HD6309CpuRegisters
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo

    public HD6309CpuRegister pc { get; } = new();
    public HD6309CpuRegister x { get; } = new();
    public HD6309CpuRegister y { get; } = new();
    public HD6309CpuRegister u { get; } = new();
    public HD6309CpuRegister s { get; } = new();
    public HD6309CpuRegister dp { get; } = new();
    public HD6309CpuRegister v { get; } = new();
    public HD6309CpuRegister z { get; } = new();

    public HD6309WideRegister q { get; } = new();

    public byte ccbits;
    public byte mdbits;

    public bool[] cc = new bool[8];
    public bool[] md = new bool[8];

    public Reg8 ureg8 { get; }
    public Reg16 xfreg16 { get; }

    public HD6309CpuRegisters()
    {
        ureg8 = new Reg8(this);
        xfreg16 = new Reg16(this);
    }
}
