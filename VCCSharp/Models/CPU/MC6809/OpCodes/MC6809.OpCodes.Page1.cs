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

    public void Com_D() // 03
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _temp8 = (byte)(0xFF - _temp8);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_C = true; //1;
        CC_V = false; //0;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Lsr_D() // 04
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;
        _temp8 >>= 1;
        CC_Z = ZTEST(_temp8);
        CC_N = false;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 05

    public void Ror_D() // 06
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _postByte = (byte)((CC_C ? 1 : 0) << 7);

        CC_C = (_temp8 & 1) != 0;
        _temp8 = (byte)((_temp8 >> 1) | _postByte);
        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

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

    public void Asl_D() // 08
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 0x80) >> 7 != 0;
        CC_V = ((CC_C ? 1 : 0) ^ ((_temp8 & 0x40) >> 6)) != 0;
        _temp8 <<= 1;
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Rol_D() // 09
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _postByte = (byte)(CC_C ? 1 : 0);

        CC_C = (_temp8 & 0x80) >> 7 != 0;
        CC_V = ((CC_C ? 1 : 0) ^ ((_temp8 & 0x40) >> 6)) != 0;
        _temp8 = (byte)((_temp8 << 1) | _postByte);
        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Dec_D() // 0A
    {
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = (byte)(MemRead8(_temp16) - 1);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = _temp8 == 0x7F;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 0B

    public void Inc_D() // 0C
    {
        _temp16 = (DPADDRESS(PC_REG++));
        _temp8 = (byte)(MemRead8(_temp16) + 1);

        CC_Z = ZTEST(_temp8);
        CC_V = _temp8 == 0x80;
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Tst_D() // 0D
    {
        _temp8 = MemRead8(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = false;

        _cycleCounter += 6;
    }

    public void Jmp_D() // 0E
    {
        PC_REG = (ushort)(DP_REG | MemRead8(PC_REG));

        _cycleCounter += 3;
    }

    public void Clr_D() // 0F
    {
        MemWrite8(0, DPADDRESS(PC_REG++));

        CC_Z = true;
        CC_N = false;
        CC_V = false;
        CC_C = false;

        _cycleCounter += 6;
    }

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

    public void Nop_I() // 12
    {
        _cycleCounter += 2;
    }

    public void Sync_I() // 13
    {
        _cycleCounter = _gCycleFor;
        _syncWaiting = 1;
    }

    // 14       //InvalidInsHandler
    // 15		//InvalidInsHandler

    public void Lbra_R() // 16
    {
        _postWord = MemRead16(PC_REG);
        PC_REG += 2;
        PC_REG += _postWord;

        _cycleCounter += 5;
    }

    public void Lbsr_R() // 17
    {
        _postWord = MemRead16(PC_REG);
        PC_REG += 2;
        S_REG--;
        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);
        PC_REG += _postWord;

        _cycleCounter += 9;
    }

    // 18		//InvalidInsHandler

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

    public void Orcc_M() // 1A
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = GetCC();
        _temp8 = (byte)(_temp8 | _postByte);

        _cpu.cc.bits = _temp8;

        _cycleCounter += 3;
    }

    // 1B		//InvalidInsHandler

    public void Andcc_M() // 1C
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = GetCC();
        _temp8 = (byte)(_temp8 & _postByte);

        _cpu.cc.bits = _temp8;

        _cycleCounter += 3;
    }

    public void Sex_I() // 1D
    {
        A_REG = (byte)(0 - (B_REG >> 7));
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 2;
    }

    public void Exg_M() // 1E
    {
        _postByte = MemRead8(PC_REG++);

        _cpu.cc.bits = GetCC();

        if (((_postByte & 0x80) >> 4) == (_postByte & 0x08)) //Verify like size registers
        {
            if ((_postByte & 0x08) != 0) //8 bit EXG
            {
                _temp8 = PUR(((_postByte & 0x70) >> 4));
                PUR(((_postByte & 0x70) >> 4), PUR(_postByte & 0x07));
                PUR(_postByte & 0x07, _temp8);
            }
            else // 16 bit EXG
            {
                _temp16 = PXF((_postByte & 0x70) >> 4);
                PXF((_postByte & 0x70) >> 4, PXF(_postByte & 0x07));
                PXF(_postByte & 0x07, _temp16);
            }
        }

        _cycleCounter += 8;
    }

    public void Tfr_M() // 1F
    {
        _postByte = MemRead8(PC_REG++);

        byte source = (byte)(_postByte >> 4);
        byte dest = (byte)(_postByte & 15);

        switch (dest)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
            case 5:
            case 6:
            case 7:
                PXF(dest, 0xFFFF);

                if (source == 12 || source == 13)
                {
                    PXF(dest, 0);
                }
                else if (source <= 7)
                {
                    PXF(dest, PXF(source));
                }

                break;

            case 8:
            case 9:
            case 10:
            case 11:
            case 14:
            case 15:
                _cpu.cc.bits = GetCC();

                PUR(dest & 7, 0xFF);

                if ((source == 12) || (source == 13))
                {
                    PUR(dest & 7, 0);
                }
                else if (source > 7)
                {
                    PUR(dest & 7, PUR(source & 7));
                }

                break;
        }

        _cycleCounter += 6;
    }

    #endregion

    #region 0x20 - 0x2F

    public void Bra_R() // 20
    {
        sbyte t = (sbyte)MemRead8(PC_REG++);

        PC_REG += (ushort)t;

        _cycleCounter += 3;
    }

    public void Brn_R() // 21
    {
        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bhi_R() // 22
    {
        if (!(CC_C || CC_Z))
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bls_R() // 23
    {
        if (CC_C | CC_Z)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bhs_R() // 24
    {
        if (!CC_C)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Blo_R() // 25
    {
        if (CC_C)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bne_R() // 26
    {
        if (!CC_Z)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Beq_R() // 27
    {
        if (CC_Z)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bvc_R() // 28
    {
        if (!CC_V)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bvs_R() // 29
    {
        if (CC_V)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bpl_R() // 2A
    {
        if (!CC_N)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bmi_R() // 2B
    {
        if (CC_N)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bge_R() // 2C
    {
        if (!(CC_N ^ CC_V))
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Blt_R() // 2D
    {
        if (CC_V ^ CC_N)
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Bgt_R() // 2E
    {
        if (!(CC_Z | (CC_N ^ CC_V)))
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    public void Ble_R() // 2F
    {
        if (CC_Z | (CC_N ^ CC_V))
        {
            PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
        }

        PC_REG++;

        _cycleCounter += 3;
    }

    #endregion

    #region 0x30 - 0x3F

    public void Leax_X() // 30
    {
        X_REG = INDADDRESS(PC_REG++);

        CC_Z = ZTEST(X_REG);

        _cycleCounter += 4;
    }

    public void Leay_X() // 31
    {
        Y_REG = INDADDRESS(PC_REG++);

        CC_Z = ZTEST(Y_REG);

        _cycleCounter += 4;
    }

    public void Leas_X() // 32
    {
        S_REG = INDADDRESS(PC_REG++);

        _cycleCounter += 4;
    }

    public void Leau_X() // 33
    {
        U_REG = INDADDRESS(PC_REG++);

        _cycleCounter += 4;
    }

    public void Pshs_M() // 34
    {
        _postByte = MemRead8(PC_REG++);

        if ((_postByte & 0x80) != 0)
        {
            MemWrite8(PC_L, --S_REG);
            MemWrite8(PC_H, --S_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x40) != 0)
        {
            MemWrite8(U_L, --S_REG);
            MemWrite8(U_H, --S_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x20) != 0)
        {
            MemWrite8(Y_L, --S_REG);
            MemWrite8(Y_H, --S_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x10) != 0)
        {
            MemWrite8(X_L, --S_REG);
            MemWrite8(X_H, --S_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x08) != 0)
        {
            MemWrite8(DPA, --S_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x04) != 0)
        {
            MemWrite8(B_REG, --S_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x02) != 0)
        {
            MemWrite8(A_REG, --S_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x01) != 0)
        {
            MemWrite8(GetCC(), --S_REG);

            _cycleCounter += 1;
        }

        _cycleCounter += 5;
    }

    public void Puls_M() // 35
    {
        _postByte = MemRead8(PC_REG++);

        if ((_postByte & 0x01) != 0)
        {
            _cpu.cc.bits = MemRead8(S_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x02) != 0)
        {
            A_REG = MemRead8(S_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x04) != 0)
        {
            B_REG = MemRead8(S_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x08) != 0)
        {
            DPA = MemRead8(S_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x10) != 0)
        {
            X_H = MemRead8(S_REG++);
            X_L = MemRead8(S_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x20) != 0)
        {
            Y_H = MemRead8(S_REG++);
            Y_L = MemRead8(S_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x40) != 0)
        {
            U_H = MemRead8(S_REG++);
            U_L = MemRead8(S_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x80) != 0)
        {
            PC_H = MemRead8(S_REG++);
            PC_L = MemRead8(S_REG++);

            _cycleCounter += 2;
        }

        _cycleCounter += 5;
    }

    public void Pshu_M() // 36
    {
        _postByte = MemRead8(PC_REG++);

        if ((_postByte & 0x80) != 0)
        {
            MemWrite8(PC_L, --U_REG);
            MemWrite8(PC_H, --U_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x40) != 0)
        {
            MemWrite8(S_L, --U_REG);
            MemWrite8(S_H, --U_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x20) != 0)
        {
            MemWrite8(Y_L, --U_REG);
            MemWrite8(Y_H, --U_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x10) != 0)
        {
            MemWrite8(X_L, --U_REG);
            MemWrite8(X_H, --U_REG);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x08) != 0)
        {
            MemWrite8(DPA, --U_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x04) != 0)
        {
            MemWrite8(B_REG, --U_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x02) != 0)
        {
            MemWrite8(A_REG, --U_REG);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x01) != 0)
        {
            MemWrite8(GetCC(), --U_REG);

            _cycleCounter += 1;
        }

        _cycleCounter += 5;
    }

    public void Pulu_M() // 37
    {
        _postByte = MemRead8(PC_REG++);

        if ((_postByte & 0x01) != 0)
        {
            _cpu.cc.bits = MemRead8(U_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x02) != 0)
        {
            A_REG = MemRead8(U_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x04) != 0)
        {
            B_REG = MemRead8(U_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x08) != 0)
        {
            DPA = MemRead8(U_REG++);

            _cycleCounter += 1;
        }

        if ((_postByte & 0x10) != 0)
        {
            X_H = MemRead8(U_REG++);
            X_L = MemRead8(U_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x20) != 0)
        {
            Y_H = MemRead8(U_REG++);
            Y_L = MemRead8(U_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x40) != 0)
        {
            S_H = MemRead8(U_REG++);
            S_L = MemRead8(U_REG++);

            _cycleCounter += 2;
        }

        if ((_postByte & 0x80) != 0)
        {
            PC_H = MemRead8(U_REG++);
            PC_L = MemRead8(U_REG++);

            _cycleCounter += 2;
        }

        _cycleCounter += 5;
    }

    // 38		//InvalidInsHandler

    public void Rts_I() // 39
    {
        PC_H = MemRead8(S_REG++);
        PC_L = MemRead8(S_REG++);

        _cycleCounter += 5;
    }

    public void Abx_I() // 3A
    {
        X_REG += B_REG;

        _cycleCounter += 3;
    }

    public void Rti_I() // 3B
    {
        _cpu.cc.bits = MemRead8(S_REG++);

        _cycleCounter += 6;
        _inInterrupt = 0;

        if (CC_E)
        {
            A_REG = MemRead8(S_REG++);
            B_REG = MemRead8(S_REG++);
            DPA = MemRead8(S_REG++);
            X_H = MemRead8(S_REG++);
            X_L = MemRead8(S_REG++);
            Y_H = MemRead8(S_REG++);
            Y_L = MemRead8(S_REG++);
            U_H = MemRead8(S_REG++);
            U_L = MemRead8(S_REG++);

            _cycleCounter += 9;
        }

        PC_H = MemRead8(S_REG++);
        PC_L = MemRead8(S_REG++);
    }

    public void Cwai_I() // 3C
    {
        _postByte = MemRead8(PC_REG++);

        _cpu.cc.bits = GetCC();
        _cpu.cc.bits &= _postByte;

        _cycleCounter = _gCycleFor;
        _syncWaiting = 1;
    }

    public void Mul_I() // 3D
    {
        D_REG = (ushort)(A_REG * B_REG);
        CC_C = B_REG > 0x7F;
        CC_Z = ZTEST(D_REG);

        _cycleCounter += 11;
    }

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

        CC_I = true; //1;
        CC_F = true; //1;
    }

    #endregion

    #region 0x40 - 0x4F

    public void Nega_I() // 40
    {
        _temp8 = (byte)(0 - A_REG);

        CC_C = _temp8 > 0;
        CC_V = A_REG == 0x80; //CC_C ^ ((A_REG^temp8)>>7);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        A_REG = _temp8;

        _cycleCounter += 2;
    }

    // 41		//InvalidInsHandler

    // 42		//InvalidInsHandler

    public void Coma_I() // 43
    {
        A_REG = (byte)(0xFF - A_REG);

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_C = true; //1;
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Lsra_I() // 44
    {
        CC_C = (A_REG & 1) != 0;

        A_REG = (byte)(A_REG >> 1);

        CC_Z = ZTEST(A_REG);
        CC_N = false; //0;

        _cycleCounter += 2;
    }

    // 45		//InvalidInsHandler

    public void Rora_I() // 46
    {
        _postByte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

        CC_C = (A_REG & 1) != 0;

        A_REG = (byte)((A_REG >> 1) | _postByte);

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Asra_I() // 47
    {
        CC_C = (A_REG & 1) != 0;

        A_REG = (byte)((A_REG & 0x80) | (A_REG >> 1));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Asla_I() // 48
    {
        CC_C = A_REG > 0x7F;
        CC_V = CC_C ^ ((A_REG & 0x40) >> 6 != 0);

        A_REG = (byte)(A_REG << 1);

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 2;
    }

    public void Rola_I() // 49
    {
        _postByte = CC_C ? (byte)1 : (byte)0;

        CC_C = A_REG > 0x7F;
        CC_V = CC_C ^ ((A_REG & 0x40) >> 6 != 0);

        A_REG = (byte)((A_REG << 1) | _postByte);

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Deca_I() // 4A
    {
        A_REG--;

        CC_Z = ZTEST(A_REG);
        CC_V = A_REG == 0x7F;
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    // 4B		//InvalidInsHandler

    public void Inca_I() // 4C
    {
        A_REG++;

        CC_Z = ZTEST(A_REG);
        CC_V = A_REG == 0x80;
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Tsta_I() // 4D
    {
        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    // 4E		//InvalidInsHandler

    public void Clra_I() // 4F
    {
        A_REG = 0;

        CC_C = false; //0;
        CC_V = false; //0;
        CC_N = false; //0;
        CC_Z = true; //1;

        _cycleCounter += 2;
    }

    #endregion

    #region 0x50 - 0x5F

    public void Negb_I() // 50
    {
        _temp8 = (byte)(0 - B_REG);

        CC_C = _temp8 > 0;
        CC_V = B_REG == 0x80;
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        B_REG = _temp8;

        _cycleCounter += 2;
    }

    // 51		//InvalidInsHandler
    // 52		//InvalidInsHandler

    public void Comb_I() // 53
    {
        B_REG = (byte)(0xFF - B_REG);

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_C = true; //1;
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Lsrb_I() // 54
    {
        CC_C = (B_REG & 1) != 0;

        B_REG = (byte)(B_REG >> 1);

        CC_Z = ZTEST(B_REG);
        CC_N = false; //0;

        _cycleCounter += 2;
    }

    // 55		//InvalidInsHandler

    public void Rorb_I() // 56
    {
        _postByte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

        CC_C = (B_REG & 1) != 0;

        B_REG = (byte)((B_REG >> 1) | _postByte);

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    public void Asrb_I() // 57
    {
        CC_C = (B_REG & 1) != 0;

        B_REG = (byte)((B_REG & 0x80) | (B_REG >> 1));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    public void Aslb_I() // 58
    {
        CC_C = B_REG > 0x7F;
        CC_V = CC_C ^ ((B_REG & 0x40) >> 6 != 0);

        B_REG = (byte)(B_REG << 1);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 2;
    }

    public void Rolb_I() // 59
    {
        _postByte = CC_C ? (byte)1 : (byte)0;

        CC_C = B_REG > 0x7F;
        CC_V = CC_C ^ ((B_REG & 0x40) >> 6 != 0);

        B_REG = (byte)((B_REG << 1) | _postByte);

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    public void Decb_I() // 5A
    {
        B_REG--;

        CC_Z = ZTEST(B_REG);
        CC_V = B_REG == 0x7F;
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    // 5B		//InvalidInsHandler

    public void Incb_I() // 5C
    {
        B_REG++;

        CC_Z = ZTEST(B_REG);
        CC_V = B_REG == 0x80;
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    public void Tstb_I() // 5D
    {
        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    // 5E		//InvalidInsHandler

    public void Clrb_I() // 5F
    {
        B_REG = 0;

        CC_C = false; //0;
        CC_N = false; //0;
        CC_V = false; //0;
        CC_Z = true; //1;

        _cycleCounter += 2;
    }

    #endregion

    #region 0x60 - 0x6F

    public void Neg_X() // 60
    {
        _temp16 = INDADDRESS(PC_REG++);
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
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _temp8 = (byte)(0xFF - _temp8);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = false; //0;
        CC_C = true; //1;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Lsr_X() // 64
    {
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);

        CC_C = (_temp8 & 1) != 0;

        _temp8 = (byte)(_temp8 >> 1);

        CC_Z = ZTEST(_temp8);
        CC_N = false; //0;

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 65

    public void Ror_X() // 66
    {
        _temp16 = INDADDRESS(PC_REG++);
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
        _temp16 = INDADDRESS(PC_REG++);
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
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);

        CC_C = _temp8 > 0x7F;
        CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);

        _temp8 = (byte)(_temp8 << 1);

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Rol_X() // 69
    {
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _postByte = CC_C ? (byte)1 : (byte)0;

        CC_C = _temp8 > 0x7F;
        CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);

        _temp8 = (byte)((_temp8 << 1) | _postByte);

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Dec_X() // 6A
    {
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _temp8--;

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = (_temp8 == 0x7F);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    // 6B

    public void Inc_X() // 6C
    {
        _temp16 = INDADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _temp8++;

        CC_V = (_temp8 == 0x80);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        MemWrite8(_temp8, _temp16);

        _cycleCounter += 6;
    }

    public void Tst_X() // 6D
    {
        _temp8 = MemRead8(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(_temp8);
        CC_N = NTEST8(_temp8);
        CC_V = false; //0;

        _cycleCounter += 6;
    }

    public void Jmp_X() // 6E
    {
        PC_REG = INDADDRESS(PC_REG++);

        _cycleCounter += 3;
    }

    public void Clr_X() // 6F
    {
        MemWrite8(0, INDADDRESS(PC_REG++));

        CC_C = false; //0;
        CC_N = false; //0;
        CC_V = false; //0;
        CC_Z = true; //1;

        _cycleCounter += 6;
    }

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
        CC_C = true; //1;
        CC_V = false; //0;

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
        CC_N = false; //0;

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
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Jmp_E() // 7E
    {
        PC_REG = MemRead16(PC_REG);

        _cycleCounter += 4;
    }

    public void Clr_E() // 7F
    {
        MemWrite8(0, MemRead16(PC_REG));

        CC_C = false; //0;
        CC_N = false; //0;
        CC_V = false; //0;
        CC_Z = true; //1;

        PC_REG += 2;

        _cycleCounter += 7;
    }

    #endregion

    #region 0x80 - 0x8F

    public void Suba_M() // 80
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(A_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 2;
    }

    public void Cmpa_M() // 81
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = (byte)(A_REG - _postByte);

        CC_C = _temp8 > A_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, A_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 2;
    }

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

    public void Subd_M() // 83
    {
        _temp16 = MemRead16(PC_REG);
        _temp32 = (uint)(D_REG - _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        PC_REG += 2;

        _cycleCounter += 4;
    }

    public void Anda_M() // 84
    {
        A_REG &= MemRead8(PC_REG++);

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Bita_M() // 85
    {
        _temp8 = (byte)(A_REG & MemRead8(PC_REG++));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Lda_M() // 86
    {
        A_REG = MemRead8(PC_REG++);

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    // 87		//InvalidInsHandler

    public void Eora_M() // 88
    {
        A_REG ^= MemRead8(PC_REG++);

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

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

    public void Ora_M() // 8A
    {
        A_REG |= MemRead8(PC_REG++);

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Adda_M() // 8B
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(A_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((A_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);

        _cycleCounter += 2;
    }

    public void Cmpx_M() // 8C
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(X_REG - _postWord);

        CC_C = _temp16 > X_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, X_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        PC_REG += 2;

        _cycleCounter += 4;
    }

    public void Bsr_R() // 8D
    {
        _postByte = MemRead8(PC_REG++);

        S_REG--;

        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);

        PC_REG += (ushort)(sbyte)(_postByte);

        _cycleCounter += 7;
    }

    public void Ldx_M() // 8E
    {
        X_REG = MemRead16(PC_REG);

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 3;
    }

    // 8F		//InvalidInsHandler

    #endregion

    #region 0x90 - 0x9F

    public void Suba_D() // 90
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(A_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 4;
    }

    public void Cmpa_D() // 91
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp8 = (byte)(A_REG - _postByte);

        CC_C = _temp8 > A_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, A_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 4;
    }

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

    public void Anda_D() // 94
    {
        A_REG &= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Bita_D() // 95
    {
        _temp8 = (byte)(A_REG & MemRead8(DPADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Lda_D() // 96
    {
        A_REG = MemRead8(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Sta_D() // 97
    {
        MemWrite8(A_REG, DPADDRESS(PC_REG++));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Eora_D() // 98
    {
        A_REG ^= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

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
        CC_V = false; //0;

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

    public void Cmpx_D() // 9C
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(X_REG - _postWord);

        CC_C = _temp16 > X_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, X_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 6;
    }

    public void Jsr_D() // 9D
    {
        _temp16 = DPADDRESS(PC_REG++);

        S_REG--;

        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);

        PC_REG = _temp16;

        _cycleCounter += 7;
    }

    public void Ldx_D() // 9E
    {
        X_REG = MemRead16(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    public void Stx_D() // 9F
    {
        MemWrite16(X_REG, DPADDRESS(PC_REG++));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    #endregion

    #region 0xA0 - 0xAF

    public void Suba_X() // A0
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(A_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, A_REG);

        A_REG = (byte)_temp16;

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);

        _cycleCounter += 4;
    }

    public void Cmpa_X() // A1
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp8 = (byte)(A_REG - _postByte);

        CC_C = _temp8 > A_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, A_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 4;
    }

    public void Sbca_X() // A2
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
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
        _temp16 = MemRead16(INDADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG - _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 6;
    }

    public void Anda_X() // A4
    {
        A_REG &= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Bita_X() // A5
    {
        _temp8 = (byte)(A_REG & MemRead8(INDADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Lda_X() // A6
    {
        A_REG = MemRead8(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Sta_X() // A7
    {
        MemWrite8(A_REG, INDADDRESS(PC_REG++));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Eora_X() // A8
    {
        A_REG ^= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Adca_X() // A9
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
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
        A_REG |= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Adda_X() // AB
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
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
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(X_REG - _postWord);
        CC_C = _temp16 > X_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, X_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        _cycleCounter += 6;
    }

    public void Jsr_X() // AD
    {
        _temp16 = INDADDRESS(PC_REG++);

        S_REG--;

        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);

        PC_REG = _temp16;

        _cycleCounter += 7;
    }

    public void Ldx_X() // AE
    {
        X_REG = MemRead16(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    public void Stx_X() // AF
    {
        MemWrite16(X_REG, INDADDRESS(PC_REG++));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

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
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Bita_E() // B5
    {
        _temp8 = (byte)(A_REG & MemRead8(MemRead16(PC_REG)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Lda_E() // B6
    {
        A_REG = MemRead8(MemRead16(PC_REG));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Sta_E() // B7
    {
        MemWrite8(A_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(A_REG);
        CC_N = NTEST8(A_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Eora_E() // B8
    {
        A_REG ^= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

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

    public void Ora_E() // BA
    {
        A_REG |= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(A_REG);
        CC_Z = ZTEST(A_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

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

    public void Cmpx_E() // BC
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(X_REG - _postWord);

        CC_C = _temp16 > X_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, X_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);

        PC_REG += 2;

        _cycleCounter += 7;
    }

    public void Bsr_E() // BD
    {
        _postWord = MemRead16(PC_REG);

        PC_REG += 2;

        S_REG--;

        MemWrite8(PC_L, S_REG--);
        MemWrite8(PC_H, S_REG);

        PC_REG = _postWord;

        _cycleCounter += 8;
    }

    public void Ldx_E() // BE
    {
        X_REG = MemRead16(MemRead16(PC_REG));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    public void Stx_E() // BF
    {
        MemWrite16(X_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(X_REG);
        CC_N = NTEST16(X_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    #endregion

    #region 0xC0 - 0CF

    public void Subb_M() // C0
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(B_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 2;
    }

    public void Cmpb_M() // C1
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = (byte)(B_REG - _postByte);

        CC_C = _temp8 > B_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, B_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 2;
    }

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

    public void Addd_M() // C3
    {
        _temp16 = MemRead16(PC_REG);
        _temp32 = (uint)(D_REG + _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        PC_REG += 2;

        _cycleCounter += 4;
    }

    public void Andb_M() // C4
    {
        B_REG &= MemRead8(PC_REG++);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Bitb_M() // C5
    {
        _temp8 = (byte)(B_REG & MemRead8(PC_REG++));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Ldb_M() // C6
    {
        B_REG = MemRead8(PC_REG++);

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    // C7	//InvalidInsHandler

    public void Eorb_M() // C8
    {
        B_REG ^= MemRead8(PC_REG++);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

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

    public void Orb_M() // CA
    {
        B_REG |= MemRead8(PC_REG++);

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

        _cycleCounter += 2;
    }

    public void Addb_M() // CB
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(B_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((B_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 2;
    }

    public void Ldd_M() // CC
    {
        D_REG = MemRead16(PC_REG);

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 2;
    }

    // CD

    public void Ldu_M() // CE
    {
        U_REG = MemRead16(PC_REG);

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 3;
    }

    // CF	//InvalidInsHandler

    #endregion

    #region 0xD0 - 0xDF

    public void Subb_D() // D0
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 4;
    }

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

    public void Addd_D() // D3
    {
        _temp16 = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG + _temp16);

        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);

        D_REG = (ushort)_temp32;

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);

        _cycleCounter += 6;
    }

    public void Andb_D() // D4
    {
        B_REG &= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Bitb_D() // D5
    {
        _temp8 = (byte)(B_REG & MemRead8(DPADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Ldb_D() // D6
    {
        B_REG = MemRead8(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Stb_D() // D7
    {
        MemWrite8(B_REG, DPADDRESS(PC_REG++));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

    public void Eorb_D() // D8
    {
        B_REG ^= MemRead8(DPADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

        _cycleCounter += 4;
    }

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
        CC_V = false; //0;

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

    public void Ldd_D() // DC
    {
        D_REG = MemRead16(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    public void Std_D() // DD
    {
        MemWrite16(D_REG, DPADDRESS(PC_REG++));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    public void Ldu_D() // DE
    {
        U_REG = MemRead16(DPADDRESS(PC_REG++));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    public void Stu_D() // DF
    {
        MemWrite16(U_REG, DPADDRESS(PC_REG++));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false; //0;

        _cycleCounter += 5;
    }

    #endregion

    #region 0xE0 - 0xEF

    public void Subb_X() // E0
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG - _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);

        _cycleCounter += 4;
    }

    public void Cmpb_X() // E1
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp8 = (byte)(B_REG - _postByte);

        CC_C = _temp8 > B_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, B_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);

        _cycleCounter += 4;
    }

    public void Sbcb_X() // E2
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
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
        _temp16 = MemRead16(INDADDRESS(PC_REG++));
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
        B_REG &= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Bitb_X() // E5
    {
        _temp8 = (byte)(B_REG & MemRead8(INDADDRESS(PC_REG++)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Ldb_X() // E6
    {
        B_REG = MemRead8(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Stb_X() // E7
    {
        MemWrite8(B_REG, CalculateEA(MemRead8(PC_REG++)));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Eorb_X() // E8
    {
        B_REG ^= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Adcb_X() // E9
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
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
        B_REG |= MemRead8(INDADDRESS(PC_REG++));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false;//0;

        _cycleCounter += 4;
    }

    public void Addb_X() // EB
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(B_REG + _postByte);

        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((B_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, B_REG);

        B_REG = (byte)_temp16;

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);

        _cycleCounter += 4;
    }

    public void Ldd_X() // EC
    {
        D_REG = MemRead16(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false;//0;

        _cycleCounter += 5;
    }

    public void Std_X() // ED
    {
        MemWrite16(D_REG, INDADDRESS(PC_REG++));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false;//0;

        _cycleCounter += 5;
    }

    public void Ldu_X() // EE
    {
        U_REG = MemRead16(INDADDRESS(PC_REG++));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false;//0;

        _cycleCounter += 5;
    }

    public void Stu_X() // EF
    {
        MemWrite16(U_REG, INDADDRESS(PC_REG++));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false;//0;

        _cycleCounter += 5;
    }

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
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Bitb_E() // F5
    {
        _temp8 = (byte)(B_REG & MemRead8(MemRead16(PC_REG)));

        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Ldb_E() // F6
    {
        B_REG = MemRead8(MemRead16(PC_REG));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Stb_E() // F7
    {
        MemWrite8(B_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(B_REG);
        CC_N = NTEST8(B_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 5;
    }

    public void Eorb_E() // F8
    {
        B_REG ^= MemRead8(MemRead16(PC_REG));

        CC_N = NTEST8(B_REG);
        CC_Z = ZTEST(B_REG);
        CC_V = false; //0;

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
        CC_V = false; //0;

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

    public void Ldd_E() // FC
    {
        D_REG = MemRead16(MemRead16(PC_REG));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    public void Std_E() // FD
    {
        MemWrite16(D_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    public void Ldu_E() // FE
    {
        U_REG = MemRead16(MemRead16(PC_REG));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    public void Stu_E() // FF
    {
        MemWrite16(U_REG, MemRead16(PC_REG));

        CC_Z = ZTEST(U_REG);
        CC_N = NTEST16(U_REG);
        CC_V = false; //0;

        PC_REG += 2;

        _cycleCounter += 6;
    }

    #endregion
}
