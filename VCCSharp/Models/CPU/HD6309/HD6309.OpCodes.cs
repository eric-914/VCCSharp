﻿using System;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    public partial class HD6309
    {
        private byte _temp8;
        private ushort _temp16;
        private uint _temp32;

        private short _signedShort;
        private int _signedInt;

        private byte _postByte;
        private ushort _postWord;

        private byte _source;
        private byte _dest;

        #region Jump Vectors

        private static Action[] _jmpVec1 = new Action[256];
        private static Action[] _jmpVec2 = new Action[256];
        private static Action[] _jmpVec3 = new Action[256];

        private void InitializeJmpVectors()
        {
            _jmpVec1 = new Action[]
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

            _jmpVec2 = new Action[]
            {
                InvalidInsHandler, // 00
                InvalidInsHandler, // 01
                InvalidInsHandler, // 02
                InvalidInsHandler, // 03
                InvalidInsHandler, // 04
                InvalidInsHandler, // 05
                InvalidInsHandler, // 06
                InvalidInsHandler, // 07
                InvalidInsHandler, // 08
                InvalidInsHandler, // 09
                InvalidInsHandler, // 0A
                InvalidInsHandler, // 0B
                InvalidInsHandler, // 0C
                InvalidInsHandler, // 0D
                InvalidInsHandler, // 0E
                InvalidInsHandler, // 0F
                InvalidInsHandler, // 10
                InvalidInsHandler, // 11
                InvalidInsHandler, // 12
                InvalidInsHandler, // 13
                InvalidInsHandler, // 14
                InvalidInsHandler, // 15
                InvalidInsHandler, // 16
                InvalidInsHandler, // 17
                InvalidInsHandler, // 18
                InvalidInsHandler, // 19
                InvalidInsHandler, // 1A
                InvalidInsHandler, // 1B
                InvalidInsHandler, // 1C
                InvalidInsHandler, // 1D
                InvalidInsHandler, // 1E
                InvalidInsHandler, // 1F
                InvalidInsHandler, // 20
                LBrn_R, // 21
                LBhi_R, // 22
                LBls_R, // 23
                LBhs_R, // 24
                LBcs_R, // 25
                LBne_R, // 26
                LBeq_R, // 27
                LBvc_R, // 28
                LBvs_R, // 29
                LBpl_R, // 2A
                LBmi_R, // 2B
                LBge_R, // 2C
                LBlt_R, // 2D
                LBgt_R, // 2E
                LBle_R, // 2F
                Addr, // 30
                Adcr, // 31
                Subr, // 32
                Sbcr, // 33
                Andr, // 34
                Orr, // 35
                Eorr, // 36
                Cmpr, // 37
                Pshsw, // 38
                Pulsw, // 39
                Pshuw, // 3A
                Puluw, // 3B
                InvalidInsHandler, // 3C
                InvalidInsHandler, // 3D
                InvalidInsHandler, // 3E
                Swi2_I, // 3F
                Negd_I, // 40
                InvalidInsHandler, // 41
                InvalidInsHandler, // 42
                Comd_I, // 43
                Lsrd_I, // 44
                InvalidInsHandler, // 45
                Rord_I, // 46
                Asrd_I, // 47
                Asld_I, // 48
                Rold_I, // 49
                Decd_I, // 4A
                InvalidInsHandler, // 4B
                Incd_I, // 4C
                Tstd_I, // 4D
                InvalidInsHandler, // 4E
                Clrd_I, // 4F
                InvalidInsHandler, // 50
                InvalidInsHandler, // 51
                InvalidInsHandler, // 52
                Comw_I, // 53
                Lsrw_I, // 54
                InvalidInsHandler, // 55
                Rorw_I, // 56
                InvalidInsHandler, // 57
                InvalidInsHandler, // 58
                Rolw_I, // 59
                Decw_I, // 5A
                InvalidInsHandler, // 5B
                Incw_I, // 5C
                Tstw_I, // 5D
                InvalidInsHandler, // 5E
                Clrw_I, // 5F
                InvalidInsHandler, // 60
                InvalidInsHandler, // 61
                InvalidInsHandler, // 62
                InvalidInsHandler, // 63
                InvalidInsHandler, // 64
                InvalidInsHandler, // 65
                InvalidInsHandler, // 66
                InvalidInsHandler, // 67
                InvalidInsHandler, // 68
                InvalidInsHandler, // 69
                InvalidInsHandler, // 6A
                InvalidInsHandler, // 6B
                InvalidInsHandler, // 6C
                InvalidInsHandler, // 6D
                InvalidInsHandler, // 6E
                InvalidInsHandler, // 6F
                InvalidInsHandler, // 70
                InvalidInsHandler, // 71
                InvalidInsHandler, // 72
                InvalidInsHandler, // 73
                InvalidInsHandler, // 74
                InvalidInsHandler, // 75
                InvalidInsHandler, // 76
                InvalidInsHandler, // 77
                InvalidInsHandler, // 78
                InvalidInsHandler, // 79
                InvalidInsHandler, // 7A
                InvalidInsHandler, // 7B
                InvalidInsHandler, // 7C
                InvalidInsHandler, // 7D
                InvalidInsHandler, // 7E
                InvalidInsHandler, // 7F
                Subw_M, // 80
                Cmpw_M, // 81
                Sbcd_M, // 82
                Cmpd_M, // 83
                Andd_M, // 84
                Bitd_M, // 85
                Ldw_M, // 86
                InvalidInsHandler, // 87
                Eord_M, // 88
                Adcd_M, // 89
                Ord_M, // 8A
                Addw_M, // 8B
                Cmpy_M, // 8C
                InvalidInsHandler, // 8D
                Ldy_M, // 8E
                InvalidInsHandler, // 8F
                Subw_D, // 90
                Cmpw_D, // 91
                Sbcd_D, // 92
                Cmpd_D, // 93
                Andd_D, // 94
                Bitd_D, // 95
                Ldw_D, // 96
                Stw_D, // 97
                Eord_D, // 98
                Adcd_D, // 99
                Ord_D, // 9A
                Addw_D, // 9B
                Cmpy_D, // 9C
                InvalidInsHandler, // 9D
                Ldy_D, // 9E
                Sty_D, // 9F
                Subw_X, // A0
                Cmpw_X, // A1
                Sbcd_X, // A2
                Cmpd_X, // A3
                Andd_X, // A4
                Bitd_X, // A5
                Ldw_X, // A6
                Stw_X, // A7
                Eord_X, // A8
                Adcd_X, // A9
                Ord_X, // AA
                Addw_X, // AB
                Cmpy_X, // AC
                InvalidInsHandler, // AD
                Ldy_X, // AE
                Sty_X, // AF
                Subw_E, // B0
                Cmpw_E, // B1
                Sbcd_E, // B2
                Cmpd_E, // B3
                Andd_E, // B4
                Bitd_E, // B5
                Ldw_E, // B6
                Stw_E, // B7
                Eord_E, // B8
                Adcd_E, // B9
                Ord_E, // BA
                Addw_E, // BB
                Cmpy_E, // BC
                InvalidInsHandler, // BD
                Ldy_E, // BE
                Sty_E, // BF
                InvalidInsHandler, // C0
                InvalidInsHandler, // C1
                InvalidInsHandler, // C2
                InvalidInsHandler, // C3
                InvalidInsHandler, // C4
                InvalidInsHandler, // C5
                InvalidInsHandler, // C6
                InvalidInsHandler, // C7
                InvalidInsHandler, // C8
                InvalidInsHandler, // C9
                InvalidInsHandler, // CA
                InvalidInsHandler, // CB
                InvalidInsHandler, // CC
                InvalidInsHandler, // CD
                Lds_I, // CE
                InvalidInsHandler, // CF
                InvalidInsHandler, // D0
                InvalidInsHandler, // D1
                InvalidInsHandler, // D2
                InvalidInsHandler, // D3
                InvalidInsHandler, // D4
                InvalidInsHandler, // D5
                InvalidInsHandler, // D6
                InvalidInsHandler, // D7
                InvalidInsHandler, // D8
                InvalidInsHandler, // D9
                InvalidInsHandler, // DA
                InvalidInsHandler, // DB
                Ldq_D, // DC
                Stq_D, // DD
                Lds_D, // DE
                Sts_D, // DF
                InvalidInsHandler, // E0
                InvalidInsHandler, // E1
                InvalidInsHandler, // E2
                InvalidInsHandler, // E3
                InvalidInsHandler, // E4
                InvalidInsHandler, // E5
                InvalidInsHandler, // E6
                InvalidInsHandler, // E7
                InvalidInsHandler, // E8
                InvalidInsHandler, // E9
                InvalidInsHandler, // EA
                InvalidInsHandler, // EB
                Ldq_X, // EC
                Stq_X, // ED
                Lds_X, // EE
                Sts_X, // EF
                InvalidInsHandler, // F0
                InvalidInsHandler, // F1
                InvalidInsHandler, // F2
                InvalidInsHandler, // F3
                InvalidInsHandler, // F4
                InvalidInsHandler, // F5
                InvalidInsHandler, // F6
                InvalidInsHandler, // F7
                InvalidInsHandler, // F8
                InvalidInsHandler, // F9
                InvalidInsHandler, // FA
                InvalidInsHandler, // FB
                Ldq_E, // FC
                Stq_E, // FD
                Lds_E, // FE
                Sts_E, // FF
            };

            _jmpVec3 = new Action[]
            {
                InvalidInsHandler, // 00
                InvalidInsHandler, // 01
                InvalidInsHandler, // 02
                InvalidInsHandler, // 03
                InvalidInsHandler, // 04
                InvalidInsHandler, // 05
                InvalidInsHandler, // 06
                InvalidInsHandler, // 07
                InvalidInsHandler, // 08
                InvalidInsHandler, // 09
                InvalidInsHandler, // 0A
                InvalidInsHandler, // 0B
                InvalidInsHandler, // 0C
                InvalidInsHandler, // 0D
                InvalidInsHandler, // 0E
                InvalidInsHandler, // 0F
                InvalidInsHandler, // 10
                InvalidInsHandler, // 11
                InvalidInsHandler, // 12
                InvalidInsHandler, // 13
                InvalidInsHandler, // 14
                InvalidInsHandler, // 15
                InvalidInsHandler, // 16
                InvalidInsHandler, // 17
                InvalidInsHandler, // 18
                InvalidInsHandler, // 19
                InvalidInsHandler, // 1A
                InvalidInsHandler, // 1B
                InvalidInsHandler, // 1C
                InvalidInsHandler, // 1D
                InvalidInsHandler, // 1E
                InvalidInsHandler, // 1F
                InvalidInsHandler, // 20
                InvalidInsHandler, // 21
                InvalidInsHandler, // 22
                InvalidInsHandler, // 23
                InvalidInsHandler, // 24
                InvalidInsHandler, // 25
                InvalidInsHandler, // 26
                InvalidInsHandler, // 27
                InvalidInsHandler, // 28
                InvalidInsHandler, // 29
                InvalidInsHandler, // 2A
                InvalidInsHandler, // 2B
                InvalidInsHandler, // 2C
                InvalidInsHandler, // 2D
                InvalidInsHandler, // 2E
                InvalidInsHandler, // 2F
                Band, // 30
                Biand, // 31
                Bor, // 32
                Bior, // 33
                Beor, // 34
                Bieor, // 35
                Ldbt, // 36
                Stbt, // 37
                Tfm1, // 38
                Tfm2, // 39
                Tfm3, // 3A
                Tfm4, // 3B
                Bitmd_M, // 3C
                Ldmd_M, // 3D
                InvalidInsHandler, // 3E
                Swi3_I, // 3F
                InvalidInsHandler, // 40
                InvalidInsHandler, // 41
                InvalidInsHandler, // 42
                Come_I, // 43
                InvalidInsHandler, // 44
                InvalidInsHandler, // 45
                InvalidInsHandler, // 46
                InvalidInsHandler, // 47
                InvalidInsHandler, // 48
                InvalidInsHandler, // 49
                Dece_I, // 4A
                InvalidInsHandler, // 4B
                Ince_I, // 4C
                Tste_I, // 4D
                InvalidInsHandler, // 4E
                Clre_I, // 4F
                InvalidInsHandler, // 50
                InvalidInsHandler, // 51
                InvalidInsHandler, // 52
                Comf_I, // 53
                InvalidInsHandler, // 54
                InvalidInsHandler, // 55
                InvalidInsHandler, // 56
                InvalidInsHandler, // 57
                InvalidInsHandler, // 58
                InvalidInsHandler, // 59
                Decf_I, // 5A
                InvalidInsHandler, // 5B
                Incf_I, // 5C
                Tstf_I, // 5D
                InvalidInsHandler, // 5E
                Clrf_I, // 5F
                InvalidInsHandler, // 60
                InvalidInsHandler, // 61
                InvalidInsHandler, // 62
                InvalidInsHandler, // 63
                InvalidInsHandler, // 64
                InvalidInsHandler, // 65
                InvalidInsHandler, // 66
                InvalidInsHandler, // 67
                InvalidInsHandler, // 68
                InvalidInsHandler, // 69
                InvalidInsHandler, // 6A
                InvalidInsHandler, // 6B
                InvalidInsHandler, // 6C
                InvalidInsHandler, // 6D
                InvalidInsHandler, // 6E
                InvalidInsHandler, // 6F
                InvalidInsHandler, // 70
                InvalidInsHandler, // 71
                InvalidInsHandler, // 72
                InvalidInsHandler, // 73
                InvalidInsHandler, // 74
                InvalidInsHandler, // 75
                InvalidInsHandler, // 76
                InvalidInsHandler, // 77
                InvalidInsHandler, // 78
                InvalidInsHandler, // 79
                InvalidInsHandler, // 7A
                InvalidInsHandler, // 7B
                InvalidInsHandler, // 7C
                InvalidInsHandler, // 7D
                InvalidInsHandler, // 7E
                InvalidInsHandler, // 7F
                Sube_M, // 80
                Cmpe_M, // 81
                InvalidInsHandler, // 82
                Cmpu_M, // 83
                InvalidInsHandler, // 84
                InvalidInsHandler, // 85
                Lde_M, // 86
                InvalidInsHandler, // 87
                InvalidInsHandler, // 88
                InvalidInsHandler, // 89
                InvalidInsHandler, // 8A
                Adde_M, // 8B
                Cmps_M, // 8C
                Divd_M, // 8D
                Divq_M, // 8E
                Muld_M, // 8F
                Sube_D, // 90
                Cmpe_D, // 91
                InvalidInsHandler, // 92
                Cmpu_D, // 93
                InvalidInsHandler, // 94
                InvalidInsHandler, // 95
                Lde_D, // 96
                Ste_D, // 97
                InvalidInsHandler, // 98
                InvalidInsHandler, // 99
                InvalidInsHandler, // 9A
                Adde_D, // 9B
                Cmps_D, // 9C
                Divd_D, // 9D
                Divq_D, // 9E
                Muld_D, // 9F
                Sube_X, // A0
                Cmpe_X, // A1
                InvalidInsHandler, // A2
                Cmpu_X, // A3
                InvalidInsHandler, // A4
                InvalidInsHandler, // A5
                Lde_X, // A6
                Ste_X, // A7
                InvalidInsHandler, // A8
                InvalidInsHandler, // A9
                InvalidInsHandler, // AA
                Adde_X, // AB
                Cmps_X, // AC
                Divd_X, // AD
                Divq_X, // AE
                Muld_X, // AF
                Sube_E, // B0
                Cmpe_E, // B1
                InvalidInsHandler, // B2
                Cmpu_E, // B3
                InvalidInsHandler, // B4
                InvalidInsHandler, // B5
                Lde_E, // B6
                Ste_E, // B7
                InvalidInsHandler, // B8
                InvalidInsHandler, // B9
                InvalidInsHandler, // BA
                Adde_E, // BB
                Cmps_E, // BC
                Divd_E, // BD
                Divq_E, // BE
                Muld_E, // BF
                Subf_M, // C0
                Cmpf_M, // C1
                InvalidInsHandler, // C2
                InvalidInsHandler, // C3
                InvalidInsHandler, // C4
                InvalidInsHandler, // C5
                Ldf_M, // C6
                InvalidInsHandler, // C7
                InvalidInsHandler, // C8
                InvalidInsHandler, // C9
                InvalidInsHandler, // CA
                Addf_M, // CB
                InvalidInsHandler, // CC
                InvalidInsHandler, // CD
                InvalidInsHandler, // CE
                InvalidInsHandler, // CF
                Subf_D, // D0
                Cmpf_D, // D1
                InvalidInsHandler, // D2
                InvalidInsHandler, // D3
                InvalidInsHandler, // D4
                InvalidInsHandler, // D5
                Ldf_D, // D6
                Stf_D, // D7
                InvalidInsHandler, // D8
                InvalidInsHandler, // D9
                InvalidInsHandler, // DA
                Addf_D, // DB
                InvalidInsHandler, // DC
                InvalidInsHandler, // DD
                InvalidInsHandler, // DE
                InvalidInsHandler, // DF
                Subf_X, // E0
                Cmpf_X, // E1
                InvalidInsHandler, // E2
                InvalidInsHandler, // E3
                InvalidInsHandler, // E4
                InvalidInsHandler, // E5
                Ldf_X, // E6
                Stf_X, // E7
                InvalidInsHandler, // E8
                InvalidInsHandler, // E9
                InvalidInsHandler, // EA
                Addf_X, // EB
                InvalidInsHandler, // EC
                InvalidInsHandler, // ED
                InvalidInsHandler, // EE
                InvalidInsHandler, // EF
                Subf_E, // F0
                Cmpf_E, // F1
                InvalidInsHandler, // F2
                InvalidInsHandler, // F3
                InvalidInsHandler, // F4
                InvalidInsHandler, // F5
                Ldf_E, // F6
                Stf_E, // F7
                InvalidInsHandler, // F8
                InvalidInsHandler, // F9
                InvalidInsHandler, // FA
                Addf_E, // FB
                InvalidInsHandler, // FC
                InvalidInsHandler, // FD
                InvalidInsHandler, // FE
                InvalidInsHandler // FF
            };
        }

        #endregion

        public void Exec(byte opCode)
        {
            _jmpVec1[opCode]();
        }

        //OpCode Definitions
        //Last Char (D) Direct (I) Inherent (R) Relative (M) Immediate (X) Indexed (E) Extended

        public unsafe void InvalidInsHandler()
        {
            MD_ILLEGAL = true;
            _instance->mdbits = getmd();

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

                _cycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(getcc(), --S_REG);

            PC_REG = MemRead16(Define.VTRAP);

            _cycleCounter += (12 + _instance->NatEmuCycles54);	//One for each byte +overhead? Guessing from PSHS
        }

        public unsafe void DivByZero()
        {
            MD_ZERODIV = true; //1;

            _instance->mdbits = getmd();

            ErrorVector();
        }
    }
}
