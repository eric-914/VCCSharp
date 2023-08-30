namespace VCCSharp.OpCodes.Page2;

using VCCSharp.OpCodes.Model.OpCodes;
using Motorola = MC6809.IState;

internal class Page2Opcodes6809
{
    private readonly Motorola _cpu;

    #region Factory

    private IOpCode __________ => new UndefinedOpCode();

    //_1000
    //_1001
    //_1002
    //_1003
    //_1004
    //_1005
    //_1006
    //_1007
    //_1008
    //_1009
    //_100A
    //_100B
    //_100C
    //_100D
    //_100E
    //_100F

    //_1010
    //_1011
    //_1012
    //_1013
    //_1014
    //_1015
    //_1016
    //_1017
    //_1018
    //_1019
    //_101A
    //_101B
    //_101C
    //_101D
    //_101E
    //_101F

    //_1020
    private IOpCode _1021_LBrn_R => new _1021_LBrn_R(_cpu);
    private IOpCode _1022_LBhi_R => new _1022_LBhi_R(_cpu);
    private IOpCode _1023_LBls_R => new _1023_LBls_R(_cpu);
    private IOpCode _1024_LBhs_R => new _1024_LBhs_R(_cpu);
    private IOpCode _1025_LBcs_R => new _1025_LBcs_R(_cpu);
    private IOpCode _1026_LBne_R => new _1026_LBne_R(_cpu);
    private IOpCode _1027_LBeq_R => new _1027_LBeq_R(_cpu);
    private IOpCode _1028_LBvc_R => new _1028_LBvc_R(_cpu);
    private IOpCode _1029_LBvs_R => new _1029_LBvs_R(_cpu);
    private IOpCode _102A_LBpl_R => new _102A_LBpl_R(_cpu);
    private IOpCode _102B_LBmi_R => new _102B_LBmi_R(_cpu);
    private IOpCode _102C_LBge_R => new _102C_LBge_R(_cpu);
    private IOpCode _102D_LBlt_R => new _102D_LBlt_R(_cpu);
    private IOpCode _102E_LBgt_R => new _102E_LBgt_R(_cpu);
    private IOpCode _102F_LBle_R => new _102F_LBle_R(_cpu);

    //_1030_Addr
    //_1031_Adcr
    //_1032_Subr
    //_1033_Sbcr
    //_1034_Andr
    //_1035_Orr
    //_1036_Eorr
    //_1037_Cmpr
    //_1038_Pshsw
    //_1039_Pulsw
    //_103A_Pshuw
    //_103B_Puluw
    //_103C
    //_103D
    //_103E
    private IOpCode _103F_Swi2_I => new _103F_Swi2_I_6809(_cpu);

    //_1040_Negd_I
    //_1041
    //_1042
    //_1043_Comd_I
    //_1044_Lsrd_I
    //_1045
    //_1046_Rord_I
    //_1047_Asrd_I
    //_1048_Asld_I
    //_1049_Rold_I
    //_104A_Decd_I
    //_104B
    //_104C_Incd_I
    //_104D_Tstd_I
    //_104E
    //_104F_Clrd_I

    //_1050
    //_1051
    //_1052
    //_1053_Comw_I
    //_1054_Lsrw_I
    //_1055
    //_1056_Rorw_I
    //_1057
    //_1058
    //_1059_Rolw_I
    //_105A_Decw_I
    //_105B
    //_105C_Incw_I
    //_105D_Tstw_I
    //_105E
    //_105F_Clrw_I

    //_1060
    //_1061
    //_1062
    //_1063
    //_1064
    //_1065
    //_1066
    //_1067
    //_1068
    //_1069
    //_106A
    //_106B
    //_106C
    //_106D
    //_106E
    //_106F

    //_1070
    //_1071
    //_1072
    //_1073
    //_1074
    //_1075
    //_1076
    //_1077
    //_1078
    //_1079
    //_107A
    //_107B
    //_107C
    //_107D
    //_107E
    //_107F

    //_1080_Subw_M
    //_1081_Cmpw_M
    //_1082_Sbcd_M
    private IOpCode _1083_Cmpd_M => new _1083_Cmpd_M(_cpu);
    //_1084_Andd_M
    //_1085_Bitd_M
    //_1086_Ldw_M
    //_1087
    //_1088_Eord_M
    //_1089_Adcd_M
    //_108A_Ord_M
    //_108B_Addw_M
    private IOpCode _108C_Cmpy_M => new _108C_Cmpy_M(_cpu);
    //_108D
    private IOpCode _108E_Ldy_M => new _108E_Ldy_M(_cpu);
    //_108F

    //_1090_Subw_D
    //_1091_Cmpw_D
    //_1092_Sbcd_D
    private IOpCode _1093_Cmpd_D => new _1093_Cmpd_D(_cpu);
    //_1094_Andd_D
    //_1095_Bitd_D
    //_1096_Ldw_D
    //_1097_Stw_D
    //_1098_Eord_D
    //_1099_Adcd_D
    //_109A_Ord_D
    //_109B_Addw_D
    private IOpCode _109C_Cmpy_D => new _109C_Cmpy_D(_cpu);
    //_109D
    private IOpCode _109E_Ldy_D => new _109E_Ldy_D(_cpu);
    private IOpCode _109F_Sty_D => new _109F_Sty_D(_cpu);

    //_10A0_Subw_X
    //_10A1_Cmpw_X
    //_10A2_Sbcd_X
    private IOpCode _10A3_Cmpd_X => new _10A3_Cmpd_X(_cpu);
    //_10A4_Andd_X
    //_10A5_Bitd_X
    //_10A6_Ldw_X
    //_10A7_Stw_X
    //_10A8_Eord_X
    //_10A9_Adcd_X
    //_10AA_Ord_X
    //_10AB_Addw_X
    private IOpCode _10AC_Cmpy_X => new _10AC_Cmpy_X(_cpu);
    //_10AD
    private IOpCode _10AE_Ldy_X => new _10AE_Ldy_X(_cpu);
    private IOpCode _10AF_Sty_X => new _10AF_Sty_X(_cpu);

    //_10B0_Subw_E
    //_10B1_Cmpw_E
    //_10B2_Sbcd_E
    private IOpCode _10B3_Cmpd_E => new _10B3_Cmpd_E(_cpu);
    //_10B4_Andd_E
    //_10B5_Bitd_E
    //_10B6_Ldw_E
    //_10B7_Stw_E
    //_10B8_Eord_E
    //_10B9_Adcd_E
    //_10BA_Ord_E
    //_10BB_Addw_E
    private IOpCode _10BC_Cmpy_E => new _10BC_Cmpy_E(_cpu);
    //_10BD
    private IOpCode _10BE_Ldy_E => new _10BE_Ldy_E(_cpu);
    private IOpCode _10BF_Sty_E => new _10BF_Sty_E(_cpu);

    //_10C0
    //_10C1
    //_10C2
    //_10C3
    //_10C4
    //_10C5
    //_10C6
    //_10C7
    //_10C8
    //_10C9
    //_10CA
    //_10CB
    //_10CC
    //_10CD
    private IOpCode _10CE_Lds_I => new _10CE_Lds_I(_cpu);
    //_10CF

    //_10D0
    //_10D1
    //_10D2
    //_10D3
    //_10D4
    //_10D5
    //_10D6
    //_10D7
    //_10D8
    //_10D9
    //_10DA
    //_10DB
    //_10DC_Ldq_D
    //_10DD_Stq_D
    private IOpCode _10DE_Lds_D => new _10DE_Lds_D(_cpu);
    private IOpCode _10DF_Sts_D => new _10DF_Sts_D(_cpu);

    //_10E0
    //_10E1
    //_10E2
    //_10E3
    //_10E4
    //_10E5
    //_10E6
    //_10E7
    //_10E8
    //_10E9
    //_10EA
    //_10EB
    //_10EC_Ldq_X
    //_10ED_Stq_X
    private IOpCode _10EE_Lds_X => new _10EE_Lds_X(_cpu);
    private IOpCode _10EF_Sts_X => new _10EF_Sts_X(_cpu);

    //_10F0
    //_10F1
    //_10F2
    //_10F3
    //_10F4
    //_10F5
    //_10F6
    //_10F7
    //_10F8
    //_10F9
    //_10FA
    //_10FB
    //_10FC_Ldq_E
    //_10FD_Stq_E
    private IOpCode _10FE_Lds_E => new _10FE_Lds_E(_cpu);
    private IOpCode _10FF_Sts_E => new _10FF_Sts_E(_cpu);

    #endregion

    public Page2Opcodes6809(Motorola cpu)
    {
        _cpu = cpu;
    }

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,_1021_LBrn_R,_1022_LBhi_R,_1023_LBls_R,_1024_LBhs_R,_1025_LBcs_R,_1026_LBne_R,_1027_LBeq_R,_1028_LBvc_R,_1029_LBvs_R,_102A_LBpl_R,_102B_LBmi_R,_102C_LBge_R,_102D_LBlt_R,_102E_LBgt_R,_102F_LBle_R,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_103F_Swi2_I,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,
        __________  ,__________  ,__________  ,_1083_Cmpd_M,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_108C_Cmpy_M,__________  ,_108E_Ldy_M ,__________  ,
        __________  ,__________  ,__________  ,_1093_Cmpd_D,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_109C_Cmpy_D,__________  ,_109E_Ldy_D ,_109F_Sty_D ,
        __________  ,__________  ,__________  ,_10A3_Cmpd_X,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10AC_Cmpy_X,__________  ,_10AE_Ldy_X ,_10AF_Sty_X ,
        __________  ,__________  ,__________  ,_10B3_Cmpd_E,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10BC_Cmpy_E,__________  ,_10BE_Ldy_E ,_10BF_Sty_E ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10CE_Lds_I ,__________  ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10DE_Lds_D ,_10DF_Sts_D ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10EE_Lds_X ,_10EF_Sts_X ,
        __________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,__________  ,_10FE_Lds_E ,_10FF_Sts_E 
    };
}
