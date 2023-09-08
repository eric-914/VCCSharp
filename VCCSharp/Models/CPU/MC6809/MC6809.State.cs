using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.MC6809;

public partial class MC6809 : VCCSharp.OpCodes.MC6809.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode => Mode.MC6809;

    public ushort PC { get => PC_REG; set => PC_REG = value; }
    public byte DP { get => DPA; set => DPA = value; }
    public ushort D { get => D_REG; set => D_REG = value; }
    public byte A { get => A_REG; set => A_REG = value; }
    public byte B { get => B_REG; set => B_REG = value; }
    public ushort X { get => X_REG; set => X_REG = value; }
    public ushort Y { get => Y_REG; set => Y_REG = value; }
    public ushort S { get => S_REG; set => S_REG = value; }
    public ushort U { get => U_REG; set => U_REG = value; }

    public int SynchronizeWithInterrupt()
    {
        _isSyncWaiting = true;

        return _syncCycle;
    }

    public void ClearInterrupt()
    {
        _isInFastInterrupt = false;
    }
}
