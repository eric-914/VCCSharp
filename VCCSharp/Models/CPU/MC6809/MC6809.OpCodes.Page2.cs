namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public partial class MC6809
    {
        //1000
        //1001
        //1002
        //1003
        //1004
        //1005
        //1006
        //1007
        //1008
        //1009
        //100A
        //100B
        //100C
        //100D
        //100E
        //100F
        
        //1010
        //1011
        //1012
        //1013
        //1014
        //1015
        //1016
        //1017
        //1018
        //1019
        //101A
        //101B
        //101C
        //101D
        //101E
        //101F

        //1020

        public void LBRN_R() //1021
        {
            LIB2(0x21);
        }

        public void LBHI_R() //1022
        {
            LIB2(0x22);
        }

        public void LBLS_R() //1023
        {
            LIB2(0x23);
        }

        public void LBHS_R() //1024
        {
            LIB2(0x24);
        }

        public void LBCS_R() //1025
        {
            LIB2(0x25);
        }

        public void LBNE_R() //1026
        {
            LIB2(0x26);
        }
        
        public void LBEQ_R() //1027
        {
            LIB2(0x27);
        }
        
        public void LBVC_R() //1028
        {
            LIB2(0x28);
        }
        
        public void LBVS_R() //1029
        {
            LIB2(0x29);
        }
        
        public void LBPL_R() //102A
        {
            LIB2(0x2A);
        }
        
        public void LBMI_R() //102B
        {
            LIB2(0x2B);
        }
        
        public void LBGE_R() //102C
        {
            LIB2(0x2C);
        }
        
        public void LBLT_R() //102D
        {
            LIB2(0x2D);
        }
        
        public void LBGT_R() //102E
        {
            LIB2(0x2E);
        }
        
        public void LBLE_R() //102F
        {
            LIB2(0x2F);
        }
        
        //1030
        //1031
        //1032
        //1033
        //1034
        //1035
        //1036
        //1037
        //1038
        //1039
        //103A
        //103B
        //103C
        //103D
        //103E

        public void SWI2_I() //103F
        {
            LIB2(0x3F);
        }

        //1040
        //1041
        //1042
        //1043
        //1044
        //1045
        //1046
        //1047
        //1048
        //1049
        //104A
        //104B
        //104C
        //104D
        //104E
        //104F

        //1050
        //1051
        //1052
        //1053
        //1054
        //1055
        //1056
        //1057
        //1058
        //1059
        //105A
        //105B
        //105C
        //105D
        //105E
        //105F

        //1060
        //1061
        //1062
        //1063
        //1064
        //1065
        //1066
        //1067
        //1068
        //1069
        //106A
        //106B
        //106C
        //106D
        //106E
        //106F

        //1070
        //1071
        //1072
        //1073
        //1074
        //1075
        //1076
        //1077
        //1078
        //1079
        //107A
        //107B
        //107C
        //107D
        //107E
        //107F

        //1080
        //1081
        //1082

        public void CMPD_M() //1083
        {
            LIB2(0x83);
        }
        
        //1084
        //1085
        //1086
        //1087
        //1088
        //1089
        //108A
        //108B

        public void CMPY_M() //108C
        {
            LIB2(0x8C);
        }
        
        public void LDY_M() //108E
        {
            LIB2(0x8E);
        }

        //108F

        //1090
        //1091
        //1092

        public void CMPD_D() //1093
        {
            LIB2(0x93);
        }
        
        //1094
        //1095
        //1096
        //1097
        //1098
        //1099
        //109A
        //109B

        public void CMPY_D()	//109C
        {
            LIB2(0x9C);
        }

        //109D

        public void LDY_D() //109E
        {
            LIB2(0x9E);
        }
        
        public void STY_D() //109F
        {
            LIB2(0x9F);
        }
        
        //10A0
        //10A1
        //10A2

        public void CMPD_X() //10A3
        {
            LIB2(0xA3);
        }
        
        //10A4
        //10A5
        //10A6
        //10A7
        //10A8
        //10A9
        //10AA
        //10AB

        public void CMPY_X() //10AC
        {
            LIB2(0xAC);
        }
        
        //10AD

        public void LDY_X() //10AE
        {
            LIB2(0xAE);
        }
        
        public void STY_X() //10AF
        {
            LIB2(0xAF);
        }
        
        //10B0
        //10B1
        //10B2

        public void CMPD_E() //10B3
        {
            LIB2(0xB3);
        }
        
        //10B4
        //10B5
        //10B6
        //10B7
        //10B8
        //10B9
        //10BA
        //10BB

        public void CMPY_E() //10BC
        {
            LIB2(0xBC);
        }
        
        //10BD

        public void LDY_E() //10BE
        {
            LIB2(0xBE);
        }
        
        public void STY_E() //10BF
        {
            LIB2(0xBF);
        }
        
        //10C0
        //10C1
        //10C2
        //10C3
        //10C4
        //10C5
        //10C6
        //10C7
        //10C8
        //10C9
        //10CA
        //10CB
        //10CC
        //10CD

        public void LDS_I() //10CE
        {
            LIB2(0xCE);
        }
        
        //10CF

        //10D0
        //10D1
        //10D2
        //10D3
        //10D4
        //10D5
        //10D6
        //10D7
        //10D8
        //10D9
        //10DA
        //10DB
        //10DC
        //10DD

        public void LDS_D() //10DE
        {
            LIB2(0xDE);
        }
        
        public void STS_D() //10DF
        {
            LIB2(0xDF);
        }
        
        //10E0
        //10E1
        //10E2
        //10E3
        //10E4
        //10E5
        //10E6
        //10E7
        //10E8
        //10E9
        //10EA
        //10EB
        //10EC
        //10ED

        public void LDS_X() //10EE
        {
            LIB2(0xEE);
        }
        
        public void STS_X() //10EF
        {
            LIB2(0xEF);
        }
        
        //10F0
        //10F1
        //10F2
        //10F3
        //10F4
        //10F5
        //10F6
        //10F7
        //10F8
        //10F9
        //10FA
        //10FB
        //10FC
        //10FD

        public void LDS_E() //10FE
        {
            LIB2(0xFE);
        }
        
        public void STS_E() //10FF
        {
            LIB2(0xFF);
        }
    }
}
