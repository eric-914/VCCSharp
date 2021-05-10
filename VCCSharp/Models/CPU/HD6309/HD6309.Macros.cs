﻿using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    partial class HD6309
    {
        #region CC Masks Macros

        public bool CC_E
        {
            get => _cpu.cc[(int)CCFlagMasks.E] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.E] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_F
        {
            get => _cpu.cc[(int)CCFlagMasks.F] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.F] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_H
        {
            get => _cpu.cc[(int)CCFlagMasks.H] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.H] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_I
        {
            get => _cpu.cc[(int)CCFlagMasks.I] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.I] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_N
        {
            get => _cpu.cc[(int)CCFlagMasks.N] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.N] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_Z
        {
            get => _cpu.cc[(int)CCFlagMasks.Z] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.Z] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_V
        {
            get => _cpu.cc[(int)CCFlagMasks.V] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.V] = value ? Define.TRUE : Define.FALSE;
        }

        public bool CC_C
        {
            get => _cpu.cc[(int)CCFlagMasks.C] == Define.TRUE;
            set => _cpu.cc[(int)CCFlagMasks.C] = value ? Define.TRUE : Define.FALSE;
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

        public byte S_L
        {
            get => _cpu.s.lsb;
            set => _cpu.s.lsb = value;
        }

        public byte S_H
        {
            get => _cpu.s.msb;
            set => _cpu.s.msb = value;
        }

        public ushort U_REG
        {
            get => _cpu.u.Reg;
            set => _cpu.u.Reg = value;
        }

        public byte PC_L
        {
            get => _cpu.pc.lsb;
            set => _cpu.pc.lsb = value;
        }

        public byte PC_H
        {
            get => _cpu.pc.msb;
            set => _cpu.pc.msb = value;
        }

        public ushort X_REG
        {
            get => _cpu.x.Reg;
            set => _cpu.x.Reg = value;
        }

        public byte X_L
        {
            get => _cpu.x.lsb;
            set => _cpu.x.lsb = value;
        }

        public byte X_H
        {
            get => _cpu.x.msb;
            set => _cpu.x.msb = value;
        }

        public ushort Y_REG
        {
            get => _cpu.y.Reg;
            set => _cpu.y.Reg = value;
        }

        public byte Y_L
        {
            get => _cpu.y.lsb;
            set => _cpu.y.lsb = value;
        }

        public byte Y_H
        {
            get => _cpu.y.msb;
            set => _cpu.y.msb = value;
        }

        public byte U_L
        {
            get => _cpu.u.lsb;
            set => _cpu.u.lsb = value;
        }

        public byte U_H
        {
            get => _cpu.u.msb;
            set => _cpu.u.msb = value;
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

        public ushort O_REG
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

        #region Macros

        public byte PUR(int i) => _cpu.ureg8[i];
        public void PUR(int i, byte value) => _cpu.ureg8[i] = value;

        public ushort PXF(int i) => _cpu.xfreg16[i];
        public void PXF(int i, ushort value) => _cpu.xfreg16[i] = value;

        public ushort DPADDRESS(ushort r) => (ushort)(_cpu.dp.Reg | MemRead8(r));

        public byte MemRead8(ushort address) => _modules.TC1014.MemRead8(address);
        public void MemWrite8(byte data, ushort address) => _modules.TC1014.MemWrite8(data, address);
        public ushort MemRead16(ushort address) => _modules.TC1014.MemRead16(address);
        public void MemWrite16(ushort data, ushort address) => _modules.TC1014.MemWrite16(data, address);
        public uint MemRead32(ushort address) => _modules.TC1014.MemRead32(address);
        public void MemWrite32(uint data, ushort address) => _modules.TC1014.MemWrite32(data, address);

        public ushort INDADDRESS(ushort address) => CalculateEA(MemRead8(address));

        public bool NTEST8(byte value) => value > 0x7F;
        public bool NTEST16(ushort value) => value > 0x7FFF;
        public bool NTEST32(uint value) => value > 0x7FFFFFFF;

        public bool ZTEST(byte value) => value == 0;
        public bool ZTEST(ushort value) => value == 0;
        public bool ZTEST(uint value) => value == 0;

        public bool OVERFLOW8(bool c, byte a, ushort b, byte r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 7) & 1)) != 0;
        public bool OVERFLOW16(bool c, uint a, ushort b, ushort r) => ((c ? (byte)1 : (byte)0) ^ (((a ^ b ^ r) >> 15) & 1)) != 0;

        #region MD Masks Macros

        public bool MD_NATIVE6309
        {
            get => _cpu.md[(int)MDFlagMasks.NATIVE6309] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.NATIVE6309] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_FIRQMODE
        {
            get => _cpu.md[(int)MDFlagMasks.FIRQMODE] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.FIRQMODE] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_UNDEFINED2
        {
            get => _cpu.md[(int)MDFlagMasks.MD_UNDEF2] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.MD_UNDEF2] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_UNDEFINED3
        {
            get => _cpu.md[(int)MDFlagMasks.MD_UNDEF3] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.MD_UNDEF3] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_UNDEFINED4
        {
            get => _cpu.md[(int)MDFlagMasks.MD_UNDEF4] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.MD_UNDEF4] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_UNDEFINED5
        {
            get => _cpu.md[(int)MDFlagMasks.MD_UNDEF5] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.MD_UNDEF5] = value ? Define.TRUE : Define.FALSE;
        }
        
        public bool MD_ILLEGAL
        {
            get => _cpu.md[(int)MDFlagMasks.ILLEGAL] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.ILLEGAL] = value ? Define.TRUE : Define.FALSE;
        }

        public bool MD_ZERODIV
        {
            get => _cpu.md[(int)MDFlagMasks.ZERODIV] == Define.TRUE;
            set => _cpu.md[(int)MDFlagMasks.ZERODIV] = value ? Define.TRUE : Define.FALSE;
        }

        #endregion

        #endregion
    }
}