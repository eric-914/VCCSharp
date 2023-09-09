namespace VCCSharp.OpCodes.Tests.Model.HD6309.O;

internal partial class OldOpcodes
{
    private readonly Action _____ = null!;

    private static Action[] _page1 = new Action[256];
    private static Action[] _page2 = new Action[256];
    private static Action[] _page3 = new Action[256];

    private Action[] Page1Vectors()
    {
        Action Reset = null!;   //Undocumented instruction
        Action Page_2 = null!;  //Overridden
        Action Page_3 = null!;  //Overridden

        return new Action[] {
                     /*   _0      _1      _2      _3      _4      _5      _6      _7      _8      _9      _A      _B      _C      _D      _E      _F   */
            /* 0_ */    Neg_D,  Oim_D,  Aim_D,  Com_D,  Lsr_D,  Eim_D,  Ror_D,  Asr_D,  Asl_D,  Rol_D,  Dec_D,  Tim_D,  Inc_D,  Tst_D,  Jmp_D,  Clr_D,
            /* 1_ */    Page_2, Page_3, Nop_I,  Sync_I, Sexw_I, _____,  Lbra_R, Lbsr_R, _____,  Daa_I,  Orcc_M, _____,  Andcc_M,Sex_I,  Exg_M,  Tfr_M,
            /* 2_ */    Bra_R,  Brn_R,  Bhi_R,  Bls_R,  Bhs_R,  Blo_R,  Bne_R,  Beq_R,  Bvc_R,  Bvs_R,  Bpl_R,  Bmi_R,  Bge_R,  Blt_R,  Bgt_R,  Ble_R,
            /* 3_ */    Leax_X, Leay_X, Leas_X, Leau_X, Pshs_M, Puls_M, Pshu_M, Pulu_M, _____,  Rts_I,  Abx_I,  Rti_I,  Cwai_I, Mul_I,  Reset,  Swi1_I,
            /* 4_ */    Nega_I, _____,  _____,  Coma_I, Lsra_I, _____,  Rora_I, Asra_I, Asla_I, Rola_I, Deca_I, _____,  Inca_I, Tsta_I, _____,  Clra_I,
            /* 5_ */    Negb_I, _____,  _____,  Comb_I, Lsrb_I, _____,  Rorb_I, Asrb_I, Aslb_I, Rolb_I, Decb_I, _____,  Incb_I, Tstb_I, _____,  Clrb_I,
            /* 6_ */    Neg_X,  Oim_X,  Aim_X,  Com_X,  Lsr_X,  Eim_X,  Ror_X,  Asr_X,  Asl_X,  Rol_X,  Dec_X,  Tim_X,  Inc_X,  Tst_X,  Jmp_X,  Clr_X,
            /* 7_ */    Neg_E,  Oim_E,  Aim_E,  Com_E,  Lsr_E,  Eim_E,  Ror_E,  Asr_E,  Asl_E,  Rol_E,  Dec_E,  Tim_E,  Inc_E,  Tst_E,  Jmp_E,  Clr_E,
            /* 8_ */    Suba_M, Cmpa_M, Sbca_M, Subd_M, Anda_M, Bita_M, Lda_M,  _____,  Eora_M, Adca_M, Ora_M,  Adda_M, Cmpx_M, Bsr_R,  Ldx_M,  _____,
            /* 9_ */    Suba_D, Cmpa_D, Scba_D, Subd_D, Anda_D, Bita_D, Lda_D,  Sta_D,  Eora_D, Adca_D, Ora_D,  Adda_D, Cmpx_D, Jsr_D,  Ldx_D,  Stx_D,
            /* A_ */    Suba_X, Cmpa_X, Sbca_X, Subd_X, Anda_X, Bita_X, Lda_X,  Sta_X,  Eora_X, Adca_X, Ora_X,  Adda_X, Cmpx_X, Jsr_X,  Ldx_X,  Stx_X,
            /* B_ */    Suba_E, Cmpa_E, Sbca_E, Subd_E, Anda_E, Bita_E, Lda_E,  Sta_E,  Eora_E, Adca_E, Ora_E,  Adda_E, Cmpx_E, Bsr_E,  Ldx_E,  Stx_E,
            /* C_ */    Subb_M, Cmpb_M, Sbcb_M, Addd_M, Andb_M, Bitb_M, Ldb_M,  _____,  Eorb_M, Adcb_M, Orb_M,  Addb_M, Ldd_M,  Ldq_M,  Ldu_M,  _____,
            /* D_ */    Subb_D, Cmpb_D, Sbcb_D, Addd_D, Andb_D, Bitb_D, Ldb_D,  Stb_D,  Eorb_D, Adcb_D, Orb_D,  Addb_D, Ldd_D,  Std_D,  Ldu_D,  Stu_D,
            /* E_ */    Subb_X, Cmpb_X, Sbcb_X, Addd_X, Andb_X, Bitb_X, Ldb_X,  Stb_X,  Eorb_X, Adcb_X, Orb_X,  Addb_X, Ldd_X,  Std_X,  Ldu_X,  Stu_X,
            /* F_ */    Subb_E, Cmpb_E, Sbcb_E, Addd_E, Andb_E, Bitb_E, Ldb_E,  Stb_E,  Eorb_E, Adcb_E, Orb_E,  Addb_E, Ldd_E,  Std_E,  Ldu_E,  Stu_E,
        };
    }

    private Action[] Page2Vectors()
    {
        /* 0x10__ */
        return new Action[] {
                        /*   _0      _1      _2      _3      _4      _5      _6      _7      _8      _9      _A      _B      _C      _D      _E      _F   */
            /* 0_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 1_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 2_ */    _____,  LBrn_R, LBhi_R, LBls_R, LBhs_R, LBcs_R, LBne_R, LBeq_R, LBvc_R, LBvs_R, LBpl_R, LBmi_R, LBge_R, LBlt_R, LBgt_R, LBle_R,
            /* 3_ */    Addr,   Adcr,   Subr,   Sbcr,   Andr,   Orr,    Eorr,   Cmpr,   Pshsw,  Pulsw,  Pshuw,  Puluw,  _____,  _____,  _____,  Swi2_I,
            /* 4_ */    Negd_I, _____,  _____,  Comd_I, Lsrd_I, _____,  Rord_I, Asrd_I, Asld_I, Rold_I, Decd_I, _____,  Incd_I, Tstd_I, _____,  Clrd_I,
            /* 5_ */    _____,  _____,  _____,  Comw_I, Lsrw_I, _____,  Rorw_I, _____,  _____,  Rolw_I, Decw_I, _____,  Incw_I, Tstw_I, _____,  Clrw_I,
            /* 6_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 7_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 8_ */    Subw_M, Cmpw_M, Sbcd_M, Cmpd_M, Andd_M, Bitd_M, Ldw_M,  _____,  Eord_M, Adcd_M, Ord_M,  Addw_M, Cmpy_M, _____,  Ldy_M,  _____,
            /* 9_ */    Subw_D, Cmpw_D, Sbcd_D, Cmpd_D, Andd_D, Bitd_D, Ldw_D,  Stw_D,  Eord_D, Adcd_D, Ord_D,  Addw_D, Cmpy_D, _____,  Ldy_D,  Sty_D,
            /* A_ */    Subw_X, Cmpw_X, Sbcd_X, Cmpd_X, Andd_X, Bitd_X, Ldw_X,  Stw_X,  Eord_X, Adcd_X, Ord_X,  Addw_X, Cmpy_X, _____,  Ldy_X,  Sty_X,
            /* B_ */    Subw_E, Cmpw_E, Sbcd_E, Cmpd_E, Andd_E, Bitd_E, Ldw_E,  Stw_E,  Eord_E, Adcd_E, Ord_E,  Addw_E, Cmpy_E, _____,  Ldy_E,  Sty_E,
            /* C_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Lds_I,  _____,
            /* D_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Ldq_D,  Stq_D,  Lds_D,  Sts_D,
            /* E_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Ldq_X,  Stq_X,  Lds_X,  Sts_X,
            /* F_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Ldq_E,  Stq_E,  Lds_E,  Sts_E,
        };
    }

    private Action[] Page3Vectors()
    {
        /* 0x11__ */
        return new Action[] {
                     /*   _0      _1      _2      _3      _4      _5      _6      _7      _8      _9      _A      _B      _C      _D      _E      _F   */
            /* 0_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 1_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 2_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 3_ */    Band,   Biand,  Bor,    Bior,   Beor,   Bieor,  Ldbt,   Stbt,   Tfm1,   Tfm2,   Tfm3,   Tfm4,   Bitmd_M,Ldmd_M, _____,  Swi3_I,
            /* 4_ */    _____,  _____,  _____,  Come_I, _____,  _____,  _____,  _____,  _____,  _____,  Dece_I, _____,  Ince_I, Tste_I, _____,  Clre_I,
            /* 5_ */    _____,  _____,  _____,  Comf_I, _____,  _____,  _____,  _____,  _____,  _____,  Decf_I, _____,  Incf_I, Tstf_I, _____,  Clrf_I,
            /* 6_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 7_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 8_ */    Sube_M, Cmpe_M, _____,  Cmpu_M, _____,  _____,  Lde_M,  _____,  _____,  _____,  _____,  Adde_M, Cmps_M, Divd_M, Divq_M, Muld_M,
            /* 9_ */    Sube_D, Cmpe_D, _____,  Cmpu_D, _____,  _____,  Lde_D,  Ste_D,  _____,  _____,  _____,  Adde_D, Cmps_D, Divd_D, Divq_D, Muld_D,
            /* A_ */    Sube_X, Cmpe_X, _____,  Cmpu_X, _____,  _____,  Lde_X,  Ste_X,  _____,  _____,  _____,  Adde_X, Cmps_X, Divd_X, Divq_X, Muld_X,
            /* B_ */    Sube_E, Cmpe_E, _____,  Cmpu_E, _____,  _____,  Lde_E,  Ste_E,  _____,  _____,  _____,  Adde_E, Cmps_E, Divd_E, Divq_E, Muld_E,
            /* C_ */    Subf_M, Cmpf_M, _____,  _____,  _____,  _____,  Ldf_M,  _____,  _____,  _____,  _____,  Addf_M, _____,  _____,  _____,  _____,
            /* D_ */    Subf_D, Cmpf_D, _____,  _____,  _____,  _____,  Ldf_D,  Stf_D,  _____,  _____,  _____,  Addf_D, _____,  _____,  _____,  _____,
            /* E_ */    Subf_X, Cmpf_X, _____,  _____,  _____,  _____,  Ldf_X,  Stf_X,  _____,  _____,  _____,  Addf_X, _____,  _____,  _____,  _____,
            /* F_ */    Subf_E, Cmpf_E, _____,  _____,  _____,  _____,  Ldf_E,  Stf_E,  _____,  _____,  _____,  Addf_E, _____,  _____,  _____,  _____,
        };
    }

    public void Exec(byte opCode)
    {
        _cycleCounter = 0;
        _page1[opCode]();
    }

    public void Exec2(byte opCode)
    {
        _cycleCounter = 0;
        Page_2(opCode);
    }

    public void Exec3(byte opCode)
    {
        _cycleCounter = 0;
        Page_3(opCode);
    }
}
