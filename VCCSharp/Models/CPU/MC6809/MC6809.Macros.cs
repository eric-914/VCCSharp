namespace VCCSharp.Models.CPU.MC6809;

public partial class MC6809
{
    #region CC Masks Macros

    public byte CC
    {
        get => _cpu.cc.bits;
        set => _cpu.cc.bits = value;
    }

    private bool CC_E
    {
        get => _cpu.cc.E;
        set => _cpu.cc.E = value;
    }

    private bool CC_F
    {
        get => _cpu.cc.F;
        set => _cpu.cc.F = value;
    }

    private bool CC_H
    {
        get => _cpu.cc.H;
        set => _cpu.cc.H = value;
    }

    private bool CC_I
    {
        get => _cpu.cc.I;
        set => _cpu.cc.I = value;
    }

    private bool CC_N
    {
        get => _cpu.cc.N;
        set => _cpu.cc.N = value;
    }

    private bool CC_Z
    {
        get => _cpu.cc.Z;
        set => _cpu.cc.Z = value;
    }

    private bool CC_V
    {
        get => _cpu.cc.V;
        set => _cpu.cc.V = value;
    }

    private bool CC_C
    {
        get => _cpu.cc.C;
        set => _cpu.cc.C = value;
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

    public ushort D_REG
    {
        get => _cpu.d.Reg;
        set => _cpu.d.Reg = value;
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
        get => _cpu.d.msb;
        set => _cpu.d.msb = value;
    }

    public byte B_REG
    {
        get => _cpu.d.lsb;
        set => _cpu.d.lsb = value;
    }

    public byte DPA
    {
        get => _cpu.dp.msb;
        set => _cpu.dp.msb = value;
    }

    #endregion

    #region Macros

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
