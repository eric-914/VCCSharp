using VCCSharp.OpCodes.HD6309;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.N;

internal partial class NewOpcodes : ISystemState, IExtendedAddress
{
    public void ClearInterrupt() { }
    public int SynchronizeWithInterrupt() => 0;

    public uint Q { get; set; } // D | W

    public ushort PC { get; set; }
    public byte DP { get; set; }
    public ushort X { get; set; }
    public ushort Y { get; set; }
    public ushort S { get; set; }
    public ushort U { get; set; }
    public byte CC { get; set; }

    public ushort D { get => (ushort)(Q >> 16); set => Q = (Q & 0x0000FFFF) | (uint)(value << 16); } // A | B
    public byte A { get => (byte)(D >> 8); set => D = (ushort)((D & 0x00FF) | (value << 8)); }
    public byte B { get => (byte)(D & 0xFF); set => D = (ushort)((D & 0xFF00) | value); }

    public ushort V { get; set; }
    public byte MD { get; set; }

    public ushort W { get => (ushort)(Q & 0xFFFF); set => Q = (Q & 0xFFFF0000) | value; } // E | F
    public byte E { get => (byte)(W >> 8); set => W = (ushort)((W & 0x00FF) | (value << 8)); }
    public byte F { get => (byte)(W & 0xFF); set => W = (ushort)((W & 0xFF00) | value); }

    VCCSharp.OpCodes.MC6809.IState VCCSharp.OpCodes.MC6809.ISystemState.State => this;
    public IState State => this;

    public Memory8Bit M8 => new(this);

    public Memory16Bit M16 => new(this);

    public MemoryDirect DIRECT => new(this);

    public MemoryIndexed INDEXED => new(State, this);

    public IRegisters8Bit R8 => new Registers8Bit<IState>(State);

    public IRegisters16Bit R16 => new Registers16Bit<IState>(State);

    public int Cycles { get; set; }

    public Memory32Bit M32 => new(this);

    public Exceptions Exceptions { get; private set; }

}
