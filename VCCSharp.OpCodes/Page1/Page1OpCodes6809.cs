namespace VCCSharp.OpCodes.Page1;

using VCCSharp.OpCodes.Model.OpCodes;
using Motorola = MC6809.IState;

internal class Page1OpCodes6809
{
    Motorola _cpu;

    #region Factory

    private IOpCode __________() => new UndefinedOpCode();

    private IOpCode _00_Neg_D() => new _00_Neg_D(_cpu);
    //01
    //02
    private IOpCode _03_Com_D() => new _03_Com_D(_cpu);
    private IOpCode _04_Lsr_D() => new _04_Lsr_D(_cpu);
    //05
    private IOpCode _06_Ror_D() => new _06_Ror_D(_cpu);
    private IOpCode _07_Asr_D() => new _07_Asr_D(_cpu);
    private IOpCode _08_Asl_D() => new _08_Asl_D(_cpu);
    private IOpCode _09_Rol_D() => new _09_Rol_D(_cpu);
    private IOpCode _0A_Dec_D() => new _0A_Dec_D(_cpu);
    //0B
    private IOpCode _0C_Inc_D() => new _0C_Inc_D(_cpu);
    private IOpCode _0D_Tst_D() => new _0D_Tst_D(_cpu);
    private IOpCode _0E_Jmp_D() => new _0E_Jmp_D(_cpu);
    private IOpCode _0F_Clr_D() => new _0F_Clr_D(_cpu);

    private IOpCode _10_Page_2() => new _10_Page_2(_cpu);
    private IOpCode _11_Page_3() => new _11_Page_3(_cpu);
    private IOpCode _12_Nop_I() => new _12_Nop_I(_cpu);
    private IOpCode _13_Sync_I() => new _13_Sync_I(_cpu);
    //14
    //15
    private IOpCode _16_Lbra_R() => new _16_Lbra_R(_cpu);
    private IOpCode _17_Lbsr_R() => new _17_Lbsr_R(_cpu);
    //18
    private IOpCode _19_Daa_I() => new _19_Daa_I(_cpu);
    private IOpCode _1A_Orcc_M() => new _1A_Orcc_M(_cpu);
    //1B
    private IOpCode _1C_Andcc_M() => new _1C_Andcc_M(_cpu);
    private IOpCode _1D_Sex_I() => new _1D_Sex_I(_cpu);
    private IOpCode _1E_Exg_M() => new _1E_Exg_M_6809(_cpu);
    private IOpCode _1F_Tfr_M() => new _1F_Tfr_M_6809(_cpu);

    private IOpCode _20_Bra_R() => new _20_Bra_R(_cpu);
    private IOpCode _21_Brn_R() => new _21_Brn_R(_cpu);
    private IOpCode _22_Bhi_R() => new _22_Bhi_R(_cpu);
    private IOpCode _23_Bls_R() => new _23_Bls_R(_cpu);
    private IOpCode _24_Bhs_R() => new _24_Bhs_R(_cpu);
    private IOpCode _25_Bcs_R() => new _25_Bcs_R(_cpu);
    private IOpCode _26_Bne_R() => new _26_Bne_R(_cpu);
    private IOpCode _27_Beq_R() => new _27_Beq_R(_cpu);
    private IOpCode _28_Bvc_R() => new _28_Bvc_R(_cpu);
    private IOpCode _29_Bvs_R() => new _29_Bvs_R(_cpu);
    private IOpCode _2A_Bpl_R() => new _2A_Bpl_R(_cpu);
    private IOpCode _2B_Bmi_R() => new _2B_Bmi_R(_cpu);
    private IOpCode _2C_Bge_R() => new _2C_Bge_R(_cpu);
    private IOpCode _2D_Blt_R() => new _2D_Blt_R(_cpu);
    private IOpCode _2E_Bgt_R() => new _2E_Bgt_R(_cpu);
    private IOpCode _2F_Ble_R() => new _2F_Ble_R(_cpu);

    private IOpCode _30_Leax_X() => new _30_Leax_X(_cpu);
    private IOpCode _31_Leay_X() => new _31_Leay_X(_cpu);
    private IOpCode _32_Leas_X() => new _32_Leas_X(_cpu);
    private IOpCode _33_Leau_X() => new _33_Leau_X(_cpu);
    private IOpCode _34_Pshs_M() => new _34_Pshs_M(_cpu);
    private IOpCode _35_Puls_M() => new _35_Puls_M(_cpu);
    private IOpCode _36_Pshu_M() => new _36_Pshu_M(_cpu);
    private IOpCode _37_Pulu_M() => new _37_Pulu_M(_cpu);
    //38
    private IOpCode _39_Rts_I() => new _39_Rts_I(_cpu);
    private IOpCode _3A_Abx_I() => new _3A_Abx_I(_cpu);
    private IOpCode _3B_Rti_I() => new _3B_Rti_I_6809(_cpu);
    private IOpCode _3C_Cwai_I() => new _3C_Cwai_I(_cpu);
    private IOpCode _3D_Mul_I() => new _3D_Mul_I(_cpu);
    //3E
    private IOpCode _3F_Swi_I() => new _3F_Swi_I_6809(_cpu);

    private IOpCode _40_Nega_I() => new _40_Nega_I(_cpu);
    //41
    //42
    private IOpCode _43_Coma_I() => new _43_Coma_I(_cpu);
    private IOpCode _44_Lsra_I() => new _44_Lsra_I(_cpu);
    //45
    private IOpCode _46_Rora_I() => new _46_Rora_I(_cpu);
    private IOpCode _47_Asra_I() => new _47_Asra_I(_cpu);
    private IOpCode _48_Asla_I() => new _48_Asla_I(_cpu);
    private IOpCode _49_Rola_I() => new _49_Rola_I(_cpu);
    private IOpCode _4A_Deca_I() => new _4A_Deca_I(_cpu);
    //4B
    private IOpCode _4C_Inca_I() => new _4C_Inca_I(_cpu);
    private IOpCode _4D_Tsta_I() => new _4D_Tsta_I(_cpu);
    //4E
    private IOpCode _4F_Clra_I() => new _4F_Clra_I(_cpu);

    private IOpCode _50_Negb_I() => new _50_Negb_I(_cpu);
    //51
    //52
    private IOpCode _53_Comb_I() => new _53_Comb_I(_cpu);
    private IOpCode _54_Lsrb_I() => new _54_Lsrb_I(_cpu);
    //55
    private IOpCode _56_Rorb_I() => new _56_Rorb_I(_cpu);
    private IOpCode _57_Asrb_I() => new _57_Asrb_I(_cpu);
    private IOpCode _58_Aslb_I() => new _58_Aslb_I(_cpu);
    private IOpCode _59_Rolb_I() => new _59_Rolb_I(_cpu);
    private IOpCode _5A_Decb_I() => new _5A_Decb_I(_cpu);
    //5B
    private IOpCode _5C_Incb_I() => new _5C_Incb_I(_cpu);
    private IOpCode _5D_Tstb_I() => new _5D_Tstb_I(_cpu);
    //5E
    private IOpCode _5F_Clrb_I() => new _5F_Clrb_I(_cpu);

    private IOpCode _60_Neg_X() => new _60_Neg_X(_cpu);
    //61
    //62
    private IOpCode _63_Com_X() => new _63_Com_X(_cpu);
    private IOpCode _64_Lsr_X() => new _64_Lsr_X(_cpu);
    //65
    private IOpCode _66_Ror_X() => new _66_Ror_X(_cpu);
    private IOpCode _67_Asr_X() => new _67_Asr_X(_cpu);
    private IOpCode _68_Asl_X() => new _68_Asl_X(_cpu);
    private IOpCode _69_Rol_X() => new _69_Rol_X(_cpu);
    private IOpCode _6A_Dec_X() => new _6A_Dec_X(_cpu);
    //6B
    private IOpCode _6C_Inc_X() => new _6C_Inc_X(_cpu);
    private IOpCode _6D_Tst_X() => new _6D_Tst_X(_cpu);
    private IOpCode _6E_Jmp_X() => new _6E_Jmp_X(_cpu);
    private IOpCode _6F_Clr_X() => new _6F_Clr_X(_cpu);

    private IOpCode _70_Neg_E() => new _70_Neg_E(_cpu);
    //71
    //72
    private IOpCode _73_Com_E() => new _73_Com_E(_cpu);
    private IOpCode _74_Lsr_E() => new _74_Lsr_E(_cpu);
    //75
    private IOpCode _76_Ror_E() => new _76_Ror_E(_cpu);
    private IOpCode _77_Asr_E() => new _77_Asr_E(_cpu);
    private IOpCode _78_Asl_E() => new _78_Asl_E(_cpu);
    private IOpCode _79_Rol_E() => new _79_Rol_E(_cpu);
    private IOpCode _7A_Dec_E() => new _7A_Dec_E(_cpu);
    //7B
    private IOpCode _7C_Inc_E() => new _7C_Inc_E(_cpu);
    private IOpCode _7D_Tst_E() => new _7D_Tst_E(_cpu);
    private IOpCode _7E_Jmp_E() => new _7E_Jmp_E(_cpu);
    private IOpCode _7F_Clr_E() => new _7F_Clr_E(_cpu);

    private IOpCode _80_Suba_M() => new _80_Suba_M(_cpu);
    private IOpCode _81_Cmpa_M() => new _81_Cmpa_M(_cpu);
    private IOpCode _82_Sbca_M() => new _82_Sbca_M(_cpu);
    private IOpCode _83_Subd_M() => new _83_Subd_M(_cpu);
    private IOpCode _84_Anda_M() => new _84_Anda_M(_cpu);
    private IOpCode _85_Bita_M() => new _85_Bita_M(_cpu);
    private IOpCode _86_Lda_M() => new _86_Lda_M(_cpu);
    //87
    private IOpCode _88_Eora_M() => new _88_Eora_M(_cpu);
    private IOpCode _89_Adca_M() => new _89_Adca_M(_cpu);
    private IOpCode _8A_Ora_M() => new _8A_Ora_M(_cpu);
    private IOpCode _8B_Adda_M() => new _8B_Adda_M(_cpu);
    private IOpCode _8C_Cmpx_M() => new _8C_Cmpx_M(_cpu);
    private IOpCode _8D_Bsr_R() => new _8D_Bsr_R(_cpu);
    private IOpCode _8E_Ldx_M() => new _8E_Ldx_M(_cpu);
    //8F

    private IOpCode _90_Suba_D() => new _90_Suba_D(_cpu);
    private IOpCode _91_Cmpa_D() => new _91_Cmpa_D(_cpu);
    private IOpCode _92_Sbca_D() => new _92_Sbca_D(_cpu);
    private IOpCode _93_Subd_D() => new _93_Subd_D(_cpu);
    private IOpCode _94_Anda_D() => new _94_Anda_D(_cpu);
    private IOpCode _95_Bita_D() => new _95_Bita_D(_cpu);
    private IOpCode _96_Lda_D() => new _96_Lda_D(_cpu);
    private IOpCode _97_Sta_D() => new _97_Sta_D(_cpu);
    private IOpCode _98_Eora_D() => new _98_Eora_D(_cpu);
    private IOpCode _99_Adca_D() => new _99_Adca_D(_cpu);
    private IOpCode _9A_Ora_D() => new _9A_Ora_D(_cpu);
    private IOpCode _9B_Adda_D() => new _9B_Adda_D(_cpu);
    private IOpCode _9C_Cmpx_D() => new _9C_Cmpx_D(_cpu);
    private IOpCode _9D_Jsr_D() => new _9D_Jsr_D(_cpu);
    private IOpCode _9E_Ldx_D() => new _9E_Ldx_D(_cpu);
    private IOpCode _9F_Stx_D() => new _9F_Stx_D(_cpu);

    private IOpCode _A0_Suba_X() => new _A0_Suba_X(_cpu);
    private IOpCode _A1_Cmpa_X() => new _A1_Cmpa_X(_cpu);
    private IOpCode _A2_Sbca_X() => new _A2_Sbca_X(_cpu);
    private IOpCode _A3_Subd_X() => new _A3_Subd_X(_cpu);
    private IOpCode _A4_Anda_X() => new _A4_Anda_X(_cpu);
    private IOpCode _A5_Bita_X() => new _A5_Bita_X(_cpu);
    private IOpCode _A6_Lda_X() => new _A6_Lda_X(_cpu);
    private IOpCode _A7_Sta_X() => new _A7_Sta_X(_cpu);
    private IOpCode _A8_Eora_X() => new _A8_Eora_X(_cpu);
    private IOpCode _A9_Adca_X() => new _A9_Adca_X(_cpu);
    private IOpCode _AA_Ora_X() => new _AA_Ora_X(_cpu);
    private IOpCode _AB_Adda_X() => new _AB_Adda_X(_cpu);
    private IOpCode _AC_Cmpx_X() => new _AC_Cmpx_X(_cpu);
    private IOpCode _AD_Jsr_X() => new _AD_Jsr_X(_cpu);
    private IOpCode _AE_Ldx_X() => new _AE_Ldx_X(_cpu);
    private IOpCode _AF_Stx_X() => new _AF_Stx_X(_cpu);

    private IOpCode _B0_Suba_E() => new _B0_Suba_E(_cpu);
    private IOpCode _B1_Cmpa_E() => new _B1_Cmpa_E(_cpu);
    private IOpCode _B2_Sbca_E() => new _B2_Sbca_E(_cpu);
    private IOpCode _B3_Subd_E() => new _B3_Subd_E(_cpu);
    private IOpCode _B4_Anda_E() => new _B4_Anda_E(_cpu);
    private IOpCode _B5_Bita_E() => new _B5_Bita_E(_cpu);
    private IOpCode _B6_Lda_E() => new _B6_Lda_E(_cpu);
    private IOpCode _B7_Sta_E() => new _B7_Sta_E(_cpu);
    private IOpCode _B8_Eora_E() => new _B8_Eora_E(_cpu);
    private IOpCode _B9_Adca_E() => new _B9_Adca_E(_cpu);
    private IOpCode _BA_Ora_E() => new _BA_Ora_E(_cpu);
    private IOpCode _BB_Adda_E() => new _BB_Adda_E(_cpu);
    private IOpCode _BC_Cmpx_E() => new _BC_Cmpx_E(_cpu);
    private IOpCode _BD_Jsr_E() => new _BD_Jsr_E(_cpu);
    private IOpCode _BE_Ldx_E() => new _BE_Ldx_E(_cpu);
    private IOpCode _BF_Stx_E() => new _BF_Stx_E(_cpu);

    private IOpCode _C0_Subb_M() => new _C0_Subb_M(_cpu);
    private IOpCode _C1_Cmpb_M() => new _C1_Cmpb_M(_cpu);
    private IOpCode _C2_Sbcb_M() => new _C2_Sbcb_M(_cpu);
    private IOpCode _C3_Addd_M() => new _C3_Addd_M(_cpu);
    private IOpCode _C4_Andb_M() => new _C4_Andb_M(_cpu);
    private IOpCode _C5_Bitb_M() => new _C5_Bitb_M(_cpu);
    private IOpCode _C6_Ldb_M() => new _C6_Ldb_M(_cpu);
    //C7
    private IOpCode _C8_Eorb_M() => new _C8_Eorb_M(_cpu);
    private IOpCode _C9_Adcb_M() => new _C9_Adcb_M(_cpu);
    private IOpCode _CA_Orb_M() => new _CA_Orb_M(_cpu);
    private IOpCode _CB_Addb_M() => new _CB_Addb_M(_cpu);
    private IOpCode _CC_Ldd_M() => new _CC_Ldd_M(_cpu);
    //CD
    private IOpCode _CE_Ldu_M() => new _CE_Ldu_M(_cpu);
    //CF

    private IOpCode _D0_Subb_D() => new _D0_Subb_D(_cpu);
    private IOpCode _D1_Cmpb_D() => new _D1_Cmpb_D(_cpu);
    private IOpCode _D2_Sbcb_D() => new _D2_Sbcb_D(_cpu);
    private IOpCode _D3_Addd_D() => new _D3_Addd_D(_cpu);
    private IOpCode _D4_Andb_D() => new _D4_Andb_D(_cpu);
    private IOpCode _D5_Bitb_D() => new _D5_Bitb_D(_cpu);
    private IOpCode _D6_Ldb_D() => new _D6_Ldb_D(_cpu);
    private IOpCode _D7_Stb_D() => new _D7_Stb_D(_cpu);
    private IOpCode _D8_Eorb_D() => new _D8_Eorb_D(_cpu);
    private IOpCode _D9_Adcb_D() => new _D9_Adcb_D(_cpu);
    private IOpCode _DA_Orb_D() => new _DA_Orb_D(_cpu);
    private IOpCode _DB_Addb_D() => new _DB_Addb_D(_cpu);
    private IOpCode _DC_Ldd_D() => new _DC_Ldd_D(_cpu);
    private IOpCode _DD_Std_D() => new _DD_Std_D(_cpu);
    private IOpCode _DE_Ldu_D() => new _DE_Ldu_D(_cpu);
    private IOpCode _DF_Stu_D() => new _DF_Stu_D(_cpu);

    private IOpCode _E0_Subb_X() => new _E0_Subb_X(_cpu);
    private IOpCode _E1_Cmpb_X() => new _E1_Cmpb_X(_cpu);
    private IOpCode _E2_Sbcb_X() => new _E2_Sbcb_X(_cpu);
    private IOpCode _E3_Addd_X() => new _E3_Addd_X(_cpu);
    private IOpCode _E4_Andb_X() => new _E4_Andb_X(_cpu);
    private IOpCode _E5_Bitb_X() => new _E5_Bitb_X(_cpu);
    private IOpCode _E6_Ldb_X() => new _E6_Ldb_X(_cpu);
    private IOpCode _E7_Stb_X() => new _E7_Stb_X(_cpu);
    private IOpCode _E8_Eorb_X() => new _E8_Eorb_X(_cpu);
    private IOpCode _E9_Adcb_X() => new _E9_Adcb_X(_cpu);
    private IOpCode _EA_Orb_X() => new _EA_Orb_X(_cpu);
    private IOpCode _EB_Addb_X() => new _EB_Addb_X(_cpu);
    private IOpCode _EC_Ldd_X() => new _EC_Ldd_X(_cpu);
    private IOpCode _ED_Std_X() => new _ED_Std_X(_cpu);
    private IOpCode _EE_Ldu_X() => new _EE_Ldu_X(_cpu);
    private IOpCode _EF_Stu_X() => new _EF_Stu_X(_cpu);

    private IOpCode _F0_Subb_E() => new _F0_Subb_E(_cpu);
    private IOpCode _F1_Cmpb_E() => new _F1_Cmpb_E(_cpu);
    private IOpCode _F2_Sbcb_E() => new _F2_Sbcb_E(_cpu);
    private IOpCode _F3_Addd_E() => new _F3_Addd_E(_cpu);
    private IOpCode _F4_Andb_E() => new _F4_Andb_E(_cpu);
    private IOpCode _F5_Bitb_E() => new _F5_Bitb_E(_cpu);
    private IOpCode _F6_Ldb_E() => new _F6_Ldb_E(_cpu);
    private IOpCode _F7_Stb_E() => new _F7_Stb_E(_cpu);
    private IOpCode _F8_Eorb_E() => new _F8_Eorb_E(_cpu);
    private IOpCode _F9_Adcb_E() => new _F9_Adcb_E(_cpu);
    private IOpCode _FA_Orb_E() => new _FA_Orb_E(_cpu);
    private IOpCode _FB_Addb_E() => new _FB_Addb_E(_cpu);
    private IOpCode _FC_Ldd_E() => new _FC_Ldd_E(_cpu);
    private IOpCode _FD_Std_E() => new _FD_Std_E(_cpu);
    private IOpCode _FE_Ldu_E() => new _FE_Ldu_E(_cpu);
    private IOpCode _FF_Stu_E() => new _FF_Stu_E(_cpu);

    #endregion

    public Page1OpCodes6809(Motorola cpu)
    {
        _cpu = cpu;
    }

    public Func<IOpCode>[] OpCodes => new Func<IOpCode>[]
    {
        _00_Neg_D , __________, __________, _03_Com_D , _04_Lsr_D , __________, _06_Ror_D , _07_Asr_D , _08_Asl_D , _09_Rol_D , _0A_Dec_D , __________, _0C_Inc_D  , _0D_Tst_D , _0E_Jmp_D , _0F_Clr_D ,
        _10_Page_2, _11_Page_3, _12_Nop_I , _13_Sync_I, __________, __________, _16_Lbra_R, _17_Lbsr_R, __________, _19_Daa_I , _1A_Orcc_M, __________, _1C_Andcc_M, _1D_Sex_I , _1E_Exg_M , _1F_Tfr_M ,
        _20_Bra_R , _21_Brn_R , _22_Bhi_R , _23_Bls_R , _24_Bhs_R , _25_Bcs_R , _26_Bne_R , _27_Beq_R , _28_Bvc_R , _29_Bvs_R , _2A_Bpl_R , _2B_Bmi_R , _2C_Bge_R  , _2D_Blt_R , _2E_Bgt_R , _2F_Ble_R ,
        _30_Leax_X, _31_Leay_X, _32_Leas_X, _33_Leau_X, _34_Pshs_M, _35_Puls_M, _36_Pshu_M, _37_Pulu_M, __________, _39_Rts_I , _3A_Abx_I , _3B_Rti_I , _3C_Cwai_I , _3D_Mul_I , __________, _3F_Swi_I ,
        _40_Nega_I, __________, __________, _43_Coma_I, _44_Lsra_I, __________, _46_Rora_I, _47_Asra_I, _48_Asla_I, _49_Rola_I, _4A_Deca_I, __________, _4C_Inca_I , _4D_Tsta_I, __________, _4F_Clra_I,
        _50_Negb_I, __________, __________, _53_Comb_I, _54_Lsrb_I, __________, _56_Rorb_I, _57_Asrb_I, _58_Aslb_I, _59_Rolb_I, _5A_Decb_I, __________, _5C_Incb_I , _5D_Tstb_I, __________, _5F_Clrb_I,
        _60_Neg_X , __________, __________, _63_Com_X , _64_Lsr_X , __________, _66_Ror_X , _67_Asr_X , _68_Asl_X , _69_Rol_X , _6A_Dec_X , __________, _6C_Inc_X  , _6D_Tst_X , _6E_Jmp_X , _6F_Clr_X ,
        _70_Neg_E , __________, __________, _73_Com_E , _74_Lsr_E , __________, _76_Ror_E , _77_Asr_E , _78_Asl_E , _79_Rol_E , _7A_Dec_E , __________, _7C_Inc_E  , _7D_Tst_E , _7E_Jmp_E , _7F_Clr_E ,
        _80_Suba_M, _81_Cmpa_M, _82_Sbca_M, _83_Subd_M, _84_Anda_M, _85_Bita_M, _86_Lda_M , __________, _88_Eora_M, _89_Adca_M, _8A_Ora_M , _8B_Adda_M, _8C_Cmpx_M , _8D_Bsr_R , _8E_Ldx_M , __________,
        _90_Suba_D, _91_Cmpa_D, _92_Sbca_D, _93_Subd_D, _94_Anda_D, _95_Bita_D, _96_Lda_D , _97_Sta_D , _98_Eora_D, _99_Adca_D, _9A_Ora_D , _9B_Adda_D, _9C_Cmpx_D , _9D_Jsr_D , _9E_Ldx_D , _9F_Stx_D ,
        _A0_Suba_X, _A1_Cmpa_X, _A2_Sbca_X, _A3_Subd_X, _A4_Anda_X, _A5_Bita_X, _A6_Lda_X , _A7_Sta_X , _A8_Eora_X, _A9_Adca_X, _AA_Ora_X , _AB_Adda_X, _AC_Cmpx_X , _AD_Jsr_X , _AE_Ldx_X , _AF_Stx_X ,
        _B0_Suba_E, _B1_Cmpa_E, _B2_Sbca_E, _B3_Subd_E, _B4_Anda_E, _B5_Bita_E, _B6_Lda_E , _B7_Sta_E , _B8_Eora_E, _B9_Adca_E, _BA_Ora_E , _BB_Adda_E, _BC_Cmpx_E , _BD_Jsr_E , _BE_Ldx_E , _BF_Stx_E ,
        _C0_Subb_M, _C1_Cmpb_M, _C2_Sbcb_M, _C3_Addd_M, _C4_Andb_M, _C5_Bitb_M, _C6_Ldb_M , __________, _C8_Eorb_M, _C9_Adcb_M, _CA_Orb_M , _CB_Addb_M, _CC_Ldd_M  , __________, _CE_Ldu_M , __________,
        _D0_Subb_D, _D1_Cmpb_D, _D2_Sbcb_D, _D3_Addd_D, _D4_Andb_D, _D5_Bitb_D, _D6_Ldb_D , _D7_Stb_D , _D8_Eorb_D, _D9_Adcb_D, _DA_Orb_D , _DB_Addb_D, _DC_Ldd_D  , _DD_Std_D , _DE_Ldu_D , _DF_Stu_D ,
        _E0_Subb_X, _E1_Cmpb_X, _E2_Sbcb_X, _E3_Addd_X, _E4_Andb_X, _E5_Bitb_X, _E6_Ldb_X , _E7_Stb_X , _E8_Eorb_X, _E9_Adcb_X, _EA_Orb_X , _EB_Addb_X, _EC_Ldd_X  , _ED_Std_X , _EE_Ldu_X , _EF_Stu_X ,
        _F0_Subb_E, _F1_Cmpb_E, _F2_Sbcb_E, _F3_Addd_E, _F4_Andb_E, _F5_Bitb_E, _F6_Ldb_E , _F7_Stb_E , _F8_Eorb_E, _F9_Adcb_E, _FA_Orb_E , _FB_Addb_E, _FC_Ldd_E  , _FD_Std_E , _FE_Ldu_E , _FF_Stu_E ,
    };
}
