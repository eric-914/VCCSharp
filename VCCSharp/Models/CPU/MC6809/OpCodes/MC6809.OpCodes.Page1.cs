using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable once InconsistentNaming
public partial class MC6809
{
    #region 0x00 - 0x0F

    public void Neg_D() // 00
    {
        _temp16 = DPADDRESS(PC_REG++);
        _postByte = MemRead8(_temp16);
        _temp8 = (byte)(0 - _postByte);

        CC_C = _temp8 > 0;
        CC_V = _postByte == 0x80;
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 01
    // 02
    public void Com_D() => Run(0x03);
    public void Lsr_D() => Run(0x04);
    // 05
    public void Ror_D() => Run(0x06);

    public void Asr_D() // 07
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;
        _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));
        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Asl_D() => Run(0x08);
    public void Rol_D() => Run(0x09);
    public void Dec_D() => Run(0x0A);
    // 0B
    public void Inc_D() => Run(0x0C);
    public void Tst_D() => Run(0x0D);
    public void Jmp_D() => Run(0x0E);
    public void Clr_D() => Run(0x0F);

    #endregion

    #region 0x10 - 0x1F

    public void Page_2() // 10
    {
        byte opCode = MemRead8(PC_REG++);

        _jumpVectors0x10[opCode]();
    }

    public void Page_3() // 11
    {
        byte opCode = MemRead8(PC_REG++);

        _jumpVectors0x11[opCode]();
    }

    public void Nop_I() => Run(0x12);

    public void Sync_I() // 13
    {
        _cycleCounter = SyncCycle;
        IsSyncWaiting = true;
    }

    // 14
    // 15
    public void Lbra_R() => Run(0x16);
    public void Lbsr_R() => Run(0x17);
    // 18

    public void Daa_I() // 19
    {
        byte msn, lsn;

        msn = (byte)(A_REG & 0xF0);
        lsn = (byte)(A_REG & 0xF);
        _temp8 = 0;

        if (CC_H || lsn > 9)
        {
            _temp8 |= 0x06;
        }

        if (msn > 0x80 && lsn > 9)
        {
            _temp8 |= 0x60;
        }

        if (msn > 0x90 || CC_C)
        {
            _temp8 |= 0x60;
        }

        _temp16 = (ushort)(A_REG + _temp8);
        CC_C |= (_temp16 & 0x100) >> 8 != 0;
        A_REG = (byte)_temp16;
        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 2;
    }

    public void Orcc_M() => Run(0x1A);
    // 1B
    public void Andcc_M() => Run(0x1C);

    public void Sex_I() // 1D
    {
        A_REG = (byte)(0 - (B_REG >> 7));
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 2;
    }

    public void Exg_M() => Run(0x1E);
    public void Tfr_M() => Run(0x1F);

    #endregion

    #region 0x20 - 0x2F

    public void Bra_R() => Run(0x20);
    public void Brn_R() => Run(0x21);
    public void Bhi_R() => Run(0x22);
    public void Bls_R() => Run(0x23);
    public void Bhs_R() => Run(0x24);
    public void Blo_R() => Run(0x25);
    public void Bne_R() => Run(0x26);
    public void Beq_R() => Run(0x27);
    public void Bvc_R() => Run(0x28);
    public void Bvs_R() => Run(0x29);
    public void Bpl_R() => Run(0x2A);
    public void Bmi_R() => Run(0x2B);
    public void Bge_R() => Run(0x2C);
    public void Blt_R() => Run(0x2D);
    public void Bgt_R() => Run(0x2E);
    public void Ble_R() => Run(0x2F);

    #endregion

    #region 0x30 - 0x3F

    public void Leax_X() => Run(0x30);
    public void Leay_X() => Run(0x31);
    public void Leas_X() => Run(0x32);

    public void Leau_X() // 33
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        U_REG = ea.CalculateEA(value);

        _cycleCounter += 4;
    }

    public void Pshs_M() => Run(0x34);
    public void Puls_M() => Run(0x35);
    public void Pshu_M() => Run(0x36);
    public void Pulu_M() => Run(0x37);
    // 38
    public void Rts_I() => Run(0x39);
    public void Abx_I() => Run(0x3A);
    public void Rti_I() => Run(0x3B);

    public void Cwai_I() // 3C
    {
        _postByte = MemRead8(PC_REG++);

        _cpu.cc.bits = GetCC();
        _cpu.cc.bits &= _postByte;

        _cycleCounter = SyncCycle;
        IsSyncWaiting = true;
    }

    public void Mul_I() => Run(0x3D);
    // 3E   //RESET - Undocumented

    public void Swi1_I() // 3F
    {
        CC_E = true; //1;

        MemWrite8(PC_L, --S_REG);
        MemWrite8(PC_H, --S_REG);
        MemWrite8(U_L, --S_REG);
        MemWrite8(U_H, --S_REG);
        MemWrite8(Y_L, --S_REG);
        MemWrite8(Y_H, --S_REG);
        MemWrite8(X_L, --S_REG);
        MemWrite8(X_H, --S_REG);
        MemWrite8(DPA, --S_REG);
        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);

        MemWrite8(GetCC(), --S_REG);

        PC_REG = MemRead16(Define.VSWI);

        _cycleCounter += 19;

        CC_I = true;
        CC_F = true;
    }

    #endregion

    #region 0x40 - 0x4F

    public void Nega_I() => Run(0x40);
    // 41
    // 42
    public void Coma_I() => Run(0x43);
    public void Lsra_I() => Run(0x44);
    // 45
    public void Rora_I() => Run(0x46);

    public void Asra_I() // 47
    {
        CC_C = (A_REG & 1) != 0;

        A_REG = (byte)((A_REG & 0x80) | (A_REG >> 1));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Asla_I() => Run(0x48);
    public void Rola_I() => Run(0x49);
    public void Deca_I() => Run(0x4A);
    // 4B
    public void Inca_I() => Run(0x4C);
    public void Tsta_I() => Run(0x4D);
    // 4E
    public void Clra_I() => Run(0x4F);

    #endregion

    #region 0x50 - 0x5F

    public void Negb_I() => Run(0x50);
    // 51
    // 52

    public void Comb_I() // 53
    {
        B_REG = (byte)(0xFF - B_REG);

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_C = true;
        CC_V = false;

        _cycleCounter += 2;
    }

    public void Lsrb_I() // 54
    {
        CC_C = (B_REG & 1) != 0;

        B_REG = (byte)(B_REG >> 1);

        CC_Z = ZTEST(B_REG);
        CC_N = false;

        _cycleCounter += 2;
    }

    // 55
    public void Rorb_I() => Run(0x56);
    public void Asrb_I() => Run(0x57);
    public void Aslb_I() => Run(0x58);
    public void Rolb_I() => Run(0x59);
    public void Decb_I() => Run(0x5A);
    // 5B
    public void Incb_I() => Run(0x5C);
    public void Tstb_I() => Run(0x5D);
    // 5E
    public void Clrb_I() => Run(0x5F);

    #endregion

    #region 0x60 - 0x6F

    public void Neg_X() // 60
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _postByte = MemRead8(_temp16);
        _temp8 = (byte)(0 - _postByte);

        CC_C = _temp8 > 0;
        CC_V = _postByte == 0x80;
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 61
    // 62

    public void Com_X() // 63
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _temp8 = MemRead8(_temp16);
        _temp8 = (byte)(0xFF - _temp8);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = false;
        CC_C = true;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Lsr_X() => Run(0x64);
    // 65

    public void Ror_X() // 66
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _temp8 = MemRead8(_temp16);
        _postByte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)((_temp8 >> 1) | _postByte);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Asr_X() // 67
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Asl_X() // 68
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _temp8 = MemRead8(_temp16);

        CC_C = _temp8 > 0x7F;
        CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);

        _temp8 = (byte)(_temp8 << 1);

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Rol_X() => Run(0x69);

    public void Dec_X() // 6A
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        _temp8 = MemRead8(_temp16);
        _temp8--;

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = (_temp8 == 0x7F);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 6B
    public void Inc_X() => Run(0x6C);
    public void Tst_X() => Run(0x6D);
    public void Jmp_X() => Run(0x6E);
    public void Clr_X() => Run(0x6F);

    #endregion

    #region 0x70 - 0x7F

    public void Neg_E() // 70
    {
        _temp16 = MemRead16(PC_REG);
        _postByte = MemRead8(_temp16);
        _temp8 = (byte)(0 - _postByte);

        CC_C = _temp8 > 0;
        CC_V = _postByte == 0x80;
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    // 71
    // 72

    public void Com_E() // 73
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);
        _temp8 = (byte)(0xFF - _temp8);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_C = true;
        CC_V = false;

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Lsr_E() // 74
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)(_temp8 >> 1);

        CC_Z = ZTEST(_temp8);
        CC_N = false;

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    // 75

    public void Ror_E() // 76
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);
        _postByte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)((_temp8 >> 1) | _postByte);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Asr_E() // 77
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Asl_E() // 78
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);

        CC_C = _temp8 > 0x7F;
        CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);

        _temp8 = (byte)(_temp8 << 1);

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Rol_E() // 79
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);
        _postByte = CC_C ? (byte)1 : (byte)0;

        CC_C = _temp8 > 0x7F;
        CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);

        _temp8 = (byte)((_temp8 << 1) | _postByte);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Dec_E() // 7A
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);
        _temp8--;

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = _temp8 == 0x7F;

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    // 7B

    public void Inc_E() // 7C
    {
        _temp16 = MemRead16(PC_REG);
        _temp8 = MemRead8(_temp16);
        _temp8++;

        CC_Z = ZTEST(_temp8);
        CC_V = _temp8 == 0x80;
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Tst_E() // 7D
    {
        _temp8 = MemRead8(MemRead16(PC_REG));

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Jmp_E() => Run(0x7E);
    public void Clr_E() => Run(0x7F);

    #endregion

    #region 0x80 - 0x8F

    public void Suba_M() => Run(0x80);
    public void Cmpa_M() => Run(0x81);

    public void Sbca_M() // 82
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(A_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 2;
    }

    public void Subd_M() => Run(0x83);
    public void Anda_M() => Run(0x84);

    public void Bita_M() // 85
    {
        _temp8 = (byte)(A_REG & MemRead8(PC_REG++));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        _cycleCounter += 2;
    }

    public void Lda_M() => Run(0x86);
    // 87
    public void Eora_M() => Run(0x88);

    public void Adca_M() // 89
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(A_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);
        CC_H = ((A_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 2;
    }

    public void Ora_M() => Run(0x8A);
    public void Adda_M() => Run(0x8B);
    public void Cmpx_M() => Run(0x8C);
    public void Bsr_R() => Run(0x8D);
    public void Ldx_M() => Run(0x8E);
    // 8F

    #endregion

    #region 0x90 - 0x9F

    public void Suba_D() => Run(0x90);
    public void Cmpa_D() => Run(0x91);

    public void Scba_D() // 92
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(A_REG - _postByte - (CC_C ? (byte)1 : (byte)0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Subd_D() // 93
    {
        _temp16 = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG - _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 6;
    }

    public void Anda_D() => Run(0x94);

    public void Bita_D() // 95
    {
        _temp8 = (byte)(A_REG & MemRead8(DPADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Lda_D() => Run(0x96);
    public void Sta_D() => Run(0x97);

    public void Eora_D() // 98
    {
        A_REG ^= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Adca_D() // 99
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(A_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);
        CC_H = ((A_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Ora_D() // 9A
    {
        A_REG |= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Adda_D() // 9B
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(A_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Cmpx_D() => Run(0x9C);
    public void Jsr_D() => Run(0x9D);
    public void Ldx_D() => Run(0x9E);
    public void Stx_D() => Run(0x9F);

    #endregion

    #region 0xA0 - 0xAF

    public void Suba_X() => Run(0xA0);
    public void Cmpa_X() => Run(0xA1);

    public void Sbca_X() // A2
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);
        _temp16 = (ushort)(A_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Subd_X() // A3
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _temp16 = MemRead16(address);
        _temp32 = (uint)(D_REG - _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 6;
    }

    public void Anda_X() => Run(0xA4);

    public void Bita_X() // A5
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _temp8 = (byte)(A_REG & MemRead8(address));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Lda_X() => Run(0xA6);
    public void Sta_X() => Run(0xA7);
    public void Eora_X() => Run(0xA8);

    public void Adca_X() // A9
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);
        _temp16 = (ushort)(A_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);
        CC_H = ((A_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Ora_X() // AA
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        A_REG |= MemRead8(address);

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Adda_X() // AB
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);
        _temp16 = (ushort)(A_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 4;
    }

    public void Cmpx_X() // AC
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postWord = MemRead16(address);
        _temp16 = (ushort)(X_REG - _postWord);
        CC_C = _temp16 > X_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, X_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 6;
    }

    public void Jsr_X() // AD
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        _temp16 = ea.CalculateEA(value);

        S_REG--;

        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);

        PC_REG = _temp16;

        _cycleCounter += 7;
    }

    public void Ldx_X() => Run(0xAE);
    public void Stx_X() => Run(0xAF);

    #endregion

    #region 0xB0 - 0xBF

    public void Suba_E() // B0
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(A_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Cmpa_E() // B1
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp8 = (byte)(A_REG - _postByte);

        CC_C = _temp8 > A_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, A_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Sbca_E() // B2
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(A_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Subd_E() // B3
    {
        _temp16 = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(D_REG - _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Anda_E() // B4
    {
        _postByte = MemRead8(MemRead16(PC_REG));

        A_REG &= _postByte;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Bita_E() // B5
    {
        _temp8 = (byte)(A_REG & MemRead8(MemRead16(PC_REG)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Lda_E() => Run(0xB6);
    public void Sta_E() => Run(0xB7);

    public void Eora_E() // B8
    {
        A_REG ^= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Adca_E() // B9
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(A_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);
        CC_H = ((A_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Ora_E() => Run(0xBA);

    public void Adda_E() // BB
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(A_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Cmpx_E() => Run(0xBC);
    public void Jsr_E() => Run(0xBD);
    public void Ldx_E() => Run(0xBE);
    public void Stx_E() => Run(0xBF);

    #endregion

    #region 0xC0 - 0CF

    public void Subb_M() => Run(0xC0);
    public void Cmpb_M() => Run(0xC1);

    public void Sbcb_M() // C2
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(B_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 2;
    }

    public void Addd_M() => Run(0xC3);
    public void Andb_M() => Run(0xC4);
    public void Bitb_M() => Run(0xC5);
    public void Ldb_M() => Run(0xC6);
    // C7

    public void Eorb_M() // C8
    {
        B_REG ^= MemRead8(PC_REG++);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 2;
    }

    public void Adcb_M() // C9
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(B_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);
        CC_H = ((B_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 2;
    }

    public void Orb_M() => Run(0xCA);
    public void Addb_M() => Run(0xCB);
    public void Ldd_M() => Run(0xCC);
    // CD
    public void Ldu_M() => Run(0xCE);
    // CF

    #endregion

    #region 0xD0 - 0xDF

    public void Subb_D() => Run(0xD0);

    public void Cmpb_D() // D1
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp8 = (byte)(B_REG - _postByte);

        CC_C = _temp8 > B_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, B_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 4;
    }

    public void Sbcb_D() // D2
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Addd_D() => Run(0xD3);

    public void Andb_D() // D4
    {
        B_REG &= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Bitb_D() // D5
    {
        _temp8 = (byte)(B_REG & MemRead8(DPADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Ldb_D() => Run(0xD6);
    public void Stb_D() => Run(0xD7);
    public void Eorb_D() => Run(0xD8);

    public void Adcb_D() // D9
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);
        CC_H = ((B_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Orb_D() // DA
    {
        B_REG |= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Addb_D() // DB
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((B_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Ldd_D() => Run(0xDC);
    public void Std_D() => Run(0xDD);
    public void Ldu_D() => Run(0xDE);
    public void Stu_D() => Run(0xDF);

    #endregion

    #region 0xE0 - 0xEF

    public void Subb_X() // E0
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);

        _temp16 = (ushort)(B_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 4;
    }

    public void Cmpb_X() => Run(0xE1);

    public void Sbcb_X() // E2
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);

        _temp16 = (ushort)(B_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Addd_X() // E3
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _temp16 = MemRead16(address);

        _temp32 = (uint)(D_REG + _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 6;
    }

    public void Andb_X() // E4
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        B_REG &= MemRead8(address);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Bitb_X() // E5
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _temp8 = (byte)(B_REG & MemRead8(address));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Ldb_X() => Run(0xE6);
    public void Stb_X() => Run(0xE7);

    public void Eorb_X() // E8
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        B_REG ^= MemRead8(address);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Adcb_X() // E9
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        _postByte = MemRead8(address);

        _temp16 = (ushort)(B_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);
        CC_H = ((B_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Orb_X() // EA
    {
        var ea = ((ITempAccess)OpCodes).EA;

        byte value = MemRead8(PC_REG++);

        ushort address = ea.CalculateEA(value);

        B_REG |= MemRead8(address);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        _cycleCounter += 4;
    }

    public void Addb_X() => Run(0xEB);
    public void Ldd_X() => Run(0xEC);
    public void Std_X() => Run(0xED);
    public void Ldu_X() => Run(0xEE);
    public void Stu_X() => Run(0xEF);

    #endregion

    #region 0xF0 - 0xFF

    public void Subb_E() // F0
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(B_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Cmpb_E() // F1
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp8 = (byte)(B_REG - _postByte);

        CC_C = _temp8 > B_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, B_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Sbcb_E() // F2
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(B_REG - _postByte - (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Addd_E() // F3
    {
        _temp16 = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(D_REG + _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Andb_E() // F4
    {
        B_REG &= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Bitb_E() // F5
    {
        _temp8 = (byte)(B_REG & MemRead8(MemRead16(PC_REG)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Ldb_E() => Run(0xF6);
    public void Stb_E() => Run(0xF7);

    public void Eorb_E() // F8
    {
        B_REG ^= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Adcb_E() // F9
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(B_REG + _postByte + (CC_C ? 1 : 0));

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);
        CC_H = ((B_REG ^ _temp16 ^ _postByte) & 0x10) >> 4 != 0;

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Orb_E() // FA
    {
        B_REG |= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Addb_E() // FB
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(B_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((B_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Ldd_E() => Run(0xFC);
    public void Std_E() => Run(0xFD);

    public void Ldu_E() // FE
    {
        U_REG = MemRead16(MemRead16(PC_REG));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    public void Stu_E() // FF
    {
        MemWrite16(U_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    #endregion
}
