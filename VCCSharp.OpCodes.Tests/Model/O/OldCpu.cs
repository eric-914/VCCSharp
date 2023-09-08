namespace VCCSharp.OpCodes.Tests;

internal partial class OldCpu
{
    public bool IsInInterrupt { get; set; }
    public bool IsSyncWaiting { get; set; }
    public int SyncCycle { get; set; }

    public byte CC { get; set; }

    public ushort PC_REG { get; set; }
    public ushort D_REG { get; set; }
    public ushort X_REG { get; set; }
    public ushort Y_REG { get; set; }
    public ushort S_REG { get; set; }
    public ushort U_REG { get; set; }
    public ushort DP_REG { get; set; }

    public int Cycles { get => _cycleCounter; }
}
