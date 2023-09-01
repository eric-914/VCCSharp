namespace VCCSharp.OpCodes.Page3;

using VCCSharp.OpCodes.Model.OpCodes;
using Hitachi = HD6309.IState;

internal class Page3Opcodes6309
{
    private readonly Hitachi _cpu;

    #region Factory

    private IOpCode _ => new UndefinedOpCode();

    //_1100
    //_1101
    //_1102
    //_1103
    //_1104
    //_1105
    //_1106
    //_1107
    //_1108
    //_1109
    //_110A
    //_110B
    //_110C
    //_110D
    //_110E
    //_110F

    //_1110
    //_1111
    //_1112
    //_1113
    //_1114
    //_1115
    //_1116
    //_1117
    //_1118
    //_1119
    //_111A
    //_111B
    //_111C
    //_111D
    //_111E
    //_111F

    //_1120
    //_1121
    //_1122
    //_1123
    //_1124
    //_1125
    //_1126
    //_1127
    //_1128
    //_1129
    //_112A
    //_112B
    //_112C
    //_112D
    //_112E
    //_112F

    private IOpCode _1130_Band_D => new _1130_Band_D(_cpu);
    private IOpCode _1131_Biand_D => new _1131_Biand_D(_cpu);
    private IOpCode _1132_Bor_D => new _1132_Bor_D(_cpu);
    private IOpCode _1133_Bior_D => new _1133_Bior_D(_cpu);
    private IOpCode _1134_Beor_D => new _1134_Beor_D(_cpu);
    private IOpCode _1135_Bieor => new _1135_Bieor(_cpu);
    private IOpCode _1136_Ldbt => new _1136_Ldbt(_cpu);
    private IOpCode _1137_Stbt => new _1137_Stbt(_cpu);
    private IOpCode _1138_Tfm1 => new _1138_Tfm1(_cpu);
    private IOpCode _1139_Tfm2 => new _1139_Tfm2(_cpu);
    private IOpCode _113A_Tfm3 => new _113A_Tfm3(_cpu);
    private IOpCode _113B_Tfm4 => new _113B_Tfm4(_cpu);
    private IOpCode _113C_Bitmd_M => new _113C_Bitmd_M(_cpu);
    private IOpCode _113D_Ldmd_M => new _113D_Ldmd_M(_cpu);
    //_113E
    private IOpCode _113F_Swi3_I => new _113F_Swi3_I_6309(_cpu);

    //_1140
    //_1141
    //_1142
    private IOpCode _1143_Come_I => new _1143_Come_I(_cpu);
    //_1144
    //_1145
    //_1146
    //_1147
    //_1148
    //_1149
    private IOpCode _114A_Dece_I => new _114A_Dece_I(_cpu);
    //_114B
    private IOpCode _114C_Ince_I => new _114C_Ince_I(_cpu);
    private IOpCode _114D_Tste_I => new _114D_Tste_I(_cpu);
    //_114E
    private IOpCode _114F_Clre_I => new _114F_Clre_I(_cpu);

    //_1150
    //_1151
    //_1152
    private IOpCode _1153_Comf_I => new _1153_Comf_I(_cpu);
    //_1154
    //_1155
    //_1156
    //_1157
    //_1158
    //_1159
    private IOpCode _115A_Decf_I => new _115A_Decf_I(_cpu);
    //_115B
    private IOpCode _115C_Incf_I => new _115C_Incf_I(_cpu);
    private IOpCode _115D_Tstf_I => new _115D_Tstf_I(_cpu);
    //_115E
    private IOpCode _115F_Clrf_I => new _115F_Clrf_I(_cpu);

    //_1160
    //_1161
    //_1162
    //_1163
    //_1164
    //_1165
    //_1166
    //_1167
    //_1168
    //_1169
    //_116A
    //_116B
    //_116C
    //_116D
    //_116E
    //_116F

    //_1170
    //_1171
    //_1172
    //_1173
    //_1174
    //_1175
    //_1176
    //_1177
    //_1178
    //_1179
    //_117A
    //_117B
    //_117C
    //_117D
    //_117E
    //_117F

    private IOpCode _1180_Sube_M => new _1180_Sube_M(_cpu);
    private IOpCode _1181_Cmpe_M => new _1181_Cmpe_M(_cpu);
    //_1182
    private IOpCode _1183_Cmpu_M => new _1183_Cmpu_M(_cpu);
    //_1184
    //_1185
    private IOpCode _1186_Lde_M => new _1186_Lde_M(_cpu);
    //_1187
    //_1188
    //_1189
    //_118A
    private IOpCode _118B_Adde_M => new _118B_Adde_M(_cpu);
    private IOpCode _118C_Cmps_M => new _118C_Cmps_M(_cpu);
    private IOpCode _118D_Divd_M => new _118D_Divd_M(_cpu);
    private IOpCode _118E_Divq_M => new _118E_Divq_M(_cpu);
    private IOpCode _118F_Muld_M => new _118F_Muld_M(_cpu);

    private IOpCode _1190_Sube_D => new _1190_Sube_D(_cpu);
    private IOpCode _1191_Cmpe_D => new _1191_Cmpe_D(_cpu);
    //_1192
    private IOpCode _1193_Cmpu_D => new _1193_Cmpu_D(_cpu);
    //_1194
    //_1195
    private IOpCode _1196_Lde_D => new _1196_Lde_D(_cpu);
    private IOpCode _1197_Ste_D => new _1197_Ste_D(_cpu);
    //_1198
    //_1199
    //_119A
    private IOpCode _119B_Adde_D => new _119B_Adde_D(_cpu);
    private IOpCode _119C_Cmps_D => new _119C_Cmps_D(_cpu);
    private IOpCode _119D_Divd_D => new _119D_Divd_D(_cpu);
    private IOpCode _119E_Divq_D => new _119E_Divq_D(_cpu);
    private IOpCode _119F_Muld_D => new _119F_Muld_D(_cpu);

    private IOpCode _11A0_Sube_X => new _11A0_Sube_X(_cpu);
    private IOpCode _11A1_Cmpe_X => new _11A1_Cmpe_X(_cpu);
    //_11A2
    private IOpCode _11A3_Cmpu_X => new _11A3_Cmpu_X(_cpu);
    //_11A4
    //_11A5
    private IOpCode _11A6_Lde_X => new _11A6_Lde_X(_cpu);
    private IOpCode _11A7_Ste_X => new _11A7_Ste_X(_cpu);
    //_11A8
    //_11A9
    //_11AA
    private IOpCode _11AB_Adde_X => new _11AB_Adde_X(_cpu);
    private IOpCode _11AC_Cmps_X => new _11AC_Cmps_X(_cpu);
    private IOpCode _11AD_Divd_X => new _11AD_Divd_X(_cpu);
    private IOpCode _11AE_Divq_X => new _11AE_Divq_X(_cpu);
    private IOpCode _11AF_Muld_X => new _11AF_Muld_X(_cpu);

    private IOpCode _11B0_Sube_E => new _11B0_Sube_E(_cpu);
    private IOpCode _11B1_Cmpe_E => new _11B1_Cmpe_E(_cpu);
    //_11B2
    private IOpCode _11B3_Cmpu_E => new _11B3_Cmpu_E(_cpu);
    //_11B4
    //_11B5
    private IOpCode _11B6_Lde_E => new _11B6_Lde_E(_cpu);
    private IOpCode _11B7_Ste_E => new _11B7_Ste_E(_cpu);
    //_11B8
    //_11B9
    //_11BA
    private IOpCode _11BB_Adde_E => new _11BB_Adde_E(_cpu);
    private IOpCode _11BC_Cmps_E => new _11BC_Cmps_E(_cpu);
    private IOpCode _11BD_Divd_E => new _11BD_Divd_E(_cpu);
    private IOpCode _11BE_Divq_E => new _11BE_Divq_E(_cpu);
    private IOpCode _11BF_Muld_E => new _11BF_Muld_E(_cpu);

    private IOpCode _11C0_Subf_M => new _11C0_Subf_M(_cpu);
    private IOpCode _11C1_Cmpf_M => new _11C1_Cmpf_M(_cpu);
    //_11C2
    //_11C3
    //_11C4
    //_11C5
    private IOpCode _11C6_Ldf_M => new _11C6_Ldf_M(_cpu);
    //_11C7
    //_11C8
    //_11C9
    //_11CA
    private IOpCode _11CB_Addf_M => new _11CB_Addf_M(_cpu);
    //_11CC
    //_11CD
    //_11CE
    //_11CF

    private IOpCode _11D0_Subf_D => new _11D0_Subf_D(_cpu);
    private IOpCode _11D1_Cmpf_D => new _11D1_Cmpf_D(_cpu);
    //_11D2
    //_11D3
    //_11D4
    //_11D5
    private IOpCode _11D6_Ldf_D => new _11D6_Ldf_D(_cpu);
    private IOpCode _11D7_Stf_D => new _11D7_Stf_D(_cpu);
    //_11D8
    //_11D9
    //_11DA
    private IOpCode _11DB_Addf_D => new _11DB_Addf_D(_cpu);
    //_11DC => new _11DC(_cpu);
    //_11DD => new _11DD(_cpu);
    //_11DE => new _11DE(_cpu);
    //_11DF => new _11DF(_cpu);

    private IOpCode _11E0_Subf_X => new _11E0_Subf_X(_cpu);
    private IOpCode _11E1_Cmpf_X => new _11E1_Cmpf_X(_cpu);
    //_1103 => new _1103(_cpu);
    //_1104 => new _1104(_cpu);
    //_1105 => new _1105(_cpu);
    //_1106 => new _1106(_cpu);
    private IOpCode _11E6_Ldf_X => new _11E6_Ldf_X(_cpu);
    private IOpCode _11E7_Stf_X => new _11E7_Stf_X(_cpu);
    //_1109
    //_1110
    //_11EA
    private IOpCode _11EB_Addf_X => new _11EB_Addf_X(_cpu);
    //_11EC
    //_11ED
    //_11EE
    //_11EF

    private IOpCode _11F0_Subf_E => new _11F0_Subf_E(_cpu);
    private IOpCode _11F1_Cmpf_E => new _11F1_Cmpf_E(_cpu);
    //_11F2
    //_11F3
    //_11F4
    //_11F5
    private IOpCode _11F6_Ldf_E => new _11F6_Ldf_E(_cpu);
    private IOpCode _11F7_Stf_E => new _11F7_Stf_E(_cpu);
    //_11F8
    //_11F9
    //_11FA
    private IOpCode _11FB_Addf_E => new _11FB_Addf_E(_cpu);
    //_11FC
    //_11FD
    //_11FE
    //_11FF

    #endregion

    public Page3Opcodes6309(Hitachi cpu)
    {
        _cpu = cpu;
    }

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _1130_Band_D,_1131_Biand_D,_1132_Bor_D ,_1133_Bior_D,_1134_Beor_D,_1135_Bieor ,_1136_Ldbt  ,_1137_Stbt  ,_1138_Tfm1  ,_1139_Tfm2  ,_113A_Tfm3  ,_113B_Tfm4  ,_113C_Bitmd_M,_113D_Ldmd_M,_           ,_113F_Swi3_I,
        _           ,_            ,_           ,_1143_Come_I,_           ,_           ,_           ,_           ,_           ,_           ,_114A_Dece_I,_           ,_114C_Ince_I ,_114D_Tste_I,_           ,_114F_Clre_I,
        _           ,_            ,_           ,_1153_Comf_I,_           ,_           ,_           ,_           ,_           ,_           ,_115A_Decf_I,_           ,_115C_Incf_I ,_115D_Tstf_I,_           ,_115F_Clrf_I,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _1180_Sube_M,_1181_Cmpe_M ,_           ,_1183_Cmpu_M,_           ,_           ,_1186_Lde_M ,_           ,_           ,_           ,_           ,_118B_Adde_M,_118C_Cmps_M ,_118D_Divd_M,_118E_Divq_M,_118F_Muld_M,
        _1190_Sube_D,_1191_Cmpe_D ,_           ,_1193_Cmpu_D,_           ,_           ,_1196_Lde_D ,_1197_Ste_D ,_           ,_           ,_           ,_119B_Adde_D,_119C_Cmps_D ,_119D_Divd_D,_119E_Divq_D,_119F_Muld_D,
        _11A0_Sube_X,_11A1_Cmpe_X ,_           ,_11A3_Cmpu_X,_           ,_           ,_11A6_Lde_X ,_11A7_Ste_X ,_           ,_           ,_           ,_11AB_Adde_X,_11AC_Cmps_X ,_11AD_Divd_X,_11AE_Divq_X,_11AF_Muld_X,
        _11B0_Sube_E,_11B1_Cmpe_E ,_           ,_11B3_Cmpu_E,_           ,_           ,_11B6_Lde_E ,_11B7_Ste_E ,_           ,_           ,_           ,_11BB_Adde_E,_11BC_Cmps_E ,_11BD_Divd_E,_11BE_Divq_E,_11BF_Muld_E,
        _11C0_Subf_M,_11C1_Cmpf_M ,_           ,_           ,_           ,_           ,_11C6_Ldf_M ,_           ,_           ,_           ,_           ,_11CB_Addf_M,_            ,_           ,_           ,_           ,
        _11D0_Subf_D,_11D1_Cmpf_D ,_           ,_           ,_           ,_           ,_11D6_Ldf_D ,_11D7_Stf_D ,_           ,_           ,_           ,_11DB_Addf_D,_            ,_           ,_           ,_           ,
        _11E0_Subf_X,_11E1_Cmpf_X ,_           ,_           ,_           ,_           ,_11E6_Ldf_X ,_11E7_Stf_X ,_           ,_           ,_           ,_11EB_Addf_X,_            ,_           ,_           ,_           ,
        _11F0_Subf_E,_11F1_Cmpf_E ,_           ,_           ,_           ,_           ,_11F6_Ldf_E ,_11F7_Stf_E ,_           ,_           ,_           ,_11FB_Addf_E,_            ,_           ,_           ,_           ,    
    };
}
