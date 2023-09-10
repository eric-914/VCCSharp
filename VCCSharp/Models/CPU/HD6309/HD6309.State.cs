using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309 : VCCSharp.OpCodes.HD6309.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode => Mode.MC6809;  //TODO: Fix this!

    #region Registers 

    public byte CC { get => _cpu.cc.bits; set => _cpu.cc.bits = value; }
    public ushort PC { get => _cpu.pc.Reg; set => _cpu.pc.Reg = value; }
    public byte DP { get => _cpu.dp.msb; set => _cpu.dp.msb = value; }
    public ushort D { get => _cpu.q.lsw; set => _cpu.q.lsw = value; }
    public byte A { get => _cpu.q.lswmsb; set => _cpu.q.lswmsb = value; }
    public byte B { get => _cpu.q.lswlsb; set => _cpu.q.lswlsb = value; }
    public ushort X { get => _cpu.x.Reg; set => _cpu.x.Reg = value; }
    public ushort Y { get => _cpu.y.Reg; set => _cpu.y.Reg = value; }
    public ushort S { get => _cpu.s.Reg; set => _cpu.s.Reg = value; }
    public ushort U { get => _cpu.u.Reg; set => _cpu.u.Reg = value; }

    public byte MD { get => _cpu.md.bits; set => _cpu.md.bits = value; }
    public ushort V { get => _cpu.v.Reg; set => _cpu.v.Reg = value; }
    public uint Q { get => _cpu.q.Reg; set => _cpu.q.Reg = value; }
    public ushort W { get => _cpu.q.msw; set => _cpu.q.msw = value; }
    public byte E { get => _cpu.q.mswmsb; set => _cpu.q.mswmsb = value; }
    public byte F { get => _cpu.q.mswlsb; set => _cpu.q.mswlsb = value; }

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

    public uint MemRead32(ushort address)
    {
        return (uint)(MemRead16(address) << 16 | MemRead16((ushort)(address + 2)));
    }

    public void MemWrite32(uint data, ushort address)
    {
        MemWrite16((ushort)(data >> 16), address);
        MemWrite16((ushort)(data & 0xFFFF), (ushort)(address + 2));
    }

    #endregion
}
