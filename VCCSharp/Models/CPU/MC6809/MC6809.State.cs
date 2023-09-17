using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.MC6809;

partial class MC6809 : VCCSharp.OpCodes.MC6809.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode => Mode.MC6809;

    #region Registers

    public byte CC { get => cc.bits; set => cc.bits = value; }
    public ushort PC { get => pc.Reg; set => pc.Reg = value; }
    public byte DP { get => dp.msb; set => dp.msb = value; }
    public ushort D { get => d.Reg; set => d.Reg = value; }
    public byte A { get => d.msb; set => d.msb = value; }
    public byte B { get => d.lsb; set => d.lsb = value; }
    public ushort X { get => x.Reg; set => x.Reg = value; }
    public ushort Y { get => y.Reg; set => y.Reg = value; }
    public ushort S { get => s.Reg; set => s.Reg = value; }
    public ushort U { get => u.Reg; set => u.Reg = value; }

    #endregion

    public int SynchronizeWithInterrupt()
    {
        _isSyncWaiting = true;

        return _syncCycle;
    }

    public void ClearInterrupt()
    {
        _isInFastInterrupt = false;
    }

    #region Memory Macros

    public byte MemRead8(ushort address) => _modules.TCC1014.MemRead8(address);
    public void MemWrite8(byte data, ushort address) => _modules.TCC1014.MemWrite8(data, address);

    public ushort MemRead16(ushort address)
    {
        return (ushort)(MemRead8(address) << 8 | MemRead8((ushort)(address + 1)));
    }

    public void MemWrite16(ushort data, ushort address)
    {
        MemWrite8((byte)(data >> 8), address);
        MemWrite8((byte)(data & 0xFF), (ushort)(address + 1));
    }

    #endregion
}
