namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        //0x1000 - 0x101F

        #region 0x20 - 0x2F

        //0x1020

        public void LBRN_R() //0x1021
        {
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBHI_R() //0x1022
        {
            if (!(CC_C | CC_Z))
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBLS_R() //0x1023
        {
            if (CC_C | CC_Z)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBHS_R() //0x1024
        {
            if (!CC_C)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        public void LBCS_R() //0x1025
        {
            if (CC_C)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBNE_R() //0x1026
        {
            if (!CC_Z)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBEQ_R() //0x1027
        {
            if (CC_Z)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBVC_R() //0x1028
        {
            if (!CC_V)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;
            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBVS_R() //0x1029
        {
            if (CC_V)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBPL_R() //0x102A
        {
            if (!CC_N)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBMI_R() //0x102B
        {
            if (CC_N)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBGE_R() //0x102C
        {
            if (!(CC_N ^ CC_V))
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBLT_R() //0x102D
        {
            if (CC_V ^ CC_N)
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBGT_R() //0x102E
        {
            if (!(CC_Z | (CC_N ^ CC_V)))
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LBLE_R() //0x102F
        {
            if (CC_Z | (CC_N ^ CC_V))
            {
                PC_REG += (ushort)(short)MemRead16(PC_REG);

                unsafe
                {
                    _instance->CycleCounter += 1;
                }
            }

            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        #endregion

        #region 0x30 - 0x3F

        //0x1030 - 0x103E

        public void SWI2_I() //0x103F
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
            MemWrite8(MC6809_getcc(), --S_REG);
            PC_REG = MemRead16(Define.VSWI2);

            unsafe
            {
                _instance->CycleCounter += 20;
            }
        }

        #endregion

        //0x1040 - 0x107F

        #region 0x80 - 0x8F

        //0x1080 - 0x1082

        public void CMPD_M() //0x1083
        {
            _postWord = MemRead16(PC_REG);
            _temp16 = (ushort)(D_REG - _postWord);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        //0x1084 - 0x108B

        public void CMPY_M() //0x108C
        {
            _postWord = MemRead16(PC_REG);
            _temp16 = (ushort)(Y_REG - _postWord);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        public void LDY_M() //0x108E
        {
            Y_REG = MemRead16(PC_REG);
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 5;
            }
        }

        //0x108F

        #endregion

        #region 0x90 - 0x9F

        //0x1090 - 0x1092

        public void CMPD_D() //0x1093
        {
            _postWord = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(D_REG - _postWord);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        //0x1094 - 0x109B

        public void CMPY_D()	//0x109C
        {
            _postWord = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(Y_REG - _postWord);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        //0x109D

        public void LDY_D() //0x109E
        {
            Y_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        public void STY_D() //0x109F
        {
            MemWrite16(Y_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        #endregion

        #region 0xA0 - 0xAF

        //0x10A0 - 0x10A2

        public void CMPD_X() //0x10A3
        {
            _postWord = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(D_REG - _postWord);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        //0x10A4 - 0x10AB

        public void CMPY_X() //0x10AC
        {
            _postWord = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(Y_REG - _postWord);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        //0x10AD

        public void LDY_X() //0x10AE
        {
            Y_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        public void STY_X() //0x10AF
        {
            MemWrite16(Y_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        #endregion

        #region 0xB0 - 0xBF

        //0x10B0 - 0x10B2

        public void CMPD_E() //0x10B3
        {
            _postWord = MemRead16(MemRead16(PC_REG));
            _temp16 = (ushort)(D_REG - _postWord);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 8;
            }
        }

        //0x10B4 - 0x10BB

        public void CMPY_E() //0x10BC
        {
            _postWord = MemRead16(MemRead16(PC_REG));
            _temp16 = (ushort)(Y_REG - _postWord);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postWord, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 8;
            }
        }

        //0x10BD

        public void LDY_E() //0x10BE
        {
            Y_REG = MemRead16(MemRead16(PC_REG));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        public void STY_E() //0x10BF
        {
            MemWrite16(Y_REG, MemRead16(PC_REG));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        #endregion

        #region 0xC0 - 0xCF

        //0x10C0 - 0x10CD

        public void LDS_I() //0x10CE
        {
            S_REG = MemRead16(PC_REG);
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 4;
            }
        }

        //0x10CF

        #endregion

        #region 0xD0 - 0xDF

        //0x10D0 - 0x10DD

        public void LDS_D() //0x10DE
        {
            S_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        public void STS_D() //0x10DF
        {
            MemWrite16(S_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        #endregion

        #region 0xE0 - 0xEF

        //0x10E0 - 0x10ED

        public void LDS_X() //0x10EE
        {
            S_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        public void STS_X() //0x10EF
        {
            MemWrite16(S_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;

            unsafe
            {
                _instance->CycleCounter += 6;
            }
        }

        #endregion

        #region 0xF0 - 0xFF

        //0x10F0 - 0x10FD

        public void LDS_E() //0x10FE
        {
            S_REG = MemRead16(MemRead16(PC_REG));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        public void STS_E() //0x10FF
        {
            MemWrite16(S_REG, MemRead16(PC_REG));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;

            unsafe
            {
                _instance->CycleCounter += 7;
            }
        }

        #endregion
    }
}
