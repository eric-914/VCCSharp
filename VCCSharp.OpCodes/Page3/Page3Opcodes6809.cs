namespace VCCSharp.OpCodes.Page3;

using VCCSharp.OpCodes.Model.OpCodes;
using Motorola = MC6809.IState;

internal class Page3Opcodes6809
{
    private readonly Motorola _cpu;

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

    //_1130_Band_D
    //_1131_Biand_D
    //_1132_Bor_D
    //_1133_Bior_D
    //_1134_Beor_D
    //_1135_Bieor
    //_1136_Ldbt
    //_1137_Stbt
    //_1138_Tfm1
    //_1139_Tfm2
    //_113A_Tfm3
    //_113B_Tfm4
    //_113C_Bitmd_M
    //_113D_Ldmd_M
    //_113E
    private IOpCode _113F_Swi3_I => new _113F_Swi3_I_6809(_cpu);

    //_1140
    //_1141
    //_1142
    //_1143_Come_I
    //_1144
    //_1145
    //_1146
    //_1147
    //_1148
    //_1149
    //_114A_Dece_I
    //_114B
    //_114C_Ince_I
    //_114D_Tste_I
    //_114E
    //_114F_Clre_I

    //_1150
    //_1151
    //_1152
    //_1153_Comf_I
    //_1154
    //_1155
    //_1156
    //_1157
    //_1158
    //_1159
    //_115A_Decf_I
    //_115B
    //_115C_Incf_I
    //_115D_Tstf_I
    //_115E
    //_115F_Clrf_I

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

    //_1180_Sube_M
    //_1181_Cmpe_M
    //_1182
    private IOpCode _1183_Cmpu_M => new _1183_Cmpu_M(_cpu);
    //_1184
    //_1185
    //_1186_Lde_M
    //_1187
    //_1188
    //_1189
    //_118A
    //_118B_Adde_M
    private IOpCode _118C_Cmps_M => new _118C_Cmps_M(_cpu);
    //_118D_Divd_M
    //_118E_Divq_M
    //_118F_Muld_M

    //_1190_Sube_D
    //_1191_Cmpe_D
    //_1192
    private IOpCode _1193_Cmpu_D => new _1193_Cmpu_D(_cpu);
    //_1194
    //_1195
    //_1196_Lde_D
    //_1197_Ste_D
    //_1198
    //_1199
    //_119A
    //_119B_Adde_D
    private IOpCode _119C_Cmps_D => new _119C_Cmps_D(_cpu);
    //_119D_Divd_D
    //_119E_Divq_D
    //_119F_Muld_D

    //_11A0_Sube_X
    //_11A1_Cmpe_X
    //_11A2
    private IOpCode _11A3_Cmpu_X => new _11A3_Cmpu_X(_cpu);
    //_11A4
    //_11A5
    //_11A6_Lde_X
    //_11A7_Ste_X
    //_11A8
    //_11A9
    //_11AA
    //_11AB_Adde_X
    private IOpCode _11AC_Cmps_X => new _11AC_Cmps_X(_cpu);
    //_11AD_Divd_X
    //_11AE_Divq_X
    //_11AF_Muld_X

    //_11B0_Sube_E
    //_11B1_Cmpe_E
    //_11B2
    //_11B3_Cmpu_E
    //_11B4
    //_11B5
    //_11B6_Lde_E
    //_11B7_Ste_E
    //_11B8
    //_11B9
    //_11BA
    //_11BB_Adde_E
    private IOpCode _11BC_Cmps_E => new _11BC_Cmps_E(_cpu);
    //_11BD_Divd_E
    //_11BE_Divq_E
    //_11BF_Muld_E

    //_11C0_Subf_M
    //_11C1_Cmpf_M
    //_11C2
    //_11C3
    //_11C4
    //_11C5
    //_11C6_Ldf_M
    //_11C7
    //_11C8
    //_11C9
    //_11CA
    //_11CB_Addf_M
    //_11CC
    //_11CD
    //_11CE
    //_11CF

    //_11D0_Subf_D
    //_11D1_Cmpf_D
    //_11D2
    //_11D3
    //_11D4
    //_11D5
    //_11D6_Ldf_D
    //_11D7_Stf_D
    //_11D8
    //_11D9
    //_11DA
    //_11DB_Addf_D
    //_11DC => new _11DC(_cpu);
    //_11DD => new _11DD(_cpu);
    //_11DE => new _11DE(_cpu);
    //_11DF => new _11DF(_cpu);

    //_11E0_Subf_X
    //_11E1_Cmpf_X
    //_1103 => new _1103(_cpu);
    //_1104 => new _1104(_cpu);
    //_1105 => new _1105(_cpu);
    //_1106 => new _1106(_cpu);
    //_11E6_Ldf_X
    //_11E7_Stf_X
    //_1109
    //_1110
    //_11EA
    //_11EB_Addf_X
    //_11EC
    //_11ED
    //_11EE
    //_11EF

    //_11F0_Subf_E
    //_11F1_Cmpf_E
    //_11F2
    //_11F3
    //_11F4
    //_11F5
    //_11F6_Ldf_E
    //_11F7_Stf_E
    //_11F8
    //_11F9
    //_11FA
    //_11FB_Addf_E
    //_11FC
    //_11FD
    //_11FE
    //_11FF

    #endregion

    public Page3Opcodes6809(Motorola cpu)
    {
        _cpu = cpu;
    }

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_113F_Swi3_I,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_1183_Cmpu_M,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_118C_Cmps_M ,_           ,_           ,_           ,
        _           ,_            ,_           ,_1193_Cmpu_D,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_119C_Cmps_D ,_           ,_           ,_           ,
        _           ,_            ,_           ,_11A3_Cmpu_X,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_11AC_Cmps_X ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_11BC_Cmps_E ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
        _           ,_            ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_           ,_            ,_           ,_           ,_           ,
    };
}
