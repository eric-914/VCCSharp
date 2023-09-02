namespace VCCSharp.OpCodes.Page3;

using VCCSharp.OpCodes.Model.OpCodes;

internal class Page3Opcodes6809
{
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

    //_30_Band_D
    //_31_Biand_D
    //_32_Bor_D
    //_33_Bior_D
    //_34_Beor_D
    //_35_Bieor_D
    //_36_Ldbt_D
    //_37_Stbt_D
    //_38_Tfm1_M
    //_39_Tfm2_M
    //_3A_Tfm3_M
    //_3B_Tfm4_M
    //_3C_Bitmd_M
    //_3D_Ldmd_M
    //_3E
    private IOpCode _3F_Swi3_I => new _113F_Swi3_I_6809();

    //_40
    //_41
    //_42
    //_43_Come_I
    //_44
    //_45
    //_46
    //_47
    //_48
    //_49
    //_4A_Dece_I
    //_4B
    //_4C_Ince_I
    //_4D_Tste_I
    //_4E
    //_4F_Clre_I

    //_50
    //_51
    //_52
    //_53_Comf_I
    //_54
    //_55
    //_56
    //_57
    //_58
    //_59
    //_5A_Decf_I
    //_5B
    //_5C_Incf_I
    //_5D_Tstf_I
    //_5E
    //_5F_Clrf_I

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

    //_80_Sube_M
    //_81_Cmpe_M
    //_82
    private IOpCode _83_Cmpu_M => new _1183_Cmpu_M();
    //_84
    //_85
    //_86_Lde_M
    //_87
    //_88
    //_89
    //_8A
    //_8B_Adde_M
    private IOpCode _8C_Cmps_M => new _118C_Cmps_M();
    //_8D_Divd_M
    //_8E_Divq_M
    //_8F_Muld_M

    //_90_Sube_D
    //_91_Cmpe_D
    //_92
    private IOpCode _93_Cmpu_D => new _1193_Cmpu_D();
    //_94
    //_95
    //_96_Lde_D
    //_97_Ste_D
    //_98
    //_99
    //_9A
    //_9B_Adde_D
    private IOpCode _9C_Cmps_D => new _119C_Cmps_D();
    //_9D_Divd_D
    //_9E_Divq_D
    //_9F_Muld_D

    //_A0_Sube_X
    //_A1_Cmpe_X
    //_A2
    private IOpCode _A3_Cmpu_X => new _11A3_Cmpu_X();
    //_A4
    //_A5
    //_A6_Lde_X
    //_A7_Ste_X
    //_A8
    //_A9
    //_AA
    //_AB_Adde_X
    private IOpCode _AC_Cmps_X => new _11AC_Cmps_X();
    //_AD_Divd_X
    //_AE_Divq_X
    //_AF_Muld_X

    //_B0_Sube_E
    //_B1_Cmpe_E
    //_B2
    //_B3_Cmpu_E
    //_B4
    //_B5
    //_B6_Lde_E
    //_B7_Ste_E
    //_B8
    //_B9
    //_BA
    //_BB_Adde_E
    private IOpCode _BC_Cmps_E => new _11BC_Cmps_E();
    //_BD_Divd_E
    //_BE_Divq_E
    //_BF_Muld_E

    //_C0_Subf_M
    //_C1_Cmpf_M
    //_C2
    //_C3
    //_C4
    //_C5
    //_C6_Ldf_M
    //_C7
    //_C8
    //_C9
    //_CA
    //_CB_Addf_M
    //_CC
    //_CD
    //_CE
    //_CF

    //_D0_Subf_D
    //_D1_Cmpf_D
    //_D2
    //_D3
    //_D4
    //_D5
    //_D6_Ldf_D
    //_D7_Stf_D
    //_D8
    //_D9
    //_DA
    //_DB_Addf_D
    //_DC => new _11DC();
    //_DD => new _11DD();
    //_DE => new _11DE();
    //_DF => new _11DF();

    //_E0_Subf_X
    //_E1_Cmpf_X
    //_03 => new _1103();
    //_04 => new _1104();
    //_05 => new _1105();
    //_06 => new _1106();
    //_E6_Ldf_X
    //_E7_Stf_X
    //_09
    //_10
    //_EA
    //_EB_Addf_X
    //_EC
    //_ED
    //_EE
    //_EF

    //_F0_Subf_E
    //_F1_Cmpf_E
    //_F2
    //_F3
    //_F4
    //_F5
    //_F6_Ldf_E
    //_F7_Stf_E
    //_F8
    //_F9
    //_FA
    //_FB_Addf_E
    //_FC
    //_FD
    //_FE
    //_FF

    #endregion

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        /*       __0           __1           __2           __3           __4           __5           __6           __7           __8           __9           __A           __B           __C           __D           __E           __F        */
        /* 0_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 1_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 2_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 3_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _3F_Swi3_I  ,
        /* 4_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 5_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 6_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 7_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 8_ */ _           , _            , _          , _83_Cmpu_M  , _           , _           , _           , _           , _           , _           , _           , _           , _8C_Cmps_M  , _           , _           , _           ,
        /* 9_ */ _           , _            , _          , _93_Cmpu_D  , _           , _           , _           , _           , _           , _           , _           , _           , _9C_Cmps_D  , _           , _           , _           ,
        /* A_ */ _           , _            , _          , _A3_Cmpu_X  , _           , _           , _           , _           , _           , _           , _           , _           , _AC_Cmps_X  , _           , _           , _           ,
        /* B_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _BC_Cmps_E  , _           , _           , _           ,
        /* C_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* D_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* E_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* F_ */ _           , _            , _          , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _          
    };
}
