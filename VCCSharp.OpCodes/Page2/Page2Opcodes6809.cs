namespace VCCSharp.OpCodes.Page2;

using VCCSharp.OpCodes.Model.OpCodes;
using Motorola = MC6809.IState;

internal class Page2Opcodes6809
{
    private readonly Motorola _cpu;

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
    private IOpCode _21_LBrn_R => new _1021_LBrn_R(_cpu);
    private IOpCode _22_LBhi_R => new _1022_LBhi_R(_cpu);
    private IOpCode _23_LBls_R => new _1023_LBls_R(_cpu);
    private IOpCode _24_LBhs_R => new _1024_LBhs_R(_cpu);
    private IOpCode _25_LBcs_R => new _1025_LBcs_R(_cpu);
    private IOpCode _26_LBne_R => new _1026_LBne_R(_cpu);
    private IOpCode _27_LBeq_R => new _1027_LBeq_R(_cpu);
    private IOpCode _28_LBvc_R => new _1028_LBvc_R(_cpu);
    private IOpCode _29_LBvs_R => new _1029_LBvs_R(_cpu);
    private IOpCode _2A_LBpl_R => new _102A_LBpl_R(_cpu);
    private IOpCode _2B_LBmi_R => new _102B_LBmi_R(_cpu);
    private IOpCode _2C_LBge_R => new _102C_LBge_R(_cpu);
    private IOpCode _2D_LBlt_R => new _102D_LBlt_R(_cpu);
    private IOpCode _2E_LBgt_R => new _102E_LBgt_R(_cpu);
    private IOpCode _2F_LBle_R => new _102F_LBle_R(_cpu);

    //_30_Addr_M
    //_31_Adcr_M
    //_32_Subr_M
    //_33_Sbcr_M
    //_34_Andr_M
    //_35_Orr_M
    //_36_Eorr_M
    //_37_Cmpr_M
    //_38_Pshsw_I
    //_39_Pulsw_I
    //_3A_Pshuw_I
    //_3B_Puluw_I
    //_3C
    //_3D
    //_3E
    private IOpCode _3F_Swi2_I => new _103F_Swi2_I_6809(_cpu);

    //_40_Negd_I
    //_41
    //_42
    //_43_Comd_I
    //_44_Lsrd_I
    //_45
    //_46_Rord_I
    //_47_Asrd_I
    //_48_Asld_I
    //_49_Rold_I
    //_4A_Decd_I
    //_4B
    //_4C_Incd_I
    //_4D_Tstd_I
    //_4E
    //_4F_Clrd_I

    //_50
    //_51
    //_52
    //_53_Comw_I
    //_54_Lsrw_I
    //_55
    //_56_Rorw_I
    //_57
    //_58
    //_59_Rolw_I
    //_5A_Decw_I
    //_5B
    //_5C_Incw_I
    //_5D_Tstw_I
    //_5E
    //_5F_Clrw_I

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

    //_80_Subw_M
    //_81_Cmpw_M
    //_82_Sbcd_M
    private IOpCode _83_Cmpd_M => new _1083_Cmpd_M(_cpu);
    //_84_Andd_M
    //_85_Bitd_M
    //_86_Ldw_M
    //_87
    //_88_Eord_M
    //_89_Adcd_M
    //_8A_Ord_M
    //_8B_Addw_M
    private IOpCode _8C_Cmpy_M => new _108C_Cmpy_M(_cpu);
    //_8D
    private IOpCode _8E_Ldy__M => new _108E_Ldy_M(_cpu);
    //_8F

    //_90_Subw_D
    //_91_Cmpw_D
    //_92_Sbcd_D
    private IOpCode _93_Cmpd_D => new _1093_Cmpd_D(_cpu);
    //_94_Andd_D
    //_95_Bitd_D
    //_96_Ldw_D
    //_97_Stw_D
    //_98_Eord_D
    //_99_Adcd_D
    //_9A_Ord_D
    //_9B_Addw_D
    private IOpCode _9C_Cmpy_D => new _109C_Cmpy_D(_cpu);
    //_9D
    private IOpCode _9E_Ldy__D => new _109E_Ldy_D(_cpu);
    private IOpCode _9F_Sty__D => new _109F_Sty_D(_cpu);

    //_A0_Subw_X
    //_A1_Cmpw_X
    //_A2_Sbcd_X
    private IOpCode _A3_Cmpd_X => new _10A3_Cmpd_X(_cpu);
    //_A4_Andd_X
    //_A5_Bitd_X
    //_A6_Ldw_X
    //_A7_Stw_X
    //_A8_Eord_X
    //_A9_Adcd_X
    //_AA_Ord_X
    //_AB_Addw_X
    private IOpCode _AC_Cmpy_X => new _10AC_Cmpy_X(_cpu);
    //_AD
    private IOpCode _AE_Ldy__X => new _10AE_Ldy_X(_cpu);
    private IOpCode _AF_Sty__X => new _10AF_Sty_X(_cpu);

    //_B0_Subw_E
    //_B1_Cmpw_E
    //_B2_Sbcd_E
    private IOpCode _B3_Cmpd_E => new _10B3_Cmpd_E(_cpu);
    //_B4_Andd_E
    //_B5_Bitd_E
    //_B6_Ldw_E
    //_B7_Stw_E
    //_B8_Eord_E
    //_B9_Adcd_E
    //_BA_Ord_E
    //_BB_Addw_E
    private IOpCode _BC_Cmpy_E => new _10BC_Cmpy_E(_cpu);
    //_BD
    private IOpCode _BE_Ldy__E => new _10BE_Ldy_E(_cpu);
    private IOpCode _BF_Sty__E => new _10BF_Sty_E(_cpu);

    //_C0
    //_C1
    //_C2
    //_C3
    //_C4
    //_C5
    //_C6
    //_C7
    //_C8
    //_C9
    //_CA
    //_CB
    //_CC
    //_CD
    private IOpCode _CE_Lds__I => new _10CE_Lds_I(_cpu);
    //_CF

    //_D0
    //_D1
    //_D2
    //_D3
    //_D4
    //_D5
    //_D6
    //_D7
    //_D8
    //_D9
    //_DA
    //_DB
    //_DC_Ldq_D
    //_DD_Stq_D
    private IOpCode _DE_Lds__D => new _10DE_Lds_D(_cpu);
    private IOpCode _DF_Sts__D => new _10DF_Sts_D(_cpu);

    //_E0
    //_E1
    //_E2
    //_E3
    //_E4
    //_E5
    //_E6
    //_E7
    //_E8
    //_E9
    //_EA
    //_EB
    //_EC_Ldq_X
    //_ED_Stq_X
    private IOpCode _EE_Lds__X => new _10EE_Lds_X(_cpu);
    private IOpCode _EF_Sts__X => new _10EF_Sts_X(_cpu);

    //_F0
    //_F1
    //_F2
    //_F3
    //_F4
    //_F5
    //_F6
    //_F7
    //_F8
    //_F9
    //_FA
    //_FB
    //_FC_Ldq_E
    //_FD_Stq_E
    private IOpCode _FE_Lds__E => new _10FE_Lds_E(_cpu);
    private IOpCode _FF_Sts__E => new _10FF_Sts_E(_cpu);

    #endregion

    public Page2Opcodes6809(Motorola cpu)
    {
        _cpu = cpu;
    }

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        /*       __0           __1           __2           __3           __4           __5           __6           __7           __8           __9           __A           __B           __C           __D           __E           __F        */
        /* 0_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 1_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 2_ */ _           , _21_LBrn_R  , _22_LBhi_R  , _23_LBls_R  , _24_LBhs_R  , _25_LBcs_R  , _26_LBne_R  , _27_LBeq_R  , _28_LBvc_R  , _29_LBvs_R  , _2A_LBpl_R  , _2B_LBmi_R  , _2C_LBge_R  , _2D_LBlt_R  , _2E_LBgt_R  , _2F_LBle_R  ,
        /* 3_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _3F_Swi2_I  ,
        /* 4_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 5_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 6_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 7_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           ,
        /* 8_ */ _           , _           , _           , _83_Cmpd_M  , _           , _           , _           , _           , _           , _           , _           , _           , _8C_Cmpy_M  , _           , _8E_Ldy__M  , _           ,
        /* 9_ */ _           , _           , _           , _93_Cmpd_D  , _           , _           , _           , _           , _           , _           , _           , _           , _9C_Cmpy_D  , _           , _9E_Ldy__D  , _9F_Sty__D  ,
        /* A_ */ _           , _           , _           , _A3_Cmpd_X  , _           , _           , _           , _           , _           , _           , _           , _           , _AC_Cmpy_X  , _           , _AE_Ldy__X  , _AF_Sty__X  ,
        /* B_ */ _           , _           , _           , _B3_Cmpd_E  , _           , _           , _           , _           , _           , _           , _           , _           , _BC_Cmpy_E  , _           , _BE_Ldy__E  , _BF_Sty__E  ,
        /* C_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _CE_Lds__I  , _           ,
        /* D_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _DE_Lds__D  , _DF_Sts__D  ,
        /* E_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _EE_Lds__X  , _EF_Sts__X  ,
        /* F_ */ _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _           , _FE_Lds__E  , _FF_Sts__E 
    };
}
