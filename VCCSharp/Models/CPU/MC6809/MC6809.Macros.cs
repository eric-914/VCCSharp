using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        #region CC Masks Macros

        public unsafe bool CC_E
        {
            get => _instance->cc[(int)CCFlagMasks.E] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.E] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_F
        {
            get => _instance->cc[(int)CCFlagMasks.F] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.F] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_H
        {
            get => _instance->cc[(int)CCFlagMasks.H] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.H] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_I
        {
            get => _instance->cc[(int)CCFlagMasks.I] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.I] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_N
        {
            get => _instance->cc[(int)CCFlagMasks.N] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.N] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_Z
        {
            get => _instance->cc[(int)CCFlagMasks.Z] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.Z] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_V
        {
            get => _instance->cc[(int)CCFlagMasks.V] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.V] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_C
        {
            get => _instance->cc[(int)CCFlagMasks.C] == Define.TRUE;
            set => _instance->cc[(int)CCFlagMasks.C] = value ? Define.TRUE : Define.FALSE;
        }

        #endregion

        #region Register Macros

        public unsafe ushort PC_REG
        {
            get => _instance->pc.Reg;
            set => _instance->pc.Reg = value;
        }

        public unsafe ushort DP_REG
        {
            get => _instance->dp.Reg;
            set => _instance->dp.Reg = value;
        }

        public unsafe ushort D_REG
        {
            get => _instance->d.Reg;
            set => _instance->d.Reg = value;
        }

        public unsafe ushort S_REG
        {
            get => _instance->s.Reg;
            set => _instance->s.Reg = value;
        }

        public unsafe byte S_L
        {
            get => _instance->s.lsb;
            set => _instance->s.lsb = value;
        }

        public unsafe byte S_H
        {
            get => _instance->s.msb;
            set => _instance->s.msb = value;
        }

        public unsafe ushort U_REG
        {
            get => _instance->u.Reg;
            set => _instance->u.Reg = value;
        }

        public unsafe byte PC_L
        {
            get => _instance->pc.lsb;
            set => _instance->pc.lsb = value;
        }

        public unsafe byte PC_H
        {
            get => _instance->pc.msb;
            set => _instance->pc.msb = value;
        }

        public unsafe ushort X_REG
        {
            get => _instance->x.Reg;
            set => _instance->x.Reg = value;
        }

        public unsafe byte X_L
        {
            get => _instance->x.lsb;
            set => _instance->x.lsb = value;
        }

        public unsafe byte X_H
        {
            get => _instance->x.msb;
            set => _instance->x.msb = value;
        }

        public unsafe ushort Y_REG
        {
            get => _instance->y.Reg;
            set => _instance->y.Reg = value;
        }

        public unsafe byte Y_L
        {
            get => _instance->y.lsb;
            set => _instance->y.lsb = value;
        }

        public unsafe byte Y_H
        {
            get => _instance->y.msb;
            set => _instance->y.msb = value;
        }

        public unsafe byte U_L
        {
            get => _instance->u.lsb;
            set => _instance->u.lsb = value;
        }

        public unsafe byte U_H
        {
            get => _instance->u.msb;
            set => _instance->u.msb = value;
        }

        public unsafe byte A_REG
        {
            get => _instance->d.msb;
            set => _instance->d.msb = value;
        }

        public unsafe byte B_REG
        {
            get => _instance->d.lsb;
            set => _instance->d.lsb = value;
        }

        public unsafe byte DPA
        {
            get => _instance->dp.msb;
            set => _instance->dp.msb = value;
        }

        #endregion

        #region Macros

        public unsafe byte PUR(int i) => *(byte*)(_instance->ureg8[i]);
        public unsafe void PUR(int i, byte value) => *(byte*)(_instance->ureg8[i]) = value;

        public unsafe ushort PXF(int i) => *(ushort*)(_instance->xfreg16[i]);
        public unsafe void PXF(int i, ushort value) => *(ushort*)(_instance->xfreg16[i]) = value;

        public unsafe ushort DPADDRESS(ushort r) => (ushort)(_instance->dp.Reg | MemRead8(r));

        public byte MemRead8(ushort address) => _modules.TC1014.MemRead8(address);
        public void MemWrite8(byte data, ushort address) => _modules.TC1014.MemWrite8(data, address);
        public ushort MemRead16(ushort address) => _modules.TC1014.MemRead16(address);
        public void MemWrite16(ushort data, ushort address) => _modules.TC1014.MemWrite16(data, address);

        public ushort INDADDRESS(ushort address) => MC6809_CalculateEA(MemRead8(address));

        public bool NTEST8(byte value) => value > 0x7F;
        public bool NTEST16(ushort value) => value > 0x7FFF;

        public bool ZTEST(byte value) => value == 0;
        public bool ZTEST(ushort value) => value == 0;

        public bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
        public bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;

        #endregion
    }
}
