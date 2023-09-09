namespace VCCSharp.OpCodes.Tests.Model.MC6809.O;

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

        return new Action[]
        {
                     /*   _0      _1      _2      _3      _4      _5      _6      _7      _8      _9      _A      _B      _C      _D      _E      _F   */
            /* 0_ */    Neg_D,  _____,  _____,  Com_D,  Lsr_D,  _____,  Ror_D,  Asr_D,  Asl_D,  Rol_D,  Dec_D,  _____,  Inc_D,  Tst_D,  Jmp_D,  Clr_D,
            /* 1_ */    Page_2, Page_3, Nop_I,  Sync_I, _____,  _____,  Lbra_R, Lbsr_R, _____,  Daa_I,  Orcc_M, _____,  Andcc_M,Sex_I,  Exg_M,  Tfr_M,
            /* 2_ */    Bra_R,  Brn_R,  Bhi_R,  Bls_R,  Bhs_R,  Blo_R,  Bne_R,  Beq_R,  Bvc_R,  Bvs_R,  Bpl_R,  Bmi_R,  Bge_R,  Blt_R,  Bgt_R,  Ble_R,
            /* 3_ */    Leax_X, Leay_X, Leas_X, Leau_X, Pshs_M, Puls_M, Pshu_M, Pulu_M, _____,  Rts_I,  Abx_I,  Rti_I,  Cwai_I, Mul_I,  Reset,  Swi1_I,
            /* 4_ */    Nega_I, _____,  _____,  Coma_I, Lsra_I, _____,  Rora_I, Asra_I, Asla_I, Rola_I, Deca_I, _____,  Inca_I, Tsta_I, _____,  Clra_I,
            /* 5_ */    Negb_I, _____,  _____,  Comb_I, Lsrb_I, _____,  Rorb_I, Asrb_I, Aslb_I, Rolb_I, Decb_I, _____,  Incb_I, Tstb_I, _____,  Clrb_I,
            /* 6_ */    Neg_X,  _____,  _____,  Com_X,  Lsr_X,  _____,  Ror_X,  Asr_X,  Asl_X,  Rol_X,  Dec_X,  _____,  Inc_X,  Tst_X,  Jmp_X,  Clr_X,
            /* 7_ */    Neg_E,  _____,  _____,  Com_E,  Lsr_E,  _____,  Ror_E,  Asr_E,  Asl_E,  Rol_E,  Dec_E,  _____,  Inc_E,  Tst_E,  Jmp_E,  Clr_E,
            /* 8_ */    Suba_M, Cmpa_M, Sbca_M, Subd_M, Anda_M, Bita_M, Lda_M,  _____,  Eora_M, Adca_M, Ora_M,  Adda_M, Cmpx_M, Bsr_R,  Ldx_M,  _____,
            /* 9_ */    Suba_D, Cmpa_D, Scba_D, Subd_D, Anda_D, Bita_D, Lda_D,  Sta_D,  Eora_D, Adca_D, Ora_D,  Adda_D, Cmpx_D, Jsr_D,  Ldx_D,  Stx_D,
            /* A_ */    Suba_X, Cmpa_X, Sbca_X, Subd_X, Anda_X, Bita_X, Lda_X,  Sta_X,  Eora_X, Adca_X, Ora_X,  Adda_X, Cmpx_X, Jsr_X,  Ldx_X,  Stx_X,
            /* B_ */    Suba_E, Cmpa_E, Sbca_E, Subd_E, Anda_E, Bita_E, Lda_E,  Sta_E,  Eora_E, Adca_E, Ora_E,  Adda_E, Cmpx_E, Jsr_E,  Ldx_E,  Stx_E,
            /* C_ */    Subb_M, Cmpb_M, Sbcb_M, Addd_M, Andb_M, Bitb_M, Ldb_M,  _____,  Eorb_M, Adcb_M, Orb_M,  Addb_M, Ldd_M,  _____,  Ldu_M,  _____,
            /* D_ */    Subb_D, Cmpb_D, Sbcb_D, Addd_D, Andb_D, Bitb_D, Ldb_D,  Stb_D,  Eorb_D, Adcb_D, Orb_D,  Addb_D, Ldd_D,  Std_D,  Ldu_D,  Stu_D,
            /* E_ */    Subb_X, Cmpb_X, Sbcb_X, Addd_X, Andb_X, Bitb_X, Ldb_X,  Stb_X,  Eorb_X, Adcb_X, Orb_X,  Addb_X, Ldd_X,  Std_X,  Ldu_X,  Stu_X,
            /* F_ */    Subb_E, Cmpb_E, Sbcb_E, Addd_E, Andb_E, Bitb_E, Ldb_E,  Stb_E,  Eorb_E, Adcb_E, Orb_E,  Addb_E, Ldd_E,  Std_E,  Ldu_E,  Stu_E
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
            /* 3_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Swi2_I,
            /* 4_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 5_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 6_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 7_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 8_ */    _____,  _____,  _____,  Cmpd_M, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmpy_M, _____,  Ldy_M,  _____,
            /* 9_ */    _____,  _____,  _____,  Cmpd_D, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmpy_D, _____,  Ldy_D,  Sty_D,
            /* A_ */    _____,  _____,  _____,  Cmpd_X, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmpy_X, _____,  Ldy_X,  Sty_X,
            /* B_ */    _____,  _____,  _____,  Cmpd_E, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmpy_E, _____,  Ldy_E,  Sty_E,
            /* C_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Lds_I,  _____,
            /* D_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Lds_D,  Sts_D,
            /* E_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Lds_X,  Sts_X,
            /* F_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Lds_E,  Sts_E,
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
            /* 3_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Swi3_I,
            /* 4_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 5_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 6_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 7_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* 8_ */    _____,  _____,  _____,  Cmpu_M, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmps_M, _____,  _____,  _____,
            /* 9_ */    _____,  _____,  _____,  Cmpu_D, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmps_D, _____,  _____,  _____,
            /* A_ */    _____,  _____,  _____,  Cmpu_X, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmps_X, _____,  _____,  _____,
            /* B_ */    _____,  _____,  _____,  Cmpu_E, _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  Cmps_E, _____,  _____,  _____,
            /* C_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* D_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* E_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
            /* F_ */    _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,  _____,
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
