using System;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        private byte _temp8;
        private ushort _temp16;
        private uint _temp32;

        private byte _postByte;
        private ushort _postWord;

        #region Jump Vectors

        private static Action[] _jmpVec1 = new Action[256];
        private static Action[] _jmpVec2 = new Action[256];
        private static Action[] _jmpVec3 = new Action[256];

        private void InitializeJmpVectors()
        {
            _jmpVec1 = new Action[]
            {
                Neg_D, // 0x00
                InvalidInsHandler, //0x01
                InvalidInsHandler, //0x02
                Com_D, // 0x03
                Lsr_D, // 0x04
                InvalidInsHandler, //0x05
                Ror_D, // 0x06
                Asr_D, // 0x07
                Asl_D, // 0x08
                Rol_D, // 0x09
                Dec_D, // 0x0A
                InvalidInsHandler, //0x0B
                Inc_D, // 0x0C
                Tst_D, // 0x0D
                Jmp_D, // 0x0E
                Clr_D, // 0x0F

                Page_2, // 0x10
                Page_3, // 0x11
                Nop_I, // 0x12
                Sync_I, // 0x13
                InvalidInsHandler, // 0x14
                InvalidInsHandler, // 0x15
                Lbra_R, // 0x16
                Lbsr_R, // 0x17
                InvalidInsHandler, // 0x18
                Daa_I, // 0x19
                Orcc_M, // 0x1A
                InvalidInsHandler, // 0x1B
                Andcc_M, // 0x1C
                Sex_I, // 0x1D
                Exg_M, // 0x1E 
                Tfr_M, // 0x1F

                Bra_R, // 0x20
                Brn_R, // 0x21
                Bhi_R, // 0x22
                Bls_R, // 0x23
                Bhs_R, // 0x24
                Blo_R, // 0x25
                Bne_R, // 0x26
                Beq_R, // 0x27
                Bvc_R, // 0x28
                Bvs_R, // 0x29
                Bpl_R, // 0x2A
                Bmi_R, // 0x2B
                Bge_R, // 0x2C
                Blt_R, // 0x2D
                Bgt_R, // 0x2E
                Ble_R, // 0x2F
                
                Leax_X, // 0x30
                Leay_X, // 0x31
                Leas_X, // 0x32
                Leau_X, // 0x33
                Pshs_M, // 0x34
                Puls_M, // 0x35
                Pshu_M, // 0x36
                Pulu_M, // 0x37
                InvalidInsHandler, // 0x38
                Rts_I, // 0x39
                Abx_I, // 0x3A
                Rti_I, // 0x3B
                Cwai_I, // 0x3C
                Mul_I, // 0x3D
                Reset, // 0x3E //Undocumented instruction
                Swi1_I, // 0x3F

                Nega_I, // 0x40 
                InvalidInsHandler, // 0x41
                InvalidInsHandler, // 0x42
                Coma_I, // 0x43
                Lsra_I, // 0x44
                InvalidInsHandler, // 0x45
                Rora_I, // 0x46
                Asra_I, // 0x47
                Asla_I, // 0x48
                Rola_I, // 0x49
                Deca_I, // 0x4A
                InvalidInsHandler, // 0x4B
                Inca_I, // 0x4C
                Tsta_I, // 0x4D
                InvalidInsHandler, // 0x4E
                Clra_I, // 0x4F

                Negb_I, // 0x50 
                InvalidInsHandler, // 0x51
                InvalidInsHandler, // 0x52
                Comb_I, // 0x53
                Lsrb_I, // 0x54
                InvalidInsHandler, // 0x55
                Rorb_I, // 0x56
                Asrb_I, // 0x57
                Aslb_I, // 0x58
                Rolb_I, // 0x59
                Decb_I, // 0x5A
                InvalidInsHandler, // 0x5B
                Incb_I, // 0x5C
                Tstb_I, // 0x5D
                InvalidInsHandler, // 5E
                Clrb_I, // 0x5F

                Neg_X, //  0x60
                InvalidInsHandler, // 0x61
                InvalidInsHandler, // 0x62
                Com_X, //  0x63
                Lsr_X, //  0x64
                InvalidInsHandler, // 0x65
                Ror_X, //  0x66
                Asr_X, //  0x67
                Asl_X, //  0x68
                Rol_X, //  0x69
                Dec_X, //  0x6A
                InvalidInsHandler, // 0x6B
                Inc_X, //  0x6C
                Tst_X, //  0x6D
                Jmp_X, //  0x6E
                Clr_X, //  0x6F

                Neg_E, // 0x70
                InvalidInsHandler, // 0x71
                InvalidInsHandler, // 0x72
                Com_E, // 0x73
                Lsr_E, // 0x74
                InvalidInsHandler, // 0x75
                Ror_E, // 0x76
                Asr_E, // 0x77
                Asl_E, // 0x78
                Rol_E, // 0x79
                Dec_E, // 0x7A
                InvalidInsHandler, // 0x7B
                Inc_E, // 0x7C
                Tst_E, // 0x7D
                Jmp_E, // 0x7E
                Clr_E, // 0x7F

                Suba_M, // 0x80
                Cmpa_M, // 0x81
                Sbca_M, // 0x82
                Subd_M, // 0x83
                Anda_M, // 0x84
                Bita_M, // 0x85
                Lda_M, // 0x86
                InvalidInsHandler, // 0x87
                Eora_M, // 0x88
                Adca_M, // 0x89
                Ora_M, // 0x8A
                Adda_M, // 0x8B
                Cmpx_M, // 0x8C
                Bsr_R, // 0x8D
                Ldx_M, // 0x8E
                InvalidInsHandler, // 8F

                Suba_D, //0x90
                Cmpa_D, //0x91
                Scba_D, //0x92
                Subd_D, //0x93
                Anda_D, //0x94
                Bita_D, //0x95
                Lda_D, //0x96
                Sta_D, //0x97
                Eora_D, //0x98
                Adca_D, //0x99
                Ora_D, //0x9A
                Adda_D, //0x9B
                Cmpx_D, //0x9C
                Jsr_D, //0x9D   //BSR_D  //Branch or Jump to Subroutine?
                Ldx_D, //0x9E
                Stx_D, //0x9F

                Suba_X, // 0xA0
                Cmpa_X, // 0xA1
                Sbca_X, // 0xA2
                Subd_X, // 0xA3
                Anda_X, // 0xA4
                Bita_X, // 0xA5
                Lda_X, // 0xA6
                Sta_X, // 0xA7
                Eora_X, // 0xA8
                Adca_X, // 0xA9
                Ora_X, // 0xAA
                Adda_X, // 0xAB
                Cmpx_X, // 0xAC
                Jsr_X, // 0xAD  //BSR_X  //Another Jump or Branch?
                Ldx_X, // 0xAE
                Stx_X, // 0xAF

                Suba_E, // 0xB0
                Cmpa_E, // 0xB1
                Sbca_E, // 0xB2
                Subd_E, // 0xB3
                Anda_E, // 0xB4
                Bita_E, // 0xB5
                Lda_E, // 0xB6
                Sta_E, // 0xB7
                Eora_E, // 0xB8
                Adca_E, // 0xB9
                Ora_E, // 0xBA
                Adda_E, // 0xBB
                Cmpx_E, // 0xBC
                Bsr_E, // 0xBD
                Ldx_E, // 0xBE
                Stx_E, // 0xBF

                Subb_M, // 0xC0
                Cmpb_M, // 0xC1
                Sbcb_M, // 0xC2
                Addd_M, // 0xC3
                Andb_M, // 0xC4
                Bitb_M, // 0xC5
                Ldb_M, // 0xC6
                InvalidInsHandler, // C7
                Eorb_M, // 0xC8
                Adcb_M, // 0xC9
                Orb_M, // 0xCA
                Addb_M, // 0xCB
                Ldd_M, // 0xCC
                InvalidInsHandler, // 0xCD
                Ldu_M, // 0xCE
                InvalidInsHandler, // CF

                Subb_D, // 0xD0
                Cmpb_D, // 0xD1
                Sbcb_D, // 0xD2
                Addd_D, // 0xD3
                Andb_D, // 0xD4
                Bitb_D, // 0xD5
                Ldb_D, // 0xD6
                Stb_D, // 0XD7
                Eorb_D, // 0xD8
                Adcb_D, // 0xD9
                Orb_D, // 0xDA
                Addb_D, // 0xDB
                Ldd_D, // 0xDC
                Std_D, // 0xDD
                Ldu_D, // 0xDE
                Stu_D, // 0xDF

                Subb_X, // 0xE0
                Cmpb_X, // 0xE1
                Sbcb_X, // 0xE2
                Addd_X, // 0xE3
                Andb_X, // 0xE4
                Bitb_X, // 0xE5
                Ldb_X, // 0xE6
                Stb_X, // 0xE7
                Eorb_X, // 0xE8
                Adcb_X, // 0xE9
                Orb_X, // 0xEA
                Addb_X, // 0xEB
                Ldd_X, // 0xEC
                Std_X, // 0xED
                Ldu_X, // 0xEE
                Stu_X, // 0xEF

                Subb_E, // 0xF0
                Cmpb_E, // 0xF1
                Sbcb_E, // 0xF2
                Addd_E, // 0xF3
                Andb_E, // 0xF4
                Bitb_E, // 0xF5
                Ldb_E, // 0xF6
                Stb_E, // 0xF7
                Eorb_E, // 0xF8
                Adcb_E, // 0xF9
                Orb_E, // 0xFA
                Addb_E, // 0xFB
                Ldd_E, // 0xFC
                Std_E, // 0xFD
                Ldu_E, // 0xFE
                Stu_E  // 0xFF
            };

            _jmpVec2 = new Action[]
            {
                InvalidInsHandler, // 1000
                InvalidInsHandler, // 1001
                InvalidInsHandler, // 1002
                InvalidInsHandler, // 1003
                InvalidInsHandler, // 1004
                InvalidInsHandler, // 1005
                InvalidInsHandler, // 1006
                InvalidInsHandler, // 1007
                InvalidInsHandler, // 1008
                InvalidInsHandler, // 1009
                InvalidInsHandler, // 100A
                InvalidInsHandler, // 100B
                InvalidInsHandler, // 100C
                InvalidInsHandler, // 100D
                InvalidInsHandler, // 100E
                InvalidInsHandler, // 100F

                InvalidInsHandler, // 1010
                InvalidInsHandler, // 1011
                InvalidInsHandler, // 1012
                InvalidInsHandler, // 1013
                InvalidInsHandler, // 1014
                InvalidInsHandler, // 1015
                InvalidInsHandler, // 1016
                InvalidInsHandler, // 1017
                InvalidInsHandler, // 1018
                InvalidInsHandler, // 1019
                InvalidInsHandler, // 101A
                InvalidInsHandler, // 101B
                InvalidInsHandler, // 101C
                InvalidInsHandler, // 101D
                InvalidInsHandler, // 101E
                InvalidInsHandler, // 101F

                InvalidInsHandler, // 1020
                LBRN_R, //1021
                LBHI_R, //1022
                LBLS_R, //1023
                LBHS_R, //1024
                LBCS_R, //1025
                LBNE_R, //1026
                LBEQ_R, //1027
                LBVC_R, //1028
                LBVS_R, //1029
                LBPL_R, //102A
                LBMI_R, //102B
                LBGE_R, //102C
                LBLT_R, //102D
                LBGT_R, //102E
                LBLE_R,	//102F

                InvalidInsHandler, // 1030
                InvalidInsHandler, // 1031
                InvalidInsHandler, // 1032
                InvalidInsHandler, // 1033
                InvalidInsHandler, // 1034
                InvalidInsHandler, // 1035
                InvalidInsHandler, // 1036
                InvalidInsHandler, // 1037
                InvalidInsHandler, // 1038
                InvalidInsHandler, // 1039
                InvalidInsHandler, // 103A
                InvalidInsHandler, // 103B
                InvalidInsHandler, // 103C
                InvalidInsHandler, // 103D
                InvalidInsHandler, // 103E
                SWI2_I, //103F

                InvalidInsHandler, // 1040
                InvalidInsHandler, // 1041
                InvalidInsHandler, // 1042
                InvalidInsHandler, // 1043
                InvalidInsHandler, // 1044
                InvalidInsHandler, // 1045
                InvalidInsHandler, // 1046
                InvalidInsHandler, // 1047
                InvalidInsHandler, // 1048
                InvalidInsHandler, // 1049
                InvalidInsHandler, // 104A
                InvalidInsHandler, // 104B
                InvalidInsHandler, // 104C
                InvalidInsHandler, // 104D
                InvalidInsHandler, // 104E
                InvalidInsHandler, // 104F

                InvalidInsHandler, // 1050
                InvalidInsHandler, // 1051
                InvalidInsHandler, // 1052
                InvalidInsHandler, // 1053
                InvalidInsHandler, // 1054
                InvalidInsHandler, // 1055
                InvalidInsHandler, // 1056
                InvalidInsHandler, // 1057
                InvalidInsHandler, // 1058
                InvalidInsHandler, // 1059
                InvalidInsHandler, // 105A
                InvalidInsHandler, // 105B
                InvalidInsHandler, // 105C
                InvalidInsHandler, // 105D
                InvalidInsHandler, // 105E
                InvalidInsHandler, // 105F

                InvalidInsHandler, // 1060
                InvalidInsHandler, // 1061
                InvalidInsHandler, // 1062
                InvalidInsHandler, // 1063
                InvalidInsHandler, // 1064
                InvalidInsHandler, // 1065
                InvalidInsHandler, // 1066
                InvalidInsHandler, // 1067
                InvalidInsHandler, // 1068
                InvalidInsHandler, // 1069
                InvalidInsHandler, // 106A
                InvalidInsHandler, // 106B
                InvalidInsHandler, // 106C
                InvalidInsHandler, // 106D
                InvalidInsHandler, // 106E
                InvalidInsHandler, // 106F

                InvalidInsHandler, // 1070
                InvalidInsHandler, // 1071
                InvalidInsHandler, // 1072
                InvalidInsHandler, // 1073
                InvalidInsHandler, // 1074
                InvalidInsHandler, // 1075
                InvalidInsHandler, // 1076
                InvalidInsHandler, // 1077
                InvalidInsHandler, // 1078
                InvalidInsHandler, // 1079
                InvalidInsHandler, // 107A
                InvalidInsHandler, // 107B
                InvalidInsHandler, // 107C
                InvalidInsHandler, // 107D
                InvalidInsHandler, // 107E
                InvalidInsHandler, // 107F

                InvalidInsHandler, // 1080
                InvalidInsHandler, // 1081
                InvalidInsHandler, // 1082
                CMPD_M, //1083
                InvalidInsHandler, // 1084
                InvalidInsHandler, // 1085
                InvalidInsHandler, // 1086
                InvalidInsHandler, // 1087
                InvalidInsHandler, // 1088
                InvalidInsHandler, // 1089
                InvalidInsHandler, // 108A
                InvalidInsHandler, // 108B
                CMPY_M, //108C
                InvalidInsHandler, // 108D
                LDY_M, //108E
                InvalidInsHandler, // 108F

                InvalidInsHandler, // 1090
                InvalidInsHandler, // 1091
                InvalidInsHandler, // 1092
                CMPD_D, //1093
                InvalidInsHandler, // 1094
                InvalidInsHandler, // 1095
                InvalidInsHandler, // 1096
                InvalidInsHandler, // 1097
                InvalidInsHandler, // 1098
                InvalidInsHandler, // 1099
                InvalidInsHandler, // 109A
                InvalidInsHandler, // 109B
                CMPY_D,	//109C
                InvalidInsHandler, // 109D
                LDY_D, //109E
                STY_D, //109F

                InvalidInsHandler, // 10A0
                InvalidInsHandler, // 10A1
                InvalidInsHandler, // 10A2
                CMPD_X, //10A3
                InvalidInsHandler, // 10A4
                InvalidInsHandler, // 10A5
                InvalidInsHandler, // 10A6
                InvalidInsHandler, // 10A7
                InvalidInsHandler, // 10A8
                InvalidInsHandler, // 10A9
                InvalidInsHandler, // 10AA
                InvalidInsHandler, // 10AB
                CMPY_X, //10AC
                InvalidInsHandler, // 10AD
                LDY_X, //10AE
                STY_X, //10AF

                InvalidInsHandler, // 10B0
                InvalidInsHandler, // 10B1
                InvalidInsHandler, // 10B2
                CMPD_E, //10B3
                InvalidInsHandler, // 10B4
                InvalidInsHandler, // 10B5
                InvalidInsHandler, // 10B6
                InvalidInsHandler, // 10B7
                InvalidInsHandler, // 10B8
                InvalidInsHandler, // 10B9
                InvalidInsHandler, // 10BA
                InvalidInsHandler, // 10BB
                CMPY_E, //10BC
                InvalidInsHandler, // 10BD
                LDY_E, //10BE
                STY_E, //10BF

                InvalidInsHandler, // 10C0
                InvalidInsHandler, // 10C1
                InvalidInsHandler, // 10C2
                InvalidInsHandler, // 10C3
                InvalidInsHandler, // 10C4
                InvalidInsHandler, // 10C5
                InvalidInsHandler, // 10C6
                InvalidInsHandler, // 10C7
                InvalidInsHandler, // 10C8
                InvalidInsHandler, // 10C9
                InvalidInsHandler, // 10CA
                InvalidInsHandler, // 10CB
                InvalidInsHandler, // 10CC
                InvalidInsHandler, // 10CD
                LDS_I,  //10CE
                InvalidInsHandler, // 10CF

                InvalidInsHandler, // 10D0
                InvalidInsHandler, // 10D1
                InvalidInsHandler, // 10D2
                InvalidInsHandler, // 10D3
                InvalidInsHandler, // 10D4
                InvalidInsHandler, // 10D5
                InvalidInsHandler, // 10D6
                InvalidInsHandler, // 10D7
                InvalidInsHandler, // 10D8
                InvalidInsHandler, // 10D9
                InvalidInsHandler, // 10DA
                InvalidInsHandler, // 10DB
                InvalidInsHandler, // 10DC
                InvalidInsHandler, // 10DD
                LDS_D, //10DE
                STS_D, //10DF

                InvalidInsHandler, // 10E0
                InvalidInsHandler, // 10E1
                InvalidInsHandler, // 10E2
                InvalidInsHandler, // 10E3
                InvalidInsHandler, // 10E4
                InvalidInsHandler, // 10E5
                InvalidInsHandler, // 10E6
                InvalidInsHandler, // 10E7
                InvalidInsHandler, // 10E8
                InvalidInsHandler, // 10E9
                InvalidInsHandler, // 10EA
                InvalidInsHandler, // 10EB
                InvalidInsHandler, // 10EC
                InvalidInsHandler, // 10ED
                LDS_X, //10EE
                STS_X, //10EF

                InvalidInsHandler, // 10F0
                InvalidInsHandler, // 10F1
                InvalidInsHandler, // 10F2
                InvalidInsHandler, // 10F3
                InvalidInsHandler, // 10F4
                InvalidInsHandler, // 10F5
                InvalidInsHandler, // 10F6
                InvalidInsHandler, // 10F7
                InvalidInsHandler, // 10F8
                InvalidInsHandler, // 10F9
                InvalidInsHandler, // 10FA
                InvalidInsHandler, // 10FB
                InvalidInsHandler, // 10FC
                InvalidInsHandler, // 10FD
                LDS_E, //10FE
                STS_E, //10FF
            };

            _jmpVec3 = new Action[]
            {
                InvalidInsHandler, // 1100
                InvalidInsHandler, // 1101
                InvalidInsHandler, // 1102
                InvalidInsHandler, // 1103
                InvalidInsHandler, // 1104
                InvalidInsHandler, // 1105
                InvalidInsHandler, // 1106
                InvalidInsHandler, // 1107
                InvalidInsHandler, // 1108
                InvalidInsHandler, // 1109
                InvalidInsHandler, // 110A
                InvalidInsHandler, // 110B
                InvalidInsHandler, // 110C
                InvalidInsHandler, // 110D
                InvalidInsHandler, // 110E
                InvalidInsHandler, // 110F

                InvalidInsHandler, // 1110
                InvalidInsHandler, // 1111
                InvalidInsHandler, // 1112
                InvalidInsHandler, // 1113
                InvalidInsHandler, // 1114
                InvalidInsHandler, // 1115
                InvalidInsHandler, // 1116
                InvalidInsHandler, // 1117
                InvalidInsHandler, // 1118
                InvalidInsHandler, // 1119
                InvalidInsHandler, // 111A
                InvalidInsHandler, // 111B
                InvalidInsHandler, // 111C
                InvalidInsHandler, // 111D
                InvalidInsHandler, // 111E
                InvalidInsHandler, // 111F

                InvalidInsHandler, // 1120
                InvalidInsHandler, // 1121
                InvalidInsHandler, // 1122
                InvalidInsHandler, // 1123
                InvalidInsHandler, // 1124
                InvalidInsHandler, // 1125
                InvalidInsHandler, // 1126
                InvalidInsHandler, // 1127
                InvalidInsHandler, // 1128
                InvalidInsHandler, // 1129
                InvalidInsHandler, // 112A
                InvalidInsHandler, // 112B
                InvalidInsHandler, // 112C
                InvalidInsHandler, // 112D
                InvalidInsHandler, // 112E
                InvalidInsHandler, // 112F

                InvalidInsHandler, // 1130
                InvalidInsHandler, // 1131
                InvalidInsHandler, // 1132
                InvalidInsHandler, // 1133
                InvalidInsHandler, // 1134
                InvalidInsHandler, // 1135
                InvalidInsHandler, // 1136
                InvalidInsHandler, // 1137
                InvalidInsHandler, // 1138
                InvalidInsHandler, // 1139
                InvalidInsHandler, // 113A
                InvalidInsHandler, // 113B
                InvalidInsHandler, // 113C
                InvalidInsHandler, // 113D
                InvalidInsHandler, // 113E
                SWI3_I, // 113F

                InvalidInsHandler, // 1140
                InvalidInsHandler, // 1141
                InvalidInsHandler, // 1142
                InvalidInsHandler, // 1143
                InvalidInsHandler, // 1144
                InvalidInsHandler, // 1145
                InvalidInsHandler, // 1146
                InvalidInsHandler, // 1147
                InvalidInsHandler, // 1148
                InvalidInsHandler, // 1149
                InvalidInsHandler, // 114A
                InvalidInsHandler, // 114B
                InvalidInsHandler, // 114C
                InvalidInsHandler, // 114D
                InvalidInsHandler, // 114E
                InvalidInsHandler, // 114F

                InvalidInsHandler, // 1150
                InvalidInsHandler, // 1151
                InvalidInsHandler, // 1152
                InvalidInsHandler, // 1153
                InvalidInsHandler, // 1154
                InvalidInsHandler, // 1155
                InvalidInsHandler, // 1156
                InvalidInsHandler, // 1157
                InvalidInsHandler, // 1158
                InvalidInsHandler, // 1159
                InvalidInsHandler, // 115A
                InvalidInsHandler, // 115B
                InvalidInsHandler, // 115C
                InvalidInsHandler, // 115D
                InvalidInsHandler, // 115E
                InvalidInsHandler, // 115F

                InvalidInsHandler, // 1160
                InvalidInsHandler, // 1161
                InvalidInsHandler, // 1162
                InvalidInsHandler, // 1163
                InvalidInsHandler, // 1164
                InvalidInsHandler, // 1165
                InvalidInsHandler, // 1166
                InvalidInsHandler, // 1167
                InvalidInsHandler, // 1168
                InvalidInsHandler, // 1169
                InvalidInsHandler, // 116A
                InvalidInsHandler, // 116B
                InvalidInsHandler, // 116C
                InvalidInsHandler, // 116D
                InvalidInsHandler, // 116E
                InvalidInsHandler, // 116F

                InvalidInsHandler, // 1170
                InvalidInsHandler, // 1171
                InvalidInsHandler, // 1172
                InvalidInsHandler, // 1173
                InvalidInsHandler, // 1174
                InvalidInsHandler, // 1175
                InvalidInsHandler, // 1176
                InvalidInsHandler, // 1177
                InvalidInsHandler, // 1178
                InvalidInsHandler, // 1179
                InvalidInsHandler, // 117A
                InvalidInsHandler, // 117B
                InvalidInsHandler, // 117C
                InvalidInsHandler, // 117D
                InvalidInsHandler, // 117E
                InvalidInsHandler, // 117F

                InvalidInsHandler, // 1180
                InvalidInsHandler, // 1181
                InvalidInsHandler, // 1182
                CMPU_M, // 1183
                InvalidInsHandler, // 1184
                InvalidInsHandler, // 1185
                InvalidInsHandler, // 1186
                InvalidInsHandler, // 1187
                InvalidInsHandler, // 1188
                InvalidInsHandler, // 1189
                InvalidInsHandler, // 118A
                InvalidInsHandler, // 118B
                CMPS_M, // 118C
                InvalidInsHandler, // 118D
                InvalidInsHandler, // 118E
                InvalidInsHandler, // 118F

                InvalidInsHandler, // 1190
                InvalidInsHandler, // 1191
                InvalidInsHandler, // 1192
                CMPU_D, // 1193
                InvalidInsHandler, // 1194
                InvalidInsHandler, // 1195
                InvalidInsHandler, // 1196
                InvalidInsHandler, // 1197
                InvalidInsHandler, // 1198
                InvalidInsHandler, // 1199
                InvalidInsHandler, // 119A
                InvalidInsHandler, // 119B
                CMPS_D, // 119C
                InvalidInsHandler, // 119D
                InvalidInsHandler, // 119E
                InvalidInsHandler, // 119F

                InvalidInsHandler, // 11A0
                InvalidInsHandler, // 11A1
                InvalidInsHandler, // 11A2
                CMPU_X, // 11A3
                InvalidInsHandler, // 11A4
                InvalidInsHandler, // 11A5
                InvalidInsHandler, // 11A6
                InvalidInsHandler, // 11A7
                InvalidInsHandler, // 11A8
                InvalidInsHandler, // 11A9
                InvalidInsHandler, // 11AA
                InvalidInsHandler, // 11AB
                CMPS_X, // 11AC
                InvalidInsHandler, // 11AD
                InvalidInsHandler, // 11AE
                InvalidInsHandler, // 11AF

                InvalidInsHandler, // 11B0
                InvalidInsHandler, // 11B1
                InvalidInsHandler, // 11B2
                CMPU_E, // 11B3
                InvalidInsHandler, // 11B4
                InvalidInsHandler, // 11B5
                InvalidInsHandler, // 11B6
                InvalidInsHandler, // 11B7
                InvalidInsHandler, // 11B8
                InvalidInsHandler, // 11B9
                InvalidInsHandler, // 11BA
                InvalidInsHandler, // 11BB
                CMPS_E, // 11BC
                InvalidInsHandler, // 11BD
                InvalidInsHandler, // 11BE
                InvalidInsHandler, // 11BF

                InvalidInsHandler, // 11C0
                InvalidInsHandler, // 11C1
                InvalidInsHandler, // 11C2
                InvalidInsHandler, // 11C3
                InvalidInsHandler, // 11C4
                InvalidInsHandler, // 11C5
                InvalidInsHandler, // 11C6
                InvalidInsHandler, // 11C7
                InvalidInsHandler, // 11C8
                InvalidInsHandler, // 11C9
                InvalidInsHandler, // 11CA
                InvalidInsHandler, // 11CB
                InvalidInsHandler, // 11CC
                InvalidInsHandler, // 11CD
                InvalidInsHandler, // 11CE
                InvalidInsHandler, // 11CF

                InvalidInsHandler, // 11D0
                InvalidInsHandler, // 11D1
                InvalidInsHandler, // 11D2
                InvalidInsHandler, // 11D3
                InvalidInsHandler, // 11D4
                InvalidInsHandler, // 11D5
                InvalidInsHandler, // 11D6
                InvalidInsHandler, // 11D7
                InvalidInsHandler, // 11D8
                InvalidInsHandler, // 11D9
                InvalidInsHandler, // 11DA
                InvalidInsHandler, // 11DB
                InvalidInsHandler, // 11DC
                InvalidInsHandler, // 11DD
                InvalidInsHandler, // 11DE
                InvalidInsHandler, // 11DF

                InvalidInsHandler, // 11E0
                InvalidInsHandler, // 11E1
                InvalidInsHandler, // 11E2
                InvalidInsHandler, // 11E3
                InvalidInsHandler, // 11E4
                InvalidInsHandler, // 11E5
                InvalidInsHandler, // 11E6
                InvalidInsHandler, // 11E7
                InvalidInsHandler, // 11E8
                InvalidInsHandler, // 11E9
                InvalidInsHandler, // 11EA
                InvalidInsHandler, // 11EB
                InvalidInsHandler, // 11EC
                InvalidInsHandler, // 11ED
                InvalidInsHandler, // 11EE
                InvalidInsHandler, // 11EF

                InvalidInsHandler, // 11F0
                InvalidInsHandler, // 11F1
                InvalidInsHandler, // 11F2
                InvalidInsHandler, // 11F3
                InvalidInsHandler, // 11F4
                InvalidInsHandler, // 11F5
                InvalidInsHandler, // 11F6
                InvalidInsHandler, // 11F7
                InvalidInsHandler, // 11F8
                InvalidInsHandler, // 11F9
                InvalidInsHandler, // 11FA
                InvalidInsHandler, // 11FB
                InvalidInsHandler, // 11FC
                InvalidInsHandler, // 11FD
                InvalidInsHandler, // 11FE
                InvalidInsHandler, // 11FF
            };
        }

        #endregion

        public void Exec(byte opCode)
        {
            _jmpVec1[opCode]();
        }

        public void LIB2(byte opCode)
        {
            Library.MC6809.MC6809ExecOpCode2(opCode);
        }

        public void LIB3(byte opCode)
        {
            Library.MC6809.MC6809ExecOpCode3(opCode);
        }

        public void InvalidInsHandler()
        {
        }
    }
}
