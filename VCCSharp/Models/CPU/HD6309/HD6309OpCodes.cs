using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.HD6309
{
    public interface IHD6309OpCodes
    {
        void Exec(byte opCode);
    }

    public class HD6309OpCodes : IHD6309OpCodes
    {
        private readonly IModules _modules;

        private static readonly Action<byte> _page1 = Library.HD6309.Page_1;
        private static readonly Action<byte> _page2 = Library.HD6309.Page_2;
        private static readonly Action<byte> _page3 = Library.HD6309.Page_3;

        private unsafe HD6309State* instance => _modules.HD6309.GetHD6309State();

        private byte temp8;
        private ushort temp16;
        private byte postbyte = 0;
        private ushort postword = 0;

        private byte Source = 0;
        private byte Dest = 0;

        #region Jump Vectors

        private static Action[] JmpVec1 = new Action[256];

        private void InitializeJmpVec1()
        {
            JmpVec1 = new Action[]
            {
                Neg_D, // 00
                Oim_D, // 01
                Aim_D, // 02
                Com_D, // 03
                Lsr_D, // 04
                Eim_D, // 05
                Ror_D, // 06
                Asr_D, // 07
                Asl_D, // 08
                Rol_D, // 09
                Dec_D, // 0A
                Tim_D, // 0B
                Inc_D, // 0C
                Tst_D, // 0D
                Jmp_D, // 0E
                Clr_D, // 0F
                Page_2, // 10
                Page_3, // 11
                Nop_I, // 12
                Sync_I, // 13
                Sexw_I, // 14
                InvalidInsHandler, // 15
                Lbra_R, // 16
                Lbsr_R, // 17
                InvalidInsHandler, // 18
                Daa_I, // 19
                Orcc_M, // 1A
                InvalidInsHandler, // 1B
                Andcc_M, // 1C
                Sex_I, // 1D
                Exg_M, // 1E
                Tfr_M, // 1F
                Bra_R, // 20
                Brn_R, // 21
                Bhi_R, // 22
                Bls_R, // 23
                Bhs_R, // 24
                Blo_R, // 25
                Bne_R, // 26
                Beq_R, // 27
                Bvc_R, // 28
                Bvs_R, // 29
                Bpl_R, // 2A
                Bmi_R, // 2B
                Bge_R, // 2C
                Blt_R, // 2D
                Bgt_R, // 2E
                Ble_R, // 2F
                Leax_X, // 30
                Leay_X, // 31
                Leas_X, // 32
                Leau_X, // 33
                Pshs_M, // 34
                Puls_M, // 35
                Pshu_M, // 36
                Pulu_M, // 37
                InvalidInsHandler, // 38
                Rts_I, // 39
                Abx_I, // 3A
                Rti_I, // 3B
                Cwai_I, // 3C
                Mul_I, // 3D
                Reset, // 3E
                Swi1_I, // 3F
                Nega_I, // 40
                InvalidInsHandler, // 41
                InvalidInsHandler, // 42
                Coma_I, // 43
                Lsra_I, // 44
                InvalidInsHandler, // 45
                Rora_I, // 46
                Asra_I, // 47
                Asla_I, // 48
                Rola_I, // 49
                Deca_I, // 4A
                InvalidInsHandler, // 4B
                Inca_I, // 4C
                Tsta_I, // 4D
                InvalidInsHandler, // 4E
                Clra_I, // 4F
                Negb_I, // 50
                InvalidInsHandler, // 51
                InvalidInsHandler, // 52
                Comb_I, // 53
                Lsrb_I, // 54
                InvalidInsHandler, // 55
                Rorb_I, // 56
                Asrb_I, // 57
                Aslb_I, // 58
                Rolb_I, // 59
                Decb_I, // 5A
                InvalidInsHandler, // 5B
                Incb_I, // 5C
                Tstb_I, // 5D
                InvalidInsHandler, // 5E
                Clrb_I, // 5F
                Neg_X, // 60
                Oim_X, // 61
                Aim_X, // 62
                Com_X, // 63
                Lsr_X, // 64
                Eim_X, // 65
                Ror_X, // 66
                Asr_X, // 67
                Asl_X, // 68
                Rol_X, // 69
                Dec_X, // 6A
                Tim_X, // 6B
                Inc_X, // 6C
                Tst_X, // 6D
                Jmp_X, // 6E
                Clr_X, // 6F
                Neg_E, // 70
                Oim_E, // 71
                Aim_E, // 72
                Com_E, // 73
                Lsr_E, // 74
                Eim_E, // 75
                Ror_E, // 76
                Asr_E, // 77
                Asl_E, // 78
                Rol_E, // 79
                Dec_E, // 7A
                Tim_E, // 7B
                Inc_E, // 7C
                Tst_E, // 7D
                Jmp_E, // 7E
                Clr_E, // 7F
                Suba_M, // 80
                Cmpa_M, // 81
                Sbca_M, // 82
                Subd_M, // 83
                Anda_M, // 84
                Bita_M, // 85
                Lda_M, // 86
                InvalidInsHandler, // 87
                Eora_M, // 88
                Adca_M, // 89
                Ora_M, // 8A
                Adda_M, // 8B
                Cmpx_M, // 8C
                Bsr_R, // 8D
                Ldx_M, // 8E
                InvalidInsHandler, // 8F
                Suba_D, // 90
                Cmpa_D, // 91
                Scba_D, // 92
                Subd_D, // 93
                Anda_D, // 94
                Bita_D, // 95
                Lda_D, // 96
                Sta_D, // 97
                Eora_D, // 98
                Adca_D, // 99
                Ora_D, // 9A
                Adda_D, // 9B
                Cmpx_D, // 9C
                Jsr_D, // 9D
                Ldx_D, // 9E
                Stx_D, // 9A
                Suba_X, // A0
                Cmpa_X, // A1
                Sbca_X, // A2
                Subd_X, // A3
                Anda_X, // A4
                Bita_X, // A5
                Lda_X, // A6
                Sta_X, // A7
                Eora_X, // a8
                Adca_X, // A9
                Ora_X, // AA
                Adda_X, // AB
                Cmpx_X, // AC
                Jsr_X, // AD
                Ldx_X, // AE
                Stx_X, // AF
                Suba_E, // B0
                Cmpa_E, // B1
                Sbca_E, // B2
                Subd_E, // B3
                Anda_E, // B4
                Bita_E, // B5
                Lda_E, // B6
                Sta_E, // B7
                Eora_E, // B8
                Adca_E, // B9
                Ora_E, // BA
                Adda_E, // BB
                Cmpx_E, // BC
                Bsr_E, // BD
                Ldx_E, // BE
                Stx_E, // BF
                Subb_M, // C0
                Cmpb_M, // C1
                Sbcb_M, // C2
                Addd_M, // C3
                Andb_M, // C4
                Bitb_M, // C5
                Ldb_M, // C6
                InvalidInsHandler, // C7
                Eorb_M, // C8
                Adcb_M, // C9
                Orb_M, // CA
                Addb_M, // CB
                Ldd_M, // CC
                Ldq_M, // CD
                Ldu_M, // CE
                InvalidInsHandler, // CF
                Subb_D, // D0
                Cmpb_D, // D1
                Sbcb_D, // D2
                Addd_D, // D3
                Andb_D, // D4
                Bitb_D, // D5
                Ldb_D, // D6
                Stb_D, // D7
                Eorb_D, // D8
                Adcb_D, // D9
                Orb_D, // DA
                Addb_D, // DB
                Ldd_D, // DC
                Std_D, // DD
                Ldu_D, // DE
                Stu_D, // DF
                Subb_X, // E0
                Cmpb_X, // E1
                Sbcb_X, // E2
                Addd_X, // E3
                Andb_X, // E4
                Bitb_X, // E5
                Ldb_X, // E6
                Stb_X, // E7
                Eorb_X, // E8
                Adcb_X, // E9
                Orb_X, // EA
                Addb_X, // EB
                Ldd_X, // EC
                Std_X, // ED
                Ldu_X, // EE
                Stu_X, // EF
                Subb_E, // F0
                Cmpb_E, // F1
                Sbcb_E, // F2
                Addd_E, // F3
                Andb_E, // F4
                Bitb_E, // F5
                Ldb_E, // F6
                Stb_E, // F7
                Eorb_E, // F8
                Adcb_E, // F9
                Orb_E, // FA
                Addb_E, // FB
                Ldd_E, // FC
                Std_E, // FD
                Ldu_E, // FE
                Stu_E, // FF
            };
        }

        #endregion

        public HD6309OpCodes(IModules modules)
        {
            _modules = modules;

            InitializeJmpVec1();
        }

        public void Exec(byte opCode)
        {
            JmpVec1[opCode]();
        }

        #region Page1 OpCodes

        #region 0x00 - 0x0F

        public unsafe void Neg_D()// 00
        {
            temp16 = DPADDRESS(PC_REG++);
            postbyte = MemRead8(temp16);
            temp8 = (byte)(0 - postbyte);

            CC_C = temp8 > 0;
            CC_V = postbyte == 0x80;
            CC_N = NTEST8(temp8);
            CC_Z = ZTEST(temp8);

            MemWrite8(temp8, temp16);

            instance->CycleCounter += instance->NatEmuCycles65;
        }

        //1 6309
        public unsafe void Oim_D()// 01
        {
            postbyte = MemRead8(PC_REG++);
            temp16 = DPADDRESS(PC_REG++);
            postbyte |= MemRead8(temp16);

            MemWrite8(postbyte, temp16);

            CC_N = NTEST8(postbyte);
            CC_Z = ZTEST(postbyte);
            CC_V = false; //0

            instance->CycleCounter += 6;
        }

        //2 Phase 2 6309
        public unsafe void Aim_D()// 02
        {
            postbyte = MemRead8(PC_REG++);
            temp16 = DPADDRESS(PC_REG++);
            postbyte &= MemRead8(temp16);

            MemWrite8(postbyte, temp16);

            CC_N = NTEST8(postbyte);
            CC_Z = ZTEST(postbyte);
            CC_V = false; //0;

            instance->CycleCounter += 6;
        }

        public unsafe void Com_D()// 03
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            temp8 = (byte)(0xFF - temp8);

            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            CC_C = true; //1;
            CC_V = false; //0;

            MemWrite8(temp8, temp16);

            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Lsr_D()// 04
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            CC_C = (temp8 & 1) != 0;
            temp8 = (byte)(temp8 >> 1);
            CC_Z = ZTEST(temp8);
            CC_N = false; //0;
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        //6309 Untested
        public unsafe void Eim_D()// 05
        {
            postbyte = MemRead8(PC_REG++);
            temp16 = DPADDRESS(PC_REG++);
            postbyte ^= MemRead8(temp16);
            MemWrite8(postbyte, temp16);
            CC_N = NTEST8(postbyte);
            CC_Z = ZTEST(postbyte);
            CC_V = false; //0;
            instance->CycleCounter += 6;
        }

        public unsafe void Ror_D()// 06
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            postbyte = (byte)((CC_C ? 1 : 0) << 7);
            CC_C = (temp8 & 1) != 0;
            temp8 = (byte)((temp8 >> 1) | postbyte);
            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Asr_D()// 07
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            CC_C = (temp8 & 1) != 0;
            temp8 = (byte)((temp8 & 0x80) | (temp8 >> 1));
            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Asl_D()// 08
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            CC_C = ((temp8 & 0x80) >> 7) != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((temp8 & 0x40) >> 6)) != 0;
            temp8 = (byte)(temp8 << 1);
            CC_N = NTEST8(temp8);
            CC_Z = ZTEST(temp8);
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Rol_D()// 09
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = MemRead8(temp16);
            postbyte = (byte)(CC_C ? 1 : 0);
            CC_C = ((temp8 & 0x80) >> 7) != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((temp8 & 0x40) >> 6)) != 0;
            temp8 = (byte)((temp8 << 1) | postbyte);
            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Dec_D()// 0A
        {
            temp16 = DPADDRESS(PC_REG++);
            temp8 = (byte)(MemRead8(temp16) - 1);
            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            CC_V = temp8 == 0x7F;
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        //6309 Untested wcreate
        public unsafe void Tim_D()// 0B
        {
            postbyte = MemRead8(PC_REG++);
            temp8 = MemRead8(DPADDRESS(PC_REG++));
            postbyte &= temp8;
            CC_N = NTEST8(postbyte);
            CC_Z = ZTEST(postbyte);
            CC_V = false; //0;
            instance->CycleCounter += 6;
        }

        public unsafe void Inc_D()// 0C
        {
            temp16 = (DPADDRESS(PC_REG++));
            temp8 = (byte)(MemRead8(temp16) + 1);
            CC_Z = ZTEST(temp8);
            CC_V = temp8 == 0x80;
            CC_N = NTEST8(temp8);
            MemWrite8(temp8, temp16);
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        public unsafe void Tst_D()// 0D
        {
            temp8 = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(temp8);
            CC_N = NTEST8(temp8);
            CC_V = false; //0;
            instance->CycleCounter += instance->NatEmuCycles64;
        }

        public unsafe void Jmp_D()// 0E
        {
            PC_REG = (ushort)(DP_REG | MemRead8(PC_REG));
            instance->CycleCounter += instance->NatEmuCycles32;
        }

        public unsafe void Clr_D()// 0F
        {
            MemWrite8(0, DPADDRESS(PC_REG++));
            CC_Z = true; //1;
            CC_N = false; //0;
            CC_V = false; //0;
            CC_C = false; //0;
            instance->CycleCounter += instance->NatEmuCycles65;
        }

        #endregion

        #region 0x10 - 0x1F

        public void Page_2()// 10
        {
            unsafe
            {
                byte opCode = MemRead8(PC_REG++);

                _page2(opCode);
            }
        }

        public void Page_3()// 11
        {
            unsafe
            {
                byte opCode = MemRead8(PC_REG++);

                _page3(opCode);
            }
        }

        public unsafe void Nop_I()// 12
        {
            instance->CycleCounter += instance->NatEmuCycles21;
        }

        public unsafe void Sync_I()// 13
        {
            instance->CycleCounter = instance->gCycleFor;
            instance->SyncWaiting = 1;
        }

        //6309 CHECK
        public unsafe void Sexw_I()// 14
        {
            D_REG = (W_REG & 32768) != 0 ? (ushort)0xFFFF : (ushort)0;

            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST16(D_REG);
            instance->CycleCounter += 4;
        }

        // 15		//InvalidInsHandler

        public unsafe void Lbra_R()// 16
        {
            fixed (ushort* spostword = &postword)
            {
                *spostword = IMMADDRESS(PC_REG);
                PC_REG += 2;
                PC_REG += *spostword;
            };

            instance->CycleCounter += instance->NatEmuCycles54;
        }

        public unsafe void Lbsr_R()// 17
        {
            fixed (ushort* spostword = &postword)
            {
                *spostword = IMMADDRESS(PC_REG);
                PC_REG += 2;
                S_REG--;
                MemWrite8(PC_L, S_REG--);
                MemWrite8(PC_H, S_REG);
                PC_REG += *spostword;
            }

            instance->CycleCounter += instance->NatEmuCycles97;
        }

        // 18		//InvalidInsHandler

        public unsafe void Daa_I()// 19
        {
            byte msn, lsn;

            msn = (byte)(A_REG & 0xF0);
            lsn = (byte)(A_REG & 0xF);
            temp8 = 0;

            if (CC_H || (lsn > 9))
            {
                temp8 |= 0x06;
            }

            if ((msn > 0x80) && (lsn > 9))
            {
                temp8 |= 0x60;
            }

            if ((msn > 0x90) || CC_C)
            {
                temp8 |= 0x60;
            }

            temp16 = (ushort)(A_REG + temp8);
            CC_C |= (((temp16 & 0x100) >> 8) != 0);
            A_REG = (byte)temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            instance->CycleCounter += instance->NatEmuCycles21;
        }

        public unsafe void Orcc_M()// 1A
        {
            postbyte = MemRead8(PC_REG++);
            temp8 = HD6309_getcc();
            temp8 = (byte)(temp8 | postbyte);
            HD6309_setcc(temp8);
            instance->CycleCounter += instance->NatEmuCycles32;
        }

        // 1B		//InvalidInsHandler

        public unsafe void Andcc_M()// 1C
        {
            postbyte = MemRead8(PC_REG++);
            temp8 = HD6309_getcc();
            temp8 = (byte)(temp8 & postbyte);
            HD6309_setcc(temp8);
            instance->CycleCounter += 3;
        }

        public unsafe void Sex_I()// 1D
        {
            A_REG = (byte)(0 - (B_REG >> 7));
            CC_Z = ZTEST(D_REG);
            CC_N = (D_REG >> 15) != 0;
            instance->CycleCounter += instance->NatEmuCycles21;
        }

        public unsafe void Exg_M()// 1E
        {
            byte tmp;

            postbyte = MemRead8(PC_REG++);
            Source = (byte)(postbyte >> 4);
            Dest = (byte)(postbyte & 15);

            instance->ccbits = HD6309_getcc();

            if ((Source & 0x08) == (Dest & 0x08)) //Verify like size registers
            {
                if ((Dest & 0x08) != 0) //8 bit EXG
                {
                    Source &= 0x07;
                    Dest &= 0x07;
                    temp8 = (PUR(Source));
                    PUR(Source, (PUR(Dest)));
                    PUR(Dest, temp8);
                    O_REG = 0;
                }
                else // 16 bit EXG
                {
                    Source &= 0x07;
                    Dest &= 0x07;
                    temp16 = (PXF(Source));
                    PXF(Source, Dest);
                    PXF(Dest, temp16);
                }
            }
            else
            {
                if ((Dest & 0x08) != 0) // Swap 16 to 8 bit exchange to be 8 to 16 bit exchange (for convenience)
                {
                    temp8 = Dest; Dest = Source; Source = temp8;
                }

                Source &= 0x07;
                Dest &= 0x07;

                switch (Source)
                {
                    case 0x04: // Z
                    case 0x05: // Z
                        PXF(Dest, 0); // Source is Zero reg. Just zero the Destination.
                        break;

                    case 0x00: // A
                    case 0x03: // DP
                    case 0x06: // E
                        temp8 = PUR(Source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(PXF(Dest) >> 8);
                        PUR(Source, tmp); // A, DP, E get high byte of 16 bit Dest
                        PXF(Dest, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;

                    case 0x01: // B
                    case 0x02: // CC
                    case 0x07: // F
                        temp8 = PUR(Source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(PXF(Dest) & 0xFF);
                        PUR(Source, tmp); // B, CC, F get low byte of 16 bit Dest
                        PXF(Dest, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;
                }
            }

            HD6309_setcc(instance->ccbits);
            instance->CycleCounter += instance->NatEmuCycles85;
        }

        public unsafe void Tfr_M()// 1F
        {
            postbyte = MemRead8(PC_REG++);
            Source = (byte)(postbyte >> 4);
            Dest = (byte)(postbyte & 15);

            if (Dest < 8)
            {
                if (Source < 8)
                {
                    PXF(Dest, PXF(Source));
                }
                else
                {
                    PXF(Dest, (ushort)((PUR(Source & 7) << 8) | PUR(Source & 7)));
                }
            }
            else
            {
                instance->ccbits = HD6309_getcc();
                Dest &= 7;

                if (Source < 8)
                    switch (Dest)
                    {
                        case 0:  // A
                        case 3: // DP
                        case 6: // E
                            PUR(Dest, (byte)(PXF(Source) >> 8));
                            break;
                        case 1:  // B
                        case 2: // CC
                        case 7: // F
                            PUR(Dest, (byte)(PXF(Source) & 0xFF));
                            break;
                    }
                else
                {
                    PUR(Dest, PUR(Source & 7));
                }

                O_REG = 0;
                HD6309_setcc(instance->ccbits);
            }

            instance->CycleCounter += instance->NatEmuCycles64;
        }

        #endregion

        #region 0x20 - 0x2F

        public void Bra_R()// 20
        {
            _page1(0x20);
        }

        public void Brn_R()// 21
        {
            _page1(0x21);
        }

        public void Bhi_R()// 22
        {
            _page1(0x22);
        }

        public void Bls_R()// 23
        {
            _page1(0x23);
        }

        public void Bhs_R()// 24
        {
            _page1(0x24);
        }

        public void Blo_R()// 25
        {
            _page1(0x25);
        }

        public void Bne_R()// 26
        {
            _page1(0x26);
        }

        public void Beq_R()// 27
        {
            _page1(0x27);
        }

        public void Bvc_R()// 28
        {
            _page1(0x28);
        }

        public void Bvs_R()// 29
        {
            _page1(0x29);
        }

        public void Bpl_R()// 2A
        {
            _page1(0x2A);
        }

        public void Bmi_R()// 2B
        {
            _page1(0x2B);
        }

        public void Bge_R()// 2C
        {
            _page1(0x2C);
        }

        public void Blt_R()// 2D
        {
            _page1(0x2D);
        }

        public void Bgt_R()// 2E
        {
            _page1(0x2E);
        }

        public void Ble_R()// 2F
        {
            _page1(0x2F);
        }

        #endregion

        #region 0x30 - 0x3F

        public void Leax_X()// 30
        {
            _page1(0x30);
        }

        public void Leay_X()// 31
        {
            _page1(0x31);
        }

        public void Leas_X()// 32
        {
            _page1(0x32);
        }

        public void Leau_X()// 33
        {
            _page1(0x33);
        }

        public void Pshs_M()// 34
        {
            _page1(0x34);
        }

        public void Puls_M()// 35
        {
            _page1(0x35);
        }

        public void Pshu_M()// 36
        {
            _page1(0x36);
        }

        public void Pulu_M()// 37
        {
            _page1(0x37);
        }

        // 38		//InvalidInsHandler

        public void Rts_I()// 39
        {
            _page1(0x39);
        }

        public void Abx_I()// 3A
        {
            _page1(0x3A);
        }

        public void Rti_I()// 3B
        {
            _page1(0x3B);
        }

        public void Cwai_I()// 3C
        {
            _page1(0x3C);
        }

        public void Mul_I()// 3D
        {
            _page1(0x3D);
        }

        public void Reset()// 3E
        {
            _page1(0x3E);
        }

        public void Swi1_I()// 3F
        {
            _page1(0x3F);
        }

        #endregion

        #region 0x40 - 0x4F

        public void Nega_I()// 40
        {
            _page1(0x40);
        }

        // 41		//InvalidInsHandler

        // 42		//InvalidInsHandler

        public void Coma_I()// 43
        {
            _page1(0x43);
        }

        public void Lsra_I()// 44
        {
            _page1(0x44);
        }

        // 45		//InvalidInsHandler

        public void Rora_I()// 46
        {
            _page1(0x46);
        }

        public void Asra_I()// 47
        {
            _page1(0x47);
        }

        public void Asla_I()// 48
        {
            _page1(0x48);
        }

        public void Rola_I()// 49
        {
            _page1(0x49);
        }

        public void Deca_I()// 4A
        {
            _page1(0x4A);
        }

        // 4B		//InvalidInsHandler

        public void Inca_I()// 4C
        {
            _page1(0x4C);
        }

        public void Tsta_I()// 4D
        {
            _page1(0x4D);
        }

        // 4E		//InvalidInsHandler

        public void Clra_I()// 4F
        {
            _page1(0x4F);
        }

        #endregion

        #region 0x50 - 0x5F

        public void Negb_I()// 50
        {
            _page1(0x50);
        }

        // 51		//InvalidInsHandler

        // 52		//InvalidInsHandler

        public void Comb_I()// 53
        {
            _page1(0x53);
        }

        public void Lsrb_I()// 54
        {
            _page1(0x54);
        }

        // 55		//InvalidInsHandler

        public void Rorb_I()// 56
        {
            _page1(0x56);
        }

        public void Asrb_I()// 57
        {
            _page1(0x57);
        }

        public void Aslb_I()// 58
        {
            _page1(0x58);
        }

        public void Rolb_I()// 59
        {
            _page1(0x59);
        }

        public void Decb_I()// 5A
        {
            _page1(0x5A);
        }

        // 5B		//InvalidInsHandler

        public void Incb_I()// 5C
        {
            _page1(0x5C);
        }

        public void Tstb_I()// 5D
        {
            _page1(0x5D);
        }

        // 5E		//InvalidInsHandler

        public void Clrb_I()// 5F
        {
            _page1(0x5F);
        }

        #endregion

        #region 0x60 - 0x6F

        public void Neg_X()// 60
        {
            _page1(0x60);
        }

        public void Oim_X()// 61
        {
            _page1(0x61);
        }

        public void Aim_X()// 62
        {
            _page1(0x62);
        }

        public void Com_X()// 63
        {
            _page1(0x63);
        }

        public void Lsr_X()// 64
        {
            _page1(0x64);
        }

        public void Eim_X()// 65
        {
            _page1(0x65);
        }

        public void Ror_X()// 66
        {
            _page1(0x66);
        }

        public void Asr_X()// 67
        {
            _page1(0x67);
        }

        public void Asl_X()// 68
        {
            _page1(0x68);
        }

        public void Rol_X()// 69
        {
            _page1(0x69);
        }

        public void Dec_X()// 6A
        {
            _page1(0x6A);
        }

        public void Tim_X()// 6B
        {
            _page1(0x6B);
        }

        public void Inc_X()// 6C
        {
            _page1(0x6C);
        }

        public void Tst_X()// 6D
        {
            _page1(0x6D);
        }

        public void Jmp_X()// 6E
        {
            _page1(0x6E);
        }

        public void Clr_X()// 6F
        {
            _page1(0x6F);
        }

        #endregion

        #region 0x70 - 0x7F

        public void Neg_E()// 70
        {
            _page1(0x70);
        }

        public void Oim_E()// 71
        {
            _page1(0x71);
        }

        public void Aim_E()// 72
        {
            _page1(0x72);
        }

        public void Com_E()// 73
        {
            _page1(0x73);
        }

        public void Lsr_E()// 74
        {
            _page1(0x74);
        }

        public void Eim_E()// 75
        {
            _page1(0x75);
        }

        public void Ror_E()// 76
        {
            _page1(0x76);
        }

        public void Asr_E()// 77
        {
            _page1(0x77);
        }

        public void Asl_E()// 78
        {
            _page1(0x78);
        }

        public void Rol_E()// 79
        {
            _page1(0x79);
        }

        public void Dec_E()// 7A
        {
            _page1(0x7A);
        }

        public void Tim_E()// 7B
        {
            _page1(0x7B);
        }

        public void Inc_E()// 7C
        {
            _page1(0x7C);
        }

        public void Tst_E()// 7D
        {
            _page1(0x7D);
        }

        public void Jmp_E()// 7E
        {
            _page1(0x7E);
        }

        public void Clr_E()// 7F
        {
            _page1(0x7F);
        }

        #endregion

        #region 0x80 - 0x8F

        public void Suba_M()// 80
        {
            _page1(0x80);
        }

        public void Cmpa_M()// 81
        {
            _page1(0x81);
        }

        public void Sbca_M()// 82
        {
            _page1(0x82);
        }

        public void Subd_M()// 83
        {
            _page1(0x83);
        }

        public void Anda_M()// 84
        {
            _page1(0x84);
        }

        public void Bita_M()// 85
        {
            _page1(0x85);
        }

        public void Lda_M()// 86
        {
            _page1(0x86);
        }

        // 87		//InvalidInsHandler

        public void Eora_M()// 88
        {
            _page1(0x88);
        }

        public void Adca_M()// 89
        {
            _page1(0x89);
        }

        public void Ora_M()// 8A
        {
            _page1(0x8A);
        }

        public void Adda_M()// 8B
        {
            _page1(0x8B);
        }

        public void Cmpx_M()// 8C
        {
            _page1(0x8C);
        }

        public void Bsr_R()// 8D
        {
            _page1(0x8D);
        }

        public void Ldx_M()// 8E
        {
            _page1(0x8E);
        }

        // 8F		//InvalidInsHandler

        #endregion

        #region 0x90 - 0x9F

        public void Suba_D()// 90
        {
            _page1(0x90);
        }

        public void Cmpa_D()// 91
        {
            _page1(0x91);
        }

        public void Scba_D()// 92
        {
            _page1(0x92);
        }

        public void Subd_D()// 93
        {
            _page1(0x93);
        }

        public void Anda_D()// 94
        {
            _page1(0x94);
        }

        public void Bita_D()// 95
        {
            _page1(0x95);
        }

        public void Lda_D()// 96
        {
            _page1(0x96);
        }

        public void Sta_D()// 97
        {
            _page1(0x97);
        }

        public void Eora_D()// 98
        {
            _page1(0x98);
        }

        public void Adca_D()// 99
        {
            _page1(0x99);
        }

        public void Ora_D()// 9A
        {
            _page1(0x9A);
        }

        public void Adda_D()// 9B
        {
            _page1(0x9B);
        }

        public void Cmpx_D()// 9C
        {
            _page1(0x9C);
        }

        public void Jsr_D()// 9D
        {
            _page1(0x9D);
        }

        public void Ldx_D()// 9E
        {
            _page1(0x9E);
        }

        public void Stx_D()// 9F
        {
            _page1(0x9F);
        }

        #endregion

        #region 0xA0 - 0xAF

        public void Suba_X()// A0
        {
            _page1(0xA0);
        }

        public void Cmpa_X()// A1
        {
            _page1(0xA1);
        }

        public void Sbca_X()// A2
        {
            _page1(0xA2);
        }

        public void Subd_X()// A3
        {
            _page1(0xA3);
        }

        public void Anda_X()// A4
        {
            _page1(0xA4);
        }

        public void Bita_X()// A5
        {
            _page1(0xA5);
        }

        public void Lda_X()// A6
        {
            _page1(0xA6);
        }

        public void Sta_X()// A7
        {
            _page1(0xA7);
        }

        public void Eora_X()// a8
        {
            _page1(0xA8);
        }

        public void Adca_X()// A9
        {
            _page1(0xA9);
        }

        public void Ora_X()// AA
        {
            _page1(0xAA);
        }

        public void Adda_X()// AB
        {
            _page1(0xAB);
        }

        public void Cmpx_X()// AC
        {
            _page1(0xAC);
        }

        public void Jsr_X()// AD
        {
            _page1(0xAD);
        }

        public void Ldx_X()// AE
        {
            _page1(0xAE);
        }

        public void Stx_X()// AF
        {
            _page1(0xAF);
        }

        #endregion

        #region 0xB0 - 0xBF

        public void Suba_E()// B0
        {
            _page1(0xB0);
        }

        public void Cmpa_E()// B1
        {
            _page1(0xB1);
        }

        public void Sbca_E()// B2
        {
            _page1(0xB2);
        }

        public void Subd_E()// B3
        {
            _page1(0xB3);
        }

        public void Anda_E()// B4
        {
            _page1(0xB4);
        }

        public void Bita_E()// B5
        {
            _page1(0xB5);
        }

        public void Lda_E()// B6
        {
            _page1(0xB6);
        }

        public void Sta_E()// B7
        {
            _page1(0xB7);
        }

        public void Eora_E()// B8
        {
            _page1(0xB8);
        }

        public void Adca_E()// B9
        {
            _page1(0xB9);
        }

        public void Ora_E()// BA
        {
            _page1(0xBA);
        }

        public void Adda_E()// BB
        {
            _page1(0xBB);
        }

        public void Cmpx_E()// BC
        {
            _page1(0xBC);
        }

        public void Bsr_E()// BD
        {
            _page1(0xBD);
        }

        public void Ldx_E()// BE
        {
            _page1(0xBE);
        }

        public void Stx_E()// BF
        {
            _page1(0xBF);
        }

        #endregion

        #region 0xC0 - 0CF

        public void Subb_M()// C0
        {
            _page1(0xC0);
        }

        public void Cmpb_M()// C1
        {
            _page1(0xC1);
        }

        public void Sbcb_M()// C2
        {
            _page1(0xC2);
        }

        public void Addd_M()// C3
        {
            _page1(0xC3);
        }

        public void Andb_M()// C4
        {
            _page1(0xC4);
        }

        public void Bitb_M()// C5
        {
            _page1(0xC5);
        }

        public void Ldb_M()// C6
        {
            _page1(0xC6);
        }

        // C7	//InvalidInsHandler

        public void Eorb_M()// C8
        {
            _page1(0xC8);
        }

        public void Adcb_M()// C9
        {
            _page1(0xC9);
        }

        public void Orb_M()// CA
        {
            _page1(0xCA);
        }

        public void Addb_M()// CB
        {
            _page1(0xCB);
        }

        public void Ldd_M()// CC
        {
            _page1(0xCC);
        }

        public void Ldq_M()// CD
        {
            _page1(0xCD);
        }

        public void Ldu_M()// CE
        {
            _page1(0xCE);
        }

        // CF	//InvalidInsHandler

        #endregion

        #region 0xD0 - 0xDF

        public void Subb_D()// D0
        {
            _page1(0xD0);
        }

        public void Cmpb_D()// D1
        {
            _page1(0xD1);
        }

        public void Sbcb_D()// D2
        {
            _page1(0xD2);
        }

        public void Addd_D()// D3
        {
            _page1(0xD3);
        }

        public void Andb_D()// D4
        {
            _page1(0xD4);
        }

        public void Bitb_D()// D5
        {
            _page1(0xD5);
        }

        public void Ldb_D()// D6
        {
            _page1(0xD6);
        }

        public void Stb_D()// D7
        {
            _page1(0xD7);
        }

        public void Eorb_D()// D8
        {
            _page1(0xD8);
        }

        public void Adcb_D()// D9
        {
            _page1(0xD9);
        }

        public void Orb_D()// DA
        {
            _page1(0xDA);
        }

        public void Addb_D()// DB
        {
            _page1(0xDB);
        }

        public void Ldd_D()// DC
        {
            _page1(0xDC);
        }

        public void Std_D()// DD
        {
            _page1(0xDD);
        }

        public void Ldu_D()// DE
        {
            _page1(0xDE);
        }

        public void Stu_D()// DF
        {
            _page1(0xDF);
        }

        #endregion

        #region 0xE0 - 0xEF

        public void Subb_X()// E0
        {
            _page1(0xE0);
        }

        public void Cmpb_X()// E1
        {
            _page1(0xE1);
        }

        public void Sbcb_X()// E2
        {
            _page1(0xE2);
        }

        public void Addd_X()// E3
        {
            _page1(0xE3);
        }

        public void Andb_X()// E4
        {
            _page1(0xE4);
        }

        public void Bitb_X()// E5
        {
            _page1(0xE5);
        }

        public void Ldb_X()// E6
        {
            _page1(0xE6);
        }

        public void Stb_X()// E7
        {
            _page1(0xE7);
        }

        public void Eorb_X()// E8
        {
            _page1(0xE8);
        }

        public void Adcb_X()// E9
        {
            _page1(0xE9);
        }

        public void Orb_X()// EA
        {
            _page1(0xEA);
        }

        public void Addb_X()// EB
        {
            _page1(0xEB);
        }

        public void Ldd_X()// EC
        {
            _page1(0xEC);
        }

        public void Std_X()// ED
        {
            _page1(0xED);
        }

        public void Ldu_X()// EE
        {
            _page1(0xEE);
        }

        public void Stu_X()// EF
        {
            _page1(0xEF);
        }

        #endregion

        #region 0xF0 - 0xFF

        public void Subb_E()// F0
        {
            _page1(0xF0);
        }

        public void Cmpb_E()// F1
        {
            _page1(0xF1);
        }

        public void Sbcb_E()// F2
        {
            _page1(0xF2);
        }

        public void Addd_E()// F3
        {
            _page1(0xF3);
        }

        public void Andb_E()// F4
        {
            _page1(0xF4);
        }

        public void Bitb_E()// F5
        {
            _page1(0xF5);
        }

        public void Ldb_E()// F6
        {
            _page1(0xF6);
        }

        public void Stb_E()// F7
        {
            _page1(0xF7);
        }

        public void Eorb_E()// F8
        {
            _page1(0xF8);
        }

        public void Adcb_E()// F9
        {
            _page1(0xF9);
        }

        public void Orb_E()// FA
        {
            _page1(0xFA);
        }

        public void Addb_E()// FB
        {
            _page1(0xFB);
        }

        public void Ldd_E()// FC
        {
            _page1(0xFC);
        }

        public void Std_E()// FD
        {
            _page1(0xFD);
        }

        public void Ldu_E()// FE
        {
            _page1(0xFE);
        }

        public void Stu_E()// FF
        {
            _page1(0xFF);
        }

        #endregion

        #endregion

        public unsafe void InvalidInsHandler()
        {
            MD_ILLEGAL = true;
            instance->mdbits = HD6309_getmd();

            ErrorVector();
        }

        public unsafe void ErrorVector()
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
                MemWrite8(F_REG, --S_REG);
                MemWrite8(E_REG, --S_REG);

                instance->CycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(HD6309_getcc(), --S_REG);

            PC_REG = MemRead16(Define.VTRAP);

            instance->CycleCounter += (12 + instance->NatEmuCycles54);	//One for each byte +overhead? Guessing from PSHS
        }

        #region CC Masks Macros

        public unsafe bool CC_E
        {
            get => instance->cc[(int)CCFlagMasks.E] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.E] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_F
        {
            get => instance->cc[(int)CCFlagMasks.F] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.F] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_H
        {
            get => instance->cc[(int)CCFlagMasks.H] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.H] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_I
        {
            get => instance->cc[(int)CCFlagMasks.I] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.I] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_N
        {
            get => instance->cc[(int)CCFlagMasks.N] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.N] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_Z
        {
            get => instance->cc[(int)CCFlagMasks.Z] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.Z] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_V
        {
            get => instance->cc[(int)CCFlagMasks.V] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.V] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool CC_C
        {
            get => instance->cc[(int)CCFlagMasks.C] == Define.TRUE;
            set => instance->cc[(int)CCFlagMasks.C] = value ? Define.TRUE : Define.FALSE;
        }

        #endregion

        #region Register Macros

        public unsafe ushort PC_REG
        {
            get => instance->pc.Reg;
            set => instance->pc.Reg = value;
        }

        public unsafe ushort DP_REG
        {
            get => instance->dp.Reg;
            set => instance->dp.Reg = value;
        }

        public unsafe ushort W_REG
        {
            get => instance->q.msw;
            set => instance->q.msw = value;
        }

        public unsafe ushort D_REG
        {
            get => instance->q.lsw;
            set => instance->q.lsw = value;
        }

        public unsafe uint Q_REG
        {
            get => instance->q.Reg;
            set => instance->q.Reg = value;
        }

        public unsafe ushort S_REG
        {
            get => instance->s.Reg;
            set => instance->s.Reg = value;
        }

        public unsafe byte PC_L
        {
            get => instance->pc.lsb;
            set => instance->pc.lsb = value;
        }

        public unsafe byte PC_H
        {
            get => instance->pc.msb;
            set => instance->pc.msb = value;
        }

        public unsafe byte X_L
        {
            get => instance->x.lsb;
            set => instance->x.lsb = value;
        }

        public unsafe byte X_H
        {
            get => instance->x.msb;
            set => instance->x.msb = value;
        }

        public unsafe byte Y_L
        {
            get => instance->y.lsb;
            set => instance->y.lsb = value;
        }

        public unsafe byte Y_H
        {
            get => instance->y.msb;
            set => instance->y.msb = value;
        }

        public unsafe byte U_L
        {
            get => instance->u.lsb;
            set => instance->u.lsb = value;
        }

        public unsafe byte U_H
        {
            get => instance->u.msb;
            set => instance->u.msb = value;
        }

        public unsafe byte A_REG
        {
            get => instance->q.lswmsb;
            set => instance->q.lswmsb = value;
        }

        public unsafe byte B_REG
        {
            get => instance->q.lswlsb;
            set => instance->q.lswlsb = value;
        }

        public unsafe ushort O_REG
        {
            get => instance->z.Reg;
            set => instance->z.Reg = value;
        }

        public unsafe byte F_REG
        {
            get => instance->q.mswlsb;
            set => instance->q.mswlsb = value;
        }

        public unsafe byte E_REG
        {
            get => instance->q.mswmsb;
            set => instance->q.mswmsb = value;
        }

        public unsafe byte DPA
        {
            get => instance->dp.msb;
            set => instance->dp.msb = value;
        }

        #endregion

        #region Macros

        public byte HD6309_getcc() => _modules.HD6309.HD6309_getcc();
        public void HD6309_setcc(byte bincc) => _modules.HD6309.HD6309_setcc(bincc);
        public byte HD6309_getmd() => _modules.HD6309.HD6309_getmd();

        public unsafe byte PUR(int i) => *(byte*)(instance->ureg8[i]);
        public unsafe void PUR(int i, byte value) => *(byte*)(instance->ureg8[i]) = value;

        public unsafe ushort PXF(int i) => *(ushort*)(instance->xfreg16[i]);
        public unsafe void PXF(int i, ushort value) => *(ushort*)(instance->xfreg16[i]) = value;

        public unsafe ushort DPADDRESS(ushort r) => (ushort)(instance->dp.Reg | MemRead8(r));

        public byte MemRead8(ushort address) => _modules.TC1014.MemRead8(address);
        public void MemWrite8(byte data, ushort address) => _modules.TC1014.MemWrite8(data, address);
        public ushort MemRead16(ushort address) => _modules.TC1014.MemRead16(address);

        public ushort IMMADDRESS(ushort address) => _modules.TC1014.MemRead16(address);

        public bool NTEST8(byte value) => value > 0x7F;

        public bool ZTEST(byte value) => value == 0;
        public bool ZTEST(ushort value) => value == 0;
        public bool ZTEST(uint value) => value == 0;

        public bool NTEST16(ushort value) => value > 0x7FFF;

        public unsafe bool MD_ILLEGAL
        {
            get => instance->md[(int)MDFlagMasks.ILLEGAL] == Define.TRUE;
            set => instance->md[(int)MDFlagMasks.ILLEGAL] = value ? Define.TRUE : Define.FALSE;
        }

        public unsafe bool MD_NATIVE6309
        {
            get => instance->md[(int)MDFlagMasks.NATIVE6309] == Define.TRUE;
            set => instance->md[(int)MDFlagMasks.NATIVE6309] = value ? Define.TRUE : Define.FALSE;
        }

        #endregion
    }
}
