using VCCSharp.OpCodes.Model;

namespace VCCSharp.OpCodes.Tests.Model.HD6309.O;

internal partial class OldOpcodes
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

    public byte MD { get; set; }

    public uint Q_REG { get; set; }
    public ushort W_REG { get; set; }
    public byte E_REG { get; set; }
    public byte F_REG { get; set; }
    public ushort V_REG { get; set; }
    public ushort O_REG { get => 0; set { } } //--Always zero

    private DynamicCycles _instance;
}
