namespace VCCSharp.Models.CPU.HD6309;

partial class HD6309
{
    #region CC/MD Masks Macros

    public byte CC
    {
        get => _cpu.cc.bits;
        set => _cpu.cc.bits = value;
    }

    public byte MD
    {
        get => _cpu.md.bits;
        set => _cpu.md.bits = value;
    }

    #endregion

    #region Register Macros

    public ushort PC_REG
    {
        get => _cpu.pc.Reg;
        set => _cpu.pc.Reg = value;
    }

    public ushort DP_REG
    {
        get => _cpu.dp.Reg;
        set => _cpu.dp.Reg = value;
    }

    public ushort W_REG
    {
        get => _cpu.q.msw;
        set => _cpu.q.msw = value;
    }

    public ushort D_REG
    {
        get => _cpu.q.lsw;
        set => _cpu.q.lsw = value;
    }

    public uint Q_REG
    {
        get => _cpu.q.Reg;
        set => _cpu.q.Reg = value;
    }

    public ushort S_REG
    {
        get => _cpu.s.Reg;
        set => _cpu.s.Reg = value;
    }

    public ushort U_REG
    {
        get => _cpu.u.Reg;
        set => _cpu.u.Reg = value;
    }

    public ushort X_REG
    {
        get => _cpu.x.Reg;
        set => _cpu.x.Reg = value;
    }

    public ushort Y_REG
    {
        get => _cpu.y.Reg;
        set => _cpu.y.Reg = value;
    }

    public byte A_REG
    {
        get => _cpu.q.lswmsb;
        set => _cpu.q.lswmsb = value;
    }

    public byte B_REG
    {
        get => _cpu.q.lswlsb;
        set => _cpu.q.lswlsb = value;
    }

    public ushort V_REG
    {
        get => _cpu.v.Reg;
        set => _cpu.v.Reg = value;
    }

    public ushort Z_REG
    {
        get => _cpu.z.Reg;
        set => _cpu.z.Reg = value;
    }

    public byte F_REG
    {
        get => _cpu.q.mswlsb;
        set => _cpu.q.mswlsb = value;
    }

    public byte E_REG
    {
        get => _cpu.q.mswmsb;
        set => _cpu.q.mswmsb = value;
    }

    public byte DPA
    {
        get => _cpu.dp.msb;
        set => _cpu.dp.msb = value;
    }

    #endregion

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
