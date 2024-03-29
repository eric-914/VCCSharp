﻿namespace VCCSharp.OpCodes.Page1;

using VCCSharp.OpCodes.Model.OpCodes;

internal class Page1OpCodes6309
{
    #region Factory

    private IOpCode _ => new UndefinedOpCode();

    private IOpCode _00_Neg__D => new _00_Neg_D();
    private IOpCode _01_Oim__D => new _01_Oim_D();
    private IOpCode _02_Aim__D => new _02_Aim_D();
    private IOpCode _03_Com__D => new _03_Com_D();
    private IOpCode _04_Lsr__D => new _04_Lsr_D();
    private IOpCode _05_Eim__D => new _05_Eim_D();
    private IOpCode _06_Ror__D => new _06_Ror_D();
    private IOpCode _07_Asr__D => new _07_Asr_D();
    private IOpCode _08_Asl__D => new _08_Asl_D();
    private IOpCode _09_Rol__D => new _09_Rol_D();
    private IOpCode _0A_Dec__D => new _0A_Dec_D();
    private IOpCode _0B_Tim__D => new _0B_Tim_D();
    private IOpCode _0C_Inc__D => new _0C_Inc_D();
    private IOpCode _0D_Tst__D => new _0D_Tst_D();
    private IOpCode _0E_Jmp__D => new _0E_Jmp_D();
    private IOpCode _0F_Clr__D => new _0F_Clr_D();

    private IOpCode _10_Page_2 => new _10_Page_2_6309();
    private IOpCode _11_Page_3 => new _11_Page_3_6309();
    private IOpCode _12_Nop__I => new _12_Nop_I();
    private IOpCode _13_Sync_I => new _13_Sync_I();
    private IOpCode _14_Sexw_I => new _14_Sexw_I();
    //15
    private IOpCode _16_Lbra_R => new _16_Lbra_R();
    private IOpCode _17_Lbsr_R => new _17_Lbsr_R();
    //18
    private IOpCode _19_Daa__I => new _19_Daa_I();
    private IOpCode _1A_Orcc_M => new _1A_Orcc_M();
    //1B
    private IOpCode _1C_Andcc_M => new _1C_Andcc_M();
    private IOpCode _1D_Sex__I => new _1D_Sex_I();
    private IOpCode _1E_Exg__M => new _1E_Exg_M_6309();
    private IOpCode _1F_Tfr__M => new _1F_Tfr_M_6309();

    private IOpCode _20_Bra__R => new _20_Bra_R();
    private IOpCode _21_Brn__R => new _21_Brn_R();
    private IOpCode _22_Bhi__R => new _22_Bhi_R();
    private IOpCode _23_Bls__R => new _23_Bls_R();
    private IOpCode _24_Bhs__R => new _24_Bhs_R();
    private IOpCode _25_Bcs__R => new _25_Bcs_R();
    private IOpCode _26_Bne__R => new _26_Bne_R();
    private IOpCode _27_Beq__R => new _27_Beq_R();
    private IOpCode _28_Bvc__R => new _28_Bvc_R();
    private IOpCode _29_Bvs__R => new _29_Bvs_R();
    private IOpCode _2A_Bpl__R => new _2A_Bpl_R();
    private IOpCode _2B_Bmi__R => new _2B_Bmi_R();
    private IOpCode _2C_Bge__R => new _2C_Bge_R();
    private IOpCode _2D_Blt__R => new _2D_Blt_R();
    private IOpCode _2E_Bgt__R => new _2E_Bgt_R();
    private IOpCode _2F_Ble__R => new _2F_Ble_R();

    private IOpCode _30_Leax_X => new _30_Leax_X();
    private IOpCode _31_Leay_X => new _31_Leay_X();
    private IOpCode _32_Leas_X => new _32_Leas_X();
    private IOpCode _33_Leau_X => new _33_Leau_X();
    private IOpCode _34_Pshs_M => new _34_Pshs_M();
    private IOpCode _35_Puls_M => new _35_Puls_M();
    private IOpCode _36_Pshu_M => new _36_Pshu_M();
    private IOpCode _37_Pulu_M => new _37_Pulu_M();
    //38
    private IOpCode _39_Rts__I => new _39_Rts_I();
    private IOpCode _3A_Abx__I => new _3A_Abx_I();
    private IOpCode _3B_Rti__I => new _3B_Rti_I_6309();
    private IOpCode _3C_Cwai_I => new _3C_Cwai_I();
    private IOpCode _3D_Mul__I => new _3D_Mul_I();
    //3E
    private IOpCode _3F_Swi__I => new _3F_Swi_I_6309();

    private IOpCode _40_Nega_I => new _40_Nega_I();
    //41
    //42
    private IOpCode _43_Coma_I => new _43_Coma_I();
    private IOpCode _44_Lsra_I => new _44_Lsra_I();
    //45
    private IOpCode _46_Rora_I => new _46_Rora_I();
    private IOpCode _47_Asra_I => new _47_Asra_I();
    private IOpCode _48_Asla_I => new _48_Asla_I();
    private IOpCode _49_Rola_I => new _49_Rola_I();
    private IOpCode _4A_Deca_I => new _4A_Deca_I();
    //4B
    private IOpCode _4C_Inca_I => new _4C_Inca_I();
    private IOpCode _4D_Tsta_I => new _4D_Tsta_I();
    //4E
    private IOpCode _4F_Clra_I => new _4F_Clra_I();

    private IOpCode _50_Negb_I => new _50_Negb_I();
    //51
    //52
    private IOpCode _53_Comb_I => new _53_Comb_I();
    private IOpCode _54_Lsrb_I => new _54_Lsrb_I();
    //55
    private IOpCode _56_Rorb_I => new _56_Rorb_I();
    private IOpCode _57_Asrb_I => new _57_Asrb_I();
    private IOpCode _58_Aslb_I => new _58_Aslb_I();
    private IOpCode _59_Rolb_I => new _59_Rolb_I();
    private IOpCode _5A_Decb_I => new _5A_Decb_I();
    //5B
    private IOpCode _5C_Incb_I => new _5C_Incb_I();
    private IOpCode _5D_Tstb_I => new _5D_Tstb_I();
    //5E
    private IOpCode _5F_Clrb_I => new _5F_Clrb_I();

    private IOpCode _60_Neg__X => new _60_Neg_X();
    private IOpCode _61_Oim__X => new _61_Oim_X();
    private IOpCode _62_Aim__X => new _62_Aim_X();
    private IOpCode _63_Com__X => new _63_Com_X();
    private IOpCode _64_Lsr__X => new _64_Lsr_X();
    private IOpCode _65_Eim__X => new _65_Eim_X();
    private IOpCode _66_Ror__X => new _66_Ror_X();
    private IOpCode _67_Asr__X => new _67_Asr_X();
    private IOpCode _68_Asl__X => new _68_Asl_X();
    private IOpCode _69_Rol__X => new _69_Rol_X();
    private IOpCode _6A_Dec__X => new _6A_Dec_X();
    private IOpCode _6B_Tim__X => new _6B_Tim_X();
    private IOpCode _6C_Inc__X => new _6C_Inc_X();
    private IOpCode _6D_Tst__X => new _6D_Tst_X();
    private IOpCode _6E_Jmp__X => new _6E_Jmp_X();
    private IOpCode _6F_Clr__X => new _6F_Clr_X();

    private IOpCode _70_Neg__E => new _70_Neg_E();
    private IOpCode _71_Oim__E => new _71_Oim_E();
    private IOpCode _72_Aim__E => new _72_Aim_E();
    private IOpCode _73_Com__E => new _73_Com_E();
    private IOpCode _74_Lsr__E => new _74_Lsr_E();
    private IOpCode _75_Eim__E => new _75_Eim_E();
    private IOpCode _76_Ror__E => new _76_Ror_E();
    private IOpCode _77_Asr__E => new _77_Asr_E();
    private IOpCode _78_Asl__E => new _78_Asl_E();
    private IOpCode _79_Rol__E => new _79_Rol_E();
    private IOpCode _7A_Dec__E => new _7A_Dec_E();
    private IOpCode _7B_Tim__E => new _7B_Tim_E();
    private IOpCode _7C_Inc__E => new _7C_Inc_E();
    private IOpCode _7D_Tst__E => new _7D_Tst_E();
    private IOpCode _7E_Jmp__E => new _7E_Jmp_E();
    private IOpCode _7F_Clr__E => new _7F_Clr_E();

    private IOpCode _80_Suba_M => new _80_Suba_M();
    private IOpCode _81_Cmpa_M => new _81_Cmpa_M();
    private IOpCode _82_Sbca_M => new _82_Sbca_M();
    private IOpCode _83_Subd_M => new _83_Subd_M();
    private IOpCode _84_Anda_M => new _84_Anda_M();
    private IOpCode _85_Bita_M => new _85_Bita_M();
    private IOpCode _86_Lda__M => new _86_Lda_M();
    //87
    private IOpCode _88_Eora_M => new _88_Eora_M();
    private IOpCode _89_Adca_M => new _89_Adca_M();
    private IOpCode _8A_Ora__M => new _8A_Ora_M();
    private IOpCode _8B_Adda_M => new _8B_Adda_M();
    private IOpCode _8C_Cmpx_M => new _8C_Cmpx_M();
    private IOpCode _8D_Bsr__R => new _8D_Bsr_R();
    private IOpCode _8E_Ldx__M => new _8E_Ldx_M();
    //8F

    private IOpCode _90_Suba_D => new _90_Suba_D();
    private IOpCode _91_Cmpa_D => new _91_Cmpa_D();
    private IOpCode _92_Sbca_D => new _92_Sbca_D();
    private IOpCode _93_Subd_D => new _93_Subd_D();
    private IOpCode _94_Anda_D => new _94_Anda_D();
    private IOpCode _95_Bita_D => new _95_Bita_D();
    private IOpCode _96_Lda__D => new _96_Lda_D();
    private IOpCode _97_Sta__D => new _97_Sta_D();
    private IOpCode _98_Eora_D => new _98_Eora_D();
    private IOpCode _99_Adca_D => new _99_Adca_D();
    private IOpCode _9A_Ora__D => new _9A_Ora_D();
    private IOpCode _9B_Adda_D => new _9B_Adda_D();
    private IOpCode _9C_Cmpx_D => new _9C_Cmpx_D();
    private IOpCode _9D_Jsr__D => new _9D_Jsr_D();
    private IOpCode _9E_Ldx__D => new _9E_Ldx_D();
    private IOpCode _9F_Stx__D => new _9F_Stx_D();

    private IOpCode _A0_Suba_X => new _A0_Suba_X();
    private IOpCode _A1_Cmpa_X => new _A1_Cmpa_X();
    private IOpCode _A2_Sbca_X => new _A2_Sbca_X();
    private IOpCode _A3_Subd_X => new _A3_Subd_X();
    private IOpCode _A4_Anda_X => new _A4_Anda_X();
    private IOpCode _A5_Bita_X => new _A5_Bita_X();
    private IOpCode _A6_Lda__X => new _A6_Lda_X();
    private IOpCode _A7_Sta__X => new _A7_Sta_X();
    private IOpCode _A8_Eora_X => new _A8_Eora_X();
    private IOpCode _A9_Adca_X => new _A9_Adca_X();
    private IOpCode _AA_Ora__X => new _AA_Ora_X();
    private IOpCode _AB_Adda_X => new _AB_Adda_X();
    private IOpCode _AC_Cmpx_X => new _AC_Cmpx_X();
    private IOpCode _AD_Jsr__X => new _AD_Jsr_X();
    private IOpCode _AE_Ldx__X => new _AE_Ldx_X();
    private IOpCode _AF_Stx__X => new _AF_Stx_X();

    private IOpCode _B0_Suba_E => new _B0_Suba_E();
    private IOpCode _B1_Cmpa_E => new _B1_Cmpa_E();
    private IOpCode _B2_Sbca_E => new _B2_Sbca_E();
    private IOpCode _B3_Subd_E => new _B3_Subd_E();
    private IOpCode _B4_Anda_E => new _B4_Anda_E();
    private IOpCode _B5_Bita_E => new _B5_Bita_E();
    private IOpCode _B6_Lda__E => new _B6_Lda_E();
    private IOpCode _B7_Sta__E => new _B7_Sta_E();
    private IOpCode _B8_Eora_E => new _B8_Eora_E();
    private IOpCode _B9_Adca_E => new _B9_Adca_E();
    private IOpCode _BA_Ora__E => new _BA_Ora_E();
    private IOpCode _BB_Adda_E => new _BB_Adda_E();
    private IOpCode _BC_Cmpx_E => new _BC_Cmpx_E();
    private IOpCode _BD_Jsr__E => new _BD_Jsr_E();
    private IOpCode _BE_Ldx__E => new _BE_Ldx_E();
    private IOpCode _BF_Stx__E => new _BF_Stx_E();

    private IOpCode _C0_Subb_M => new _C0_Subb_M();
    private IOpCode _C1_Cmpb_M => new _C1_Cmpb_M();
    private IOpCode _C2_Sbcb_M => new _C2_Sbcb_M();
    private IOpCode _C3_Addd_M => new _C3_Addd_M();
    private IOpCode _C4_Andb_M => new _C4_Andb_M();
    private IOpCode _C5_Bitb_M => new _C5_Bitb_M();
    private IOpCode _C6_Ldb__M => new _C6_Ldb_M();
    //C7
    private IOpCode _C8_Eorb_M => new _C8_Eorb_M();
    private IOpCode _C9_Adcb_M => new _C9_Adcb_M();
    private IOpCode _CA_Orb__M => new _CA_Orb_M();
    private IOpCode _CB_Addb_M => new _CB_Addb_M();
    private IOpCode _CC_Ldd__M => new _CC_Ldd_M();
    private IOpCode _CD_Ldq__M => new _CD_Ldq_M();
    private IOpCode _CE_Ldu__M => new _CE_Ldu_M();
    //CF

    private IOpCode _D0_Subb_D => new _D0_Subb_D();
    private IOpCode _D1_Cmpb_D => new _D1_Cmpb_D();
    private IOpCode _D2_Sbcb_D => new _D2_Sbcb_D();
    private IOpCode _D3_Addd_D => new _D3_Addd_D();
    private IOpCode _D4_Andb_D => new _D4_Andb_D();
    private IOpCode _D5_Bitb_D => new _D5_Bitb_D();
    private IOpCode _D6_Ldb__D => new _D6_Ldb_D();
    private IOpCode _D7_Stb__D => new _D7_Stb_D();
    private IOpCode _D8_Eorb_D => new _D8_Eorb_D();
    private IOpCode _D9_Adcb_D => new _D9_Adcb_D();
    private IOpCode _DA_Orb__D => new _DA_Orb_D();
    private IOpCode _DB_Addb_D => new _DB_Addb_D();
    private IOpCode _DC_Ldd__D => new _DC_Ldd_D();
    private IOpCode _DD_Std__D => new _DD_Std_D();
    private IOpCode _DE_Ldu__D => new _DE_Ldu_D();
    private IOpCode _DF_Stu__D => new _DF_Stu_D();

    private IOpCode _E0_Subb_X => new _E0_Subb_X();
    private IOpCode _E1_Cmpb_X => new _E1_Cmpb_X();
    private IOpCode _E2_Sbcb_X => new _E2_Sbcb_X();
    private IOpCode _E3_Addd_X => new _E3_Addd_X();
    private IOpCode _E4_Andb_X => new _E4_Andb_X();
    private IOpCode _E5_Bitb_X => new _E5_Bitb_X();
    private IOpCode _E6_Ldb__X => new _E6_Ldb_X();
    private IOpCode _E7_Stb__X => new _E7_Stb_X();
    private IOpCode _E8_Eorb_X => new _E8_Eorb_X();
    private IOpCode _E9_Adcb_X => new _E9_Adcb_X();
    private IOpCode _EA_Orb__X => new _EA_Orb_X();
    private IOpCode _EB_Addb_X => new _EB_Addb_X();
    private IOpCode _EC_Ldd__X => new _EC_Ldd_X();
    private IOpCode _ED_Std__X => new _ED_Std_X();
    private IOpCode _EE_Ldu__X => new _EE_Ldu_X();
    private IOpCode _EF_Stu__X => new _EF_Stu_X();

    private IOpCode _F0_Subb_E => new _F0_Subb_E();
    private IOpCode _F1_Cmpb_E => new _F1_Cmpb_E();
    private IOpCode _F2_Sbcb_E => new _F2_Sbcb_E();
    private IOpCode _F3_Addd_E => new _F3_Addd_E();
    private IOpCode _F4_Andb_E => new _F4_Andb_E();
    private IOpCode _F5_Bitb_E => new _F5_Bitb_E();
    private IOpCode _F6_Ldb__E => new _F6_Ldb_E();
    private IOpCode _F7_Stb__E => new _F7_Stb_E();
    private IOpCode _F8_Eorb_E => new _F8_Eorb_E();
    private IOpCode _F9_Adcb_E => new _F9_Adcb_E();
    private IOpCode _FA_Orb__E => new _FA_Orb_E();
    private IOpCode _FB_Addb_E => new _FB_Addb_E();
    private IOpCode _FC_Ldd__E => new _FC_Ldd_E();
    private IOpCode _FD_Std__E => new _FD_Std_E();
    private IOpCode _FE_Ldu__E => new _FE_Ldu_E();
    private IOpCode _FF_Stu__E => new _FF_Stu_E();

    #endregion

    public IOpCode[] OpCodes => new IOpCode[256]
    {
        /*       __0           __1           __2           __3           __4           __5           __6           __7           __8           __9           __A           __B           __C           __D           __E           __F        */
        /* 0_ */ _00_Neg__D  , _01_Oim__D  , _02_Aim__D  , _03_Com__D  , _04_Lsr__D  , _05_Eim__D  , _06_Ror__D  , _07_Asr__D  , _08_Asl__D  , _09_Rol__D  , _0A_Dec__D  , _0B_Tim__D  , _0C_Inc__D  , _0D_Tst__D  , _0E_Jmp__D  , _0F_Clr__D  ,
        /* 1_ */ _10_Page_2  , _11_Page_3  , _12_Nop__I  , _13_Sync_I  , _14_Sexw_I  , _           , _16_Lbra_R  , _17_Lbsr_R  , _           , _19_Daa__I  , _1A_Orcc_M  , _           , _1C_Andcc_M , _1D_Sex__I  , _1E_Exg__M  , _1F_Tfr__M  ,
        /* 2_ */ _20_Bra__R  , _21_Brn__R  , _22_Bhi__R  , _23_Bls__R  , _24_Bhs__R  , _25_Bcs__R  , _26_Bne__R  , _27_Beq__R  , _28_Bvc__R  , _29_Bvs__R  , _2A_Bpl__R  , _2B_Bmi__R  , _2C_Bge__R  , _2D_Blt__R  , _2E_Bgt__R  , _2F_Ble__R  ,
        /* 3_ */ _30_Leax_X  , _31_Leay_X  , _32_Leas_X  , _33_Leau_X  , _34_Pshs_M  , _35_Puls_M  , _36_Pshu_M  , _37_Pulu_M  , _           , _39_Rts__I  , _3A_Abx__I  , _3B_Rti__I  , _3C_Cwai_I  , _3D_Mul__I  , _           , _3F_Swi__I  ,
        /* 4_ */ _40_Nega_I  , _           , _           , _43_Coma_I  , _44_Lsra_I  , _           , _46_Rora_I  , _47_Asra_I  , _48_Asla_I  , _49_Rola_I  , _4A_Deca_I  , _           , _4C_Inca_I  , _4D_Tsta_I  , _           , _4F_Clra_I  ,
        /* 5_ */ _50_Negb_I  , _           , _           , _53_Comb_I  , _54_Lsrb_I  , _           , _56_Rorb_I  , _57_Asrb_I  , _58_Aslb_I  , _59_Rolb_I  , _5A_Decb_I  , _           , _5C_Incb_I  , _5D_Tstb_I  , _           , _5F_Clrb_I  ,
        /* 6_ */ _60_Neg__X  , _61_Oim__X  , _62_Aim__X  , _63_Com__X  , _64_Lsr__X  , _65_Eim__X  , _66_Ror__X  , _67_Asr__X  , _68_Asl__X  , _69_Rol__X  , _6A_Dec__X  , _6B_Tim__X  , _6C_Inc__X  , _6D_Tst__X  , _6E_Jmp__X  , _6F_Clr__X  ,
        /* 7_ */ _70_Neg__E  , _71_Oim__E  , _72_Aim__E  , _73_Com__E  , _74_Lsr__E  , _75_Eim__E  , _76_Ror__E  , _77_Asr__E  , _78_Asl__E  , _79_Rol__E  , _7A_Dec__E  , _7B_Tim__E  , _7C_Inc__E  , _7D_Tst__E  , _7E_Jmp__E  , _7F_Clr__E  ,
        /* 8_ */ _80_Suba_M  , _81_Cmpa_M  , _82_Sbca_M  , _83_Subd_M  , _84_Anda_M  , _85_Bita_M  , _86_Lda__M  , _           , _88_Eora_M  , _89_Adca_M  , _8A_Ora__M  , _8B_Adda_M  , _8C_Cmpx_M  , _8D_Bsr__R  , _8E_Ldx__M  , _           ,
        /* 9_ */ _90_Suba_D  , _91_Cmpa_D  , _92_Sbca_D  , _93_Subd_D  , _94_Anda_D  , _95_Bita_D  , _96_Lda__D  , _97_Sta__D  , _98_Eora_D  , _99_Adca_D  , _9A_Ora__D  , _9B_Adda_D  , _9C_Cmpx_D  , _9D_Jsr__D  , _9E_Ldx__D  , _9F_Stx__D  ,
        /* A_ */ _A0_Suba_X  , _A1_Cmpa_X  , _A2_Sbca_X  , _A3_Subd_X  , _A4_Anda_X  , _A5_Bita_X  , _A6_Lda__X  , _A7_Sta__X  , _A8_Eora_X  , _A9_Adca_X  , _AA_Ora__X  , _AB_Adda_X  , _AC_Cmpx_X  , _AD_Jsr__X  , _AE_Ldx__X  , _AF_Stx__X  ,
        /* B_ */ _B0_Suba_E  , _B1_Cmpa_E  , _B2_Sbca_E  , _B3_Subd_E  , _B4_Anda_E  , _B5_Bita_E  , _B6_Lda__E  , _B7_Sta__E  , _B8_Eora_E  , _B9_Adca_E  , _BA_Ora__E  , _BB_Adda_E  , _BC_Cmpx_E  , _BD_Jsr__E  , _BE_Ldx__E  , _BF_Stx__E  ,
        /* C_ */ _C0_Subb_M  , _C1_Cmpb_M  , _C2_Sbcb_M  , _C3_Addd_M  , _C4_Andb_M  , _C5_Bitb_M  , _C6_Ldb__M  , _           , _C8_Eorb_M  , _C9_Adcb_M  , _CA_Orb__M  , _CB_Addb_M  , _CC_Ldd__M  , _CD_Ldq__M  , _CE_Ldu__M  , _           ,
        /* D_ */ _D0_Subb_D  , _D1_Cmpb_D  , _D2_Sbcb_D  , _D3_Addd_D  , _D4_Andb_D  , _D5_Bitb_D  , _D6_Ldb__D  , _D7_Stb__D  , _D8_Eorb_D  , _D9_Adcb_D  , _DA_Orb__D  , _DB_Addb_D  , _DC_Ldd__D  , _DD_Std__D  , _DE_Ldu__D  , _DF_Stu__D  ,
        /* E_ */ _E0_Subb_X  , _E1_Cmpb_X  , _E2_Sbcb_X  , _E3_Addd_X  , _E4_Andb_X  , _E5_Bitb_X  , _E6_Ldb__X  , _E7_Stb__X  , _E8_Eorb_X  , _E9_Adcb_X  , _EA_Orb__X  , _EB_Addb_X  , _EC_Ldd__X  , _ED_Std__X  , _EE_Ldu__X  , _EF_Stu__X  ,
        /* F_ */ _F0_Subb_E  , _F1_Cmpb_E  , _F2_Sbcb_E  , _F3_Addd_E  , _F4_Andb_E  , _F5_Bitb_E  , _F6_Ldb__E  , _F7_Stb__E  , _F8_Eorb_E  , _F9_Adcb_E  , _FA_Orb__E  , _FB_Addb_E  , _FC_Ldd__E  , _FD_Std__E  , _FE_Ldu__E  , _FF_Stu__E 
    };
}
