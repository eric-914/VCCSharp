using VCCSharp.OpCodes.Model;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.HD6309;

internal interface ISystemState
{
    IState cpu { get; }

    Memory8Bit M8 { get; }
    Memory16Bit M16 { get; }
    Memory32Bit M32 { get; }

    MemoryDP DIRECT { get; }
    MemoryIndexed INDEXED { get; }

    IRegisters8Bit R8 { get; }
    IRegisters16Bit R16 { get; }

    Exceptions Exceptions { get; }

    DynamicCycles DynamicCycles { get; }
}

internal class SystemState : ISystemState
{
    public IState cpu { get; }

    public Memory8Bit M8 { get; }
    public Memory16Bit M16 { get; }
    public Memory32Bit M32 { get; }

    public MemoryDP DIRECT { get; }
    public MemoryIndexed INDEXED { get; }

    public IRegisters8Bit R8 { get; }
    public IRegisters16Bit R16 { get; }

    public Exceptions Exceptions { get; }

    public DynamicCycles DynamicCycles { get; }

    public SystemState(IState cpu)
    {
        this.cpu = cpu;

        var memory = new Memory(cpu);
        M8 = memory.Byte;
        M16 = memory.Word;
        M32 = memory.DWord;
        DIRECT = memory.DP;
        INDEXED = memory.Indexed;

        R8 = new Registers8Bit<IState>(cpu);
        R16 = new Registers16Bit<IState>(cpu);

        Exceptions = new Exceptions() { SS = this };

        DynamicCycles = new DynamicCycles(cpu);
    }
}