using VCCSharp.OpCodes.MC6809;
using VCCSharp.OpCodes.Model;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Tests;

internal partial class NewCpu : ISystemState
{
    public bool IsInInterrupt { get; set; }
    public bool IsSyncWaiting { get; set; }
    public int SyncCycle { get; set; }

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

    public MemoryIndexed INDEXED => throw new NotImplementedException();

    public IRegisters8Bit R8 => throw new NotImplementedException();

    public IRegisters16Bit R16 => throw new NotImplementedException();

    public DynamicCycles DynamicCycles => new DynamicCycles(this);

    public int Cycles { get; set; }
}
