using VCCSharp.OpCodes.HD6309;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.N;

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

    public uint Q { get; set; }
    public ushort W { get; set; }
    public byte E { get; set; }
    public byte F { get; set; }
    public ushort V { get; set; }
    public byte MD { get; set; }

    VCCSharp.OpCodes.MC6809.IState VCCSharp.OpCodes.MC6809.ISystemState.cpu => this;
    public IState cpu => this;

    public Memory8Bit M8 => new(this);

    public Memory16Bit M16 => new(this);

    public MemoryDP DIRECT => new(this);

    public MemoryIndexed INDEXED => new(cpu, this);

    public IRegisters8Bit R8 => new Registers8Bit<IState>(cpu);

    public IRegisters16Bit R16 => new Registers16Bit<IState>(cpu);

    public int Cycles { get; set; }

    public Memory32Bit M32 => new(this);

    public Exceptions Exceptions => new();

}
