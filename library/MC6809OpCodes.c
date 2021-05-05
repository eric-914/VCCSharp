#include "MC6809.h"
#include "TC1014MMU.h"

#include "MC6809Macros.h"

static MC6809State* instance = GetMC6809State();

extern "C" {
  __declspec(dllexport) void __cdecl MC6809ExecOpCode2(unsigned char opCode) {
    short unsigned postword = 0;

    signed short* spostword = (signed short*)&postword;

    unsigned short temp16;

    switch (opCode) {
    case LBRN_R: //1021
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBHI_R: //1022
      if (!(CC_C | CC_Z))
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBLS_R: //1023
      if (CC_C | CC_Z)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBHS_R: //1024
      if (!CC_C)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 6;
      break;

    case LBCS_R: //1025
      if (CC_C)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBNE_R: //1026
      if (!CC_Z)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBEQ_R: //1027
      if (CC_Z)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBVC_R: //1028
      if (!CC_V)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBVS_R: //1029
      if (CC_V)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBPL_R: //102A
      if (!CC_N)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBMI_R: //102B
      if (CC_N)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBGE_R: //102C
      if (!(CC_N ^ CC_V))
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBLT_R: //102D
      if (CC_V ^ CC_N)
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBGT_R: //102E
      if (!(CC_Z | (CC_N ^ CC_V)))
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LBLE_R:	//102F
      if (CC_Z | (CC_N ^ CC_V))
      {
        *spostword = MemRead16(PC_REG);
        PC_REG += *spostword;
        instance->CycleCounter += 1;
      }

      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case SWI2_I: //103F
      CC_E = 1;
      MemWrite8(PC_L, --S_REG);
      MemWrite8(PC_H, --S_REG);
      MemWrite8(U_L, --S_REG);
      MemWrite8(U_H, --S_REG);
      MemWrite8(Y_L, --S_REG);
      MemWrite8(Y_H, --S_REG);
      MemWrite8(X_L, --S_REG);
      MemWrite8(X_H, --S_REG);
      MemWrite8(DPA, --S_REG);
      MemWrite8(B_REG, --S_REG);
      MemWrite8(A_REG, --S_REG);
      MemWrite8(MC6809_getcc(), --S_REG);

      PC_REG = MemRead16(VSWI2);
      instance->CycleCounter += 20;
      break;

    case CMPD_M: //1083
      postword = MemRead16(PC_REG);
      temp16 = D_REG - postword;
      CC_C = temp16 > D_REG;
      CC_V = OTEST16(CC_C, postword, temp16, D_REG);//CC_C^((postword^temp16^D_REG)>>15);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case CMPY_M: //108C
      postword = MemRead16(PC_REG);
      temp16 = Y_REG - postword;
      CC_C = temp16 > Y_REG;
      CC_V = OTEST16(CC_C, postword, temp16, Y_REG);//CC_C^((postword^temp16^Y_REG)>>15);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case LDY_M: //108E
      Y_REG = MemRead16(PC_REG);
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case CMPD_D: //1093
      postword = MemRead16((DP_REG | MemRead8(PC_REG++)));
      temp16 = D_REG - postword;
      CC_C = temp16 > D_REG;
      CC_V = OTEST16(CC_C, postword, temp16, D_REG); //CC_C^((postword^temp16^D_REG)>>15);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPY_D:	//109C
      postword = MemRead16((DP_REG | MemRead8(PC_REG++)));
      temp16 = Y_REG - postword;
      CC_C = temp16 > Y_REG;
      CC_V = OTEST16(CC_C, postword, temp16, Y_REG);//CC_C^((postword^temp16^Y_REG)>>15);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case LDY_D: //109E
      Y_REG = MemRead16((DP_REG | MemRead8(PC_REG++)));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case STY_D: //109F
      MemWrite16(Y_REG, (DP_REG | MemRead8(PC_REG++)));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case CMPD_X: //10A3
      postword = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      temp16 = D_REG - postword;
      CC_C = temp16 > D_REG;
      CC_V = OTEST16(CC_C, postword, temp16, D_REG);//CC_C^((postword^temp16^D_REG)>>15);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPY_X: //10AC
      postword = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      temp16 = Y_REG - postword;
      CC_C = temp16 > Y_REG;
      CC_V = OTEST16(CC_C, postword, temp16, Y_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case LDY_X: //10AE
      Y_REG = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case STY_X: //10AF
      MemWrite16(Y_REG, MC6809_CalculateEA(MemRead8(PC_REG++)));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case CMPD_E: //10B3
      postword = MemRead16(MemRead16(PC_REG));
      temp16 = D_REG - postword;
      CC_C = temp16 > D_REG;
      CC_V = OTEST16(CC_C, postword, temp16, D_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 8;
      break;

    case CMPY_E: //10BC
      postword = MemRead16(MemRead16(PC_REG));
      temp16 = Y_REG - postword;
      CC_C = temp16 > Y_REG;
      CC_V = OTEST16(CC_C, postword, temp16, Y_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 8;
      break;

    case LDY_E: //10BE
      Y_REG = MemRead16(MemRead16(PC_REG));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 7;
      break;

    case STY_E: //10BF
      MemWrite16(Y_REG, MemRead16(PC_REG));
      CC_Z = ZTEST(Y_REG);
      CC_N = NTEST16(Y_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 7;
      break;

    case LDS_I:  //10CE
      S_REG = MemRead16(PC_REG);
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 4;
      break;

    case LDS_D: //10DE
      S_REG = MemRead16((DP_REG | MemRead8(PC_REG++)));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case STS_D: //10DF
      MemWrite16(S_REG, (DP_REG | MemRead8(PC_REG++)));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case LDS_X: //10EE
      S_REG = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case STS_X: //10EF
      MemWrite16(S_REG, MC6809_CalculateEA(MemRead8(PC_REG++)));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      instance->CycleCounter += 6;
      break;

    case LDS_E: //10FE
      S_REG = MemRead16(MemRead16(PC_REG));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 7;
      break;

    case STS_E: //10FF
      MemWrite16(S_REG, MemRead16(PC_REG));
      CC_Z = ZTEST(S_REG);
      CC_N = NTEST16(S_REG);
      CC_V = 0;
      PC_REG += 2;
      instance->CycleCounter += 7;
      break;

    default:
      break;
    } //Page 2 switch END
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl MC6809ExecOpCode3(unsigned char opCode) {
    short unsigned postword = 0;

    unsigned short temp16;

    switch (opCode) {
    case SWI3_I: //113F
      CC_E = 1;
      MemWrite8(PC_L, --S_REG);
      MemWrite8(PC_H, --S_REG);
      MemWrite8(U_L, --S_REG);
      MemWrite8(U_H, --S_REG);
      MemWrite8(Y_L, --S_REG);
      MemWrite8(Y_H, --S_REG);
      MemWrite8(X_L, --S_REG);
      MemWrite8(X_H, --S_REG);
      MemWrite8(DPA, --S_REG);
      MemWrite8(B_REG, --S_REG);
      MemWrite8(A_REG, --S_REG);
      MemWrite8(MC6809_getcc(), --S_REG);

      PC_REG = MemRead16(VSWI3);
      instance->CycleCounter += 20;
      break;

    case CMPU_M: //1183
      postword = MemRead16(PC_REG);
      temp16 = U_REG - postword;
      CC_C = temp16 > U_REG;
      CC_V = OTEST16(CC_C, postword, temp16, U_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case CMPS_M: //118C
      postword = MemRead16(PC_REG);
      temp16 = S_REG - postword;
      CC_C = temp16 > S_REG;
      CC_V = OTEST16(CC_C, postword, temp16, S_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 5;
      break;

    case CMPU_D: //1193
      postword = MemRead16((DP_REG | MemRead8(PC_REG++)));
      temp16 = U_REG - postword;
      CC_C = temp16 > U_REG;
      CC_V = OTEST16(CC_C, postword, temp16, U_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPS_D: //119C
      postword = MemRead16((DP_REG | MemRead8(PC_REG++)));
      temp16 = S_REG - postword;
      CC_C = temp16 > S_REG;
      CC_V = OTEST16(CC_C, postword, temp16, S_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPU_X: //11A3
      postword = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      temp16 = U_REG - postword;
      CC_C = temp16 > U_REG;
      CC_V = OTEST16(CC_C, postword, temp16, U_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPS_X:  //11AC
      postword = MemRead16(MC6809_CalculateEA(MemRead8(PC_REG++)));
      temp16 = S_REG - postword;
      CC_C = temp16 > S_REG;
      CC_V = OTEST16(CC_C, postword, temp16, S_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      instance->CycleCounter += 7;
      break;

    case CMPU_E: //11B3
      postword = MemRead16(MemRead16(PC_REG));
      temp16 = U_REG - postword;
      CC_C = temp16 > U_REG;
      CC_V = OTEST16(CC_C, postword, temp16, U_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 8;
      break;

    case CMPS_E: //11BC
      postword = MemRead16(MemRead16(PC_REG));
      temp16 = S_REG - postword;
      CC_C = temp16 > S_REG;
      CC_V = OTEST16(CC_C, postword, temp16, S_REG);
      CC_N = NTEST16(temp16);
      CC_Z = ZTEST(temp16);
      PC_REG += 2;
      instance->CycleCounter += 8;
      break;

    default:
      break;

    } //Page 3 switch END
  }
}
