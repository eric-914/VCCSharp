using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

internal class SystemState : ISystemState, IExtendedAddress
{
    public IState cpu { get; }

    public Memory8Bit M8 { get; }
    public Memory16Bit M16 { get; }

    public MemoryDirect DIRECT { get; }
    public MemoryIndexed INDEXED { get; }

    public IRegisters8Bit R8 { get; }
    public IRegisters16Bit R16 { get; }

    public int Cycles { get; set; }

    public ushort PC { get => cpu.PC; set => cpu.PC = value; }
    public ushort D { get => cpu.D; set => cpu.D = value; }
    public byte A { get => cpu.A(); set => cpu.A(value); }
    public byte B { get => cpu.B(); set => cpu.B(value); }

    public Mode Mode => Mode.MC6809;

    public SystemState(IState cpu)
    {
        this.cpu = cpu;

        var memory = new Memory(cpu, this);
        M8 = memory.Byte;
        M16 = memory.Word;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        R8 = new Registers8Bit<IState>(cpu);
        R16 = new Registers16Bit<IState>(cpu);
    }
}