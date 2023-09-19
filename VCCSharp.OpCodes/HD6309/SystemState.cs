using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Memory;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.HD6309;

internal class SystemState : ISystemState, IExtendedAddress
{
    MC6809.IState MC6809.ISystemState.State => State;
    public IState State { get; }

    public Memory8Bit M8 { get; }
    public Memory16Bit M16 { get; }
    public Memory32Bit M32 { get; }

    public MemoryDirect DIRECT { get; }
    public MemoryIndexed INDEXED { get; }

    public IRegisters8Bit R8 { get; }
    public IRegisters16Bit R16 { get; }

    public int Cycles { get; set; }

    public ushort PC { get => State.PC; set => State.PC = value; }
    public ushort D { get => State.D; set => State.D = value; }
    public byte A { get => State.A(); set => State.A(value); }
    public byte B { get => State.B(); set => State.B(value); }

    public Mode Mode => State.Mode;

    public Exceptions Exceptions { get; }

    public SystemState(IState cpu)
    {
        this.State = cpu;

        var memory = new Memory(cpu, this);
        M8 = memory.Byte;
        M16 = memory.Word;
        M32 = memory.DWord;
        DIRECT = memory.Direct;
        INDEXED = memory.Indexed;

        R8 = new Registers8Bit<IState>(cpu);
        R16 = new Registers16Bit<IState>(cpu);

        Exceptions = new Exceptions() { System = this };
    }
}