using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.HD6309
{
    partial class HD6309
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
            get => _instance->r.pc.Reg;
            set => _instance->r.pc.Reg = value;
        }

        public unsafe ushort DP_REG
        {
            get => _instance->r.dp.Reg;
            set => _instance->r.dp.Reg = value;
        }

        public unsafe ushort W_REG
        {
            get => _instance->r.q.msw;
            set => _instance->r.q.msw = value;
        }

        public unsafe ushort D_REG
        {
            get => _instance->r.q.lsw;
            set => _instance->r.q.lsw = value;
        }

        public unsafe uint Q_REG
        {
            get => _instance->r.q.Reg;
            set => _instance->r.q.Reg = value;
        }

        public unsafe ushort S_REG
        {
            get => _instance->r.s.Reg;
            set => _instance->r.s.Reg = value;
        }

        public unsafe byte S_L
        {
            get => _instance->r.s.lsb;
            set => _instance->r.s.lsb = value;
        }

        public unsafe byte S_H
        {
            get => _instance->r.s.msb;
            set => _instance->r.s.msb = value;
        }

        public unsafe ushort U_REG
        {
            get => _instance->r.u.Reg;
            set => _instance->r.u.Reg = value;
        }

        public unsafe byte PC_L
        {
            get => _instance->r.pc.lsb;
            set => _instance->r.pc.lsb = value;
        }

        public unsafe byte PC_H
        {
            get => _instance->r.pc.msb;
            set => _instance->r.pc.msb = value;
        }

        public unsafe ushort X_REG
        {
            get => _instance->r.x.Reg;
            set => _instance->r.x.Reg = value;
        }

        public unsafe byte X_L
        {
            get => _instance->r.x.lsb;
            set => _instance->r.x.lsb = value;
        }

        public unsafe byte X_H
        {
            get => _instance->r.x.msb;
            set => _instance->r.x.msb = value;
        }

        public unsafe ushort Y_REG
        {
            get => _instance->r.y.Reg;
            set => _instance->r.y.Reg = value;
        }

        public unsafe byte Y_L
        {
            get => _instance->r.y.lsb;
            set => _instance->r.y.lsb = value;
        }

        public unsafe byte Y_H
        {
            get => _instance->r.y.msb;
            set => _instance->r.y.msb = value;
        }

        public unsafe byte U_L
        {
            get => _instance->r.u.lsb;
            set => _instance->r.u.lsb = value;
        }

        public unsafe byte U_H
        {
            get => _instance->r.u.msb;
            set => _instance->r.u.msb = value;
        }

        public unsafe byte A_REG
        {
            get => _instance->r.q.lswmsb;
            set => _instance->r.q.lswmsb = value;
        }

        public unsafe byte B_REG
        {
            get => _instance->r.q.lswlsb;
            set => _instance->r.q.lswlsb = value;
        }

        public unsafe ushort O_REG
        {
            get => _instance->r.z.Reg;
            set => _instance->r.z.Reg = value;
        }

        public unsafe byte F_REG
        {
            get => _instance->r.q.mswlsb;
            set => _instance->r.q.mswlsb = value;
        }

        public unsafe byte E_REG
        {
            get => _instance->r.q.mswmsb;
            set => _instance->r.q.mswmsb = value;
        }

        public unsafe byte DPA
        {
            get => _instance->r.dp.msb;
            set => _instance->r.dp.msb = value;
        }

        #endregion

        #region Macros

        public unsafe byte PUR(int i) => *(byte*)(_instance->ureg8[i]);
        public unsafe void PUR(int i, byte value) => *(byte*)(_instance->ureg8[i]) = value;

        public unsafe ushort PXF(int i) => *(ushort*)(_instance->xfreg16[i]);
        public unsafe void PXF(int i, ushort value) => *(ushort*)(_instance->xfreg16[i]) = value;

        public unsafe ushort DPADDRESS(ushort r) => (ushort)(_instance->r.dp.Reg | MemRead8(r));

        public byte MemRead8(ushort address) => _modules.TC1014.MemRead8(address);
        public void MemWrite8(byte data, ushort address) => _modules.TC1014.MemWrite8(data, address);
        public ushort MemRead16(ushort address) => _modules.TC1014.MemRead16(address);
        public void MemWrite16(ushort data, ushort address) => _modules.TC1014.MemWrite16(data, address);
        public uint MemRead32(ushort address) => _modules.TC1014.MemRead32(address);
        public void MemWrite32(uint data, ushort address) => _modules.TC1014.MemWrite32(data, address);

        public ushort IMMADDRESS(ushort address) => MemRead16(address);
        public ushort INDADDRESS(ushort address) => CalculateEA(MemRead8(address));

        public bool NTEST8(byte value) => value > 0x7F;
        public bool NTEST16(ushort value) => value > 0x7FFF;
        public bool NTEST32(uint value) => value > 0x7FFFFFFF;

        public bool ZTEST(byte value) => value == 0;
        public bool ZTEST(ushort value) => value == 0;
        public bool ZTEST(uint value) => value == 0;

        public bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
        public bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;

        public unsafe bool MD_NATIVE6309
        {
            get => _instance->md[(int)MDFlagMasks.NATIVE6309] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.NATIVE6309] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_FIRQMODE
        {
            get => _instance->md[(int)MDFlagMasks.FIRQMODE] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.FIRQMODE] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_UNDEFINED2
        {
            get => _instance->md[(int)MDFlagMasks.MD_UNDEF2] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.MD_UNDEF2] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_UNDEFINED3
        {
            get => _instance->md[(int)MDFlagMasks.MD_UNDEF3] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.MD_UNDEF3] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_UNDEFINED4
        {
            get => _instance->md[(int)MDFlagMasks.MD_UNDEF4] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.MD_UNDEF4] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_UNDEFINED5
        {
            get => _instance->md[(int)MDFlagMasks.MD_UNDEF5] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.MD_UNDEF5] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_ILLEGAL
        {
            get => _instance->md[(int)MDFlagMasks.ILLEGAL] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.ILLEGAL] = value ? Define.TRUE : Define.FALSE;
        }
        public unsafe bool MD_ZERODIV
        {
            get => _instance->md[(int)MDFlagMasks.ZERODIV] == Define.TRUE;
            set => _instance->md[(int)MDFlagMasks.ZERODIV] = value ? Define.TRUE : Define.FALSE;
        }

        #endregion
    }
}
