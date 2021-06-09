using System;
using VCCSharp.Modules.TC1014.Modes;

namespace VCCSharp.Modules.TC1014
{
    public delegate void Mode(ModeModel model, int start, int yStride);

    // ReSharper disable once InconsistentNaming
    public partial class TC1014
    {
        public Mode[] Modes;

        private void InitializeModes()
        {
            Modes = new Mode[]
            {
                Mode0,          //00
                Mode1_2,        //01
                Mode1_2,        //02
                NA,             //03
                NA,             //04
                NA,             //05
                NA,             //06
                NA,             //07
                NA,             //08
                NA,             //09
                NA,             //0A
                NA,             //0B
                NA,             //0C
                NA,             //0D
                NA,             //0E
                NA,             //0F
                NA,             //10
                NA,             //11
                NA,             //12
                NA,             //13
                NA,             //14
                NA,             //15
                NA,             //16
                NA,             //17
                NA,             //18
                NA,             //19
                NA,             //1A
                NA,             //1B
                NA,             //1C
                NA,             //1D
                NA,             //1E
                NA,             //1F
                NA,             //20
                NA,             //21
                NA,             //22
                NA,             //23
                NA,             //24
                NA,             //25
                NA,             //26
                NA,             //27
                NA,             //28
                NA,             //29
                NA,             //2A
                NA,             //2B
                NA,             //2C
                NA,             //2D
                NA,             //2E
                NA,             //2F
                NA,             //30
                NA,             //31
                NA,             //32
                NA,             //33
                NA,             //34
                NA,             //35
                NA,             //36
                NA,             //37
                NA,             //38
                NA,             //39
                NA,             //3A
                NA,             //3B
                NA,             //3C
                NA,             //3D
                NA,             //3E
                NA,             //3F
                Mode64_127,     //40
                Mode64_127,     //41
                Mode64_127,     //42
                Mode64_127,     //43
                Mode64_127,     //44
                Mode64_127,     //45
                Mode64_127,     //46
                Mode64_127,     //47
                Mode64_127,     //48
                Mode64_127,     //49
                Mode64_127,     //4A
                Mode64_127,     //4B
                Mode64_127,     //4C
                Mode64_127,     //4D
                Mode64_127,     //4E
                Mode64_127,     //4F
                Mode64_127,     //50
                Mode64_127,     //51
                Mode64_127,     //52
                Mode64_127,     //53
                Mode64_127,     //54
                Mode64_127,     //55
                Mode64_127,     //56
                Mode64_127,     //57
                Mode64_127,     //58
                Mode64_127,     //59
                Mode64_127,     //5A
                Mode64_127,     //5B
                Mode64_127,     //5C
                Mode64_127,     //5D
                Mode64_127,     //5E
                Mode64_127,     //5F
                Mode64_127,     //60
                Mode64_127,     //61
                Mode64_127,     //62
                Mode64_127,     //63
                Mode64_127,     //64
                Mode64_127,     //65
                Mode64_127,     //66
                Mode64_127,     //67
                Mode64_127,     //68
                Mode64_127,     //69
                Mode64_127,     //6A
                Mode64_127,     //6B
                Mode64_127,     //6C
                Mode64_127,     //6D
                Mode64_127,     //6E
                Mode64_127,     //6F
                Mode64_127,     //70
                Mode64_127,     //71
                Mode64_127,     //72
                Mode64_127,     //73
                Mode64_127,     //74
                Mode64_127,     //75
                Mode64_127,     //76
                Mode64_127,     //77
                Mode64_127,     //78
                Mode64_127,     //79
                Mode64_127,     //7A
                Mode64_127,     //7B
                Mode64_127,     //7C
                Mode64_127,     //7D
                Mode64_127,     //7E
                Mode64_127,     //7F
                Mode128_0,      //80
                Mode128_1_2,    //81
                Mode128_1_2,    //82
                Mode128_3_6,    //83
                Mode128_3_6,    //84
                Mode128_3_6,    //85
                Mode128_3_6,    //86
                Mode128_7_14,   //87
                Mode128_7_14,   //88
                Mode128_7_14,   //89
                Mode128_7_14,   //8A
                Mode128_7_14,   //8B
                Mode128_7_14,   //8C
                Mode128_7_14,   //8D
                Mode128_7_14,   //8E
                Mode128_15_16,  //8F
                Mode128_15_16,  //90
                Mode128_17_18,  //91
                Mode128_17_18,  //92
                Mode128_19_22,  //93
                Mode128_19_22,  //94
                Mode128_19_22,  //95
                Mode128_19_22,  //96
                Mode128_23_30,  //97
                Mode128_23_30,  //98
                Mode128_23_30,  //99
                Mode128_23_30,  //9A
                Mode128_23_30,  //9B
                Mode128_23_30,  //9C
                Mode128_23_30,  //9D
                Mode128_23_30,  //9E
                Mode128_31,     //9F
                Mode128_32,     //A0
                Mode128_33_34,  //A1
                Mode128_33_34,  //A2
                Mode128_35_38,  //A3
                Mode128_35_38,  //A4
                Mode128_35_38,  //A5
                Mode128_35_38,  //A6
                Mode128_39_46,  //A7
                Mode128_39_46,  //A8
                Mode128_39_46,  //A9
                Mode128_39_46,  //AA
                Mode128_39_46,  //AB
                Mode128_39_46,  //AC
                Mode128_39_46,  //AD
                Mode128_39_46,  //AE
                Mode128_47,     //AF
                NA,             //B0
                NA,             //B1
                NA,             //B2
                NA,             //B3
                NA,             //B4
                NA,             //B5
                NA,             //B6
                NA,             //B7
                NA,             //B8
                NA,             //B9
                NA,             //BA
                NA,             //BB
                NA,             //BC
                NA,             //BD
                NA,             //BE
                NA,             //BF
                Mode192_0,      //C0
                Mode192_1_2,    //C1
                Mode192_1_2,    //C2
                Mode192_3_6,    //C3
                Mode192_3_6,    //C4
                Mode192_3_6,    //C5
                Mode192_3_6,    //C6
                Mode192_7_14,   //C7
                Mode192_7_14,   //C8
                Mode192_7_14,   //C9
                Mode192_7_14,   //CA
                Mode192_7_14,   //CB
                Mode192_7_14,   //CC
                Mode192_7_14,   //CD
                Mode192_7_14,   //CE
                Mode192_15_16,  //CF
                Mode192_15_16,  //D0
                Mode192_17_18,  //D1
                Mode192_17_18,  //D2
                Mode192_19_22,  //D3
                Mode192_19_22,  //D4
                Mode192_19_22,  //D5
                Mode192_19_22,  //D6
                Mode192_23_30,  //D7
                Mode192_23_30,  //D8
                Mode192_23_30,  //D9
                Mode192_23_30,  //DA
                Mode192_23_30,  //DB
                Mode192_23_30,  //DC
                Mode192_23_30,  //DD
                Mode192_23_30,  //DE
                Mode192_31,     //DF
                NA,             //E0
                NA,             //E1
                NA,             //E2
                NA,             //E3
                NA,             //E4
                NA,             //E5
                NA,             //E6
                NA,             //E7
                NA,             //E8
                NA,             //E9
                NA,             //EA
                NA,             //EB
                NA,             //EC
                NA,             //ED
                NA,             //EE
                NA,             //EF
                NA,             //F0
                NA,             //F1
                NA,             //F2
                NA,             //F3
                NA,             //F4
                NA,             //F5
                NA,             //F6
                NA,             //F7
                NA,             //F8
                NA,             //F9
                NA,             //FA
                NA,             //FB
                NA,             //FC
                NA,             //FD
                NA,             //FE
                NA              //FF
            };
        }

        private static void NA(ModeModel model, int start, int yStride) { throw new NotImplementedException(); }

        public unsafe void SwitchMasterMode32(byte masterMode, int start, int yStride)
        {
            var model = new ModeModel
            {
                Memory = Memory,
                Modules = _modules
            };

            // (GraphicsMode <<7) | (CompatMode<<6)  | ((Bpp & 3)<<4) | (Stretch & 15);
            Modes[masterMode](model, start, yStride);
        }
    }
}
