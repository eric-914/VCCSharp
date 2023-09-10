using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.MC6809;

partial class MC6809 : VCCSharp.OpCodes.MC6809.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode => Mode.MC6809;

    #region Registers

    public byte CC { get => _cpu.cc.bits; set => _cpu.cc.bits = value; }
    public ushort PC { get => _cpu.pc.Reg; set => _cpu.pc.Reg = value; }
    public byte DP { get => _cpu.dp.msb; set => _cpu.dp.msb = value; }
    public ushort D { get => _cpu.d.Reg; set => _cpu.d.Reg = value; }
    public byte A { get => _cpu.d.msb; set => _cpu.d.msb = value; }
    public byte B { get => _cpu.d.lsb; set => _cpu.d.lsb = value; }
    public ushort X { get => _cpu.x.Reg; set => _cpu.x.Reg = value; }
    public ushort Y { get => _cpu.y.Reg; set => _cpu.y.Reg = value; }
    public ushort S { get => _cpu.s.Reg; set => _cpu.s.Reg = value; }
    public ushort U { get => _cpu.u.Reg; set => _cpu.u.Reg = value; }

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
