using VCCSharp.OpCodes;
using VCCSharp.OpCodes.Definitions;

namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309 : VCCSharp.OpCodes.HD6309.IState
{
    public IOpCodes OpCodes { get; }

    public Mode Mode { get; set; } = Mode.MC6809;  //TODO: Fix this!

    #region Registers 

    public byte CC { get => cc.bits; set => cc.bits = value; }
    public ushort PC { get => pc.Reg; set => pc.Reg = value; }
    public byte DP { get => dp.msb; set => dp.msb = value; }
    public ushort D { get => q.lsw; set => q.lsw = value; }
    public byte A { get => q.lswmsb; set => q.lswmsb = value; }
    public byte B { get => q.lswlsb; set => q.lswlsb = value; }
    public ushort X { get => x.Reg; set => x.Reg = value; }
    public ushort Y { get => y.Reg; set => y.Reg = value; }
    public ushort S { get => s.Reg; set => s.Reg = value; }
    public ushort U { get => u.Reg; set => u.Reg = value; }

    public byte MD { get => md.bits; set => md.bits = value; }
    public ushort V { get => v.Reg; set => v.Reg = value; }
    public uint Q { get => q.Reg; set => q.Reg = value; }
    public ushort W { get => q.msw; set => q.msw = value; }
    public byte E { get => q.mswmsb; set => q.mswmsb = value; }
    public byte F { get => q.mswlsb; set => q.mswlsb = value; }

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
