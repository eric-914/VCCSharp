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
                _0.Mode,          //00
                _1_2.Mode,        //01
                _1_2.Mode,        //02
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
                _64_127.Mode,     //40
                _64_127.Mode,     //41
                _64_127.Mode,     //42
                _64_127.Mode,     //43
                _64_127.Mode,     //44
                _64_127.Mode,     //45
                _64_127.Mode,     //46
                _64_127.Mode,     //47
                _64_127.Mode,     //48
                _64_127.Mode,     //49
                _64_127.Mode,     //4A
                _64_127.Mode,     //4B
                _64_127.Mode,     //4C
                _64_127.Mode,     //4D
                _64_127.Mode,     //4E
                _64_127.Mode,     //4F
                _64_127.Mode,     //50
                _64_127.Mode,     //51
                _64_127.Mode,     //52
                _64_127.Mode,     //53
                _64_127.Mode,     //54
                _64_127.Mode,     //55
                _64_127.Mode,     //56
                _64_127.Mode,     //57
                _64_127.Mode,     //58
                _64_127.Mode,     //59
                _64_127.Mode,     //5A
                _64_127.Mode,     //5B
                _64_127.Mode,     //5C
                _64_127.Mode,     //5D
                _64_127.Mode,     //5E
                _64_127.Mode,     //5F
                _64_127.Mode,     //60
                _64_127.Mode,     //61
                _64_127.Mode,     //62
                _64_127.Mode,     //63
                _64_127.Mode,     //64
                _64_127.Mode,     //65
                _64_127.Mode,     //66
                _64_127.Mode,     //67
                _64_127.Mode,     //68
                _64_127.Mode,     //69
                _64_127.Mode,     //6A
                _64_127.Mode,     //6B
                _64_127.Mode,     //6C
                _64_127.Mode,     //6D
                _64_127.Mode,     //6E
                _64_127.Mode,     //6F
                _64_127.Mode,     //70
                _64_127.Mode,     //71
                _64_127.Mode,     //72
                _64_127.Mode,     //73
                _64_127.Mode,     //74
                _64_127.Mode,     //75
                _64_127.Mode,     //76
                _64_127.Mode,     //77
                _64_127.Mode,     //78
                _64_127.Mode,     //79
                _64_127.Mode,     //7A
                _64_127.Mode,     //7B
                _64_127.Mode,     //7C
                _64_127.Mode,     //7D
                _64_127.Mode,     //7E
                _64_127.Mode,     //7F
                _128_0.Mode,      //80
                _128_1_2.Mode,    //81
                _128_1_2.Mode,    //82
                _128_3_6.Mode,    //83
                _128_3_6.Mode,    //84
                _128_3_6.Mode,    //85
                _128_3_6.Mode,    //86
                _128_7_14.Mode,   //87
                _128_7_14.Mode,   //88
                _128_7_14.Mode,   //89
                _128_7_14.Mode,   //8A
                _128_7_14.Mode,   //8B
                _128_7_14.Mode,   //8C
                _128_7_14.Mode,   //8D
                _128_7_14.Mode,   //8E
                _128_15_16.Mode,  //8F
                _128_15_16.Mode,  //90
                _128_17_18.Mode,  //91
                _128_17_18.Mode,  //92
                _128_19_22.Mode,  //93
                _128_19_22.Mode,  //94
                _128_19_22.Mode,  //95
                _128_19_22.Mode,  //96
                _128_23_30.Mode,  //97
                _128_23_30.Mode,  //98
                _128_23_30.Mode,  //99
                _128_23_30.Mode,  //9A
                _128_23_30.Mode,  //9B
                _128_23_30.Mode,  //9C
                _128_23_30.Mode,  //9D
                _128_23_30.Mode,  //9E
                _128_31.Mode,     //9F
                _128_32.Mode,     //A0
                _128_33_34.Mode,  //A1
                _128_33_34.Mode,  //A2
                _128_35_38.Mode,  //A3
                _128_35_38.Mode,  //A4
                _128_35_38.Mode,  //A5
                _128_35_38.Mode,  //A6
                _128_39_46.Mode,  //A7
                _128_39_46.Mode,  //A8
                _128_39_46.Mode,  //A9
                _128_39_46.Mode,  //AA
                _128_39_46.Mode,  //AB
                _128_39_46.Mode,  //AC
                _128_39_46.Mode,  //AD
                _128_39_46.Mode,  //AE
                _128_47.Mode,     //AF
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
                _192_0.Mode,      //C0
                _192_1_2.Mode,    //C1
                _192_1_2.Mode,    //C2
                _192_3_6.Mode,    //C3
                _192_3_6.Mode,    //C4
                _192_3_6.Mode,    //C5
                _192_3_6.Mode,    //C6
                _192_7_14.Mode,   //C7
                _192_7_14.Mode,   //C8
                _192_7_14.Mode,   //C9
                _192_7_14.Mode,   //CA
                _192_7_14.Mode,   //CB
                _192_7_14.Mode,   //CC
                _192_7_14.Mode,   //CD
                _192_7_14.Mode,   //CE
                _192_15_16.Mode,  //CF
                _192_15_16.Mode,  //D0
                _192_17_18.Mode,  //D1
                _192_17_18.Mode,  //D2
                _192_19_22.Mode,  //D3
                _192_19_22.Mode,  //D4
                _192_19_22.Mode,  //D5
                _192_19_22.Mode,  //D6
                _192_23_30.Mode,  //D7
                _192_23_30.Mode,  //D8
                _192_23_30.Mode,  //D9
                _192_23_30.Mode,  //DA
                _192_23_30.Mode,  //DB
                _192_23_30.Mode,  //DC
                _192_23_30.Mode,  //DD
                _192_23_30.Mode,  //DE
                _192_31.Mode,     //DF
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

        public unsafe void SwitchMasterMode(byte masterMode, int start, int yStride)
        {
            var model = new ModeModel
            {
                Memory = _memory,
                Modules = _modules
            };

            // (GraphicsMode <<7) | (CompatibilityMode<<6)  | ((Bpp & 3)<<4) | (Stretch & 15);
            Modes[masterMode](model, start, yStride);
        }
    }
}
