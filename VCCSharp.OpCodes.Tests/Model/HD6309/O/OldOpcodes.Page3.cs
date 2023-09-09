namespace VCCSharp.OpCodes.Tests.Model.HD6309.O;

internal partial class OldOpcodes
{
    //0x00 - 0x0F
    //0x10 - 0x1F
    //0x20 - 0x2F

    #region 0x30 - 0x3F

    public void Band() // 30 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) == 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] &= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() & ~(1 << _dest)));
                    break;
            }
        }

        // Else nothing changes
        _cycleCounter += _instance._76;
    }

    public void Biand() // 31 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) != 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] &= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() & ~(1 << _dest)));
                    break;
            }
        }

        // Else do nothing
        _cycleCounter += _instance._76;
    }

    public void Bor() // 32 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) != 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] |= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() | (1 << _dest)));
                    break;
            }
        }

        // Else do nothing
        _cycleCounter += _instance._76;
    }

    public void Bior() // 33 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) == 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] |= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() | (1 << _dest)));
                    break;
            }
        }

        // Else do nothing
        _cycleCounter += _instance._76;
    }

    public void Beor() // 34 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) != 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] ^= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() ^ (1 << _dest)));
                    break;
            }
        }

        _cycleCounter += _instance._76;
    }

    public void Bieor() // 35 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) == 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] ^= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() ^ (1 << _dest)));
                    break;
            }
        }

        _cycleCounter += _instance._76;
    }

    public void Ldbt() // 36 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = MemRead8(DPADDRESS(PC_REG++));
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        if ((_temp8 & (1 << _source)) != 0)
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] |= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() | (1 << _dest)));
                    break;
            }
        }
        else
        {
            switch (_postByte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    byte t = PUR(_postByte);
                    t &= (byte)~(1 << _dest);
                    PUR(_postByte, t);
                    //_cpu.ureg8[_postByte] &= (byte)~(1 << _dest);
                    break;

                case 2: // CC Reg
                    SetCC((byte)(GetCC() & ~(1 << _dest)));
                    break;
            }
        }

        _cycleCounter += _instance._76;
    }

    public void Stbt() // 37 
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = DPADDRESS(PC_REG++);
        _temp8 = MemRead8(_temp16);
        _source = (byte)((_postByte >> 3) & 7);
        _dest = (byte)(_postByte & 7);
        _postByte >>= 6;

        if (_postByte == 3)
        {
            _____();
            return;
        }

        switch (_postByte)
        {
            case 0: // A Reg
            case 1: // B Reg
                _postByte = PUR(_postByte);
                break;

            case 2: // CC Reg
                _postByte = GetCC();
                break;
        }

        if ((_postByte & (1 << _source)) != 0)
        {
            _temp8 |= (byte)(1 << _dest);
        }
        else
        {
            _temp8 &= (byte)~(1 << _dest);
        }

        MemWrite8(_temp8, _temp16);
        _cycleCounter += _instance._87;
    }

    public void Tfm1() // 38 
    {
        if (W_REG == 0)
        {
            _cycleCounter += 6;
            PC_REG++;
            return;
        }

        _postByte = MemRead8(PC_REG);
        _source = (byte)(_postByte >> 4);
        _dest = (byte)(_postByte & 15);

        if (_source > 4 || _dest > 4)
        {
            _____();
            return;
        }

        _temp8 = MemRead8(PXF(_source));
        MemWrite8(_temp8, PXF(_dest));
        PXF(_dest, (ushort)(PXF(_dest) + 1));
        PXF(_source, (ushort)(PXF(_source) + 1));
        W_REG--;
        _cycleCounter += 3;
        PC_REG -= 2;
    }

    public void Tfm2() // 39 
    {
        if (W_REG == 0)
        {
            _cycleCounter += 6;
            PC_REG++;
            return;
        }

        _postByte = MemRead8(PC_REG);
        _source = (byte)(_postByte >> 4);
        _dest = (byte)(_postByte & 15);

        if (_source > 4 || _dest > 4)
        {
            _____();
            return;
        }

        _temp8 = MemRead8(PXF(_source));
        MemWrite8(_temp8, PXF(_dest));
        PXF(_dest, (ushort)(PXF(_dest) - 1));
        PXF(_source, (ushort)(PXF(_source) - 1));
        W_REG--;
        _cycleCounter += 3;
        PC_REG -= 2;
    }

    public void Tfm3() // 3A 
    {
        if (W_REG == 0)
        {
            _cycleCounter += 6;
            PC_REG++;
            return;
        }

        _postByte = MemRead8(PC_REG);
        _source = (byte)(_postByte >> 4);
        _dest = (byte)(_postByte & 15);

        if (_source > 4 || _dest > 4)
        {
            _____();
            return;
        }

        _temp8 = MemRead8(PXF(_source));
        MemWrite8(_temp8, PXF(_dest));
        PXF(_source, (ushort)(PXF(_source) + 1));
        W_REG--;
        PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
        _cycleCounter += 3;
    }

    public void Tfm4() // 3B 
    {
        if (W_REG == 0)
        {
            _cycleCounter += 6;
            PC_REG++;
            return;
        }

        _postByte = MemRead8(PC_REG);
        _source = (byte)(_postByte >> 4);
        _dest = (byte)(_postByte & 15);

        if (_source > 4 || _dest > 4)
        {
            _____();
            return;
        }

        _temp8 = MemRead8(PXF(_source));
        MemWrite8(_temp8, PXF(_dest));
        PXF(_dest, (ushort)(PXF(_dest) + 1));
        W_REG--;
        PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
        _cycleCounter += 3;
    }

    public void Bitmd_M() // 3C 
    {
        _postByte = (byte)(MemRead8(PC_REG++) & 0xC0);
        _temp8 = (byte)(MD & _postByte);
        CC_Z = ZTEST(_temp8);

        if ((_temp8 & 0x80) != 0) MD_ZERODIV = false;
        if ((_temp8 & 0x40) != 0) MD_ILLEGAL = false;

        _cycleCounter += 4;
    }

    public void Ldmd_M() // 3D 
    {
        MD = (byte)(MemRead8(PC_REG++) & 0x03);
        //SetMD(_cpu.md.bits);
        _cycleCounter += 5;
    }

    // 3E

    public void Swi3_I() // 3F 
    {
        CC_E = true;
        MemWrite8(PC_L, --S_REG);
        MemWrite8(PC_H, --S_REG);
        MemWrite8(U_L, --S_REG);
        MemWrite8(U_H, --S_REG);
        MemWrite8(Y_L, --S_REG);
        MemWrite8(Y_H, --S_REG);
        MemWrite8(X_L, --S_REG);
        MemWrite8(X_H, --S_REG);
        MemWrite8(DPA, --S_REG);

        if (MD_NATIVE6309)
        {
            MemWrite8(F_REG, --S_REG);
            MemWrite8(E_REG, --S_REG);
            _cycleCounter += 2;
        }

        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);
        MemWrite8(GetCC(), --S_REG);
        PC_REG = MemRead16(VSWI3);
        _cycleCounter += 20;
    }

    #endregion

    #region 0x40 - 0x4F

    //40
    //41
    //42

    public void Come_I() // 43 
    {
        E_REG = (byte)(0xFF - E_REG);
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_C = true;
        CC_V = false;
        _cycleCounter += _instance._32;
    }

    // 44
    // 45
    // 46
    // 47
    // 48
    // 49

    public void Dece_I() // 4A 
    {
        E_REG--;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = E_REG == 0x7F;
        _cycleCounter += _instance._32;
    }

    // 4B

    public void Ince_I() // 4C 
    {
        E_REG++;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = E_REG == 0x80;
        _cycleCounter += _instance._32;
    }

    public void Tste_I() // 4D 
    {
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += _instance._32;
    }

    // 4E

    public void Clre_I() // 4F 
    {
        E_REG = 0;
        CC_C = false;
        CC_V = false;
        CC_N = false;
        CC_Z = true;
        _cycleCounter += _instance._32;
    }

    #endregion

    #region 0x50 - 0x5F

    // 50
    // 51
    // 52

    public void Comf_I() // 53 
    {
        F_REG = (byte)(0xFF - F_REG);
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_C = true;
        CC_V = false;
        _cycleCounter += _instance._32;
    }

    // 54
    // 55
    // 56
    // 57
    // 58
    // 59

    public void Decf_I() // 5A 
    {
        F_REG--;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = F_REG == 0x7F;
        _cycleCounter += _instance._21;
    }

    // 5B

    public void Incf_I() // 5C 
    {
        F_REG++;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = F_REG == 0x80;
        _cycleCounter += _instance._32;
    }

    public void Tstf_I() // 5D 
    {
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += _instance._32;
    }

    // 5E

    public void Clrf_I() // 5F 
    {
        F_REG = 0;
        CC_C = false;
        CC_V = false;
        CC_N = false;
        CC_Z = true;
        _cycleCounter += _instance._32;
    }

    #endregion

    //0x60 - 0x6F
    //0x70 - 0x7F

    #region 0x80 - 0x8F

    public void Sube_M() // 80 
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(E_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        _cycleCounter += 3;
    }

    public void Cmpe_M() // 81 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = (byte)(E_REG - _postByte);
        CC_C = _temp8 > E_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, E_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += 3;
    }

    // 82

    public void Cmpu_M() // 83 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    // 84
    // 85

    public void Lde_M() // 86 
    {
        E_REG = MemRead8(PC_REG++);
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += 3;
    }

    // 87
    // 88
    // 89
    // 8A

    public void Adde_M() // 8B 
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(E_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((E_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_N = NTEST8(E_REG);
        CC_Z = ZTEST(E_REG);
        _cycleCounter += 3;
    }

    public void Cmps_M() // 8C 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Divd_M() // 8D 
    {
        _postByte = MemRead8(PC_REG++);

        if (_postByte == 0)
        {
            _cycleCounter += 3;
            DivByZero();
            return;
        }

        _postWord = D_REG;
        _signedShort = (short)((short)_postWord / (sbyte)_postByte);

        if (_signedShort > 255 || _signedShort < -256) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += 17;
            return;
        }

        A_REG = (byte)((short)_postWord % (sbyte)_postByte);
        B_REG = (byte)_signedShort;

        if (_signedShort > 127 || _signedShort < -128)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += 25;
    }

    public void Divq_M() // 8E 
    {
        _postWord = MemRead16(PC_REG);
        PC_REG += 2;

        if (_postWord == 0)
        {
            _cycleCounter += 4;
            DivByZero();
            return;
        }

        _temp32 = Q_REG;
        _signedInt = (int)_temp32 / (short)_postWord;

        if (_signedInt > 65535 || _signedInt < -65536) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += 34 - 21;
            return;
        }

        D_REG = (ushort)((int)_temp32 % (short)_postWord);
        W_REG = (ushort)_signedInt;

        if (_signedShort > 32767 || _signedShort < -32768)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += 34;
    }

    public void Muld_M() // 8F 
    {
        Q_REG = (uint)((short)D_REG * (short)MemRead16(PC_REG));
        CC_C = false;
        CC_Z = ZTEST(Q_REG);
        CC_V = false;
        CC_N = NTEST32(Q_REG);
        PC_REG += 2;
        _cycleCounter += 28;
    }

    #endregion

    #region 0x90 - 0x9F

    public void Sube_D() // 90 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(E_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        _cycleCounter += _instance._54;
    }

    public void Cmpe_D() // 91 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp8 = (byte)(E_REG - _postByte);
        CC_C = _temp8 > E_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, E_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += _instance._54;
    }

    // 92

    public void Cmpu_D() // 93 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._75;
    }

    // 94
    // 95

    public void Lde_D() // 96 
    {
        E_REG = MemRead8(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += _instance._54;
    }

    public void Ste_D() // 97 
    {
        MemWrite8(E_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += _instance._54;
    }

    // 98
    // 99
    // 9A

    public void Adde_D() // 9B 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(E_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((E_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_N = NTEST8(E_REG);
        CC_Z = ZTEST(E_REG);
        _cycleCounter += _instance._54;
    }

    public void Cmps_D() // 9C 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._75;
    }

    public void Divd_D() // 9D 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));

        if (_postByte == 0)
        {
            _cycleCounter += 3;
            DivByZero();
            return;
        }

        _postWord = D_REG;
        _signedShort = (short)((short)_postWord / (char)_postByte);

        if (_signedShort > 255 || _signedShort < -256) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += 19;
            return;
        }

        A_REG = (byte)((short)_postWord % (sbyte)_postByte);
        B_REG = (byte)_signedShort;

        if (_signedShort > 127 || _signedShort < -128)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += 27;
    }

    public void Divq_D() // 9E 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));

        if (_postWord == 0)
        {
            _cycleCounter += 4;
            DivByZero();
            return;
        }

        _temp32 = Q_REG;
        _signedInt = (int)_temp32 / (short)_postWord;

        if (_signedInt > 65535 || _signedInt < -65536) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += _instance._3635 - 21;
            return;
        }

        D_REG = (ushort)((int)_temp32 % (short)_postWord);
        W_REG = (ushort)_signedInt;

        if (_signedShort > 32767 || _signedInt < -32768)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += _instance._3635;
    }

    public void Muld_D() // 9F 
    {
        Q_REG = (uint)((short)D_REG * (short)MemRead16(DPADDRESS(PC_REG++)));
        CC_C = false;
        CC_Z = ZTEST(Q_REG);
        CC_V = false;
        CC_N = NTEST32(Q_REG);
        _cycleCounter += _instance._3029;
    }

    #endregion

    #region 0xA0 - 0xAF

    public void Sube_X() // A0 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(E_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        _cycleCounter += 5;
    }

    public void Cmpe_X() // A1 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp8 = (byte)(E_REG - _postByte);
        CC_C = _temp8 > E_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, E_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += 5;
    }

    // A2

    public void Cmpu_X() // A3 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._76;
    }

    // A4
    // A5

    public void Lde_X() // A6 
    {
        E_REG = MemRead8(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += 5;
    }

    public void Ste_X() // A7 
    {
        MemWrite8(E_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        _cycleCounter += 5;
    }

    // A8
    // A9
    // AA

    public void Adde_X() // AB 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(E_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((E_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_N = NTEST8(E_REG);
        CC_Z = ZTEST(E_REG);
        _cycleCounter += 5;
    }

    public void Cmps_X() // AC 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._76;
    }

    public void Divd_X() // AD 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));

        if (_postByte == 0)
        {
            _cycleCounter += 3;
            DivByZero();
            return;
        }

        _postWord = D_REG;
        _signedShort = (short)((short)_postWord / (sbyte)_postByte);

        if (_signedShort > 255 || _signedShort < -256) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += 19;
            return;
        }

        A_REG = (byte)((short)_postWord % (sbyte)_postByte);
        B_REG = (byte)_signedShort;

        if (_signedShort > 127 || _signedShort < -128)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += 27;
    }

    public void Divq_X() // AE 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));

        if (_postWord == 0)
        {
            _cycleCounter += 4;
            DivByZero();
            return;
        }

        _temp32 = Q_REG;
        _signedInt = (int)_temp32 / (short)_postWord;

        if (_signedInt > 65535 || _signedInt < -65536) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += _instance._3635 - 21;
            return;
        }

        D_REG = (ushort)((int)_temp32 % (short)_postWord);
        W_REG = (ushort)_signedInt;

        if (_signedShort > 32767 || _signedShort < -32768)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false;
        }

        CC_C = (B_REG & 1) != 0;
        _cycleCounter += _instance._3635;
    }

    public void Muld_X() // AF 
    {
        Q_REG = (ushort)((short)D_REG * (short)MemRead16(INDADDRESS(PC_REG++)));
        CC_C = false;
        CC_Z = ZTEST(Q_REG);
        CC_V = false;
        CC_N = NTEST32(Q_REG);
        _cycleCounter += 30;
    }

    #endregion

    #region 0xB0 - 0xBF

    public void Sube_E() // B0 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(E_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    public void Cmpe_E() // B1 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp8 = (byte)(E_REG - _postByte);
        CC_C = _temp8 > E_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, E_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    // B2

    public void Cmpu_E() // B3 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(U_REG - _postWord);
        CC_C = _temp16 > U_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, U_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    // B4
    // B5

    public void Lde_E() // B6 
    {
        E_REG = MemRead8(MemRead16(PC_REG));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    public void Ste_E() // B7 
    {
        MemWrite8(E_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(E_REG);
        CC_N = NTEST8(E_REG);
        CC_V = false;
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    // B8
    // B9
    // BA

    public void Adde_E() // BB 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(E_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((E_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, E_REG);
        E_REG = (byte)_temp16;
        CC_N = NTEST8(E_REG);
        CC_Z = ZTEST(E_REG);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    public void Cmps_E() // BC 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(S_REG - _postWord);
        CC_C = _temp16 > S_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, S_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Divd_E() // BD 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        PC_REG += 2;

        if (_postByte == 0)
        {
            _cycleCounter += 3;
            DivByZero();
            return;
        }

        _postWord = D_REG;
        _signedShort = (short)((short)_postWord / (sbyte)_postByte);

        if (_signedShort > 255 || _signedShort < -256) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += 17;
            return;
        }

        A_REG = (byte)((short)_postWord % (sbyte)_postByte);
        B_REG = (byte)_signedShort;

        if (_signedShort > 127 || _signedShort < -128)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;
        }
        CC_C = (B_REG & 1) != 0;
        _cycleCounter += 25;
    }

    public void Divq_E() // BE 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        PC_REG += 2;

        if (_postWord == 0)
        {
            _cycleCounter += 4;
            DivByZero();
            return;
        }

        _temp32 = Q_REG;
        _signedInt = (int)_temp32 / (short)_postWord;

        if (_signedInt > 65535 || _signedInt < -65536) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;
            _cycleCounter += _instance._3635 - 21;
            return;
        }

        D_REG = (ushort)((int)_temp32 % (short)_postWord);
        W_REG = (ushort)_signedInt;

        if (_signedShort > 32767 || _signedShort < -32768)
        {
            CC_V = true;
            CC_N = true;
        }
        else
        {
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false;
        }
        CC_C = (B_REG & 1) != 0;
        _cycleCounter += _instance._3635;
    }

    public void Muld_E() // BF 
    {
        Q_REG = (ushort)((short)D_REG * (short)MemRead16(MemRead16(PC_REG)));
        PC_REG += 2;
        CC_C = false;
        CC_Z = ZTEST(Q_REG);
        CC_V = false;
        CC_N = NTEST32(Q_REG);
        _cycleCounter += _instance._3130;
    }

    #endregion

    #region 0xC0 - 0xCF

    public void Subf_M() // C0 
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(F_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        _cycleCounter += 3;
    }

    public void Cmpf_M() // C1 
    {
        _postByte = MemRead8(PC_REG++);
        _temp8 = (byte)(F_REG - _postByte);
        CC_C = _temp8 > F_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, F_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += 3;
    }

    // C2
    // C3
    // C4
    // C5

    public void Ldf_M() // C6 
    {
        F_REG = MemRead8(PC_REG++);
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += 3;
    }

    // C7
    // C8
    // C9
    // CA

    public void Addf_M() // CB 
    {
        _postByte = MemRead8(PC_REG++);
        _temp16 = (ushort)(F_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((F_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_N = NTEST8(F_REG);
        CC_Z = ZTEST(F_REG);
        _cycleCounter += 3;
    }

    // CC
    // CD
    // CE
    // CF

    #endregion

    #region 0xD0 - 0xDF

    public void Subf_D() // D0 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(F_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        _cycleCounter += _instance._54;
    }

    public void Cmpf_D() // D1 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp8 = (byte)(F_REG - _postByte);
        CC_C = _temp8 > F_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, F_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += _instance._54;
    }

    // D2
    // D3
    // D4
    // D5

    public void Ldf_D() // D6 
    {
        F_REG = MemRead8(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += _instance._54;
    }

    public void Stf_D() // D7 
    {
        MemWrite8(F_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += _instance._54;
    }

    // D8
    // D9
    // DA

    public void Addf_D() // DB 
    {
        _postByte = MemRead8(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(F_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((F_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_N = NTEST8(F_REG);
        CC_Z = ZTEST(F_REG);
        _cycleCounter += _instance._54;
    }

    // DC
    // DD
    // DE
    // DF

    #endregion

    #region 0xE0 - 0xEF

    public void Subf_X() // E0 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(F_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        _cycleCounter += 5;
    }

    public void Cmpf_X() // E1 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp8 = (byte)(F_REG - _postByte);
        CC_C = _temp8 > F_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, F_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        _cycleCounter += 5;
    }

    // E2
    // E3
    // E4
    // E5

    public void Ldf_X() // E6 
    {
        F_REG = MemRead8(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += 5;
    }

    public void Stf_X() // E7 
    {
        MemWrite8(F_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        _cycleCounter += 5;
    }

    // E8
    // E9
    // EA

    public void Addf_X() // EB 
    {
        _postByte = MemRead8(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(F_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((F_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_N = NTEST8(F_REG);
        CC_Z = ZTEST(F_REG);
        _cycleCounter += 5;
    }

    // EC
    // ED
    // EE
    // EF

    #endregion

    #region 0xF0 - 0xFF

    public void Subf_E() // F0 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(F_REG - _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    public void Cmpf_E() // F1 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp8 = (byte)(F_REG - _postByte);
        CC_C = _temp8 > F_REG;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp8, F_REG);
        CC_N = NTEST8(_temp8);
        CC_Z = ZTEST(_temp8);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    // F2
    // F3
    // F4
    // F5

    public void Ldf_E() // F6 
    {
        F_REG = MemRead8(MemRead16(PC_REG));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    public void Stf_E() // F7 
    {
        MemWrite8(F_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(F_REG);
        CC_N = NTEST8(F_REG);
        CC_V = false;
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    // F8
    // F9
    // FA

    public void Addf_E() // FB 
    {
        _postByte = MemRead8(MemRead16(PC_REG));
        _temp16 = (ushort)(F_REG + _postByte);
        CC_C = (_temp16 & 0x100) >> 8 != 0;
        CC_H = ((F_REG ^ _postByte ^ _temp16) & 0x10) >> 4 != 0;
        CC_V = OVERFLOW8(CC_C, _postByte, _temp16, F_REG);
        F_REG = (byte)_temp16;
        CC_N = NTEST8(F_REG);
        CC_Z = ZTEST(F_REG);
        PC_REG += 2;
        _cycleCounter += _instance._65;
    }

    // FC
    // FD
    // FE
    // FF

    #endregion
}
