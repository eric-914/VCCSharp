namespace VCCSharp.Models.CPU.MC6809;

// ReSharper disable once InconsistentNaming
public partial class MC6809
{
    #region 0x00 - 0x0F

    public void Neg_D() => Run(0x00);
    // 01
    // 02
    public void Com_D() => Run(0x03);
    public void Lsr_D() => Run(0x04);
    // 05
    public void Ror_D() => Run(0x06);

    public void Asr_D() => Run(0x07);
    public void Asl_D() => Run(0x08);
    public void Rol_D() => Run(0x09);
    public void Dec_D() => Run(0x0A);
    // 0B
    public void Inc_D() => Run(0x0C);
    public void Tst_D() => Run(0x0D);
    public void Jmp_D() => Run(0x0E);
    public void Clr_D() => Run(0x0F);

    #endregion

    #region 0x10 - 0x1F

    public void Page_2() // 10
    {
        byte opCode = MemRead8(PC_REG++);

        _jumpVectors0x10[opCode]();
    }

    public void Page_3() // 11
    {
        byte opCode = MemRead8(PC_REG++);

        _jumpVectors0x11[opCode]();
    }

    public void Nop_I() => Run(0x12);

    public void Sync_I() // 13
    {
        _cycleCounter = SyncWait();
    }

    // 14
    // 15
    public void Lbra_R() => Run(0x16);
    public void Lbsr_R() => Run(0x17);
    // 18

    public void Daa_I() => Run(0x19);
    public void Orcc_M() => Run(0x1A);
    // 1B
    public void Andcc_M() => Run(0x1C);
    public void Sex_I() => Run(0x1D);
    public void Exg_M() => Run(0x1E);
    public void Tfr_M() => Run(0x1F);

    #endregion

    #region 0x20 - 0x2F

    public void Bra_R() => Run(0x20);
    public void Brn_R() => Run(0x21);
    public void Bhi_R() => Run(0x22);
    public void Bls_R() => Run(0x23);
    public void Bhs_R() => Run(0x24);
    public void Blo_R() => Run(0x25);
    public void Bne_R() => Run(0x26);
    public void Beq_R() => Run(0x27);
    public void Bvc_R() => Run(0x28);
    public void Bvs_R() => Run(0x29);
    public void Bpl_R() => Run(0x2A);
    public void Bmi_R() => Run(0x2B);
    public void Bge_R() => Run(0x2C);
    public void Blt_R() => Run(0x2D);
    public void Bgt_R() => Run(0x2E);
    public void Ble_R() => Run(0x2F);

    #endregion

    #region 0x30 - 0x3F

    public void Leax_X() => Run(0x30);
    public void Leay_X() => Run(0x31);
    public void Leas_X() => Run(0x32);
    public void Leau_X() => Run(0x33);
    public void Pshs_M() => Run(0x34);
    public void Puls_M() => Run(0x35);
    public void Pshu_M() => Run(0x36);
    public void Pulu_M() => Run(0x37);
    // 38
    public void Rts_I() => Run(0x39);
    public void Abx_I() => Run(0x3A);
    public void Rti_I() => Run(0x3B);
    public void Cwai_I() => Run(0x3C);
    public void Mul_I() => Run(0x3D);
    // 3E   //RESET - Undocumented
    public void Swi1_I() => Run(0x3F);

    #endregion

    #region 0x40 - 0x4F

    public void Nega_I() => Run(0x40);
    // 41
    // 42
    public void Coma_I() => Run(0x43);
    public void Lsra_I() => Run(0x44);
    // 45
    public void Rora_I() => Run(0x46);
    public void Asra_I() => Run(0x47);
    public void Asla_I() => Run(0x48);
    public void Rola_I() => Run(0x49);
    public void Deca_I() => Run(0x4A);
    // 4B
    public void Inca_I() => Run(0x4C);
    public void Tsta_I() => Run(0x4D);
    // 4E
    public void Clra_I() => Run(0x4F);

    #endregion

    #region 0x50 - 0x5F

    public void Negb_I() => Run(0x50);
    // 51
    // 52
    public void Comb_I() => Run(0x53);
    public void Lsrb_I() => Run(0x54);
    // 55
    public void Rorb_I() => Run(0x56);
    public void Asrb_I() => Run(0x57);
    public void Aslb_I() => Run(0x58);
    public void Rolb_I() => Run(0x59);
    public void Decb_I() => Run(0x5A);
    // 5B
    public void Incb_I() => Run(0x5C);
    public void Tstb_I() => Run(0x5D);
    // 5E
    public void Clrb_I() => Run(0x5F);

    #endregion

    #region 0x60 - 0x6F

    public void Neg_X() => Run(0x60);
    // 61
    // 62
    public void Com_X() => Run(0x63);
    public void Lsr_X() => Run(0x64);
    // 65
    public void Ror_X() => Run(0x66);
    public void Asr_X() => Run(0x67);
    public void Asl_X() => Run(0x68);
    public void Rol_X() => Run(0x69);
    public void Dec_X() => Run(0x6A);
    // 6B
    public void Inc_X() => Run(0x6C);
    public void Tst_X() => Run(0x6D);
    public void Jmp_X() => Run(0x6E);
    public void Clr_X() => Run(0x6F);

    #endregion

    #region 0x70 - 0x7F

    public void Neg_E() => Run(0x70);
    // 71
    // 72
    public void Com_E() => Run(0x73);
    public void Lsr_E() => Run(0x74);
    // 75
    public void Ror_E() => Run(0x76);
    public void Asr_E() => Run(0x77);
    public void Asl_E() => Run(0x78);
    public void Rol_E() => Run(0x79);
    public void Dec_E() => Run(0x7A);
    // 7B
    public void Inc_E() => Run(0x7C);
    public void Tst_E() => Run(0x7D);
    public void Jmp_E() => Run(0x7E);
    public void Clr_E() => Run(0x7F);

    #endregion

    #region 0x80 - 0x8F

    public void Suba_M() => Run(0x80);
    public void Cmpa_M() => Run(0x81);
    public void Sbca_M() => Run(0x82);
    public void Subd_M() => Run(0x83);
    public void Anda_M() => Run(0x84);
    public void Bita_M() => Run(0x85);
    public void Lda_M() => Run(0x86);
    // 87
    public void Eora_M() => Run(0x88);
    public void Adca_M() => Run(0x89);
    public void Ora_M() => Run(0x8A);
    public void Adda_M() => Run(0x8B);
    public void Cmpx_M() => Run(0x8C);
    public void Bsr_R() => Run(0x8D);
    public void Ldx_M() => Run(0x8E);
    // 8F

    #endregion

    #region 0x90 - 0x9F

    public void Suba_D() => Run(0x90);
    public void Cmpa_D() => Run(0x91);
    public void Scba_D() => Run(0x92);
    public void Subd_D() => Run(0x93);
    public void Anda_D() => Run(0x94);
    public void Bita_D() => Run(0x95);
    public void Lda_D() => Run(0x96);
    public void Sta_D() => Run(0x97);
    public void Eora_D() => Run(0x98);
    public void Adca_D() => Run(0x99);
    public void Ora_D() => Run(0x9A);
    public void Adda_D() => Run(0x9B);
    public void Cmpx_D() => Run(0x9C);
    public void Jsr_D() => Run(0x9D);
    public void Ldx_D() => Run(0x9E);
    public void Stx_D() => Run(0x9F);

    #endregion

    #region 0xA0 - 0xAF

    public void Suba_X() => Run(0xA0);
    public void Cmpa_X() => Run(0xA1);
    public void Sbca_X() => Run(0xA2);
    public void Subd_X() => Run(0xA3);
    public void Anda_X() => Run(0xA4);
    public void Bita_X() => Run(0xA5);
    public void Lda_X() => Run(0xA6);
    public void Sta_X() => Run(0xA7);
    public void Eora_X() => Run(0xA8);
    public void Adca_X() => Run(0xA9);
    public void Ora_X() => Run(0xAA);
    public void Adda_X() => Run(0xAB);
    public void Cmpx_X() => Run(0xAC);
    public void Jsr_X() => Run(0xAD);
    public void Ldx_X() => Run(0xAE);
    public void Stx_X() => Run(0xAF);

    #endregion

    #region 0xB0 - 0xBF

    public void Suba_E() => Run(0xB0);
    public void Cmpa_E() => Run(0xB1);
    public void Sbca_E() => Run(0xB2);
    public void Subd_E() => Run(0xB3);
    public void Anda_E() => Run(0xB4);
    public void Bita_E() => Run(0xB5);
    public void Lda_E() => Run(0xB6);
    public void Sta_E() => Run(0xB7);
    public void Eora_E() => Run(0xB8);
    public void Adca_E() => Run(0xB9);
    public void Ora_E() => Run(0xBA);
    public void Adda_E() => Run(0xBB);
    public void Cmpx_E() => Run(0xBC);
    public void Jsr_E() => Run(0xBD);
    public void Ldx_E() => Run(0xBE);
    public void Stx_E() => Run(0xBF);

    #endregion

    #region 0xC0 - 0CF

    public void Subb_M() => Run(0xC0);
    public void Cmpb_M() => Run(0xC1);
    public void Sbcb_M() => Run(0xC2);
    public void Addd_M() => Run(0xC3);
    public void Andb_M() => Run(0xC4);
    public void Bitb_M() => Run(0xC5);
    public void Ldb_M() => Run(0xC6);
    // C7
    public void Eorb_M() => Run(0xC8);
    public void Adcb_M() => Run(0xC9);
    public void Orb_M() => Run(0xCA);
    public void Addb_M() => Run(0xCB);
    public void Ldd_M() => Run(0xCC);
    // CD
    public void Ldu_M() => Run(0xCE);
    // CF

    #endregion

    #region 0xD0 - 0xDF

    public void Subb_D() => Run(0xD0);
    public void Cmpb_D() => Run(0xD1);
    public void Sbcb_D() => Run(0xD2);
    public void Addd_D() => Run(0xD3);
    public void Andb_D() => Run(0xD4);
    public void Bitb_D() => Run(0xD5);
    public void Ldb_D() => Run(0xD6);
    public void Stb_D() => Run(0xD7);
    public void Eorb_D() => Run(0xD8);
    public void Adcb_D() => Run(0xD9);
    public void Orb_D() => Run(0xDA);
    public void Addb_D() => Run(0xDB);
    public void Ldd_D() => Run(0xDC);
    public void Std_D() => Run(0xDD);
    public void Ldu_D() => Run(0xDE);
    public void Stu_D() => Run(0xDF);

    #endregion

    #region 0xE0 - 0xEF

    public void Subb_X() => Run(0xE0);
    public void Cmpb_X() => Run(0xE1);
    public void Sbcb_X() => Run(0xE2);
    public void Addd_X() => Run(0xE3);
    public void Andb_X() => Run(0xE4);
    public void Bitb_X() => Run(0xE5);
    public void Ldb_X() => Run(0xE6);
    public void Stb_X() => Run(0xE7);
    public void Eorb_X() => Run(0xE8);
    public void Adcb_X() => Run(0xE9);
    public void Orb_X() => Run(0xEA);
    public void Addb_X() => Run(0xEB);
    public void Ldd_X() => Run(0xEC);
    public void Std_X() => Run(0xED);
    public void Ldu_X() => Run(0xEE);
    public void Stu_X() => Run(0xEF);

    #endregion

    #region 0xF0 - 0xFF

    public void Subb_E() => Run(0xF0);
    public void Cmpb_E() => Run(0xF1);
    public void Sbcb_E() => Run(0xF2);
    public void Addd_E() => Run(0xF3);
    public void Andb_E() => Run(0xF4);
    public void Bitb_E() => Run(0xF5);
    public void Ldb_E() => Run(0xF6);
    public void Stb_E() => Run(0xF7);
    public void Eorb_E() => Run(0xF8);
    public void Adcb_E() => Run(0xF9);
    public void Orb_E() => Run(0xFA);
    public void Addb_E() => Run(0xFB);
    public void Ldd_E() => Run(0xFC);
    public void Std_E() => Run(0xFD);
    public void Ldu_E() => Run(0xFE);
    public void Stu_E() => Run(0xFF);

    #endregion
}
