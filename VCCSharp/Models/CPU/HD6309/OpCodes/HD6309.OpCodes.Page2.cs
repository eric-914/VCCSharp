namespace VCCSharp.Models.CPU.HD6309;

// ReSharper disable once InconsistentNaming
partial class HD6309
{
    //0x00 - 0x0F
    //0x10 - 0x1F

    #region 0x20 - 0x2F

    public void LBrn_R() // 21 
    {
        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBhi_R() // 22 
    {
        if (!(CC_C | CC_Z))
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBls_R() // 23 
    {
        if (CC_C | CC_Z)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBhs_R() // 24 
    {
        if (!CC_C)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 6;
    }

    public void LBcs_R() // 25 
    {
        if (CC_C)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBne_R() // 26 
    {
        if (!CC_Z)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBeq_R() // 27 
    {
        if (CC_Z)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBvc_R() // 28 
    {
        if (!CC_V)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBvs_R() // 29 
    {
        if (CC_V)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBpl_R() // 2A 
    {
        if (!CC_N)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBmi_R() // 2B 
    {
        if (CC_N)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBge_R() // 2C 
    {
        if (!(CC_N ^ CC_V))
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBlt_R() // 2D 
    {
        if (CC_V ^ CC_N)
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBgt_R() // 2E 
    {
        if (!(CC_Z | (CC_N ^ CC_V)))
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    public void LBle_R() // 2F 
    {
        if (CC_Z | (CC_N ^ CC_V))
        {
            PC_REG += (ushort)(short)MemRead16(PC_REG);
            _cycleCounter += 1;
        }

        PC_REG += 2;
        _cycleCounter += 5;
    }

    #endregion

    #region 0x30 - 0x3F

    public void Addr() // 30 
    {
        ushort source16 = 0;

        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7)
        { // 8 bit dest
            _dest &= 7;

            byte dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7)
            {
                // 8 bit source
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp16 = (ushort)(source8 + dest8);

            switch (_dest)
            {
                case 2: SetCC((byte)_temp16); break;
                case 4: case 5: break; // never assign to zero reg
                default: PUR(_dest, (byte)_temp16); break;
            }

            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, source8, dest8, (byte)_temp16);
            CC_N = NTEST8(PUR(_dest));
            CC_Z = ZTEST(PUR(_dest));
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0: case 1: source16 = D_REG; break; // A & B Reg
                    case 2: source16 = GetCC(); break; // CC
                    case 3: source16 = DP_REG; break; // DP
                    case 4: case 5: source16 = 0; break; // Zero Reg
                    case 6: case 7: source16 = W_REG; break; // E & F Reg
                }
            }

            _temp32 = (uint)(source16 + dest16);
            PXF(_dest, (ushort)_temp32);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, source16, dest16, (ushort)_temp32);
            CC_N = NTEST16(PXF(_dest));
            CC_Z = ZTEST(PXF(_dest));
        }

        _cycleCounter += 4;
    }

    public void Adcr() // 31 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7)
            {
                // 8 bit source
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp16 = (ushort)(source8 + dest8 + (CC_C ? 1 : 0));

            switch (_dest)
            {
                case 2:
                    SetCC((byte)_temp16);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, (byte)_temp16);
                    break;
            }

            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, source8, dest8, (byte)_temp16);
            CC_N = NTEST8(PUR(_dest));
            CC_Z = ZTEST(PUR(_dest));
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp32 = (uint)(source16 + dest16 + (CC_C ? 1 : 0));
            PXF(_dest, (ushort)_temp32);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, source16, dest16, (ushort)_temp32);
            CC_N = NTEST16(PXF(_dest));
            CC_Z = ZTEST(PXF(_dest));
        }

        _cycleCounter += 4;
    }

    public void Subr() // 32 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7)
            {
                // 8 bit source
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else
            { // 16 bit source - demote to 8 bit
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp16 = (ushort)(dest8 - source8);

            switch (_dest)
            {
                case 2:
                    SetCC((byte)_temp16);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, (byte)_temp16);
                    break;
            }

            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((dest8 ^ PUR(_dest) ^ source8) >> 7)) != 0;
            CC_N = PUR(_dest) >> 7 != 0;
            CC_Z = ZTEST(PUR(_dest));
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp32 = (uint)(dest16 - source16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = ((dest16 ^ source16 ^ _temp32 ^ (_temp32 >> 1)) & 0x8000) != 0;
            PXF(_dest, (ushort)_temp32);
            CC_N = (_temp32 & 0x8000) >> 15 != 0;
            CC_Z = ZTEST(_temp32);
        }

        _cycleCounter += 4;
    }

    public void Sbcr() // 33 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7) // 8 bit source
            {
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp16 = (ushort)(dest8 - source8 - (CC_C ? 1 : 0));

            switch (_dest)
            {
                case 2:
                    SetCC((byte)_temp16);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, (byte)_temp16);
                    break;
            }

            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((dest8 ^ PUR(_dest) ^ source8) >> 7)) != 0;
            CC_N = PUR(_dest) >> 7 != 0;
            CC_Z = ZTEST(PUR(_dest));
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp32 = (uint)(dest16 - source16 - (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = ((dest16 ^ source16 ^ _temp32 ^ (_temp32 >> 1)) & 0x8000) != 0;
            PXF(_dest, (ushort)_temp32);
            CC_N = (_temp32 & 0x8000) >> 15 != 0;
            CC_Z = ZTEST(_temp32);
        }

        _cycleCounter += 4;
    }

    public void Andr() // 34 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7) // 8 bit source
            {
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp8 = (byte)(dest8 & source8);

            switch (_dest)
            {
                case 2:
                    SetCC(_temp8);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, _temp8);
                    break;
            }

            CC_N = _temp8 >> 7 != 0;
            CC_Z = ZTEST(_temp8);
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;
                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp16 = (ushort)(dest16 & source16);
            PXF(_dest, _temp16);
            CC_N = _temp16 >> 15 != 0;
            CC_Z = ZTEST(_temp16);
        }

        CC_V = false; //0;
        _cycleCounter += 4;
    }

    public void Orr() // 35     
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7) // 8 bit source
            {
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp8 = (byte)(dest8 | source8);

            switch (_dest)
            {
                case 2:
                    SetCC(_temp8);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, _temp8);
                    break;
            }

            CC_N = _temp8 >> 7 != 0;
            CC_Z = ZTEST(_temp8);
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp16 = (ushort)(dest16 | source16);
            PXF(_dest, _temp16);
            CC_N = _temp16 >> 15 != 0;
            CC_Z = ZTEST(_temp16);
        }

        CC_V = false; //0;
        _cycleCounter += 4;
    }

    public void Eorr() // 36 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7) // 8 bit source
            {
                _source &= 7;

                source8 = _source == 2 ? GetCC() : PUR(_source);
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp8 = (byte)(dest8 ^ source8);

            switch (_dest)
            {
                case 2:
                    SetCC(_temp8);
                    break;

                case 4:
                case 5:
                    break; // never assign to zero reg

                default:
                    PUR(_dest, _temp8);
                    break;
            }

            CC_N = _temp8 >> 7 != 0;
            CC_Z = ZTEST(_temp8);
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp16 = (ushort)(dest16 ^ source16);
            PXF(_dest, _temp16);
            CC_N = _temp16 >> 15 != 0;
            CC_Z = ZTEST(_temp16);
        }

        CC_V = false; //0;
        _cycleCounter += 4;
    }

    public void Cmpr() // 37 
    {
        ushort source16 = 0;
        _temp8 = MemRead8(PC_REG++);
        _source = (byte)(_temp8 >> 4);
        _dest = (byte)(_temp8 & 15);

        if (_dest > 7) // 8 bit dest
        {
            _dest &= 7;

            var dest8 = _dest == 2 ? GetCC() : PUR(_dest);

            byte source8;
            if (_source > 7) // 8 bit source
            {
                _source &= 7;

                if (_source == 2)
                {
                    source8 = GetCC();
                }
                else
                {
                    source8 = PUR(_source);
                }
            }
            else // 16 bit source - demote to 8 bit
            {
                _source &= 7;
                source8 = (byte)PXF(_source);
            }

            _temp16 = (ushort)(dest8 - source8);
            _temp8 = (byte)_temp16;
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((dest8 ^ _temp8 ^ source8) >> 7)) != 0;
            CC_N = _temp8 >> 7 != 0;
            CC_Z = ZTEST(_temp8);
        }
        else // 16 bit dest
        {
            var dest16 = PXF(_dest);

            if (_source < 8) // 16 bit source
            {
                source16 = PXF(_source);
            }
            else // 8 bit source - promote to 16 bit
            {
                _source &= 7;

                switch (_source)
                {
                    case 0:
                    case 1:
                        source16 = D_REG;
                        break; // A & B Reg

                    case 2:
                        source16 = GetCC();
                        break; // CC

                    case 3:
                        source16 = DP_REG;
                        break; // DP

                    case 4:
                    case 5:
                        source16 = 0;
                        break; // Zero Reg

                    case 6:
                    case 7:
                        source16 = W_REG;
                        break; // E & F Reg
                }
            }

            _temp32 = (uint)(dest16 - source16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = ((dest16 ^ source16 ^ _temp32 ^ (_temp32 >> 1)) & 0x8000) != 0;
            CC_N = (_temp32 & 0x8000) >> 15 != 0;
            CC_Z = ZTEST(_temp32);
        }

        _cycleCounter += 4;
    }

    public void Pshsw() // 38 
    {
        MemWrite8((F_REG), --S_REG);
        MemWrite8((E_REG), --S_REG);
        _cycleCounter += 6;
    }

    public void Pulsw() // 39 
    {
        E_REG = MemRead8(S_REG++);
        F_REG = MemRead8(S_REG++);
        _cycleCounter += 6;
    }

    public void Pshuw() // 3A 
    {
        MemWrite8((F_REG), --U_REG);
        MemWrite8((E_REG), --U_REG);
        _cycleCounter += 6;
    }

    public void Puluw() // 3B 
    {
        E_REG = MemRead8(U_REG++);
        F_REG = MemRead8(U_REG++);
        _cycleCounter += 6;
    }

    // 3C
    // 3D
    // 3E

    public void Swi2_I() // 3F 
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

        if (MD_NATIVE6309)
        {
            MemWrite8((F_REG), --S_REG);
            MemWrite8((E_REG), --S_REG);
            _cycleCounter += 2;
        }

        MemWrite8(B_REG, --S_REG);
        MemWrite8(A_REG, --S_REG);
        MemWrite8(GetCC(), --S_REG);
        PC_REG = MemRead16(Define.VSWI2);
        _cycleCounter += 20;
    }

    #endregion

    #region 0x40 - 0x4F

    public void Negd_I() // 40 
    {
        D_REG = (ushort)(0 - D_REG);
        CC_C = D_REG > 0;
        CC_V = D_REG == 0x8000;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._32;
    }

    // 41
    // 42

    public void Comd_I() // 43 
    {
        D_REG = (ushort)(0xFFFF - D_REG);
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_C = true; //1;
        CC_V = false; //0;
        _cycleCounter += _instance._32;
    }

    public void Lsrd_I() // 44 
    {
        CC_C = (D_REG & 1) != 0;
        D_REG = (ushort)(D_REG >> 1);
        CC_Z = ZTEST(D_REG);
        CC_N = false; //0;
        _cycleCounter += _instance._32;
    }

    // 45

    public void Rord_I() // 46 
    {
        _postWord = CC_C ? (ushort)0x8000 : (ushort)0x0000; //CC_C<< 15;
        CC_C = (D_REG & 1) != 0;
        D_REG = (ushort)((D_REG >> 1) | _postWord);
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        _cycleCounter += _instance._32;
    }

    public void Asrd_I() // 47 
    {
        CC_C = (D_REG & 1) != 0;
        D_REG = (ushort)((D_REG & 0x8000) | (D_REG >> 1));
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        _cycleCounter += _instance._32;
    }

    public void Asld_I() // 48 
    {
        CC_C = D_REG >> 15 != 0;
        CC_V = ((CC_C ? 1 : 0) ^ ((D_REG & 0x4000) >> 14)) != 0;
        D_REG = (ushort)(D_REG << 1);
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._32;
    }

    public void Rold_I() // 49 
    {
        _postWord = (ushort)(CC_C ? 1 : 0);
        CC_C = D_REG >> 15 != 0;
        CC_V = ((CC_C ? 1 : 0) ^ ((D_REG & 0x4000) >> 14)) != 0;
        D_REG = (ushort)((D_REG << 1) | _postWord);
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        _cycleCounter += _instance._32;
    }

    public void Decd_I() // 4A 
    {
        D_REG--;
        CC_Z = ZTEST(D_REG);
        CC_V = D_REG == 0x7FFF;
        CC_N = NTEST16(D_REG);
        _cycleCounter += _instance._32;
    }

    // 4B

    public void Incd_I() // 4C 
    {
        D_REG++;
        CC_Z = ZTEST(D_REG);
        CC_V = D_REG == 0x8000;
        CC_N = NTEST16(D_REG);
        _cycleCounter += _instance._32;
    }

    public void Tstd_I() // 4D 
    {
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._32;
    }

    // 4E

    public void Clrd_I() // 4F 
    {
        D_REG = 0;
        CC_C = false; //0;
        CC_V = false; //0;
        CC_N = false; //0;
        CC_Z = true; //1;
        _cycleCounter += _instance._32;
    }

    #endregion

    #region 0x50 - 0x5F

    public void Comw_I() // 53 
    {
        W_REG = (ushort)(0xFFFF - W_REG);
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_C = true; //1;
        CC_V = false; //0;
        _cycleCounter += _instance._32;

    }

    public void Lsrw_I() // 54 
    {
        CC_C = (W_REG & 1) != 0;
        W_REG = (ushort)(W_REG >> 1);
        CC_Z = ZTEST(W_REG);
        CC_N = false; //0;
        _cycleCounter += _instance._32;
    }

    public void Rorw_I() // 56 
    {
        _postWord = (ushort)((CC_C ? 1 : 0) << 15);
        CC_C = (W_REG & 1) != 0;
        W_REG = (ushort)((W_REG >> 1) | _postWord);
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._32;
    }

    public void Rolw_I() // 59 
    {
        _postWord = CC_C ? (ushort)1 : (ushort)0;
        CC_C = W_REG >> 15 != 0;
        CC_V = ((CC_C ? 1 : 0) ^ ((W_REG & 0x4000) >> 14)) != 0;
        W_REG = (ushort)((W_REG << 1) | _postWord);
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._32;
    }

    public void Decw_I() // 5A 
    {
        W_REG--;
        CC_Z = ZTEST(W_REG);
        CC_V = W_REG == 0x7FFF;
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._32;
    }

    public void Incw_I() // 5C 
    {
        W_REG++;
        CC_Z = ZTEST(W_REG);
        CC_V = W_REG == 0x8000;
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._32;
    }

    public void Tstw_I() // 5D 
    {
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._32;
    }

    public void Clrw_I() // 5F 
    {
        W_REG = 0;
        CC_C = false; //0;
        CC_V = false; //0;
        CC_N = false; //0;
        CC_Z = true; //1;
        _cycleCounter += _instance._32;
    }

    #endregion

    //0x60 - 0x6F
    //0x70 - 0x7F

    #region 0x80 - 0x8F

    public void Subw_M() // 80 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(W_REG - _postWord);
        CC_C = _temp16 > W_REG;
        CC_V = OVERFLOW16(CC_C, _temp16, W_REG, _postWord);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        W_REG = _temp16;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Cmpw_M() // 81 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(W_REG - _postWord);
        CC_C = _temp16 > W_REG;
        CC_V = OVERFLOW16(CC_C, _temp16, W_REG, _postWord);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Sbcd_M() // 82 
    {
        _postWord = MemRead16(PC_REG);
        _temp32 = (uint)(D_REG - _postWord - (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, D_REG, _postWord);
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Cmpd_M() // 83 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(D_REG - _postWord);
        CC_C = _temp16 > D_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Andd_M() // 84 
    {
        D_REG &= MemRead16(PC_REG);
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Bitd_M() // 85 
    {
        _temp16 = (ushort)(D_REG & MemRead16(PC_REG));
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Ldw_M() // 86  
    {
        W_REG = MemRead16(PC_REG);
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Eord_M() // 88 
    {
        D_REG ^= MemRead16(PC_REG);
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Adcd_M() // 89 
    {
        _postWord = MemRead16(PC_REG);
        _temp32 = (ushort)(D_REG + _postWord + (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _postWord, (ushort)_temp32, D_REG);
        CC_H = ((D_REG ^ _temp32 ^ _postWord) & 0x100) >> 8 != 0;
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Ord_M() // 8A  
    {
        D_REG |= MemRead16(PC_REG);
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Addw_M() // 8B 
    {
        _temp16 = MemRead16(PC_REG);
        _temp32 = (uint)(W_REG + _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Cmpy_M() // 8C 
    {
        _postWord = MemRead16(PC_REG);
        _temp16 = (ushort)(Y_REG - _postWord);
        CC_C = _temp16 > Y_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    public void Ldy_M() // 8E  
    {
        Y_REG = MemRead16(PC_REG);
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._54;
    }

    #endregion

    #region 0x90 - 0x9F

    public void Subw_D() // 90 
    {
        _temp16 = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(W_REG - _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._75;
    }

    public void Cmpw_D() // 91 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(W_REG - _postWord);
        CC_C = _temp16 > W_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, W_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._75;
    }

    public void Sbcd_D() // 92 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG - _postWord - (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, D_REG, _postWord);
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._75;
    }

    public void Cmpd_D() // 93 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(D_REG - _postWord);
        CC_C = _temp16 > D_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._75;
    }

    public void Andd_D() // 94 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        D_REG &= _postWord;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._75;
    }

    public void Bitd_D() // 95 
    {
        _temp16 = (ushort)(D_REG & MemRead16(DPADDRESS(PC_REG++)));
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        CC_V = false; //0;
        _cycleCounter += _instance._75;
    }

    public void Ldw_D() // 96 
    {
        W_REG = MemRead16(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    public void Stw_D() // 97 
    {
        MemWrite16(W_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    public void Eord_D() // 98 
    {
        D_REG ^= MemRead16(DPADDRESS(PC_REG++));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._75;
    }

    public void Adcd_D() // 99 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG + _postWord + (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _postWord, (ushort)_temp32, D_REG);
        CC_H = ((D_REG ^ _temp32 ^ _postWord) & 0x100) >> 8 != 0;
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._75;
    }

    public void Ord_D() // 9A 
    {
        D_REG |= MemRead16(DPADDRESS(PC_REG++));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._75;
    }

    public void Addw_D() // 9B 
    {
        _temp16 = MemRead16(DPADDRESS(PC_REG++));
        _temp32 = (uint)(W_REG + _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._75;
    }

    public void Cmpy_D() // 9C 
    {
        _postWord = MemRead16(DPADDRESS(PC_REG++));
        _temp16 = (ushort)(Y_REG - _postWord);
        CC_C = _temp16 > Y_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._75;
    }

    public void Ldy_D() // 9E 
    {
        Y_REG = MemRead16(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    public void Sty_D() // 9F 
    {
        MemWrite16(Y_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    #endregion

    #region 0xA0 - 0xAF

    public void Subw_X() // A0 
    {
        _temp16 = MemRead16(INDADDRESS(PC_REG++));
        _temp32 = (uint)(W_REG - _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._76;
    }

    public void Cmpw_X() // A1 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(W_REG - _postWord);
        CC_C = _temp16 > W_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, W_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._76;
    }

    public void Sbcd_X() // A2 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG - _postWord - (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _postWord, (ushort)_temp32, D_REG);
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._76;
    }

    public void Cmpd_X() // A3 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(D_REG - _postWord);
        CC_C = _temp16 > D_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._76;
    }

    public void Andd_X() // A4 
    {
        D_REG &= MemRead16(INDADDRESS(PC_REG++));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._76;
    }

    public void Bitd_X() // A5 
    {
        _temp16 = (ushort)(D_REG & MemRead16(INDADDRESS(PC_REG++)));
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        CC_V = false; //0;
        _cycleCounter += _instance._76;
    }

    public void Ldw_X() // A6 
    {
        W_REG = MemRead16(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    public void Stw_X() // A7 
    {
        MemWrite16(W_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    public void Eord_X() // A8 
    {
        D_REG ^= MemRead16(INDADDRESS(PC_REG++));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._76;
    }

    public void Adcd_X() // A9 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp32 = (uint)(D_REG + _postWord + (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _postWord, (ushort)_temp32, D_REG);
        CC_H = (((D_REG ^ _temp32 ^ _postWord) & 0x100) >> 8) != 0;
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        _cycleCounter += _instance._76;
    }

    public void Ord_X() // AA 
    {
        D_REG |= MemRead16(INDADDRESS(PC_REG++));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._76;
    }

    public void Addw_X() // AB 
    {
        _temp16 = MemRead16(INDADDRESS(PC_REG++));
        _temp32 = (uint)(W_REG + _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        _cycleCounter += _instance._76;
    }

    public void Cmpy_X() // AC 
    {
        _postWord = MemRead16(INDADDRESS(PC_REG++));
        _temp16 = (ushort)(Y_REG - _postWord);
        CC_C = _temp16 > Y_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        _cycleCounter += _instance._76;
    }

    public void Ldy_X() // AE 
    {
        Y_REG = MemRead16(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    public void Sty_X() // AF 
    {
        MemWrite16(Y_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    #endregion

    #region 0xB0 - 0xBF

    public void Subw_E() // B0 
    {
        _temp16 = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(W_REG - _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Cmpw_E() // B1 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(W_REG - _postWord);
        CC_C = _temp16 > W_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, W_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Sbcd_E() // B2 
    {
        _temp16 = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(D_REG - _temp16 - (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
        D_REG = (ushort)_temp32;
        CC_Z = ZTEST(D_REG);
        CC_N = NTEST16(D_REG);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Cmpd_E() // B3 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(D_REG - _postWord);
        CC_C = _temp16 > D_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Andd_E() // B4 
    {
        D_REG &= MemRead16(MemRead16(PC_REG));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Bitd_E() // B5 
    {
        _temp16 = (ushort)(D_REG & MemRead16(MemRead16(PC_REG)));
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Ldw_E() // B6 
    {
        W_REG = MemRead16(MemRead16(PC_REG));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    public void Stw_E() // B7 
    {
        MemWrite16(W_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    public void Eord_E() // B8 
    {
        D_REG ^= MemRead16(MemRead16(PC_REG));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Adcd_E() // B9 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(D_REG + _postWord + (CC_C ? 1 : 0));
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _postWord, (ushort)_temp32, D_REG);
        CC_H = (((D_REG ^ _temp32 ^ _postWord) & 0x100) >> 8) != 0;
        D_REG = (ushort)_temp32;
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Ord_E() // BA 
    {
        D_REG |= MemRead16(MemRead16(PC_REG));
        CC_N = NTEST16(D_REG);
        CC_Z = ZTEST(D_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Addw_E() // BB 
    {
        _temp16 = MemRead16(MemRead16(PC_REG));
        _temp32 = (uint)(W_REG + _temp16);
        CC_C = (_temp32 & 0x10000) >> 16 != 0;
        CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
        W_REG = (ushort)_temp32;
        CC_Z = ZTEST(W_REG);
        CC_N = NTEST16(W_REG);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Cmpy_E() // BC 
    {
        _postWord = MemRead16(MemRead16(PC_REG));
        _temp16 = (ushort)(Y_REG - _postWord);
        CC_C = _temp16 > Y_REG;
        CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
        CC_N = NTEST16(_temp16);
        CC_Z = ZTEST(_temp16);
        PC_REG += 2;
        _cycleCounter += _instance._86;
    }

    public void Ldy_E() // BE 
    {
        Y_REG = MemRead16(MemRead16(PC_REG));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    public void Sty_E() // BF 
    {
        MemWrite16(Y_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(Y_REG);
        CC_N = NTEST16(Y_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    #endregion

    #region 0xC0 - 0xCF

    public void Lds_I() // CE 
    {
        S_REG = MemRead16(PC_REG);
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += 4;
    }

    #endregion

    #region 0xD0 - 0xDF

    public void Ldq_D() // DC 
    {
        Q_REG = MemRead32(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._87;
    }

    public void Stq_D() // DD 
    {
        MemWrite32(Q_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._87;
    }

    public void Lds_D() // DE 
    {
        S_REG = MemRead16(DPADDRESS(PC_REG++));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    public void Sts_D() // DF 
    {
        MemWrite16(S_REG, DPADDRESS(PC_REG++));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        _cycleCounter += _instance._65;
    }

    #endregion

    #region 0xE0 - 0xEF

    public void Ldq_X() // EC 
    {
        Q_REG = MemRead32(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        _cycleCounter += 8;
    }

    public void Stq_X() // ED 
    {
        MemWrite32(Q_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        _cycleCounter += 8;
    }

    public void Lds_X() // EE 
    {
        S_REG = MemRead16(INDADDRESS(PC_REG++));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    public void Sts_X() // EF 
    {
        MemWrite16(S_REG, INDADDRESS(PC_REG++));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        _cycleCounter += 6;
    }

    #endregion

    #region 0xF0 - 0xFF

    public void Ldq_E() // FC 
    {
        Q_REG = MemRead32(MemRead16(PC_REG));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._98;
    }

    public void Stq_E() // FD 
    {
        MemWrite32(Q_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(Q_REG);
        CC_N = NTEST32(Q_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._98;
    }

    public void Lds_E() // FE 
    {
        S_REG = MemRead16(MemRead16(PC_REG));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    public void Sts_E() // FF 
    {
        MemWrite16(S_REG, MemRead16(PC_REG));
        CC_Z = ZTEST(S_REG);
        CC_N = NTEST16(S_REG);
        CC_V = false; //0;
        PC_REG += 2;
        _cycleCounter += _instance._76;
    }

    #endregion
}
