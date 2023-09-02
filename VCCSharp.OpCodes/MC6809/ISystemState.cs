using VCCSharp.OpCodes.Model;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

internal interface ISystemState
{
    IState cpu { get; }

    Memory8Bit M8 { get; }
    Memory16Bit M16 { get; }

    MemoryDP DIRECT { get; }
    MemoryIndexed INDEXED { get; }

    IRegisters8Bit R8 { get; }
    IRegisters16Bit R16 { get; }

    DynamicCycles DynamicCycles { get; }
}

internal class SystemState : ISystemState
{
    public IState cpu { get; }

    public Memory8Bit M8 { get; }
    public Memory16Bit M16 { get; }

    public MemoryDP DIRECT { get; }
    public MemoryIndexed INDEXED { get; }

    public IRegisters8Bit R8 { get; }
    public IRegisters16Bit R16 { get; }

    public DynamicCycles DynamicCycles { get; }

    public SystemState(IState cpu)
    {
        this.cpu = cpu;

        var memory = new Memory(cpu);
        M8 = memory.Byte;
        M16 = memory.Word;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        R8 = new Registers8Bit<IState>(cpu);
        R16 = new Registers16Bit<IState>(cpu);

        DynamicCycles = new DynamicCycles(cpu);
    }
}