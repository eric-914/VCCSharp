namespace VCCSharp.OpCodes.Page3;

using VCCSharp.OpCodes.Model.OpCodes;
using Hitachi = HD6309.IState;

internal class Page3Opcodes6309
{
    private readonly Hitachi _cpu;

    #region Factory

    private IOpCode _ => new UndefinedOpCode();

    //_00
    //_01
    //_02
    //_03
    //_04
    //_05
    //_06
    //_07
    //_08
    //_09
    //_0A
    //_0B
    //_0C
    //_0D
    //_0E
    //_0F

    //_10
    //_11
    //_12
    //_13
    //_14
    //_15
    //_16
    //_17
    //_18
    //_19
    //_1A
    //_1B
    //_1C
    //_1D
    //_1E
    //_1F

    //_20
    //_21
    //_22
    //_23
    //_24
    //_25
    //_26
    //_27
    //_28
    //_29
    //_2A
    //_2B
    //_2C
    //_2D
    //_2E
    //_2F

    private IOpCode _30_Band_D => new _1130_Band_D(_cpu);
    private IOpCode _31_Biand_D => new _1131_Biand_D(_cpu);
    private IOpCode _32_Bor__D => new _1132_Bor_D(_cpu);
    private IOpCode _33_Bior_D => new _1133_Bior_D(_cpu);
    private IOpCode _34_Beor_D => new _1134_Beor_D(_cpu);
    private IOpCode _35_Bieor => new _1135_Bieor(_cpu);
    private IOpCode _36_Ldbt => new _1136_Ldbt(_cpu);
    private IOpCode _37_Stbt => new _1137_Stbt(_cpu);
    private IOpCode _38_Tfm1 => new _1138_Tfm1(_cpu);
    private IOpCode _39_Tfm2 => new _1139_Tfm2(_cpu);
    private IOpCode _3A_Tfm3 => new _113A_Tfm3(_cpu);
    private IOpCode _3B_Tfm4 => new _113B_Tfm4(_cpu);
    private IOpCode _3C_Bitmd_M => new _113C_Bitmd_M(_cpu);
    private IOpCode _3D_Ldmd_M => new _113D_Ldmd_M(_cpu);
    //_3E
    private IOpCode _3F_Swi3_I => new _113F_Swi3_I_6309(_cpu);

    //_40
    //_41
    //_42
    private IOpCode _43_Come_I => new _1143_Come_I(_cpu);
    //_44
    //_45
    //_46
    //_47
    //_48
    //_49
    private IOpCode _4A_Dece_I => new _114A_Dece_I(_cpu);
    //_4B
    private IOpCode _4C_Ince_I => new _114C_Ince_I(_cpu);
    private IOpCode _4D_Tste_I => new _114D_Tste_I(_cpu);
    //_4E
    private IOpCode _4F_Clre_I => new _114F_Clre_I(_cpu);

    //_50
    //_51
    //_52
    private IOpCode _53_Comf_I => new _1153_Comf_I(_cpu);
    //_54
    //_55
    //_56
    //_57
    //_58
    //_59
    private IOpCode _5A_Decf_I => new _115A_Decf_I(_cpu);
    //_5B
    private IOpCode _5C_Incf_I => new _115C_Incf_I(_cpu);
    private IOpCode _5D_Tstf_I => new _115D_Tstf_I(_cpu);
    //_5E
    private IOpCode _5F_Clrf_I => new _115F_Clrf_I(_cpu);

    //_60
    //_61
    //_62
    //_63
    //_64
    //_65
    //_66
    //_67
    //_68
    //_69
    //_6A
    //_6B
    //_6C
    //_6D
    //_6E
    //_6F

    //_70
    //_71
    //_72
    //_73
    //_74
    //_75
    //_76
    //_77
    //_78
    //_79
    //_7A
    //_7B
    //_7C
    //_7D
    //_7E
    //_7F

    private IOpCode _80_Sube_M => new _1180_Sube_M(_cpu);
    private IOpCode _81_Cmpe_M => new _1181_Cmpe_M(_cpu);
    //_82
    private IOpCode _83_Cmpu_M => new _1183_Cmpu_M(_cpu);
    //_84
    //_85
    private IOpCode _86_Lde__M => new _1186_Lde_M(_cpu);
    //_87
    //_88
    //_89
    //_8A
    private IOpCode _8B_Adde_M => new _118B_Adde_M(_cpu);
    private IOpCode _8C_Cmps_M => new _118C_Cmps_M(_cpu);
    private IOpCode _8D_Divd_M => new _118D_Divd_M(_cpu);
    private IOpCode _8E_Divq_M => new _118E_Divq_M(_cpu);
    private IOpCode _8F_Muld_M => new _118F_Muld_M(_cpu);

    private IOpCode _90_Sube_D => new _1190_Sube_D(_cpu);
    private IOpCode _91_Cmpe_D => new _1191_Cmpe_D(_cpu);
    //_92
    private IOpCode _93_Cmpu_D => new _1193_Cmpu_D(_cpu);
    //_94
    //_95
    private IOpCode _96_Lde__D => new _1196_Lde_D(_cpu);
    private IOpCode _97_Ste__D => new _1197_Ste_D(_cpu);
    //_98
    //_99
    //_9A
    private IOpCode _9B_Adde_D => new _119B_Adde_D(_cpu);
    private IOpCode _9C_Cmps_D => new _119C_Cmps_D(_cpu);
    private IOpCode _9D_Divd_D => new _119D_Divd_D(_cpu);
    private IOpCode _9E_Divq_D => new _119E_Divq_D(_cpu);
    private IOpCode _9F_Muld_D => new _119F_Muld_D(_cpu);

    private IOpCode _A0_Sube_X => new _11A0_Sube_X(_cpu);
    private IOpCode _A1_Cmpe_X => new _11A1_Cmpe_X(_cpu);
    //_A2
    private IOpCode _A3_Cmpu_X => new _11A3_Cmpu_X(_cpu);
    //_A4
    //_A5
    private IOpCode _A6_Lde__X => new _11A6_Lde_X(_cpu);
    private IOpCode _A7_Ste__X => new _11A7_Ste_X(_cpu);
    //_A8
    //_A9
    //_AA
    private IOpCode _AB_Adde_X => new _11AB_Adde_X(_cpu);
    private IOpCode _AC_Cmps_X => new _11AC_Cmps_X(_cpu);
    private IOpCode _AD_Divd_X => new _11AD_Divd_X(_cpu);
    private IOpCode _AE_Divq_X => new _11AE_Divq_X(_cpu);
    private IOpCode _AF_Muld_X => new _11AF_Muld_X(_cpu);

    private IOpCode _B0_Sube_E => new _11B0_Sube_E(_cpu);
    private IOpCode _B1_Cmpe_E => new _11B1_Cmpe_E(_cpu);
    //_B2
    private IOpCode _B3_Cmpu_E => new _11B3_Cmpu_E(_cpu);
    //_B4
    //_B5
    private IOpCode _B6_Lde__E => new _11B6_Lde_E(_cpu);
    private IOpCode _B7_Ste__E => new _11B7_Ste_E(_cpu);
    //_B8
    //_B9
    //_BA
    private IOpCode _BB_Adde_E => new _11BB_Adde_E(_cpu);
    private IOpCode _BC_Cmps_E => new _11BC_Cmps_E(_cpu);
    private IOpCode _BD_Divd_E => new _11BD_Divd_E(_cpu);
    private IOpCode _BE_Divq_E => new _11BE_Divq_E(_cpu);
    private IOpCode _BF_Muld_E => new _11BF_Muld_E(_cpu);

    private IOpCode _C0_Subf_M => new _11C0_Subf_M(_cpu);
    private IOpCode _C1_Cmpf_M => new _11C1_Cmpf_M(_cpu);
    //_C2
    //_C3
    //_C4
    //_C5
    private IOpCode _C6_Ldf__M => new _11C6_Ldf_M(_cpu);
    //_C7
    //_C8
    //_C9
    //_CA
    private IOpCode _CB_Addf_M => new _11CB_Addf_M(_cpu);
    //_CC
    //_CD
    //_CE
    //_CF

    private IOpCode _D0_Subf_D => new _11D0_Subf_D(_cpu);
    private IOpCode _D1_Cmpf_D => new _11D1_Cmpf_D(_cpu);
    //_D2
    //_D3
    //_D4
    //_D5
    private IOpCode _D6_Ldf__D => new _11D6_Ldf_D(_cpu);
    private IOpCode _D7_Stf__D => new _11D7_Stf_D(_cpu);
    //_D8
    //_D9
    //_DA
    private IOpCode _DB_Addf_D => new _11DB_Addf_D(_cpu);
    //_DC => new _11DC(_cpu);
    //_DD => new _11DD(_cpu);
    //_DE => new _11DE(_cpu);
    //_DF => new _11DF(_cpu);

    private IOpCode _E0_Subf_X => new _11E0_Subf_X(_cpu);
    private IOpCode _E1_Cmpf_X => new _11E1_Cmpf_X(_cpu);
    //_03 => new _1103(_cpu);
    //_04 => new _1104(_cpu);
    //_05 => new _1105(_cpu);
    //_06 => new _1106(_cpu);
    private IOpCode _E6_Ldf__X => new _11E6_Ldf_X(_cpu);
    private IOpCode _E7_Stf__X => new _11E7_Stf_X(_cpu);
    //_09
    //_10
    //_EA
    private IOpCode _EB_Addf_X => new _11EB_Addf_X(_cpu);
    //_EC
    //_ED
    //_EE
    //_EF

    private IOpCode _F0_Subf_E => new _11F0_Subf_E(_cpu);
    private IOpCode _F1_Cmpf_E => new _11F1_Cmpf_E(_cpu);
    //_F2
    //_F3
    //_F4
    //_F5
    private IOpCode _F6_Ldf__E => new _11F6_Ldf_E(_cpu);
    private IOpCode _F7_Stf__E => new _11F7_Stf_E(_cpu);
    //_F8
    //_F9
    //_FA
    private IOpCode _FB_Addf_E => new _11FB_Addf_E(_cpu);
    //_FC
    //_FD
    //_FE
    //_FF

    #endregion

    public Page3Opcodes6309(Hitachi cpu)
    {
        _cpu = cpu;
    }

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        /*        _0            _1            _2           _3           _4           _6           _6           _7           _8           _9           _A           _B           _C            _D           _E           _F       */
        /* 0_ */ _           , _           , _          , _          , _          , _          , _          , _          , _          , _          , _          , _          , _           , _          , _          , _          ,
        /* 1_ */ _           , _           , _          , _          , _          , _          , _          , _          , _          , _          , _          , _          , _           , _          , _          , _          ,
        /* 2_ */ _           , _           , _          , _          , _          , _          , _          , _          , _          , _          , _          , _          , _           , _          , _          , _          ,
        /* 3_ */ _30_Band_D  , _31_Biand_D , _32_Bor__D , _33_Bior_D , _34_Beor_D , _35_Bieor  , _36_Ldbt   , _37_Stbt   , _38_Tfm1   , _39_Tfm2   , _3A_Tfm3   , _3B_Tfm4   , _3C_Bitmd_M , _3D_Ldmd_M , _          , _3F_Swi3_I ,
        /* 4_ */ _           , _           , _          , _43_Come_I , _          , _          , _          , _          , _          , _          , _4A_Dece_I , _          , _4C_Ince_I  , _4D_Tste_I , _          , _4F_Clre_I ,
        /* 5_ */ _           , _           , _          , _53_Comf_I , _          , _          , _          , _          , _          , _          , _5A_Decf_I , _          , _5C_Incf_I  , _5D_Tstf_I , _          , _5F_Clrf_I ,
        /* 6_ */ _           , _           , _          , _          , _          , _          , _          , _          , _          , _          , _          , _          , _           , _          , _          , _          ,
        /* 7_ */ _           , _           , _          , _          , _          , _          , _          , _          , _          , _          , _          , _          , _           , _          , _          , _          ,
        /* 8_ */ _80_Sube_M  , _81_Cmpe_M  , _          , _83_Cmpu_M , _          , _          , _86_Lde__M , _          , _          , _          , _          , _8B_Adde_M , _8C_Cmps_M  , _8D_Divd_M , _8E_Divq_M , _8F_Muld_M ,
        /* 9_ */ _90_Sube_D  , _91_Cmpe_D  , _          , _93_Cmpu_D , _          , _          , _96_Lde__D , _97_Ste__D , _          , _          , _          , _9B_Adde_D , _9C_Cmps_D  , _9D_Divd_D , _9E_Divq_D , _9F_Muld_D ,
        /* A_ */ _A0_Sube_X  , _A1_Cmpe_X  , _          , _A3_Cmpu_X , _          , _          , _A6_Lde__X , _A7_Ste__X , _          , _          , _          , _AB_Adde_X , _AC_Cmps_X  , _AD_Divd_X , _AE_Divq_X , _AF_Muld_X ,
        /* B_ */ _B0_Sube_E  , _B1_Cmpe_E  , _          , _B3_Cmpu_E , _          , _          , _B6_Lde__E , _B7_Ste__E , _          , _          , _          , _BB_Adde_E , _BC_Cmps_E  , _BD_Divd_E , _BE_Divq_E , _BF_Muld_E ,
        /* C_ */ _C0_Subf_M  , _C1_Cmpf_M  , _          , _          , _          , _          , _C6_Ldf__M , _          , _          , _          , _          , _CB_Addf_M , _           , _          , _          , _          ,
        /* D_ */ _D0_Subf_D  , _D1_Cmpf_D  , _          , _          , _          , _          , _D6_Ldf__D , _D7_Stf__D , _          , _          , _          , _DB_Addf_D , _           , _          , _          , _          ,
        /* E_ */ _E0_Subf_X  , _E1_Cmpf_X  , _          , _          , _          , _          , _E6_Ldf__X , _E7_Stf__X , _          , _          , _          , _EB_Addf_X , _           , _          , _          , _          ,
        /* F_ */ _F0_Subf_E  , _F1_Cmpf_E  , _          , _          , _          , _          , _F6_Ldf__E , _F7_Stf__E , _          , _          , _          , _FB_Addf_E , _           , _          , _          , _          ,    
    };
}
