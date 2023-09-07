﻿using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Model.Support;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState, IExtendedAddress
{
    public void EndInterrupt() { }
    public int SyncWait() => 0;

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

    public IState cpu => this;

    public Memory8Bit M8 => new Memory8Bit(this);

    public Memory16Bit M16 => new Memory16Bit(this);

    public MemoryDP DIRECT => new MemoryDP(this);

    public MemoryIndexed INDEXED => new MemoryIndexed(cpu, this);

    public IRegisters8Bit R8 => new Registers8Bit<IState>(cpu);

    public IRegisters16Bit R16 => new Registers16Bit<IState>(cpu);

    public int Cycles { get; set; }
}