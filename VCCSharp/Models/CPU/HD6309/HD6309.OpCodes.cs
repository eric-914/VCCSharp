using System;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.HD6309
{
    public interface IHD6309OpCodes
    {
        void Exec(byte opCode);

        void PUR(int i, byte value);
        ushort PXF(int i);
        void PXF(int i, ushort value);

        ushort PC_REG { get; set; }
        ushort DP_REG { get; set; }
        ushort W_REG { get; set; }
        ushort D_REG { get; set; }

        byte A_REG { get; set; }
        byte B_REG { get; set; }
        byte E_REG { get; set; }
        byte F_REG { get; set; }

        public bool CC_E { get; set; }
        public bool CC_F { get; set; }
        public bool CC_H { get; set; }
        public bool CC_I { get; set; }
        public bool CC_N { get; set; }
        public bool CC_Z { get; set; }
        public bool CC_V { get; set; }
        public bool CC_C { get; set; }

        bool MD_NATIVE6309 { get; set; }
        bool MD_FIRQMODE { get; set; }
        bool MD_UNDEFINED2 { get; set; }
        bool MD_UNDEFINED3 { get; set; }
        bool MD_UNDEFINED4 { get; set; }
        bool MD_UNDEFINED5 { get; set; }
        bool MD_ILLEGAL { get; set; }
        bool MD_ZERODIV { get; set; }

        byte MemRead8(ushort address);
        ushort MemRead16(ushort address);
    }

    public partial class HD6309 : IHD6309OpCodes
    {
        private byte _temp8;
        private ushort _temp16;
        private uint _temp32;

        private short _stemp16;
        private int _stemp32;

        private byte _postbyte = 0;
        private ushort _postword = 0;

        private byte _source = 0;
        private byte _dest = 0;

        #region Jump Vectors

        private static Action[] JmpVec1 = new Action[256];
        private static Action[] JmpVec2 = new Action[256];
        private static Action[] JmpVec3 = new Action[256];

        private void InitializeJmpVectors()
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

            JmpVec2 = new Action[]
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

            JmpVec3 = new Action[]
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
            JmpVec1[opCode]();
        }

        //Opcode Defs
        //Last Char (D) Direct (I) Inherent (R) Relative (M) Immediate (X) Indexed (E) Extended

        #region Page1 OpCodes

        #region 0x00 - 0x0F

        public unsafe void Neg_D()// 00
        {
            _temp16 = DPADDRESS(PC_REG++);
            _postbyte = MemRead8(_temp16);
            _temp8 = (byte)(0 - _postbyte);

            CC_C = _temp8 > 0;
            CC_V = _postbyte == 0x80;
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);

            MemWrite8(_temp8, _temp16);

            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        //1 6309
        public unsafe void Oim_D()// 01
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = DPADDRESS(PC_REG++);
            _postbyte |= MemRead8(_temp16);

            MemWrite8(_postbyte, _temp16);

            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0

            _instance->CycleCounter += 6;
        }

        //2 Phase 2 6309
        public unsafe void Aim_D()// 02
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = DPADDRESS(PC_REG++);
            _postbyte &= MemRead8(_temp16);

            MemWrite8(_postbyte, _temp16);

            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;

            _instance->CycleCounter += 6;
        }

        public unsafe void Com_D()// 03
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _temp8 = (byte)(0xFF - _temp8);

            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_C = true; //1;
            CC_V = false; //0;

            MemWrite8(_temp8, _temp16);

            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Lsr_D()// 04
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)(_temp8 >> 1);
            CC_Z = ZTEST(_temp8);
            CC_N = false; //0;
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        //6309 Untested
        public unsafe void Eim_D()// 05
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = DPADDRESS(PC_REG++);
            _postbyte ^= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Ror_D()// 06
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _postbyte = (byte)((CC_C ? 1 : 0) << 7);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 >> 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Asr_D()// 07
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Asl_D()// 08
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = ((_temp8 & 0x80) >> 7) != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((_temp8 & 0x40) >> 6)) != 0;
            _temp8 = (byte)(_temp8 << 1);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Rol_D()// 09
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _postbyte = (byte)(CC_C ? 1 : 0);
            CC_C = ((_temp8 & 0x80) >> 7) != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((_temp8 & 0x40) >> 6)) != 0;
            _temp8 = (byte)((_temp8 << 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Dec_D()// 0A
        {
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = (byte)(MemRead8(_temp16) - 1);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = _temp8 == 0x7F;
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        //6309 Untested wcreate
        public unsafe void Tim_D()// 0B
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _postbyte &= _temp8;
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Inc_D()// 0C
        {
            _temp16 = (DPADDRESS(PC_REG++));
            _temp8 = (byte)(MemRead8(_temp16) + 1);
            CC_Z = ZTEST(_temp8);
            CC_V = _temp8 == 0x80;
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Tst_D()// 0D
        {
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles64;
        }

        public unsafe void Jmp_D()// 0E
        {
            PC_REG = (ushort)(DP_REG | MemRead8(PC_REG));
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Clr_D()// 0F
        {
            MemWrite8(0, DPADDRESS(PC_REG++));
            CC_Z = true; //1;
            CC_N = false; //0;
            CC_V = false; //0;
            CC_C = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #region 0x10 - 0x1F

        public void Page_2()// 10
        {
            byte opCode = MemRead8(PC_REG++);

            JmpVec2[opCode]();
        }

        public void Page_3()// 11
        {
            byte opCode = MemRead8(PC_REG++);

            JmpVec3[opCode]();
        }

        public unsafe void Nop_I()// 12
        {
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Sync_I()// 13
        {
            _instance->CycleCounter = _instance->gCycleFor;
            _instance->SyncWaiting = 1;
        }

        //6309 CHECK
        public unsafe void Sexw_I()// 14
        {
            D_REG = (W_REG & 32768) != 0 ? (ushort)0xFFFF : (ushort)0;

            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += 4;
        }

        // 15		//InvalidInsHandler

        public unsafe void Lbra_R()// 16
        {
            fixed (ushort* spostword = &_postword)
            {
                *spostword = IMMADDRESS(PC_REG);
                PC_REG += 2;
                PC_REG += *spostword;
            };

            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Lbsr_R()// 17
        {
            fixed (ushort* spostword = &_postword)
            {
                *spostword = IMMADDRESS(PC_REG);
                PC_REG += 2;
                S_REG--;
                MemWrite8(PC_L, S_REG--);
                MemWrite8(PC_H, S_REG);
                PC_REG += *spostword;
            }

            _instance->CycleCounter += _instance->NatEmuCycles97;
        }

        // 18		//InvalidInsHandler

        public unsafe void Daa_I()// 19
        {
            byte msn, lsn;

            msn = (byte)(A_REG & 0xF0);
            lsn = (byte)(A_REG & 0xF);
            _temp8 = 0;

            if (CC_H || (lsn > 9))
            {
                _temp8 |= 0x06;
            }

            if ((msn > 0x80) && (lsn > 9))
            {
                _temp8 |= 0x60;
            }

            if ((msn > 0x90) || CC_C)
            {
                _temp8 |= 0x60;
            }

            _temp16 = (ushort)(A_REG + _temp8);
            CC_C |= (((_temp16 & 0x100) >> 8) != 0);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Orcc_M()// 1A
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = HD6309_getcc();
            _temp8 = (byte)(_temp8 | _postbyte);
            HD6309_setcc(_temp8);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        // 1B		//InvalidInsHandler

        public unsafe void Andcc_M()// 1C
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = HD6309_getcc();
            _temp8 = (byte)(_temp8 & _postbyte);
            HD6309_setcc(_temp8);
            _instance->CycleCounter += 3;
        }

        public unsafe void Sex_I()// 1D
        {
            A_REG = (byte)(0 - (B_REG >> 7));
            CC_Z = ZTEST(D_REG);
            CC_N = (D_REG >> 15) != 0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Exg_M()// 1E
        {
            byte tmp;

            _postbyte = MemRead8(PC_REG++);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            _instance->ccbits = HD6309_getcc();

            if ((_source & 0x08) == (_dest & 0x08)) //Verify like size registers
            {
                if ((_dest & 0x08) != 0) //8 bit EXG
                {
                    _source &= 0x07;
                    _dest &= 0x07;
                    _temp8 = (PUR(_source));
                    PUR(_source, (PUR(_dest)));
                    PUR(_dest, _temp8);
                    O_REG = 0;
                }
                else // 16 bit EXG
                {
                    _source &= 0x07;
                    _dest &= 0x07;
                    _temp16 = (PXF(_source));
                    PXF(_source, _dest);
                    PXF(_dest, _temp16);
                }
            }
            else
            {
                if ((_dest & 0x08) != 0) // Swap 16 to 8 bit exchange to be 8 to 16 bit exchange (for convenience)
                {
                    _temp8 = _dest; _dest = _source; _source = _temp8;
                }

                _source &= 0x07;
                _dest &= 0x07;

                switch (_source)
                {
                    case 0x04: // Z
                    case 0x05: // Z
                        PXF(_dest, 0); // Source is Zero reg. Just zero the Destination.
                        break;

                    case 0x00: // A
                    case 0x03: // DP
                    case 0x06: // E
                        _temp8 = PUR(_source);
                        _temp16 = (ushort)((_temp8 << 8) | _temp8);
                        tmp = (byte)(PXF(_dest) >> 8);
                        PUR(_source, tmp); // A, DP, E get high byte of 16 bit Dest
                        PXF(_dest, _temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;

                    case 0x01: // B
                    case 0x02: // CC
                    case 0x07: // F
                        _temp8 = PUR(_source);
                        _temp16 = (ushort)((_temp8 << 8) | _temp8);
                        tmp = (byte)(PXF(_dest) & 0xFF);
                        PUR(_source, tmp); // B, CC, F get low byte of 16 bit Dest
                        PXF(_dest, _temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;
                }
            }

            HD6309_setcc(_instance->ccbits);
            _instance->CycleCounter += _instance->NatEmuCycles85;
        }

        public unsafe void Tfr_M()// 1F
        {
            _postbyte = MemRead8(PC_REG++);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            if (_dest < 8)
            {
                if (_source < 8)
                {
                    PXF(_dest, PXF(_source));
                }
                else
                {
                    PXF(_dest, (ushort)((PUR(_source & 7) << 8) | PUR(_source & 7)));
                }
            }
            else
            {
                _instance->ccbits = HD6309_getcc();
                _dest &= 7;

                if (_source < 8)
                    switch (_dest)
                    {
                        case 0:  // A
                        case 3: // DP
                        case 6: // E
                            PUR(_dest, (byte)(PXF(_source) >> 8));
                            break;
                        case 1:  // B
                        case 2: // CC
                        case 7: // F
                            PUR(_dest, (byte)(PXF(_source) & 0xFF));
                            break;
                    }
                else
                {
                    PUR(_dest, PUR(_source & 7));
                }

                O_REG = 0;
                HD6309_setcc(_instance->ccbits);
            }

            _instance->CycleCounter += _instance->NatEmuCycles64;
        }

        #endregion

        #region 0x20 - 0x2F

        public unsafe void Bra_R()// 20
        {
            sbyte t = (sbyte)MemRead8(PC_REG++);
            PC_REG += (ushort)t;
            _instance->CycleCounter += 3;
        }

        public unsafe void Brn_R()// 21
        {
            _instance->CycleCounter += 3;
            PC_REG++;
        }

        public unsafe void Bhi_R()// 22
        {
            if (!(CC_C || CC_Z))
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bls_R()// 23
        {
            if (CC_C | CC_Z)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bhs_R()// 24
        {
            if (!CC_C)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Blo_R()// 25
        {
            if (CC_C)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bne_R()// 26
        {
            if (!CC_Z)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Beq_R()// 27
        {
            if (CC_Z)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bvc_R()// 28
        {
            if (!CC_V)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bvs_R()// 29
        {
            if (CC_V)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bpl_R()// 2A
        {
            if (!CC_N)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bmi_R()// 2B
        {
            if (CC_N)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bge_R()// 2C
        {
            if (!(CC_N ^ CC_V))
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Blt_R()// 2D
        {
            if (CC_V ^ CC_N)
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Bgt_R()// 2E
        {
            if (!(CC_Z | (CC_N ^ CC_V)))
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        public unsafe void Ble_R()// 2F
        {
            if (CC_Z | (CC_N ^ CC_V))
            {
                PC_REG += (ushort)(sbyte)MemRead8(PC_REG);
            }

            PC_REG++;
            _instance->CycleCounter += 3;
        }

        #endregion

        #region 0x30 - 0x3F

        public unsafe void Leax_X()// 30
        {
            X_REG = INDADDRESS(PC_REG++);
            CC_Z = ZTEST(X_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Leay_X()// 31
        {
            Y_REG = INDADDRESS(PC_REG++);
            CC_Z = ZTEST(Y_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Leas_X()// 32
        {
            S_REG = INDADDRESS(PC_REG++);
            _instance->CycleCounter += 4;
        }

        public unsafe void Leau_X()// 33
        {
            U_REG = INDADDRESS(PC_REG++);
            _instance->CycleCounter += 4;
        }

        public unsafe void Pshs_M()// 34
        {
            _postbyte = MemRead8(PC_REG++);

            if ((_postbyte & 0x80) != 0)
            {
                MemWrite8(PC_L, --S_REG);
                MemWrite8(PC_H, --S_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x40) != 0)
            {
                MemWrite8(U_L, --S_REG);
                MemWrite8(U_H, --S_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x20) != 0)
            {
                MemWrite8(Y_L, --S_REG);
                MemWrite8(Y_H, --S_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x10) != 0)
            {
                MemWrite8(X_L, --S_REG);
                MemWrite8(X_H, --S_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x08) != 0)
            {
                MemWrite8(DPA, --S_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x04) != 0)
            {
                MemWrite8(B_REG, --S_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x02) != 0)
            {
                MemWrite8(A_REG, --S_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x01) != 0)
            {
                MemWrite8(HD6309_getcc(), --S_REG);
                _instance->CycleCounter += 1;
            }

            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Puls_M()// 35
        {
            _postbyte = MemRead8(PC_REG++);

            if ((_postbyte & 0x01) != 0)
            {
                HD6309_setcc(MemRead8(S_REG++));
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x02) != 0)
            {
                A_REG = MemRead8(S_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x04) != 0)
            {
                B_REG = MemRead8(S_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x08) != 0)
            {
                DPA = MemRead8(S_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x10) != 0)
            {
                X_H = MemRead8(S_REG++);
                X_L = MemRead8(S_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x20) != 0)
            {
                Y_H = MemRead8(S_REG++);
                Y_L = MemRead8(S_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x40) != 0)
            {
                U_H = MemRead8(S_REG++);
                U_L = MemRead8(S_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x80) != 0)
            {
                PC_H = MemRead8(S_REG++);
                PC_L = MemRead8(S_REG++);
                _instance->CycleCounter += 2;
            }

            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Pshu_M()// 36
        {
            _postbyte = MemRead8(PC_REG++);

            if ((_postbyte & 0x80) != 0)
            {
                MemWrite8(PC_L, --U_REG);
                MemWrite8(PC_H, --U_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x40) != 0)
            {
                MemWrite8(S_L, --U_REG);
                MemWrite8(S_H, --U_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x20) != 0)
            {
                MemWrite8(Y_L, --U_REG);
                MemWrite8(Y_H, --U_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x10) != 0)
            {
                MemWrite8(X_L, --U_REG);
                MemWrite8(X_H, --U_REG);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x08) != 0)
            {
                MemWrite8(DPA, --U_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x04) != 0)
            {
                MemWrite8(B_REG, --U_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x02) != 0)
            {
                MemWrite8(A_REG, --U_REG);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x01) != 0)
            {
                MemWrite8(HD6309_getcc(), --U_REG);
                _instance->CycleCounter += 1;
            }

            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Pulu_M()// 37
        {
            _postbyte = MemRead8(PC_REG++);

            if ((_postbyte & 0x01) != 0)
            {
                HD6309_setcc(MemRead8(U_REG++));
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x02) != 0)
            {
                A_REG = MemRead8(U_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x04) != 0)
            {
                B_REG = MemRead8(U_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x08) != 0)
            {
                DPA = MemRead8(U_REG++);
                _instance->CycleCounter += 1;
            }

            if ((_postbyte & 0x10) != 0)
            {
                X_H = MemRead8(U_REG++);
                X_L = MemRead8(U_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x20) != 0)
            {
                Y_H = MemRead8(U_REG++);
                Y_L = MemRead8(U_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x40) != 0)
            {
                S_H = MemRead8(U_REG++);
                S_L = MemRead8(U_REG++);
                _instance->CycleCounter += 2;
            }

            if ((_postbyte & 0x80) != 0)
            {
                PC_H = MemRead8(U_REG++);
                PC_L = MemRead8(U_REG++);
                _instance->CycleCounter += 2;
            }

            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        // 38		//InvalidInsHandler

        public unsafe void Rts_I()// 39
        {
            PC_H = MemRead8(S_REG++);
            PC_L = MemRead8(S_REG++);
            _instance->CycleCounter += _instance->NatEmuCycles51;
        }

        public unsafe void Abx_I()// 3A
        {
            X_REG += B_REG;
            _instance->CycleCounter += _instance->NatEmuCycles31;
        }

        public unsafe void Rti_I()// 3B
        {
            HD6309_setcc(MemRead8(S_REG++));
            _instance->CycleCounter += 6;
            _instance->InInterrupt = 0;

            if (CC_E)
            {
                A_REG = MemRead8(S_REG++);
                B_REG = MemRead8(S_REG++);

                if (MD_NATIVE6309)
                {
                    (E_REG) = MemRead8(S_REG++);
                    (F_REG) = MemRead8(S_REG++);
                    _instance->CycleCounter += 2;
                }

                DPA = MemRead8(S_REG++);
                X_H = MemRead8(S_REG++);
                X_L = MemRead8(S_REG++);
                Y_H = MemRead8(S_REG++);
                Y_L = MemRead8(S_REG++);
                U_H = MemRead8(S_REG++);
                U_L = MemRead8(S_REG++);
                _instance->CycleCounter += 9;
            }

            PC_H = MemRead8(S_REG++);
            PC_L = MemRead8(S_REG++);
        }

        public unsafe void Cwai_I()// 3C
        {
            _postbyte = MemRead8(PC_REG++);
            _instance->ccbits = HD6309_getcc();
            _instance->ccbits = (byte)(_instance->ccbits & _postbyte);
            HD6309_setcc(_instance->ccbits);
            _instance->CycleCounter = _instance->gCycleFor;
            _instance->SyncWaiting = 1;
        }

        public unsafe void Mul_I()// 3D
        {
            D_REG = (ushort)(A_REG * B_REG);
            CC_C = B_REG > 0x7F;
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles1110;
        }

        public unsafe void Swi1_I()// 3F
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
                _instance->CycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(HD6309_getcc(), --S_REG);

            PC_REG = MemRead16(Define.VSWI);
            _instance->CycleCounter += 19;

            CC_I = true; //1;
            CC_F = true; //1;
        }

        #endregion

        #region 0x40 - 0x4F

        public unsafe void Nega_I()// 40
        {
            _temp8 = (byte)(0 - A_REG);
            CC_C = _temp8 > 0;
            CC_V = A_REG == 0x80; //CC_C ^ ((A_REG^temp8)>>7);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            A_REG = _temp8;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 41		//InvalidInsHandler

        // 42		//InvalidInsHandler

        public unsafe void Coma_I()// 43
        {
            A_REG = (byte)(0xFF - A_REG);
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Lsra_I()// 44
        {
            CC_C = (A_REG & 1) != 0;
            A_REG = (byte)(A_REG >> 1);
            CC_Z = ZTEST(A_REG);
            CC_N = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 45		//InvalidInsHandler

        public unsafe void Rora_I()// 46
        {
            _postbyte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;
            CC_C = (A_REG & 1) != 0;
            A_REG = (byte)((A_REG >> 1) | _postbyte);
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;

        }

        public unsafe void Asra_I()// 47
        {
            CC_C = (A_REG & 1) != 0;
            A_REG = (byte)((A_REG & 0x80) | (A_REG >> 1));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Asla_I()// 48
        {
            CC_C = A_REG > 0x7F;
            CC_V = CC_C ^ ((A_REG & 0x40) >> 6 != 0);
            A_REG = (byte)(A_REG << 1);
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Rola_I()// 49
        {
            _postbyte = CC_C ? (byte)1 : (byte)0;
            CC_C = A_REG > 0x7F;
            CC_V = CC_C ^ ((A_REG & 0x40) >> 6 != 0);
            A_REG = (byte)((A_REG << 1) | _postbyte);
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Deca_I()// 4A
        {
            A_REG--;
            CC_Z = ZTEST(A_REG);
            CC_V = A_REG == 0x7F;
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 4B		//InvalidInsHandler

        public unsafe void Inca_I()// 4C
        {
            A_REG++;
            CC_Z = ZTEST(A_REG);
            CC_V = A_REG == 0x80;
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Tsta_I()// 4D
        {
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 4E		//InvalidInsHandler

        public unsafe void Clra_I()// 4F
        {
            A_REG = 0;
            CC_C = false; //0;
            CC_V = false; //0;
            CC_N = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        #endregion

        #region 0x50 - 0x5F

        public unsafe void Negb_I()// 50
        {
            _temp8 = (byte)(0 - B_REG);
            CC_C = _temp8 > 0;
            CC_V = B_REG == 0x80;
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            B_REG = _temp8;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 51		//InvalidInsHandler

        // 52		//InvalidInsHandler

        public unsafe void Comb_I()// 53
        {
            B_REG = (byte)(0xFF - B_REG);
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Lsrb_I()// 54
        {
            CC_C = (B_REG & 1) != 0;
            B_REG = (byte)(B_REG >> 1);
            CC_Z = ZTEST(B_REG);
            CC_N = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 55		//InvalidInsHandler

        public unsafe void Rorb_I()// 56
        {
            _postbyte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;
            CC_C = (B_REG & 1) != 0;
            B_REG = (byte)((B_REG >> 1) | _postbyte);
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Asrb_I()// 57
        {
            CC_C = (B_REG & 1) != 0;
            B_REG = (byte)((B_REG & 0x80) | (B_REG >> 1));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Aslb_I()// 58
        {
            CC_C = B_REG > 0x7F;
            CC_V = CC_C ^ ((B_REG & 0x40) >> 6 != 0);
            B_REG = (byte)(B_REG << 1);
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Rolb_I()// 59
        {
            _postbyte = CC_C ? (byte)1 : (byte)0;
            CC_C = B_REG > 0x7F;
            CC_V = CC_C ^ ((B_REG & 0x40) >> 6 != 0);
            B_REG = (byte)((B_REG << 1) | _postbyte);
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Decb_I()// 5A
        {
            B_REG--;
            CC_Z = ZTEST(B_REG);
            CC_V = B_REG == 0x7F;
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 5B		//InvalidInsHandler

        public unsafe void Incb_I()// 5C
        {
            B_REG++;
            CC_Z = ZTEST(B_REG);
            CC_V = B_REG == 0x80;
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Tstb_I()// 5D
        {
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        // 5E		//InvalidInsHandler

        public unsafe void Clrb_I()// 5F
        {
            B_REG = 0;
            CC_C = false; //0;
            CC_N = false; //0;
            CC_V = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        #endregion

        #region 0x60 - 0x6F

        public unsafe void Neg_X()// 60
        {
            _temp16 = INDADDRESS(PC_REG++);
            _postbyte = MemRead8(_temp16);
            _temp8 = (byte)(0 - _postbyte);
            CC_C = _temp8 > 0;
            CC_V = _postbyte == 0x80;
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Oim_X()// 61
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = INDADDRESS(PC_REG++);
            _postbyte |= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Aim_X()// 62
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = INDADDRESS(PC_REG++);
            _postbyte &= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Com_X()// 63
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _temp8 = (byte)(0xFF - _temp8);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = false; //0;
            CC_C = true; //1;
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Lsr_X()// 64
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)(_temp8 >> 1);
            CC_Z = ZTEST(_temp8);
            CC_N = false; //0;
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Eim_X()// 65
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = INDADDRESS(PC_REG++);
            _postbyte ^= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 7;
        }

        public unsafe void Ror_X()// 66
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _postbyte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 >> 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Asr_X()// 67
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Asl_X()// 68
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            CC_C = _temp8 > 0x7F;
            CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);
            _temp8 = (byte)(_temp8 << 1);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Rol_X()// 69
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _postbyte = CC_C ? (byte)1 : (byte)0;
            CC_C = _temp8 > 0x7F;
            CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);
            _temp8 = (byte)((_temp8 << 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Dec_X()// 6A
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _temp8--;
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = (_temp8 == 0x7F);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Tim_X()// 6B
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(INDADDRESS(PC_REG++));
            _postbyte &= _temp8;
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            _instance->CycleCounter += 7;
        }

        public unsafe void Inc_X()// 6C
        {
            _temp16 = INDADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _temp8++;
            CC_V = (_temp8 == 0x80);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += 6;
        }

        public unsafe void Tst_X()// 6D
        {
            _temp8 = MemRead8(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Jmp_X()// 6E
        {
            PC_REG = INDADDRESS(PC_REG++);
            _instance->CycleCounter += 3;
        }

        public unsafe void Clr_X()// 6F
        {
            MemWrite8(0, INDADDRESS(PC_REG++));
            CC_C = false; //0;
            CC_N = false; //0;
            CC_V = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += 6;
        }

        #endregion

        #region 0x70 - 0x7F

        public unsafe void Neg_E()// 70
        {
            _temp16 = IMMADDRESS(PC_REG);
            _postbyte = MemRead8(_temp16);
            _temp8 = (byte)(0 - _postbyte);
            CC_C = _temp8 > 0;
            CC_V = _postbyte == 0x80;
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Oim_E()// 71
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = IMMADDRESS(PC_REG);
            _postbyte |= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 7;
        }

        public unsafe void Aim_E()// 72
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = IMMADDRESS(PC_REG);
            _postbyte &= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 7;
        }

        public unsafe void Com_E()// 73
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            _temp8 = (byte)(0xFF - _temp8);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_C = true; //1;
            CC_V = false; //0;
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Lsr_E()// 74
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)(_temp8 >> 1);
            CC_Z = ZTEST(_temp8);
            CC_N = false; //0;
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Eim_E()// 75
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = IMMADDRESS(PC_REG);
            _postbyte ^= MemRead8(_temp16);
            MemWrite8(_postbyte, _temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 7;
        }

        public unsafe void Ror_E()// 76
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            _postbyte = CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 >> 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Asr_E()// 77
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            CC_C = (_temp8 & 1) != 0;
            _temp8 = (byte)((_temp8 & 0x80) | (_temp8 >> 1));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Asl_E()// 78
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            CC_C = _temp8 > 0x7F;
            CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);
            _temp8 = (byte)(_temp8 << 1);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Rol_E()// 79
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            _postbyte = CC_C ? (byte)1 : (byte)0;
            CC_C = _temp8 > 0x7F;
            CC_V = CC_C ^ ((_temp8 & 0x40) >> 6 != 0);
            _temp8 = (byte)((_temp8 << 1) | _postbyte);
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Dec_E()// 7A
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            _temp8--;
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = _temp8 == 0x7F;
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Tim_E()// 7B
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = IMMADDRESS(PC_REG);
            _postbyte &= MemRead8(_temp16);
            CC_N = NTEST8(_postbyte);
            CC_Z = ZTEST(_postbyte);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 7;
        }

        public unsafe void Inc_E()// 7C
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp8 = MemRead8(_temp16);
            _temp8++;
            CC_Z = ZTEST(_temp8);
            CC_V = _temp8 == 0x80;
            CC_N = NTEST8(_temp8);
            MemWrite8(_temp8, _temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Tst_E()// 7D
        {
            _temp8 = MemRead8(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(_temp8);
            CC_N = NTEST8(_temp8);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Jmp_E()// 7E
        {
            PC_REG = IMMADDRESS(PC_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Clr_E()// 7F
        {
            MemWrite8(0, IMMADDRESS(PC_REG));
            CC_C = false; //0;
            CC_N = false; //0;
            CC_V = false; //0;
            CC_Z = true; //1;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        #endregion

        #region 0x80 - 0x8F

        public unsafe void Suba_M()// 80
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (byte)(A_REG - _postbyte);
            CC_C = ((_temp16 & 0x100) >> 8) != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Cmpa_M()// 81
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = (byte)(A_REG - _postbyte);
            CC_C = _temp8 > A_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, A_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 2;
        }

        public unsafe void Sbca_M()// 82
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(A_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Subd_M()// 83
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp32 = (uint)(D_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Anda_M()// 84
        {
            A_REG &= MemRead8(PC_REG++);
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Bita_M()// 85
        {
            _temp8 = (byte)(A_REG & MemRead8(PC_REG++));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Lda_M()// 86
        {
            A_REG = MemRead8(PC_REG++);
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        // 87		//InvalidInsHandler

        public unsafe void Eora_M()// 88
        {
            A_REG ^= MemRead8(PC_REG++);
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Adca_M()// 89
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(A_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            CC_H = ((A_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Ora_M()// 8A
        {
            A_REG |= MemRead8(PC_REG++);
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Adda_M()// 8B
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(A_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((A_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Cmpx_M()// 8C
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(X_REG - _postword);
            CC_C = _temp16 > X_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, X_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Bsr_R()// 8D
        {
            _postbyte = MemRead8(PC_REG++);
            S_REG--;
            MemWrite8(PC_L, S_REG--);
            MemWrite8(PC_H, S_REG);
            PC_REG += (ushort)(sbyte)(_postbyte);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ldx_M()// 8E
        {
            X_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 3;
        }

        // 8F		//InvalidInsHandler

        #endregion

        #region 0x90 - 0x9F

        public unsafe void Suba_D()// 90
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Cmpa_D()// 91
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp8 = (byte)(A_REG - _postbyte);
            CC_C = _temp8 > A_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, A_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Scba_D()// 92
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG - _postbyte - (CC_C ? (byte)1 : (byte)0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Subd_D()// 93
        {
            _temp16 = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles64;
        }

        public unsafe void Anda_D()// 94
        {
            A_REG &= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Bita_D()// 95
        {
            _temp8 = (byte)(A_REG & MemRead8(DPADDRESS(PC_REG++)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Lda_D()// 96
        {
            A_REG = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Sta_D()// 97
        {
            MemWrite8(A_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Eora_D()// 98
        {
            A_REG ^= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Adca_D()// 99
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            CC_H = ((A_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Ora_D()// 9A
        {
            A_REG |= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Adda_D()// 9B
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((A_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Cmpx_D()// 9C
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(X_REG - _postword);
            CC_C = _temp16 > X_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, X_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles64;
        }

        public unsafe void Jsr_D()// 9D
        {
            _temp16 = DPADDRESS(PC_REG++);
            S_REG--;
            MemWrite8(PC_L, S_REG--);
            MemWrite8(PC_H, S_REG);
            PC_REG = _temp16;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ldx_D()// 9E
        {
            X_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Stx_D()// 9F
        {
            MemWrite16(X_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        #endregion

        #region 0xA0 - 0xAF

        public unsafe void Suba_X()// A0
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Cmpa_X()// A1
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp8 = (byte)(A_REG - _postbyte);
            CC_C = _temp8 > A_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, A_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 4;
        }

        public unsafe void Sbca_X()// A2
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Subd_X()// A3
        {
            _temp16 = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Anda_X()// A4
        {
            A_REG &= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Bita_X()// A5
        {
            _temp8 = (byte)(A_REG & MemRead8(INDADDRESS(PC_REG++)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Lda_X()// A6
        {
            A_REG = MemRead8(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Sta_X()// A7
        {
            MemWrite8(A_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Eora_X()// a8
        {
            A_REG ^= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Adca_X()// A9
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            CC_H = ((A_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Ora_X()// AA
        {
            A_REG |= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Adda_X()// AB
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(A_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((A_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Cmpx_X()// AC
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(X_REG - _postword);
            CC_C = _temp16 > X_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, X_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Jsr_X()// AD
        {
            _temp16 = INDADDRESS(PC_REG++);
            S_REG--;
            MemWrite8(PC_L, S_REG--);
            MemWrite8(PC_H, S_REG);
            PC_REG = _temp16;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ldx_X()// AE
        {
            X_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Stx_X()// AF
        {
            MemWrite16(X_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        #endregion

        #region 0xB0 - 0xBF

        public unsafe void Suba_E()// B0
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(A_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpa_E()// B1
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp8 = (byte)(A_REG - _postbyte);
            CC_C = _temp8 > A_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, A_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Sbca_E()// B2
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(A_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Subd_E()// B3
        {
            _temp16 = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(D_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Anda_E()// B4
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            A_REG &= _postbyte;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Bita_E()// B5
        {
            _temp8 = (byte)(A_REG & MemRead8(IMMADDRESS(PC_REG)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Lda_E()// B6
        {
            A_REG = MemRead8(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Sta_E()// B7
        {
            MemWrite8(A_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(A_REG);
            CC_N = NTEST8(A_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Eora_E()// B8
        {
            A_REG ^= MemRead8(IMMADDRESS(PC_REG));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Adca_E()// B9
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(A_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            CC_H = ((A_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ora_E()// BA
        {
            A_REG |= MemRead8(IMMADDRESS(PC_REG));
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Adda_E()// BB
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(A_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((A_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, A_REG);
            A_REG = (byte)_temp16;
            CC_N = NTEST8(A_REG);
            CC_Z = ZTEST(A_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpx_E()// BC
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(X_REG - _postword);
            CC_C = _temp16 > X_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, X_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Bsr_E()// BD
        {
            _postword = IMMADDRESS(PC_REG);
            PC_REG += 2;
            S_REG--;
            MemWrite8(PC_L, S_REG--);
            MemWrite8(PC_H, S_REG);
            PC_REG = _postword;
            _instance->CycleCounter += _instance->NatEmuCycles87;
        }

        public unsafe void Ldx_E()// BE
        {
            X_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Stx_E()// BF
        {
            MemWrite16(X_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(X_REG);
            CC_N = NTEST16(X_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #region 0xC0 - 0CF

        public unsafe void Subb_M()// C0
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(B_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Cmpb_M()// C1
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = (byte)(B_REG - _postbyte);
            CC_C = _temp8 > B_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, B_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 2;
        }

        public unsafe void Sbcb_M()// C2
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(B_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Addd_M()// C3
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp32 = (uint)(D_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Andb_M()// C4
        {
            B_REG &= MemRead8(PC_REG++);
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Bitb_M()// C5
        {
            _temp8 = (byte)(B_REG & MemRead8(PC_REG++));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Ldb_M()// C6
        {
            B_REG = MemRead8(PC_REG++);
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        // C7	//InvalidInsHandler

        public unsafe void Eorb_M()// C8
        {
            B_REG ^= MemRead8(PC_REG++);
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Adcb_M()// C9
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(B_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            CC_H = ((B_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Orb_M()// CA
        {
            B_REG |= MemRead8(PC_REG++);
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 2;
        }

        public unsafe void Addb_M()// CB
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(B_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((B_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 2;
        }

        public unsafe void Ldd_M()// CC
        {
            D_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 3;
        }

        public unsafe void Ldq_M()// CD
        {
            Q_REG = MemRead32(PC_REG);
            PC_REG += 4;
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Ldu_M()// CE
        {
            U_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 3;
        }

        // CF	//InvalidInsHandler

        #endregion

        #region 0xD0 - 0xDF

        public unsafe void Subb_D()// D0
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Cmpb_D()// D1
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp8 = (byte)(B_REG - _postbyte);
            CC_C = _temp8 > B_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, B_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Sbcb_D()// D2
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Addd_D()// D3
        {
            _temp16 = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles64;
        }

        public unsafe void Andb_D()// D4
        {
            B_REG &= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Bitb_D()// D5
        {
            _temp8 = (byte)(B_REG & MemRead8(DPADDRESS(PC_REG++)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Ldb_D()// D6
        {
            B_REG = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Stb_D()// D7
        {
            MemWrite8(B_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Eorb_D()// D8
        {
            B_REG ^= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Adcb_D()// D9
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            CC_H = ((B_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Orb_D()// DA
        {
            B_REG |= MemRead8(DPADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Addb_D()// DB
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((B_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += _instance->NatEmuCycles43;
        }

        public unsafe void Ldd_D()// DC
        {
            D_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Std_D()// DD
        {
            MemWrite16(D_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldu_D()// DE
        {
            U_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Stu_D()// DF
        {
            MemWrite16(U_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        #endregion

        #region 0xE0 - 0xEF

        public unsafe void Subb_X()// E0
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Cmpb_X()// E1
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp8 = (byte)(B_REG - _postbyte);
            CC_C = _temp8 > B_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, B_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 4;
        }

        public unsafe void Sbcb_X()// E2
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Addd_X()// E3
        {
            _temp16 = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Andb_X()// E4
        {
            B_REG &= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Bitb_X()// E5
        {
            _temp8 = (byte)(B_REG & MemRead8(INDADDRESS(PC_REG++)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Ldb_X()// E6
        {
            B_REG = MemRead8(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Stb_X()// E7
        {
            MemWrite8(B_REG, HD6309_CalculateEA(MemRead8(PC_REG++)));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Eorb_X()// E8
        {
            B_REG ^= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Adcb_X()// E9
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            CC_H = ((B_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0;
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Orb_X()// EA
        {
            B_REG |= MemRead8(INDADDRESS(PC_REG++));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 4;
        }

        public unsafe void Addb_X()// EB
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(B_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((B_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            _instance->CycleCounter += 4;
        }

        public unsafe void Ldd_X()// EC
        {
            D_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Std_X()// ED
        {
            MemWrite16(D_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Ldu_X()// EE
        {
            U_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Stu_X()// EF
        {
            MemWrite16(U_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false;//0;
            _instance->CycleCounter += 5;
        }

        #endregion

        #region 0xF0 - 0xFF

        public unsafe void Subb_E()// F0
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(B_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpb_E()// F1
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp8 = (byte)(B_REG - _postbyte);
            CC_C = _temp8 > B_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, B_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Sbcb_E()// F2
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(B_REG - _postbyte - (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Addd_E()// F3
        {
            _temp16 = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(D_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Andb_E()// F4
        {
            B_REG &= MemRead8(IMMADDRESS(PC_REG));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Bitb_E()// F5
        {
            _temp8 = (byte)(B_REG & MemRead8(IMMADDRESS(PC_REG)));
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldb_E()// F6
        {
            B_REG = MemRead8(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Stb_E()// F7
        {
            MemWrite8(B_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(B_REG);
            CC_N = NTEST8(B_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Eorb_E()// F8
        {
            B_REG ^= MemRead8(IMMADDRESS(PC_REG));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Adcb_E()// F9
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(B_REG + _postbyte + (CC_C ? 1 : 0));
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            CC_H = ((B_REG ^ _temp16 ^ _postbyte) & 0x10) >> 4 != 0; ;
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Orb_E()// FA
        {
            B_REG |= MemRead8(IMMADDRESS(PC_REG));
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Addb_E()// FB
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(B_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((B_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, B_REG);
            B_REG = (byte)_temp16;
            CC_N = NTEST8(B_REG);
            CC_Z = ZTEST(B_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldd_E()// FC
        {
            D_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Std_E()// FD
        {
            MemWrite16(D_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Ldu_E()// FE
        {
            U_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Stu_E()// FF
        {
            MemWrite16(U_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(U_REG);
            CC_N = NTEST16(U_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #endregion

        #region Page2 OpCodes

        //0x00 - 0x0F
        //0x10 - 0x1F

        #region 0x20 - 0x2F

        public unsafe void LBrn_R() // 21 
        {
            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBhi_R() // 22 
        {
            if (!(CC_C | CC_Z))
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBls_R() // 23 
        {
            if (CC_C | CC_Z)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBhs_R() // 24 
        {
            if (!CC_C)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 6;

        }

        public unsafe void LBcs_R() // 25 
        {
            if (CC_C)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBne_R() // 26 
        {
            if (!CC_Z)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBeq_R() // 27 
        {
            if (CC_Z)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBvc_R() // 28 
        {
            if (!CC_V)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBvs_R() // 29 
        {
            if (CC_V)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBpl_R() // 2A 
        {
            if (!CC_N)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBmi_R() // 2B 
        {
            if (CC_N)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBge_R() // 2C 
        {
            if (!(CC_N ^ CC_V))
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBlt_R() // 2D 
        {
            if (CC_V ^ CC_N)
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBgt_R() // 2E 
        {
            if (!(CC_Z | (CC_N ^ CC_V)))
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        public unsafe void LBle_R() // 2F 
        {
            if (CC_Z | (CC_N ^ CC_V))
            {
                PC_REG += (ushort)(short)IMMADDRESS(PC_REG);
                _instance->CycleCounter += 1;
            }

            PC_REG += 2;
            _instance->CycleCounter += 5;
        }

        #endregion

        #region 0x30 - 0x3F

        public unsafe void Addr() // 30 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;

            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7)
            { // 8 bit dest
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7)
                { // 8 bit source
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp16 = (ushort)(source8 + dest8);

                switch (_dest)
                {
                    case 2: HD6309_setcc((byte)_temp16); break;
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
                dest16 = PXF(_dest);

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
                        case 2: source16 = (ushort)HD6309_getcc(); break; // CC
                        case 3: source16 = (ushort)DP_REG; break; // DP
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

            _instance->CycleCounter += 4;
        }

        public unsafe void Adcr() // 31 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7)
                { // 8 bit source
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp16 = (ushort)(source8 + dest8 + (CC_C ? 1 : 0));

                switch (_dest)
                {
                    case 2:
                        HD6309_setcc((byte)_temp16);
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
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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

            _instance->CycleCounter += 4;
        }

        public unsafe void Subr() // 32 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7)
                { // 8 bit source
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
                    }
                    else
                    {
                        source8 = PUR(_source);
                    }
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
                        HD6309_setcc((byte)_temp16);
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
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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

            _instance->CycleCounter += 4;
        }

        public unsafe void Sbcr() // 33 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7) // 8 bit source
                {
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp16 = (ushort)(dest8 - source8 - (CC_C ? 1 : 0));

                switch (_dest)
                {
                    case 2:
                        HD6309_setcc((byte)_temp16);
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
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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

            _instance->CycleCounter += 4;
        }

        public unsafe void Andr() // 34 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7) // 8 bit source
                {
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp8 = (byte)(dest8 & source8);

                switch (_dest)
                {
                    case 2:
                        HD6309_setcc((byte)_temp8);
                        break;

                    case 4:
                    case 5:
                        break; // never assign to zero reg

                    default:
                        PUR(_dest, (byte)_temp8);
                        break;
                }

                CC_N = _temp8 >> 7 != 0;
                CC_Z = ZTEST(_temp8);
            }
            else // 16 bit dest
            {
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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
            _instance->CycleCounter += 4;
        }

        public unsafe void Orr() // 35     
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7) // 8 bit source
                {
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp8 = (byte)(dest8 | source8);

                switch (_dest)
                {
                    case 2:
                        HD6309_setcc((byte)_temp8);
                        break;

                    case 4:
                    case 5:
                        break; // never assign to zero reg

                    default:
                        PUR(_dest, (byte)_temp8);
                        break;
                }

                CC_N = _temp8 >> 7 != 0;
                CC_Z = ZTEST(_temp8);
            }
            else // 16 bit dest
            {
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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
            _instance->CycleCounter += 4;
        }

        public unsafe void Eorr() // 36 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7) // 8 bit source
                {
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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

                _temp8 = (byte)(dest8 ^ source8);

                switch (_dest)
                {
                    case 2:
                        HD6309_setcc((byte)_temp8);
                        break;

                    case 4:
                    case 5:
                        break; // never assign to zero reg

                    default:
                        PUR(_dest, (byte)_temp8);
                        break;
                }

                CC_N = _temp8 >> 7 != 0;
                CC_Z = ZTEST(_temp8);
            }
            else // 16 bit dest
            {
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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
            _instance->CycleCounter += 4;
        }

        public unsafe void Cmpr() // 37 
        {
            byte dest8 = 0, source8 = 0;
            ushort dest16 = 0, source16 = 0;
            _temp8 = MemRead8(PC_REG++);
            _source = (byte)(_temp8 >> 4);
            _dest = (byte)(_temp8 & 15);

            if (_dest > 7) // 8 bit dest
            {
                _dest &= 7;

                if (_dest == 2)
                {
                    dest8 = HD6309_getcc();
                }
                else
                {
                    dest8 = PUR(_dest);
                }

                if (_source > 7) // 8 bit source
                {
                    _source &= 7;

                    if (_source == 2)
                    {
                        source8 = HD6309_getcc();
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
                dest16 = PXF(_dest);

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
                            source16 = (ushort)HD6309_getcc();
                            break; // CC

                        case 3:
                            source16 = (ushort)DP_REG;
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

            _instance->CycleCounter += 4;
        }

        public unsafe void Pshsw() // 38 
        {
            MemWrite8((F_REG), --S_REG);
            MemWrite8((E_REG), --S_REG);
            _instance->CycleCounter += 6;
        }

        public unsafe void Pulsw() // 39 
        {
            E_REG = MemRead8(S_REG++);
            F_REG = MemRead8(S_REG++);
            _instance->CycleCounter += 6;
        }

        public unsafe void Pshuw() // 3A 
        {
            MemWrite8((F_REG), --U_REG);
            MemWrite8((E_REG), --U_REG);
            _instance->CycleCounter += 6;
        }

        public unsafe void Puluw() // 3B 
        {
            E_REG = MemRead8(U_REG++);
            F_REG = MemRead8(U_REG++);
            _instance->CycleCounter += 6;
        }

        public unsafe void Swi2_I() // 3F 
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
                _instance->CycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(HD6309_getcc(), --S_REG);
            PC_REG = MemRead16(Define.VSWI2);
            _instance->CycleCounter += 20;
        }

        #endregion

        #region 0x40 - 0x4F

        public unsafe void Negd_I() // 40 
        {
            D_REG = (ushort)(0 - D_REG);
            CC_C = _temp16 > 0;
            CC_V = D_REG == 0x8000;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Comd_I() // 43 
        {
            D_REG = (ushort)(0xFFFF - D_REG);
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Lsrd_I() // 44 
        {
            CC_C = (D_REG & 1) != 0;
            D_REG = (ushort)(D_REG >> 1);
            CC_Z = ZTEST(D_REG);
            CC_N = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Rord_I() // 46 
        {
            _postword = CC_C ? (ushort)0x8000 : (ushort)0x0000; //CC_C<< 15;
            CC_C = (D_REG & 1) != 0;
            D_REG = (ushort)((D_REG >> 1) | _postword);
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Asrd_I() // 47 
        {
            CC_C = (D_REG & 1) != 0;
            D_REG = (ushort)((D_REG & 0x8000) | (D_REG >> 1));
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Asld_I() // 48 
        {
            CC_C = D_REG >> 15 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((D_REG & 0x4000) >> 14)) != 0;
            D_REG = (ushort)(D_REG << 1);
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Rold_I() // 49 
        {
            _postword = (ushort)(CC_C ? 1 : 0);
            CC_C = D_REG >> 15 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((D_REG & 0x4000) >> 14)) != 0;
            D_REG = (ushort)((D_REG << 1) | _postword);
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Decd_I() // 4A 
        {
            D_REG--;
            CC_Z = ZTEST(D_REG);
            CC_V = D_REG == 0x7FFF;
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Incd_I() // 4C 
        {
            D_REG++;
            CC_Z = ZTEST(D_REG);
            CC_V = D_REG == 0x8000;
            CC_N = NTEST16(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Tstd_I() // 4D 
        {
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Clrd_I() // 4F 
        {
            D_REG = 0;
            CC_C = false; //0;
            CC_V = false; //0;
            CC_N = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        #endregion

        #region 0x50 - 0x5F

        public unsafe void Comw_I() // 53 
        {
            W_REG = (ushort)(0xFFFF - W_REG);
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;

        }

        public unsafe void Lsrw_I() // 54 
        {
            CC_C = (W_REG & 1) != 0;
            W_REG = (ushort)(W_REG >> 1);
            CC_Z = ZTEST(W_REG);
            CC_N = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Rorw_I() // 56 
        {
            _postword = (ushort)((CC_C ? 1 : 0) << 15);
            CC_C = (W_REG & 1) != 0;
            W_REG = (ushort)((W_REG >> 1) | _postword);
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Rolw_I() // 59 
        {
            _postword = CC_C ? (ushort)1 : (ushort)0;
            CC_C = W_REG >> 15 != 0;
            CC_V = ((CC_C ? 1 : 0) ^ ((W_REG & 0x4000) >> 14)) != 0;
            W_REG = (ushort)((W_REG << 1) | _postword);
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Decw_I() // 5A 
        {
            W_REG--;
            CC_Z = ZTEST(W_REG);
            CC_V = W_REG == 0x7FFF;
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Incw_I() // 5C 
        {
            W_REG++;
            CC_Z = ZTEST(W_REG);
            CC_V = W_REG == 0x8000;
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Tstw_I() // 5D 
        {
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Clrw_I() // 5F 
        {
            W_REG = 0;
            CC_C = false; //0;
            CC_V = false; //0;
            CC_N = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        #endregion

        //0x60 - 0x6F
        //0x70 - 0x7F

        #region 0x80 - 0x8F

        public unsafe void Subw_M() // 80 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(W_REG - _postword);
            CC_C = _temp16 > W_REG;
            CC_V = OVERFLOW16(CC_C, _temp16, W_REG, _postword);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            W_REG = _temp16;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpw_M() // 81 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(W_REG - _postword);
            CC_C = _temp16 > W_REG;
            CC_V = OVERFLOW16(CC_C, _temp16, W_REG, _postword);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Sbcd_M() // 82 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp32 = (uint)(D_REG - _postword - (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, D_REG, _postword);
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpd_M() // 83 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(D_REG - _postword);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Andd_M() // 84 
        {
            D_REG &= IMMADDRESS(PC_REG);
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Bitd_M() // 85 
        {
            _temp16 = (ushort)(D_REG & IMMADDRESS(PC_REG));
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldw_M() // 86  
        {
            W_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Eord_M() // 88 
        {
            D_REG ^= IMMADDRESS(PC_REG);
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Adcd_M() // 89 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp32 = (ushort)(D_REG + _postword + (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _postword, (ushort)_temp32, D_REG);
            CC_H = ((D_REG ^ _temp32 ^ _postword) & 0x100) >> 8 != 0;
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ord_M() // 8A  
        {
            D_REG |= IMMADDRESS(PC_REG);
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Addw_M() // 8B 
        {
            _temp16 = IMMADDRESS(PC_REG);
            _temp32 = (uint)(W_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpy_M() // 8C 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(Y_REG - _postword);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldy_M() // 8E  
        {
            Y_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        #endregion

        #region 0x90 - 0x9F

        public unsafe void Subw_D() // 90 
        {
            _temp16 = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(W_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Cmpw_D() // 91 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(W_REG - _postword);
            CC_C = _temp16 > W_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, W_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Sbcd_D() // 92 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG - _postword - (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, D_REG, _postword);
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Cmpd_D() // 93 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(D_REG - _postword);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Andd_D() // 94 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            D_REG &= _postword;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Bitd_D() // 95 
        {
            _temp16 = (ushort)(D_REG & MemRead16(DPADDRESS(PC_REG++)));
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Ldw_D() // 96 
        {
            W_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Stw_D() // 97 
        {
            MemWrite16(W_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Eord_D() // 98 
        {
            D_REG ^= MemRead16(DPADDRESS(PC_REG++));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Adcd_D() // 99 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG + _postword + (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _postword, (ushort)_temp32, D_REG);
            CC_H = ((D_REG ^ _temp32 ^ _postword) & 0x100) >> 8 != 0;
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Ord_D() // 9A 
        {
            D_REG |= MemRead16(DPADDRESS(PC_REG++));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Addw_D() // 9B 
        {
            _temp16 = MemRead16(DPADDRESS(PC_REG++));
            _temp32 = (uint)(W_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Cmpy_D() // 9C 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(Y_REG - _postword);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Ldy_D() // 9E 
        {
            Y_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Sty_D() // 9F 
        {
            MemWrite16(Y_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #region 0xA0 - 0xAF

        public unsafe void Subw_X() // A0 
        {
            _temp16 = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(W_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Cmpw_X() // A1 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(W_REG - _postword);
            CC_C = _temp16 > W_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, W_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Sbcd_X() // A2 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG - _postword - (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _postword, (ushort)_temp32, D_REG);
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Cmpd_X() // A3 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(D_REG - _postword);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Andd_X() // A4 
        {
            D_REG &= MemRead16(INDADDRESS(PC_REG++));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Bitd_X() // A5 
        {
            _temp16 = (ushort)(D_REG & MemRead16(INDADDRESS(PC_REG++)));
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ldw_X() // A6 
        {
            W_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Stw_X() // A7 
        {
            MemWrite16(W_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Eord_X() // A8 
        {
            D_REG ^= MemRead16(INDADDRESS(PC_REG++));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Adcd_X() // A9 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(D_REG + _postword + (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _postword, (ushort)_temp32, D_REG);
            CC_H = (((D_REG ^ _temp32 ^ _postword) & 0x100) >> 8) != 0;
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ord_X() // AA 
        {
            D_REG |= MemRead16(INDADDRESS(PC_REG++));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Addw_X() // AB 
        {
            _temp16 = MemRead16(INDADDRESS(PC_REG++));
            _temp32 = (uint)(W_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Cmpy_X() // AC 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(Y_REG - _postword);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Ldy_X() // AE 
        {
            Y_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Sty_X() // AF 
        {
            MemWrite16(Y_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        #endregion

        #region 0xB0 - 0xBF

        public unsafe void Subw_E() // B0 
        {
            _temp16 = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(W_REG - _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Cmpw_E() // B1 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(W_REG - _postword);
            CC_C = _temp16 > W_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, W_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Sbcd_E() // B2 
        {
            _temp16 = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(D_REG - _temp16 - (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, D_REG);
            D_REG = (ushort)_temp32;
            CC_Z = ZTEST(D_REG);
            CC_N = NTEST16(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Cmpd_E() // B3 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(D_REG - _postword);
            CC_C = _temp16 > D_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, D_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Andd_E() // B4 
        {
            D_REG &= MemRead16(IMMADDRESS(PC_REG));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Bitd_E() // B5 
        {
            _temp16 = (ushort)(D_REG & MemRead16(IMMADDRESS(PC_REG)));
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Ldw_E() // B6 
        {
            W_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Stw_E() // B7 
        {
            MemWrite16(W_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Eord_E() // B8 
        {
            D_REG ^= MemRead16(IMMADDRESS(PC_REG));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Adcd_E() // B9 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(D_REG + _postword + (CC_C ? 1 : 0));
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _postword, (ushort)_temp32, D_REG);
            CC_H = (((D_REG ^ _temp32 ^ _postword) & 0x100) >> 8) != 0;
            D_REG = (ushort)_temp32;
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Ord_E() // BA 
        {
            D_REG |= MemRead16(IMMADDRESS(PC_REG));
            CC_N = NTEST16(D_REG);
            CC_Z = ZTEST(D_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Addw_E() // BB 
        {
            _temp16 = MemRead16(IMMADDRESS(PC_REG));
            _temp32 = (uint)(W_REG + _temp16);
            CC_C = (_temp32 & 0x10000) >> 16 != 0;
            CC_V = OVERFLOW16(CC_C, _temp32, _temp16, W_REG);
            W_REG = (ushort)_temp32;
            CC_Z = ZTEST(W_REG);
            CC_N = NTEST16(W_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Cmpy_E() // BC 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(Y_REG - _postword);
            CC_C = _temp16 > Y_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, Y_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Ldy_E() // BE 
        {
            Y_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Sty_E() // BF 
        {
            MemWrite16(Y_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(Y_REG);
            CC_N = NTEST16(Y_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        #endregion

        #region 0xC0 - 0xCF

        public unsafe void Lds_I() // CE 
        {
            S_REG = IMMADDRESS(PC_REG);
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += 4;
        }

        #endregion

        #region 0xD0 - 0xDF

        public unsafe void Ldq_D() // DC 
        {
            Q_REG = MemRead32(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles87;
        }

        public unsafe void Stq_D() // DD 
        {
            MemWrite32(Q_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles87;
        }

        public unsafe void Lds_D() // DE 
        {
            S_REG = MemRead16(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Sts_D() // DF 
        {
            MemWrite16(S_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #region 0xE0 - 0xEF

        public unsafe void Ldq_X() // EC 
        {
            Q_REG = MemRead32(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 8;
        }

        public unsafe void Stq_X() // ED 
        {
            MemWrite32(Q_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 8;
        }

        public unsafe void Lds_X() // EE 
        {
            S_REG = MemRead16(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        public unsafe void Sts_X() // EF 
        {
            MemWrite16(S_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 6;
        }

        #endregion

        #region 0xF0 - 0xFF

        public unsafe void Ldq_E() // FC 
        {
            Q_REG = MemRead32(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles98;
        }

        public unsafe void Stq_E() // FD 
        {
            MemWrite32(Q_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(Q_REG);
            CC_N = NTEST32(Q_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles98;
        }

        public unsafe void Lds_E() // FE 
        {
            S_REG = MemRead16(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Sts_E() // FF 
        {
            MemWrite16(S_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(S_REG);
            CC_N = NTEST16(S_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        #endregion

        #endregion

        #region Page3 OpCodes

        //0x00 - 0x0F
        //0x10 - 0x1F
        //0x20 - 0x2F

        #region 0x30 - 0x3F

        public unsafe void Band() // 30 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)((_postbyte) & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) == 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) &= (byte)~(1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) &= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() & ~(1 << _dest)));
                        break;
                }
            }

            // Else nothing changes
            _instance->CycleCounter += _instance->NatEmuCycles76;

        }

        public unsafe void Biand() // 31 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) &= ~(1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) &= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() & ~(1 << _dest)));
                        break;
                }
            }

            // Else do nothing
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Bor() // 32 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) |= (1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) |= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() | (1 << _dest)));
                        break;
                }
            }

            // Else do nothing
            _instance->CycleCounter += _instance->NatEmuCycles76;

        }

        public unsafe void Bior() // 33 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) == 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) |= (1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) |= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() | (1 << _dest)));
                        break;
                }
            }

            // Else do nothing
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Beor() // 34 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) ^= (1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) ^= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() ^ (1 << _dest)));
                        break;
                }
            }

            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Bieor() // 35 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) == 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) ^= (1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) ^= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() ^ (1 << _dest)));
                        break;
                }
            }

            _instance->CycleCounter += _instance->NatEmuCycles76;

        }

        public unsafe void Ldbt() // 36 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp8 = MemRead8(DPADDRESS(PC_REG++));
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            if ((_temp8 & (1 << _source)) != 0)
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) |= (1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) |= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() | (1 << _dest)));
                        break;
                }
            }
            else
            {
                switch (_postbyte)
                {
                    case 0: // A Reg
                    case 1: // B Reg
                        //PUR(postbyte) &= ~(1 << Dest);
                        *(byte*)(_instance->ureg8[_postbyte]) &= (byte)~(1 << _dest);
                        break;

                    case 2: // CC Reg
                        HD6309_setcc((byte)(HD6309_getcc() & ~(1 << _dest)));
                        break;
                }
            }

            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Stbt() // 37 
        {


            _postbyte = MemRead8(PC_REG++);
            _temp16 = DPADDRESS(PC_REG++);
            _temp8 = MemRead8(_temp16);
            _source = (byte)((_postbyte >> 3) & 7);
            _dest = (byte)(_postbyte & 7);
            _postbyte >>= 6;

            if (_postbyte == 3)
            {
                InvalidInsHandler();
                return;
            }

            switch (_postbyte)
            {
                case 0: // A Reg
                case 1: // B Reg
                    _postbyte = PUR(_postbyte);
                    break;

                case 2: // CC Reg
                    _postbyte = HD6309_getcc();
                    break;
            }

            if ((_postbyte & (1 << _source)) != 0)
            {
                _temp8 |= (byte)(1 << _dest);
            }
            else
            {
                _temp8 &= (byte)~(1 << _dest);
            }

            MemWrite8(_temp8, _temp16);
            _instance->CycleCounter += _instance->NatEmuCycles87;
        }

        public unsafe void Tfm1() // 38 
        {


            if (W_REG == 0)
            {
                _instance->CycleCounter += 6;
                PC_REG++;
                return;
            }

            _postbyte = MemRead8(PC_REG);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            if (_source > 4 || _dest > 4)
            {
                InvalidInsHandler();
                return;
            }

            _temp8 = MemRead8(PXF(_source));
            MemWrite8(_temp8, PXF(_dest));
            PXF(_dest, (ushort)(PXF(_dest) + 1));
            PXF(_source, (ushort)(PXF(_source) + 1));
            W_REG--;
            _instance->CycleCounter += 3;
            PC_REG -= 2;
        }

        public unsafe void Tfm2() // 39 
        {


            if (W_REG == 0)
            {
                _instance->CycleCounter += 6;
                PC_REG++;
                return;
            }

            _postbyte = MemRead8(PC_REG);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            if (_source > 4 || _dest > 4)
            {
                InvalidInsHandler();
                return;
            }

            _temp8 = MemRead8(PXF(_source));
            MemWrite8(_temp8, PXF(_dest));
            PXF(_dest, (ushort)(PXF(_dest) - 1));
            PXF(_source, (ushort)(PXF(_source) - 1));
            W_REG--;
            _instance->CycleCounter += 3;
            PC_REG -= 2;
        }

        public unsafe void Tfm3() // 3A 
        {


            if (W_REG == 0)
            {
                _instance->CycleCounter += 6;
                PC_REG++;
                return;
            }

            _postbyte = MemRead8(PC_REG);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            if (_source > 4 || _dest > 4)
            {
                InvalidInsHandler();
                return;
            }

            _temp8 = MemRead8(PXF(_source));
            MemWrite8(_temp8, PXF(_dest));
            PXF(_source, (ushort)(PXF(_source) + 1));
            W_REG--;
            PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
            _instance->CycleCounter += 3;
        }

        public unsafe void Tfm4() // 3B 
        {


            if (W_REG == 0)
            {
                _instance->CycleCounter += 6;
                PC_REG++;
                return;
            }

            _postbyte = MemRead8(PC_REG);
            _source = (byte)(_postbyte >> 4);
            _dest = (byte)(_postbyte & 15);

            if (_source > 4 || _dest > 4)
            {
                InvalidInsHandler();
                return;
            }

            _temp8 = MemRead8(PXF(_source));
            MemWrite8(_temp8, PXF(_dest));
            PXF(_dest, (ushort)(PXF(_dest) + 1));
            W_REG--;
            PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
            _instance->CycleCounter += 3;
        }

        public unsafe void Bitmd_M() // 3C 
        {


            _postbyte = (byte)(MemRead8(PC_REG++) & 0xC0);
            _temp8 = (byte)(HD6309_getmd() & _postbyte);
            CC_Z = ZTEST(_temp8);

            if ((_temp8 & 0x80) != 0) MD_ZERODIV = false; //0;
            if ((_temp8 & 0x40) != 0) MD_ILLEGAL = false; //0;

            _instance->CycleCounter += 4;
        }

        public unsafe void Ldmd_M() // 3D 
        {


            _instance->mdbits = (byte)(MemRead8(PC_REG++) & 0x03);
            HD6309_setmd(_instance->mdbits);
            _instance->CycleCounter += 5;
        }

        public unsafe void Swi3_I() // 3F 
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
                _instance->CycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(HD6309_getcc(), --S_REG);
            PC_REG = MemRead16(Define.VSWI3);
            _instance->CycleCounter += 20;
        }

        #endregion

        #region 0x40 - 0x4F

        public unsafe void Come_I() // 43 
        {


            E_REG = (byte)(0xFF - E_REG);
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Dece_I() // 4A 
        {


            E_REG--;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = E_REG == 0x7F;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Ince_I() // 4C 
        {


            E_REG++;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = E_REG == 0x80;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Tste_I() // 4D 
        {


            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Clre_I() // 4F 
        {


            E_REG = 0;
            CC_C = false; //0;
            CC_V = false; //0;
            CC_N = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        #endregion

        #region 0x50 - 0x5F

        public unsafe void Comf_I() // 53 
        {
            F_REG = (byte)(0xFF - F_REG);
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_C = true; //1;
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Decf_I() // 5A 
        {
            F_REG--;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = F_REG == 0x7F;
            _instance->CycleCounter += _instance->NatEmuCycles21;
        }

        public unsafe void Incf_I() // 5C 
        {
            F_REG++;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = F_REG == 0x80;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Tstf_I() // 5D 
        {
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        public unsafe void Clrf_I() // 5F 
        {
            F_REG = 0;
            CC_C = false; //0;
            CC_V = false; //0;
            CC_N = false; //0;
            CC_Z = true; //1;
            _instance->CycleCounter += _instance->NatEmuCycles32;
        }

        #endregion

        //0x60 - 0x6F
        //0x70 - 0x7F

        #region 0x80 - 0x8F

        public unsafe void Sube_M() // 80 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(E_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            _instance->CycleCounter += 3;
        }

        public unsafe void Cmpe_M() // 81 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = (byte)(E_REG - _postbyte);
            CC_C = _temp8 > E_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, E_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 3;
        }

        public unsafe void Cmpu_M() // 83 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(U_REG - _postword);
            CC_C = _temp16 > U_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, U_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Lde_M() // 86 
        {
            E_REG = MemRead8(PC_REG++);
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 3;
        }

        public unsafe void Adde_M() // 8B 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(E_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((E_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_N = NTEST8(E_REG);
            CC_Z = ZTEST(E_REG);
            _instance->CycleCounter += 3;
        }

        public unsafe void Cmps_M() // 8C 
        {
            _postword = IMMADDRESS(PC_REG);
            _temp16 = (ushort)(S_REG - _postword);
            CC_C = _temp16 > S_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, S_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Divd_M() // 8D 
        {
            _postbyte = MemRead8(PC_REG++);

            if (_postbyte == 0)
            {
                _instance->CycleCounter += 3;
                DivbyZero();
                return;
            }

            _postword = D_REG;
            _stemp16 = (short)((short)_postword / (sbyte)_postbyte);

            if ((_stemp16 > 255) || (_stemp16 < -256)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += 17;
                return;
            }

            A_REG = (byte)((short)_postword % (sbyte)_postbyte);
            B_REG = (byte)_stemp16;

            if ((_stemp16 > 127) || (_stemp16 < -128))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(B_REG);
                CC_N = NTEST8(B_REG);
                CC_V = false; //0;
            }

            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += 25;
        }

        public unsafe void Divq_M() // 8E 
        {
            _postword = MemRead16(PC_REG);
            PC_REG += 2;

            if (_postword == 0)
            {
                _instance->CycleCounter += 4;
                DivbyZero();
                return;
            }

            _temp32 = Q_REG;
            _stemp32 = (int)_temp32 / (short)_postword;

            if ((_stemp32 > 65535) || (_stemp32 < -65536)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += 34 - 21;
                return;
            }

            D_REG = (ushort)((int)_temp32 % (short)_postword);
            W_REG = (ushort)_stemp32;

            if ((_stemp16 > 32767) || (_stemp16 < -32768))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(W_REG);
                CC_N = NTEST16(W_REG);
                CC_V = false; //0;
            }

            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += 34;
        }

        public unsafe void Muld_M() // 8F 
        {
            Q_REG = (uint)((short)D_REG * (short)IMMADDRESS(PC_REG));
            CC_C = false; //0;
            CC_Z = ZTEST(Q_REG);
            CC_V = false; //0;
            CC_N = NTEST32(Q_REG);
            PC_REG += 2;
            _instance->CycleCounter += 28;
        }

        #endregion

        #region 0x90 - 0x9F

        public unsafe void Sube_D() // 90 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(E_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpe_D() // 91 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp8 = (byte)(E_REG - _postbyte);
            CC_C = _temp8 > E_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, E_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpu_D() // 93 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(U_REG - _postword);
            CC_C = _temp16 > U_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, U_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Lde_D() // 96 
        {
            E_REG = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ste_D() // 97 
        {
            MemWrite8(E_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Adde_D() // 9B 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(E_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((E_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_N = NTEST8(E_REG);
            CC_Z = ZTEST(E_REG);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmps_D() // 9C 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(S_REG - _postword);
            CC_C = _temp16 > S_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, S_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles75;
        }

        public unsafe void Divd_D() // 9D 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));

            if (_postbyte == 0)
            {
                _instance->CycleCounter += 3;
                DivbyZero();
                return;
            }

            _postword = D_REG;
            _stemp16 = (short)((short)_postword / (char)_postbyte);

            if ((_stemp16 > 255) || (_stemp16 < -256)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += 19;
                return;
            }

            A_REG = (byte)((short)_postword % (sbyte)_postbyte);
            B_REG = (byte)_stemp16;

            if ((_stemp16 > 127) || (_stemp16 < -128))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(B_REG);
                CC_N = NTEST8(B_REG);
                CC_V = false; //0;
            }

            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += 27;
        }

        public unsafe void Divq_D() // 9E 
        {
            _postword = MemRead16(DPADDRESS(PC_REG++));

            if (_postword == 0)
            {
                _instance->CycleCounter += 4;
                DivbyZero();
                return;
            }

            _temp32 = Q_REG;
            _stemp32 = (int)_temp32 / (short)_postword;

            if ((_stemp32 > 65535) || (_stemp32 < -65536)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += _instance->NatEmuCycles3635 - 21;
                return;
            }

            D_REG = (ushort)((int)_temp32 % (short)_postword);
            W_REG = (ushort)_stemp32;

            if ((_stemp16 > 32767) || (_stemp32 < -32768))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(W_REG);
                CC_N = NTEST16(W_REG);
                CC_V = false; //0;
            }

            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += _instance->NatEmuCycles3635;
        }

        public unsafe void Muld_D() // 9F 
        {
            Q_REG = (uint)((short)D_REG * (short)MemRead16(DPADDRESS(PC_REG++)));
            CC_C = false; //0;
            CC_Z = ZTEST(Q_REG);
            CC_V = false; //0;
            CC_N = NTEST32(Q_REG);
            _instance->CycleCounter += _instance->NatEmuCycles3029;
        }

        #endregion

        #region 0xA0 - 0xAF

        public unsafe void Sube_X() // A0 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(E_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            _instance->CycleCounter += 5;
        }

        public unsafe void Cmpe_X() // A1 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp8 = (byte)(E_REG - _postbyte);
            CC_C = _temp8 > E_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, E_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 5;
        }

        public unsafe void Cmpu_X() // A3 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(U_REG - _postword);
            CC_C = _temp16 > U_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, U_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Lde_X() // A6 
        {
            E_REG = MemRead8(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Ste_X() // A7 
        {
            MemWrite8(E_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Adde_X() // AB 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(E_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((E_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_N = NTEST8(E_REG);
            CC_Z = ZTEST(E_REG);
            _instance->CycleCounter += 5;
        }

        public unsafe void Cmps_X() // AC 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(S_REG - _postword);
            CC_C = _temp16 > S_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, S_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            _instance->CycleCounter += _instance->NatEmuCycles76;
        }

        public unsafe void Divd_X() // AD 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));

            if (_postbyte == 0)
            {
                _instance->CycleCounter += 3;
                DivbyZero();
                return;
            }

            _postword = D_REG;
            _stemp16 = (short)((short)_postword / (sbyte)_postbyte);

            if ((_stemp16 > 255) || (_stemp16 < -256)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += 19;
                return;
            }

            A_REG = (byte)((short)_postword % (sbyte)_postbyte);
            B_REG = (byte)_stemp16;

            if ((_stemp16 > 127) || (_stemp16 < -128))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(B_REG);
                CC_N = NTEST8(B_REG);
                CC_V = false; //0;
            }
            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += 27;
        }

        public unsafe void Divq_X() // AE 
        {
            _postword = MemRead16(INDADDRESS(PC_REG++));

            if (_postword == 0)
            {
                _instance->CycleCounter += 4;
                DivbyZero();
                return;
            }

            _temp32 = Q_REG;
            _stemp32 = (int)_temp32 / (short)_postword;

            if ((_stemp32 > 65535) || (_stemp32 < -65536)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += _instance->NatEmuCycles3635 - 21;
                return;
            }

            D_REG = (ushort)((int)_temp32 % (short)_postword);
            W_REG = (ushort)_stemp32;

            if ((_stemp16 > 32767) || (_stemp16 < -32768))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(W_REG);
                CC_N = NTEST16(W_REG);
                CC_V = false; //0;
            }

            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += _instance->NatEmuCycles3635;
        }

        public unsafe void Muld_X() // AF 
        {
            Q_REG = (ushort)((short)D_REG * (short)MemRead16(INDADDRESS(PC_REG++)));
            CC_C = false; //0;
            CC_Z = ZTEST(Q_REG);
            CC_V = false; //0;
            CC_N = NTEST32(Q_REG);
            _instance->CycleCounter += 30;
        }

        #endregion

        #region 0xB0 - 0xBF

        public unsafe void Sube_E() // B0 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(E_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Cmpe_E() // B1 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp8 = (byte)(E_REG - _postbyte);
            CC_C = _temp8 > E_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, E_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Cmpu_E() // B3 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(U_REG - _postword);
            CC_C = _temp16 > U_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, U_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Lde_E() // B6 
        {
            E_REG = MemRead8(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Ste_E() // B7 
        {
            MemWrite8(E_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(E_REG);
            CC_N = NTEST8(E_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Adde_E() // BB 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(E_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((E_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, E_REG);
            E_REG = (byte)_temp16;
            CC_N = NTEST8(E_REG);
            CC_Z = ZTEST(E_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Cmps_E() // BC 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(S_REG - _postword);
            CC_C = _temp16 > S_REG;
            CC_V = OVERFLOW16(CC_C, _postword, _temp16, S_REG);
            CC_N = NTEST16(_temp16);
            CC_Z = ZTEST(_temp16);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles86;
        }

        public unsafe void Divd_E() // BD 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            PC_REG += 2;

            if (_postbyte == 0)
            {
                _instance->CycleCounter += 3;
                DivbyZero();
                return;
            }

            _postword = D_REG;
            _stemp16 = (short)((short)_postword / (sbyte)_postbyte);

            if ((_stemp16 > 255) || (_stemp16 < -256)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += 17;
                return;
            }

            A_REG = (byte)((short)_postword % (sbyte)_postbyte);
            B_REG = (byte)_stemp16;

            if ((_stemp16 > 127) || (_stemp16 < -128))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(B_REG);
                CC_N = NTEST8(B_REG);
                CC_V = false; //0;
            }
            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += 25;
        }

        public unsafe void Divq_E() // BE 
        {
            _postword = MemRead16(IMMADDRESS(PC_REG));
            PC_REG += 2;

            if (_postword == 0)
            {
                _instance->CycleCounter += 4;
                DivbyZero();
                return;
            }

            _temp32 = Q_REG;
            _stemp32 = (int)_temp32 / (short)_postword;

            if ((_stemp32 > 65535) || (_stemp32 < -65536)) //Abort
            {
                CC_V = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_C = false; //0;
                _instance->CycleCounter += _instance->NatEmuCycles3635 - 21;
                return;
            }

            D_REG = (ushort)((int)_temp32 % (short)_postword);
            W_REG = (ushort)_stemp32;

            if ((_stemp16 > 32767) || (_stemp16 < -32768))
            {
                CC_V = true; //1;
                CC_N = true; //1;
            }
            else
            {
                CC_Z = ZTEST(W_REG);
                CC_N = NTEST16(W_REG);
                CC_V = false; //0;
            }
            CC_C = (B_REG & 1) != 0;
            _instance->CycleCounter += _instance->NatEmuCycles3635;
        }

        public unsafe void Muld_E() // BF 
        {
            Q_REG = (ushort)((short)D_REG * (short)MemRead16(IMMADDRESS(PC_REG)));
            PC_REG += 2;
            CC_C = false; //0;
            CC_Z = ZTEST(Q_REG);
            CC_V = false; //0;
            CC_N = NTEST32(Q_REG);
            _instance->CycleCounter += _instance->NatEmuCycles3130;
        }

        #endregion

        #region 0xC0 - 0xCF

        public unsafe void Subf_M() // C0 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(F_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            _instance->CycleCounter += 3;
        }

        public unsafe void Cmpf_M() // C1 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp8 = (byte)(F_REG - _postbyte);
            CC_C = _temp8 > F_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, F_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 3;
        }

        public unsafe void Ldf_M() // C6 
        {
            F_REG = MemRead8(PC_REG++);
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 3;
        }

        public unsafe void Addf_M() // CB 
        {
            _postbyte = MemRead8(PC_REG++);
            _temp16 = (ushort)(F_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((F_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_N = NTEST8(F_REG);
            CC_Z = ZTEST(F_REG);
            _instance->CycleCounter += 3;
        }

        #endregion

        #region 0xD0 - 0xDF

        public unsafe void Subf_D() // D0 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(F_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Cmpf_D() // D1 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp8 = (byte)(F_REG - _postbyte);
            CC_C = _temp8 > F_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, F_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Ldf_D() // D6 
        {
            F_REG = MemRead8(DPADDRESS(PC_REG++));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Stf_D() // D7 
        {
            MemWrite8(F_REG, DPADDRESS(PC_REG++));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        public unsafe void Addf_D() // DB 
        {
            _postbyte = MemRead8(DPADDRESS(PC_REG++));
            _temp16 = (ushort)(F_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((F_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_N = NTEST8(F_REG);
            CC_Z = ZTEST(F_REG);
            _instance->CycleCounter += _instance->NatEmuCycles54;
        }

        #endregion

        #region 0xE0 - 0xEF

        public unsafe void Subf_X() // E0 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(F_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            _instance->CycleCounter += 5;
        }

        public unsafe void Cmpf_X() // E1 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp8 = (byte)(F_REG - _postbyte);
            CC_C = _temp8 > F_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, F_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            _instance->CycleCounter += 5;
        }

        public unsafe void Ldf_X() // E6 
        {
            F_REG = MemRead8(INDADDRESS(PC_REG++));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Stf_X() // E7 
        {
            MemWrite8(F_REG, INDADDRESS(PC_REG++));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            _instance->CycleCounter += 5;
        }

        public unsafe void Addf_X() // EB 
        {
            _postbyte = MemRead8(INDADDRESS(PC_REG++));
            _temp16 = (ushort)(F_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((F_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_N = NTEST8(F_REG);
            CC_Z = ZTEST(F_REG);
            _instance->CycleCounter += 5;
        }

        #endregion

        #region 0xF0 - 0xFF

        public unsafe void Subf_E() // F0 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(F_REG - _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Cmpf_E() // F1 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp8 = (byte)(F_REG - _postbyte);
            CC_C = _temp8 > F_REG;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp8, F_REG);
            CC_N = NTEST8(_temp8);
            CC_Z = ZTEST(_temp8);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Ldf_E() // F6 
        {
            F_REG = MemRead8(IMMADDRESS(PC_REG));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Stf_E() // F7 
        {
            MemWrite8(F_REG, IMMADDRESS(PC_REG));
            CC_Z = ZTEST(F_REG);
            CC_N = NTEST8(F_REG);
            CC_V = false; //0;
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        public unsafe void Addf_E() // FB 
        {
            _postbyte = MemRead8(IMMADDRESS(PC_REG));
            _temp16 = (ushort)(F_REG + _postbyte);
            CC_C = (_temp16 & 0x100) >> 8 != 0;
            CC_H = ((F_REG ^ _postbyte ^ _temp16) & 0x10) >> 4 != 0;
            CC_V = OVERFLOW8(CC_C, _postbyte, _temp16, F_REG);
            F_REG = (byte)_temp16;
            CC_N = NTEST8(F_REG);
            CC_Z = ZTEST(F_REG);
            PC_REG += 2;
            _instance->CycleCounter += _instance->NatEmuCycles65;
        }

        #endregion

        #endregion

        public unsafe void InvalidInsHandler()
        {
            MD_ILLEGAL = true;
            _instance->mdbits = HD6309_getmd();

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

                _instance->CycleCounter += 2;
            }

            MemWrite8(B_REG, --S_REG);
            MemWrite8(A_REG, --S_REG);
            MemWrite8(HD6309_getcc(), --S_REG);

            PC_REG = MemRead16(Define.VTRAP);

            _instance->CycleCounter += (12 + _instance->NatEmuCycles54);	//One for each byte +overhead? Guessing from PSHS
        }

        public unsafe void DivbyZero()
        {
            MD_ZERODIV = true; //1;

            _instance->mdbits = HD6309_getmd();

            ErrorVector();
        }

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

        public unsafe ushort W_REG
        {
            get => _instance->q.msw;
            set => _instance->q.msw = value;
        }

        public unsafe ushort D_REG
        {
            get => _instance->q.lsw;
            set => _instance->q.lsw = value;
        }

        public unsafe uint Q_REG
        {
            get => _instance->q.Reg;
            set => _instance->q.Reg = value;
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
            get => _instance->q.lswmsb;
            set => _instance->q.lswmsb = value;
        }

        public unsafe byte B_REG
        {
            get => _instance->q.lswlsb;
            set => _instance->q.lswlsb = value;
        }

        public unsafe ushort O_REG
        {
            get => _instance->z.Reg;
            set => _instance->z.Reg = value;
        }

        public unsafe byte F_REG
        {
            get => _instance->q.mswlsb;
            set => _instance->q.mswlsb = value;
        }

        public unsafe byte E_REG
        {
            get => _instance->q.mswmsb;
            set => _instance->q.mswmsb = value;
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
        public uint MemRead32(ushort address) => _modules.TC1014.MemRead32(address);
        public void MemWrite32(uint data, ushort address) => _modules.TC1014.MemWrite32(data, address);

        public ushort IMMADDRESS(ushort address) => MemRead16(address);
        public ushort INDADDRESS(ushort address) => HD6309_CalculateEA(MemRead8(address));

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
