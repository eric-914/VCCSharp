namespace VCCSharp.Models.CPU.HD6309;

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
            Neg_D,   // 0x00
            Oim_D,   // 0x01
            Aim_D,   // 0x02
            Com_D,   // 0x03
            Lsr_D,   // 0x04
            Eim_D,   // 0x05
            Ror_D,   // 0x06
            Asr_D,   // 0x07
            Asl_D,   // 0x08
            Rol_D,   // 0x09
            Dec_D,   // 0x0A
            Tim_D,   // 0x0B
            Inc_D,   // 0x0C
            Tst_D,   // 0x0D
            Jmp_D,   // 0x0E
            Clr_D,   // 0x0F

            Page_2,  // 0x10
            Page_3,  // 0x11
            Nop_I,   // 0x12
            Sync_I,  // 0x13
            Sexw_I,  // 0x14
            _____,   // 0x15
            Lbra_R,  // 0x16
            Lbsr_R,  // 0x17
            _____,   // 0x18
            Daa_I,   // 0x19
            Orcc_M,  // 0x1A
            _____,   // 0x1B
            Andcc_M, // 0x1C
            Sex_I,   // 0x1D
            Exg_M,   // 0x1E
            Tfr_M,   // 0x1F

            Bra_R,   // 0x20
            Brn_R,   // 0x21
            Bhi_R,   // 0x22
            Bls_R,   // 0x23
            Bhs_R,   // 0x24
            Blo_R,   // 0x25
            Bne_R,   // 0x26
            Beq_R,   // 0x27
            Bvc_R,   // 0x28
            Bvs_R,   // 0x29
            Bpl_R,   // 0x2A
            Bmi_R,   // 0x2B
            Bge_R,   // 0x2C
            Blt_R,   // 0x2D
            Bgt_R,   // 0x2E
            Ble_R,   // 0x2F

            Leax_X,  // 0x30
            Leay_X,  // 0x31
            Leas_X,  // 0x32
            Leau_X,  // 0x33
            Pshs_M,  // 0x34
            Puls_M,  // 0x35
            Pshu_M,  // 0x36
            Pulu_M,  // 0x37
            _____,   // 0x38
            Rts_I,   // 0x39
            Abx_I,   // 0x3A
            Rti_I,   // 0x3B
            Cwai_I,  // 0x3C
            Mul_I,   // 0x3D
            Reset,   // 0x3E
            Swi1_I,  // 0x3F

            Nega_I,  // 0x40
            _____,   // 0x41
            _____,   // 0x42
            Coma_I,  // 0x43
            Lsra_I,  // 0x44
            _____,   // 0x45
            Rora_I,  // 0x46
            Asra_I,  // 0x47
            Asla_I,  // 0x48
            Rola_I,  // 0x49
            Deca_I,  // 0x4A
            _____,   // 0x4B
            Inca_I,  // 0x4C
            Tsta_I,  // 0x4D
            _____,   // 0x4E
            Clra_I,  // 0x4F

            Negb_I,  // 0x50
            _____,   // 0x51
            _____,   // 0x52
            Comb_I,  // 0x53
            Lsrb_I,  // 0x54
            _____,   // 0x55
            Rorb_I,  // 0x56
            Asrb_I,  // 0x57
            Aslb_I,  // 0x58
            Rolb_I,  // 0x59
            Decb_I,  // 0x5A
            _____,   // 0x5B
            Incb_I,  // 0x5C
            Tstb_I,  // 0x5D
            _____,   // 0x5E
            Clrb_I,  // 0x5F

            Neg_X,   // 0x60
            Oim_X,   // 0x61
            Aim_X,   // 0x62
            Com_X,   // 0x63
            Lsr_X,   // 0x64
            Eim_X,   // 0x65
            Ror_X,   // 0x66
            Asr_X,   // 0x67
            Asl_X,   // 0x68
            Rol_X,   // 0x69
            Dec_X,   // 0x6A
            Tim_X,   // 0x6B
            Inc_X,   // 0x6C
            Tst_X,   // 0x6D
            Jmp_X,   // 0x6E
            Clr_X,   // 0x6F

            Neg_E,   // 0x70
            Oim_E,   // 0x71
            Aim_E,   // 0x72
            Com_E,   // 0x73
            Lsr_E,   // 0x74
            Eim_E,   // 0x75
            Ror_E,   // 0x76
            Asr_E,   // 0x77
            Asl_E,   // 0x78
            Rol_E,   // 0x79
            Dec_E,   // 0x7A
            Tim_E,   // 0x7B
            Inc_E,   // 0x7C
            Tst_E,   // 0x7D
            Jmp_E,   // 0x7E
            Clr_E,   // 0x7F

            Suba_M,  // 0x80
            Cmpa_M,  // 0x81
            Sbca_M,  // 0x82
            Subd_M,  // 0x83
            Anda_M,  // 0x84
            Bita_M,  // 0x85
            Lda_M,   // 0x86
            _____,   // 0x87
            Eora_M,  // 0x88
            Adca_M,  // 0x89
            Ora_M,   // 0x8A
            Adda_M,  // 0x8B
            Cmpx_M,  // 0x8C
            Bsr_R,   // 0x8D
            Ldx_M,   // 0x8E
            _____,   // 0x8F

            Suba_D,  // 0x90
            Cmpa_D,  // 0x91
            Scba_D,  // 0x92
            Subd_D,  // 0x93
            Anda_D,  // 0x94
            Bita_D,  // 0x95
            Lda_D,   // 0x96
            Sta_D,   // 0x97
            Eora_D,  // 0x98
            Adca_D,  // 0x99
            Ora_D,   // 0x9A
            Adda_D,  // 0x9B
            Cmpx_D,  // 0x9C
            Jsr_D,   // 0x9D
            Ldx_D,   // 0x9E
            Stx_D,   // 0x9F

            Suba_X,  // 0xA0
            Cmpa_X,  // 0xA1
            Sbca_X,  // 0xA2
            Subd_X,  // 0xA3
            Anda_X,  // 0xA4
            Bita_X,  // 0xA5
            Lda_X,   // 0xA6
            Sta_X,   // 0xA7
            Eora_X,  // 0xa8
            Adca_X,  // 0xA9
            Ora_X,   // 0xAA
            Adda_X,  // 0xAB
            Cmpx_X,  // 0xAC
            Jsr_X,   // 0xAD
            Ldx_X,   // 0xAE
            Stx_X,   // 0xAF

            Suba_E,  // 0xB0
            Cmpa_E,  // 0xB1
            Sbca_E,  // 0xB2
            Subd_E,  // 0xB3
            Anda_E,  // 0xB4
            Bita_E,  // 0xB5
            Lda_E,   // 0xB6
            Sta_E,   // 0xB7
            Eora_E,  // 0xB8
            Adca_E,  // 0xB9
            Ora_E,   // 0xBA
            Adda_E,  // 0xBB
            Cmpx_E,  // 0xBC
            Bsr_E,   // 0xBD
            Ldx_E,   // 0xBE
            Stx_E,   // 0xBF

            Subb_M,  // 0xC0
            Cmpb_M,  // 0xC1
            Sbcb_M,  // 0xC2
            Addd_M,  // 0xC3
            Andb_M,  // 0xC4
            Bitb_M,  // 0xC5
            Ldb_M,   // 0xC6
            _____,   // 0xC7
            Eorb_M,  // 0xC8
            Adcb_M,  // 0xC9
            Orb_M,   // 0xCA
            Addb_M,  // 0xCB
            Ldd_M,   // 0xCC
            Ldq_M,   // 0xCD
            Ldu_M,   // 0xCE
            _____,   // 0xCF

            Subb_D,  // 0xD0
            Cmpb_D,  // 0xD1
            Sbcb_D,  // 0xD2
            Addd_D,  // 0xD3
            Andb_D,  // 0xD4
            Bitb_D,  // 0xD5
            Ldb_D,   // 0xD6
            Stb_D,   // 0xD7
            Eorb_D,  // 0xD8
            Adcb_D,  // 0xD9
            Orb_D,   // 0xDA
            Addb_D,  // 0xDB
            Ldd_D,   // 0xDC
            Std_D,   // 0xDD
            Ldu_D,   // 0xDE
            Stu_D,   // 0xDF

            Subb_X,  // 0xE0
            Cmpb_X,  // 0xE1
            Sbcb_X,  // 0xE2
            Addd_X,  // 0xE3
            Andb_X,  // 0xE4
            Bitb_X,  // 0xE5
            Ldb_X,   // 0xE6
            Stb_X,   // 0xE7
            Eorb_X,  // 0xE8
            Adcb_X,  // 0xE9
            Orb_X,   // 0xEA
            Addb_X,  // 0xEB
            Ldd_X,   // 0xEC
            Std_X,   // 0xED
            Ldu_X,   // 0xEE
            Stu_X,   // 0xEF

            Subb_E,  // 0xF0
            Cmpb_E,  // 0xF1
            Sbcb_E,  // 0xF2
            Addd_E,  // 0xF3
            Andb_E,  // 0xF4
            Bitb_E,  // 0xF5
            Ldb_E,   // 0xF6
            Stb_E,   // 0xF7
            Eorb_E,  // 0xF8
            Adcb_E,  // 0xF9
            Orb_E,   // 0xFA
            Addb_E,  // 0xFB
            Ldd_E,   // 0xFC
            Std_E,   // 0xFD
            Ldu_E,   // 0xFE
            Stu_E,   // 0xFF
        };

        _jmpVec2 = new Action[]
        {
            _____,   // 0x1000
            _____,   // 0x1001
            _____,   // 0x1002
            _____,   // 0x1003
            _____,   // 0x1004
            _____,   // 0x1005
            _____,   // 0x1006
            _____,   // 0x1007
            _____,   // 0x1008
            _____,   // 0x1009
            _____,   // 0x100A
            _____,   // 0x100B
            _____,   // 0x100C
            _____,   // 0x100D
            _____,   // 0x100E
            _____,   // 0x100F

            _____,   // 0x1010
            _____,   // 0x1011
            _____,   // 0x1012
            _____,   // 0x1013
            _____,   // 0x1014
            _____,   // 0x1015
            _____,   // 0x1016
            _____,   // 0x1017
            _____,   // 0x1018
            _____,   // 0x1019
            _____,   // 0x101A
            _____,   // 0x101B
            _____,   // 0x101C
            _____,   // 0x101D
            _____,   // 0x101E
            _____,   // 0x101F

            _____,   // 0x1020
            LBrn_R,  // 0x1021
            LBhi_R,  // 0x1022
            LBls_R,  // 0x1023
            LBhs_R,  // 0x1024
            LBcs_R,  // 0x1025
            LBne_R,  // 0x1026
            LBeq_R,  // 0x1027
            LBvc_R,  // 0x1028
            LBvs_R,  // 0x1029
            LBpl_R,  // 0x102A
            LBmi_R,  // 0x102B
            LBge_R,  // 0x102C
            LBlt_R,  // 0x102D
            LBgt_R,  // 0x102E
            LBle_R,  // 0x102F

            Addr,    // 0x1030
            Adcr,    // 0x1031
            Subr,    // 0x1032
            Sbcr,    // 0x1033
            Andr,    // 0x1034
            Orr,     // 0x1035
            Eorr,    // 0x1036
            Cmpr,    // 0x1037
            Pshsw,   // 0x1038
            Pulsw,   // 0x1039
            Pshuw,   // 0x103A
            Puluw,   // 0x103B
            _____,   // 0x103C
            _____,   // 0x103D
            _____,   // 0x103E
            Swi2_I,  // 0x103F

            Negd_I,  // 0x1040
            _____,   // 0x1041
            _____,   // 0x1042
            Comd_I,  // 0x1043
            Lsrd_I,  // 0x1044
            _____,   // 0x1045
            Rord_I,  // 0x1046
            Asrd_I,  // 0x1047
            Asld_I,  // 0x1048
            Rold_I,  // 0x1049
            Decd_I,  // 0x104A
            _____,   // 0x104B
            Incd_I,  // 0x104C
            Tstd_I,  // 0x104D
            _____,   // 0x104E
            Clrd_I,  // 0x104F

            _____,   // 0x1050
            _____,   // 0x1051
            _____,   // 0x1052
            Comw_I,  // 0x1053
            Lsrw_I,  // 0x1054
            _____,   // 0x1055
            Rorw_I,  // 0x1056
            _____,   // 0x1057
            _____,   // 0x1058
            Rolw_I,  // 0x1059
            Decw_I,  // 0x105A
            _____,   // 0x105B
            Incw_I,  // 0x105C
            Tstw_I,  // 0x105D
            _____,   // 0x105E
            Clrw_I,  // 0x105F

            _____,   // 0x1060
            _____,   // 0x1061
            _____,   // 0x1062
            _____,   // 0x1063
            _____,   // 0x1064
            _____,   // 0x1065
            _____,   // 0x1066
            _____,   // 0x1067
            _____,   // 0x1068
            _____,   // 0x1069
            _____,   // 0x106A
            _____,   // 0x106B
            _____,   // 0x106C
            _____,   // 0x106D
            _____,   // 0x106E
            _____,   // 0x106F

            _____,   // 0x1070
            _____,   // 0x1071
            _____,   // 0x1072
            _____,   // 0x1073
            _____,   // 0x1074
            _____,   // 0x1075
            _____,   // 0x1076
            _____,   // 0x1077
            _____,   // 0x1078
            _____,   // 0x1079
            _____,   // 0x107A
            _____,   // 0x107B
            _____,   // 0x107C
            _____,   // 0x107D
            _____,   // 0x107E
            _____,   // 0x107F

            Subw_M,  // 0x1080
            Cmpw_M,  // 0x1081
            Sbcd_M,  // 0x1082
            Cmpd_M,  // 0x1083
            Andd_M,  // 0x1084
            Bitd_M,  // 0x1085
            Ldw_M,   // 0x1086
            _____,   // 0x1087
            Eord_M,  // 0x1088
            Adcd_M,  // 0x1089
            Ord_M,   // 0x108A
            Addw_M,  // 0x108B
            Cmpy_M,  // 0x108C
            _____,   // 0x108D
            Ldy_M,   // 0x108E
            _____,   // 0x108F

            Subw_D,  // 0x1090
            Cmpw_D,  // 0x1091
            Sbcd_D,  // 0x1092
            Cmpd_D,  // 0x1093
            Andd_D,  // 0x1094
            Bitd_D,  // 0x1095
            Ldw_D,   // 0x1096
            Stw_D,   // 0x1097
            Eord_D,  // 0x1098
            Adcd_D,  // 0x1099
            Ord_D,   // 0x109A
            Addw_D,  // 0x109B
            Cmpy_D,  // 0x109C
            _____,   // 0x109D
            Ldy_D,   // 0x109E
            Sty_D,   // 0x109F

            Subw_X,  // 0x10A0
            Cmpw_X,  // 0x10A1
            Sbcd_X,  // 0x10A2
            Cmpd_X,  // 0x10A3
            Andd_X,  // 0x10A4
            Bitd_X,  // 0x10A5
            Ldw_X,   // 0x10A6
            Stw_X,   // 0x10A7
            Eord_X,  // 0x10A8
            Adcd_X,  // 0x10A9
            Ord_X,   // 0x10AA
            Addw_X,  // 0x10AB
            Cmpy_X,  // 0x10AC
            _____,   // 0x10AD
            Ldy_X,   // 0x10AE
            Sty_X,   // 0x10AF

            Subw_E,  // 0x10B0
            Cmpw_E,  // 0x10B1
            Sbcd_E,  // 0x10B2
            Cmpd_E,  // 0x10B3
            Andd_E,  // 0x10B4
            Bitd_E,  // 0x10B5
            Ldw_E,   // 0x10B6
            Stw_E,   // 0x10B7
            Eord_E,  // 0x10B8
            Adcd_E,  // 0x10B9
            Ord_E,   // 0x10BA
            Addw_E,  // 0x10BB
            Cmpy_E,  // 0x10BC
            _____,   // 0x10BD
            Ldy_E,   // 0x10BE
            Sty_E,   // 0x10BF

            _____,   // 0x10C0
            _____,   // 0x10C1
            _____,   // 0x10C2
            _____,   // 0x10C3
            _____,   // 0x10C4
            _____,   // 0x10C5
            _____,   // 0x10C6
            _____,   // 0x10C7
            _____,   // 0x10C8
            _____,   // 0x10C9
            _____,   // 0x10CA
            _____,   // 0x10CB
            _____,   // 0x10CC
            _____,   // 0x10CD
            Lds_I,   // 0x10CE
            _____,   // 0x10CF

            _____,   // 0x10D0
            _____,   // 0x10D1
            _____,   // 0x10D2
            _____,   // 0x10D3
            _____,   // 0x10D4
            _____,   // 0x10D5
            _____,   // 0x10D6
            _____,   // 0x10D7
            _____,   // 0x10D8
            _____,   // 0x10D9
            _____,   // 0x10DA
            _____,   // 0x10DB
            Ldq_D,   // 0x10DC
            Stq_D,   // 0x10DD
            Lds_D,   // 0x10DE
            Sts_D,   // 0x10DF

            _____,   // 0x10E0
            _____,   // 0x10E1
            _____,   // 0x10E2
            _____,   // 0x10E3
            _____,   // 0x10E4
            _____,   // 0x10E5
            _____,   // 0x10E6
            _____,   // 0x10E7
            _____,   // 0x10E8
            _____,   // 0x10E9
            _____,   // 0x10EA
            _____,   // 0x10EB
            Ldq_X,   // 0x10EC
            Stq_X,   // 0x10ED
            Lds_X,   // 0x10EE
            Sts_X,   // 0x10EF

            _____,   // 0x10F0
            _____,   // 0x10F1
            _____,   // 0x10F2
            _____,   // 0x10F3
            _____,   // 0x10F4
            _____,   // 0x10F5
            _____,   // 0x10F6
            _____,   // 0x10F7
            _____,   // 0x10F8
            _____,   // 0x10F9
            _____,   // 0x10FA
            _____,   // 0x10FB
            Ldq_E,   // 0x10FC
            Stq_E,   // 0x10FD
            Lds_E,   // 0x10FE
            Sts_E,   // 0x10FF
        };

        _jmpVec3 = new Action[]
        {
            _____,   // 0x1100
            _____,   // 0x1101
            _____,   // 0x1102
            _____,   // 0x1103
            _____,   // 0x1104
            _____,   // 0x1105
            _____,   // 0x1106
            _____,   // 0x1107
            _____,   // 0x1108
            _____,   // 0x1109
            _____,   // 0x110A
            _____,   // 0x110B
            _____,   // 0x110C
            _____,   // 0x110D
            _____,   // 0x110E
            _____,   // 0x110F

            _____,   // 0x1110
            _____,   // 0x1111
            _____,   // 0x1112
            _____,   // 0x1113
            _____,   // 0x1114
            _____,   // 0x1115
            _____,   // 0x1116
            _____,   // 0x1117
            _____,   // 0x1118
            _____,   // 0x1119
            _____,   // 0x111A
            _____,   // 0x111B
            _____,   // 0x111C
            _____,   // 0x111D
            _____,   // 0x111E
            _____,   // 0x111F

            _____,   // 0x1120
            _____,   // 0x1121
            _____,   // 0x1122
            _____,   // 0x1123
            _____,   // 0x1124
            _____,   // 0x1125
            _____,   // 0x1126
            _____,   // 0x1127
            _____,   // 0x1128
            _____,   // 0x1129
            _____,   // 0x112A
            _____,   // 0x112B
            _____,   // 0x112C
            _____,   // 0x112D
            _____,   // 0x112E
            _____,   // 0x112F

            Band,    // 0x1130
            Biand,   // 0x1131
            Bor,     // 0x1132
            Bior,    // 0x1133
            Beor,    // 0x1134
            Bieor,   // 0x1135
            Ldbt,    // 0x1136
            Stbt,    // 0x1137
            Tfm1,    // 0x1138
            Tfm2,    // 0x1139
            Tfm3,    // 0x113A
            Tfm4,    // 0x113B
            Bitmd_M, // 0x113C
            Ldmd_M,  // 0x113D
            _____,   // 0x113E
            Swi3_I,  // 0x113F

            _____,   // 0x1140
            _____,   // 0x1141
            _____,   // 0x1142
            Come_I,  // 0x1143
            _____,   // 0x1144
            _____,   // 0x1145
            _____,   // 0x1146
            _____,   // 0x1147
            _____,   // 0x1148
            _____,   // 0x1149
            Dece_I,  // 0x114A
            _____,   // 0x114B
            Ince_I,  // 0x114C
            Tste_I,  // 0x114D
            _____,   // 0x114E
            Clre_I,  // 0x114F

            _____,   // 0x1150
            _____,   // 0x1151
            _____,   // 0x1152
            Comf_I,  // 0x1153
            _____,   // 0x1154
            _____,   // 0x1155
            _____,   // 0x1156
            _____,   // 0x1157
            _____,   // 0x1158
            _____,   // 0x1159
            Decf_I,  // 0x115A
            _____,   // 0x115B
            Incf_I,  // 0x115C
            Tstf_I,  // 0x115D
            _____,   // 0x115E
            Clrf_I,  // 0x115F

            _____,   // 0x1160
            _____,   // 0x1161
            _____,   // 0x1162
            _____,   // 0x1163
            _____,   // 0x1164
            _____,   // 0x1165
            _____,   // 0x1166
            _____,   // 0x1167
            _____,   // 0x1168
            _____,   // 0x1169
            _____,   // 0x116A
            _____,   // 0x116B
            _____,   // 0x116C
            _____,   // 0x116D
            _____,   // 0x116E
            _____,   // 0x116F

            _____,   // 0x1170
            _____,   // 0x1171
            _____,   // 0x1172
            _____,   // 0x1173
            _____,   // 0x1174
            _____,   // 0x1175
            _____,   // 0x1176
            _____,   // 0x1177
            _____,   // 0x1178
            _____,   // 0x1179
            _____,   // 0x117A
            _____,   // 0x117B
            _____,   // 0x117C
            _____,   // 0x117D
            _____,   // 0x117E
            _____,   // 0x117F

            Sube_M,  // 0x1180
            Cmpe_M,  // 0x1181
            _____,   // 0x1182
            Cmpu_M,  // 0x1183
            _____,   // 0x1184
            _____,   // 0x1185
            Lde_M,   // 0x1186
            _____,   // 0x1187
            _____,   // 0x1188
            _____,   // 0x1189
            _____,   // 0x118A
            Adde_M,  // 0x118B
            Cmps_M,  // 0x118C
            Divd_M,  // 0x118D
            Divq_M,  // 0x118E
            Muld_M,  // 0x118F

            Sube_D,  // 0x1190
            Cmpe_D,  // 0x1191
            _____,   // 0x1192
            Cmpu_D,  // 0x1193
            _____,   // 0x1194
            _____,   // 0x1195
            Lde_D,   // 0x1196
            Ste_D,   // 0x1197
            _____,   // 0x1198
            _____,   // 0x1199
            _____,   // 0x119A
            Adde_D,  // 0x119B
            Cmps_D,  // 0x119C
            Divd_D,  // 0x119D
            Divq_D,  // 0x119E
            Muld_D,  // 0x119F

            Sube_X,  // 0x11A0
            Cmpe_X,  // 0x11A1
            _____,   // 0x11A2
            Cmpu_X,  // 0x11A3
            _____,   // 0x11A4
            _____,   // 0x11A5
            Lde_X,   // 0x11A6
            Ste_X,   // 0x11A7
            _____,   // 0x11A8
            _____,   // 0x11A9
            _____,   // 0x11AA
            Adde_X,  // 0x11AB
            Cmps_X,  // 0x11AC
            Divd_X,  // 0x11AD
            Divq_X,  // 0x11AE
            Muld_X,  // 0x11AF

            Sube_E,  // 0x11B0
            Cmpe_E,  // 0x11B1
            _____,   // 0x11B2
            Cmpu_E,  // 0x11B3
            _____,   // 0x11B4
            _____,   // 0x11B5
            Lde_E,   // 0x11B6
            Ste_E,   // 0x11B7
            _____,   // 0x11B8
            _____,   // 0x11B9
            _____,   // 0x11BA
            Adde_E,  // 0x11BB
            Cmps_E,  // 0x11BC
            Divd_E,  // 0x11BD
            Divq_E,  // 0x11BE
            Muld_E,  // 0x11BF

            Subf_M,  // 0x11C0
            Cmpf_M,  // 0x11C1
            _____,   // 0x11C2
            _____,   // 0x11C3
            _____,   // 0x11C4
            _____,   // 0x11C5
            Ldf_M,   // 0x11C6
            _____,   // 0x11C7
            _____,   // 0x11C8
            _____,   // 0x11C9
            _____,   // 0x11CA
            Addf_M,  // 0x11CB
            _____,   // 0x11CC
            _____,   // 0x11CD
            _____,   // 0x11CE
            _____,   // 0x11CF

            Subf_D,  // 0x11D0
            Cmpf_D,  // 0x11D1
            _____,   // 0x11D2
            _____,   // 0x11D3
            _____,   // 0x11D4
            _____,   // 0x11D5
            Ldf_D,   // 0x11D6
            Stf_D,   // 0x11D7
            _____,   // 0x11D8
            _____,   // 0x11D9
            _____,   // 0x11DA
            Addf_D,  // 0x11DB
            _____,   // 0x11DC
            _____,   // 0x11DD
            _____,   // 0x11DE
            _____,   // 0x11DF

            Subf_X,  // 0x11E0
            Cmpf_X,  // 0x11E1
            _____,   // 0x11E2
            _____,   // 0x11E3
            _____,   // 0x11E4
            _____,   // 0x11E5
            Ldf_X,   // 0x11E6
            Stf_X,   // 0x11E7
            _____,   // 0x11E8
            _____,   // 0x11E9
            _____,   // 0x11EA
            Addf_X,  // 0x11EB
            _____,   // 0x11EC
            _____,   // 0x11ED
            _____,   // 0x11EE
            _____,   // 0x11EF

            Subf_E,  // 0x11F0
            Cmpf_E,  // 0x11F1
            _____,   // 0x11F2
            _____,   // 0x11F3
            _____,   // 0x11F4
            _____,   // 0x11F5
            Ldf_E,   // 0x11F6
            Stf_E,   // 0x11F7
            _____,   // 0x11F8
            _____,   // 0x11F9
            _____,   // 0x11FA
            Addf_E,  // 0x11FB
            _____,   // 0x11FC
            _____,   // 0x11FD
            _____,   // 0x11FE
            _____    // 0x11FF
        };
    }

    #endregion

    private static void Exec(byte opCode) => _jmpVec1[opCode]();

    //OpCode Definitions
    //Last Char (D) Direct (I) Inherent (R) Relative (M) Immediate (X) Indexed (E) Extended

    /// <summary>
    /// Invalid Instruction Handler
    /// </summary>
    public void _____()
    {
        MD_ILLEGAL = true;
        _cpu.md.bits = GetMD();

        ErrorVector();
    }

    public void ErrorVector()
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
        MemWrite8(GetCC(), --S_REG);

        PC_REG = MemRead16(Define.VTRAP);

        _cycleCounter += 12 + _instance._54;	//One for each byte +overhead? Guessing from PSHS
    }

    public void DivByZero()
    {
        MD_ZERODIV = true; //1;

        _cpu.md.bits = GetMD();

        ErrorVector();
    }
}
