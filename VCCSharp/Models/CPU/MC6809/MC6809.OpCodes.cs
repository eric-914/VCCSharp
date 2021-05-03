using System;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        private byte _temp8;
        private ushort _temp16;
        private uint _temp32;

        private byte _postByte;
        private ushort _postWord;

        #region Jump Vectors

        private static Action[] _jmpVec1 = new Action[256];
        private static Action[] _jmpVec2 = new Action[256];
        private static Action[] _jmpVec3 = new Action[256];

        private void InitializeJmpVectors()
        {
            _jmpVec1 = new Action[]
            {
                Neg_D, // 0x00
                InvalidInsHandler, //0x01
                InvalidInsHandler, //0x02
                Com_D, // 0x03
                Lsr_D, // 0x04
                InvalidInsHandler, //0x05
                Ror_D, // 0x06
                Asr_D, // 0x07
                Asl_D, // 0x08
                Rol_D, // 0x09
                Dec_D, // 0x0A
                InvalidInsHandler, //0x0B
                Inc_D, // 0x0C
                Tst_D, // 0x0D
                Jmp_D, // 0x0E
                Clr_D, // 0x0F

                Page_2, // 0x10
                Page_3, // 0x11
                Nop_I, // 0x12
                Sync_I, // 0x13
                InvalidInsHandler, // 0x14
                InvalidInsHandler, // 0x15
                Lbra_R, // 0x16
                Lbsr_R, // 0x17
                InvalidInsHandler, // 0x18
                Daa_I, // 0x19
                Orcc_M, // 0x1A
                InvalidInsHandler, // 0x1B
                Andcc_M, // 0x1C
                Sex_I, // 0x1D
                Exg_M, // 0x1E 
                Tfr_M, // 0x1F

                Bra_R, // 0x20
                Brn_R, // 0x21
                Bhi_R, // 0x22
                Bls_R, // 0x23
                Bhs_R, // 0x24
                Blo_R, // 0x25
                Bne_R, // 0x26
                Beq_R, // 0x27
                Bvc_R, // 0x28
                Bvs_R, // 0x29
                Bpl_R, // 0x2A
                Bmi_R, // 0x2B
                Bge_R, // 0x2C
                Blt_R, // 0x2D
                Bgt_R, // 0x2E
                Ble_R, // 0x2F
                
                Leax_X, // 0x30
                Leay_X, // 0x31
                Leas_X, // 0x32
                Leau_X, // 0x33
                Pshs_M, // 0x34
                Puls_M, // 0x35
                Pshu_M, // 0x36
                Pulu_M, // 0x37
                InvalidInsHandler, // 0x38
                Rts_I, // 0x39
                Abx_I, // 0x3A
                Rti_I, // 0x3B
                Cwai_I, // 0x3C
                Mul_I, // 0x3D
                Reset, // 0x3E //Undocumented instruction
                Swi1_I, // 0x3F

                Nega_I, // 0x40 
                InvalidInsHandler, // 0x41
                InvalidInsHandler, // 0x42
                Coma_I, // 0x43
                Lsra_I, // 0x44
                InvalidInsHandler, // 0x45
                Rora_I, // 0x46
                Asra_I, // 0x47
                Asla_I, // 0x48
                Rola_I, // 0x49
                Deca_I, // 0x4A
                InvalidInsHandler, // 0x4B
                Inca_I, // 0x4C
                Tsta_I, // 0x4D
                InvalidInsHandler, // 0x4E
                Clra_I, // 0x4F

                Negb_I, // 0x50 
                InvalidInsHandler, // 0x51
                InvalidInsHandler, // 0x52
                Comb_I, // 0x53
                Lsrb_I, // 0x54
                InvalidInsHandler, // 0x55
                Rorb_I, // 0x56
                Asrb_I, // 0x57
                Aslb_I, // 0x58
                Rolb_I, // 0x59
                Decb_I, // 0x5A
                InvalidInsHandler, // 0x5B
                Incb_I, // 0x5C
                Tstb_I, // 0x5D
                InvalidInsHandler, // 5E
                Clrb_I, // 0x5F

                Neg_X, //  0x60
                InvalidInsHandler, // 0x61
                InvalidInsHandler, // 0x62
                Com_X, //  0x63
                Lsr_X, //  0x64
                InvalidInsHandler, // 0x65
                Ror_X, //  0x66
                Asr_X, //  0x67
                Asl_X, //  0x68
                Rol_X, //  0x69
                Dec_X, //  0x6A
                InvalidInsHandler, // 0x6B
                Inc_X, //  0x6C
                Tst_X, //  0x6D
                Jmp_X, //  0x6E
                Clr_X, //  0x6F

                Neg_E, // 0x70
                InvalidInsHandler, // 0x71
                InvalidInsHandler, // 0x72
                Com_E, // 0x73
                Lsr_E, // 0x74
                InvalidInsHandler, // 0x75
                Ror_E, // 0x76
                Asr_E, // 0x77
                Asl_E, // 0x78
                Rol_E, // 0x79
                Dec_E, // 0x7A
                InvalidInsHandler, // 0x7B
                Inc_E, // 0x7C
                Tst_E, // 0x7D
                Jmp_E, // 0x7E
                Clr_E, // 0x7F

                Suba_M, // 0x80
                Cmpa_M, // 0x81
                Sbca_M, // 0x82
                Subd_M, // 0x83
                Anda_M, // 0x84
                Bita_M, // 0x85
                Lda_M, // 0x86
                InvalidInsHandler, // 0x87
                Eora_M, // 0x88
                Adca_M, // 0x89
                Ora_M, // 0x8A
                Adda_M, // 0x8B
                Cmpx_M, // 0x8C
                Bsr_R, // 0x8D
                Ldx_M, // 0x8E
                InvalidInsHandler, // 8F

                Suba_D, //0x90
                Cmpa_D, //0x91
                Scba_D, //0x92
                Subd_D, //0x93
                Anda_D, //0x94
                Bita_D, //0x95
                Lda_D, //0x96
                Sta_D, //0x97
                Eora_D, //0x98
                Adca_D, //0x99
                Ora_D, //0x9A
                Adda_D, //0x9B
                Cmpx_D, //0x9C
                Jsr_D, //0x9D   //BSR_D  //Branch or Jump to Subroutine?
                Ldx_D, //0x9E
                Stx_D, //0x9F

                Suba_X, // 0xA0
                Cmpa_X, // 0xA1
                Sbca_X, // 0xA2
                Subd_X, // 0xA3
                Anda_X, // 0xA4
                Bita_X, // 0xA5
                Lda_X, // 0xA6
                Sta_X, // 0xA7
                Eora_X, // 0xA8
                Adca_X, // 0xA9
                Ora_X, // 0xAA
                Adda_X, // 0xAB
                Cmpx_X, // 0xAC
                Jsr_X, // 0xAD  //BSR_X  //Another Jump or Branch?
                Ldx_X, // 0xAE
                Stx_X, // 0xAF

                Suba_E, // 0xB0
                Cmpa_E, // 0xB1
                Sbca_E, // 0xB2
                Subd_E, // 0xB3
                Anda_E, // 0xB4
                Bita_E, // 0xB5
                Lda_E, // 0xB6
                Sta_E, // 0xB7
                Eora_E, // 0xB8
                Adca_E, // 0xB9
                Ora_E, // 0xBA
                Adda_E, // 0xBB
                Cmpx_E, // 0xBC
                Bsr_E, // 0xBD
                Ldx_E, // 0xBE
                Stx_E, // 0xBF

                Subb_M, // 0xC0
                Cmpb_M, // 0xC1
                Sbcb_M, // 0xC2
                Addd_M, // 0xC3
                Andb_M, // 0xC4
                Bitb_M, // 0xC5
                Ldb_M, // 0xC6
                InvalidInsHandler, // C7
                Eorb_M, // 0xC8
                Adcb_M, // 0xC9
                Orb_M, // 0xCA
                Addb_M, // 0xCB
                Ldd_M, // 0xCC
                InvalidInsHandler, // 0xCD
                Ldu_M, // 0xCE
                InvalidInsHandler, // CF

                Subb_D, // 0xD0
                Cmpb_D, // 0xD1
                Sbcb_D, // 0xD2
                Addd_D, // 0xD3
                Andb_D, // 0xD4
                Bitb_D, // 0xD5
                Ldb_D, // 0xD6
                Stb_D, // 0XD7
                Eorb_D, // 0xD8
                Adcb_D, // 0xD9
                Orb_D, // 0xDA
                Addb_D, // 0xDB
                Ldd_D, // 0xDC
                Std_D, // 0xDD
                Ldu_D, // 0xDE
                Stu_D, // 0xDF

                Subb_X, // 0xE0
                Cmpb_X, // 0xE1
                Sbcb_X, // 0xE2
                Addd_X, // 0xE3
                Andb_X, // 0xE4
                Bitb_X, // 0xE5
                Ldb_X, // 0xE6
                Stb_X, // 0xE7
                Eorb_X, // 0xE8
                Adcb_X, // 0xE9
                Orb_X, // 0xEA
                Addb_X, // 0xEB
                Ldd_X, // 0xEC
                Std_X, // 0xED
                Ldu_X, // 0xEE
                Stu_X, // 0xEF

                Subb_E, // 0xF0
                Cmpb_E, // 0xF1
                Sbcb_E, // 0xF2
                Addd_E, // 0xF3
                Andb_E, // 0xF4
                Bitb_E, // 0xF5
                Ldb_E, // 0xF6
                Stb_E, // 0xF7
                Eorb_E, // 0xF8
                Adcb_E, // 0xF9
                Orb_E, // 0xFA
                Addb_E, // 0xFB
                Ldd_E, // 0xFC
                Std_E, // 0xFD
                Ldu_E, // 0xFE
                Stu_E  // 0xFF
            };
        }

        #endregion

        public void Exec(byte opCode)
        {
            //Library.MC6809.MC6809ExecOpCode(_gCycleFor, opCode);
            _jmpVec1[opCode]();
        }

        public void LIB(byte opCode)
        {
            Library.MC6809.MC6809ExecOpCode(_gCycleFor, opCode);
        }

        public void InvalidInsHandler()
        {
        }
    }
}
