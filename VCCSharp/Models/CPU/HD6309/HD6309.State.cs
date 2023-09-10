using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309 : VCCSharp.OpCodes.HD6309.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode => Mode.MC6809;  //TODO: Fix this!

    public ushort PC { get => PC_REG; set => PC_REG = value; }
    public byte DP { get => DPA; set => DPA = value; }
    public ushort D { get => D_REG; set => D_REG = value; }
    public byte A { get => A_REG; set => A_REG = value; }
    public byte B { get => B_REG; set => B_REG = value; }
    public ushort X { get => X_REG; set => X_REG = value; }
    public ushort Y { get => Y_REG; set => Y_REG = value; }
    public ushort S { get => S_REG; set => S_REG = value; }
    public ushort U { get => U_REG; set => U_REG = value; }

    public ushort V { get => V_REG; set => V_REG = value; }
    public uint Q { get => Q_REG; set => Q_REG = value; }
    public ushort W { get => W_REG; set => W_REG = value; }
    public byte E { get => E_REG; set => E_REG = value; }
    public byte F { get => F_REG; set => F_REG = value; }

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
