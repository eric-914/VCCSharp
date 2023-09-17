using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests.Model.MC6809.N;

internal partial class NewOpcodes : ISystemState, IExtendedAddress
{
    public void ClearInterrupt() { }
    public int SynchronizeWithInterrupt() => 0;

    public ushort PC { get; set; }
    public byte DP { get; set; }
    public ushort D { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort S { get; set; }
    public ushort U { get; set; }
    public byte CC { get; set; }

    public byte A { get => (byte)(D >> 8); set => D = (ushort)((D & 0x00FF) | (value << 8)); }
    public byte B { get => (byte)(D & 0xFF); set => D = (ushort)((D & 0xFF00) | value); }

    public IState State => this;

    public Memory8Bit M8 => new(this);

    public Memory16Bit M16 => new(this);

    public MemoryDirect DIRECT => new(this);

    public MemoryIndexed INDEXED => new(State, this);

    public IRegisters8Bit R8 => new Registers8Bit<IState>(State);

    public IRegisters16Bit R16 => new Registers16Bit<IState>(State);

    public int Cycles { get; set; }
}
