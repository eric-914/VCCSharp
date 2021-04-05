#include <windows.h>

#include "HD6309.h"
#include "TC1014MMU.h"

#include "HD6309Macros.h"

static HD6309State* instance = GetHD6309State();

static signed short stemp16;
static int stemp32;

static unsigned char temp8;
static unsigned short temp16;
static unsigned int temp32;

static unsigned char postbyte = 0;
static short unsigned postword = 0;
static signed char* spostbyte = (signed char*)&postbyte;
static signed short* spostword = (signed short*)&postword;

static unsigned char Source = 0;
static unsigned char Dest = 0;

void ___();
void ErrorVector();
void InvalidInsHandler();
void DivbyZero();

//void Neg_D()
//{ //0
//  temp16 = DPADDRESS(PC_REG++);
//  postbyte = MemRead8(temp16);
//  temp8 = 0 - postbyte;
//
//  CC_C = temp8 > 0;
//  CC_V = (postbyte == 0x80);
//  CC_N = NTEST8(temp8);
//  CC_Z = ZTEST(temp8);
//
//  MemWrite8(temp8, temp16);
//
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Oim_D()
//{//1 6309
//  postbyte = MemRead8(PC_REG++);
//  temp16 = DPADDRESS(PC_REG++);
//  postbyte |= MemRead8(temp16);
//
//  MemWrite8(postbyte, temp16);
//
//  CC_N = NTEST8(postbyte);
//  CC_Z = ZTEST(postbyte);
//  CC_V = 0;
//
//  instance->CycleCounter += 6;
//}
//
//void Aim_D()
//{//2 Phase 2 6309
//  postbyte = MemRead8(PC_REG++);
//  temp16 = DPADDRESS(PC_REG++);
//  postbyte &= MemRead8(temp16);
//
//  MemWrite8(postbyte, temp16);
//
//  CC_N = NTEST8(postbyte);
//  CC_Z = ZTEST(postbyte);
//  CC_V = 0;
//
//  instance->CycleCounter += 6;
//}
//
//void Com_D()
//{ //03
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  temp8 = 0xFF - temp8;
//
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  CC_C = 1;
//  CC_V = 0;
//
//  MemWrite8(temp8, temp16);
//
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Lsr_D()
//{ //04 S2
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  CC_C = temp8 & 1;
//  temp8 = temp8 >> 1;
//  CC_Z = ZTEST(temp8);
//  CC_N = 0;
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Eim_D()
//{ //05 6309 Untested
//  postbyte = MemRead8(PC_REG++);
//  temp16 = DPADDRESS(PC_REG++);
//  postbyte ^= MemRead8(temp16);
//  MemWrite8(postbyte, temp16);
//  CC_N = NTEST8(postbyte);
//  CC_Z = ZTEST(postbyte);
//  CC_V = 0;
//  instance->CycleCounter += 6;
//}
//
//void Ror_D()
//{ //06 S2
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  postbyte = CC_C << 7;
//  CC_C = temp8 & 1;
//  temp8 = (temp8 >> 1) | postbyte;
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Asr_D()
//{ //7
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  CC_C = temp8 & 1;
//  temp8 = (temp8 & 0x80) | (temp8 >> 1);
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Asl_D()
//{ //8 
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  CC_C = (temp8 & 0x80) >> 7;
//  CC_V = CC_C ^ ((temp8 & 0x40) >> 6);
//  temp8 = temp8 << 1;
//  CC_N = NTEST8(temp8);
//  CC_Z = ZTEST(temp8);
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Rol_D()
//{	//9
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16);
//  postbyte = CC_C;
//  CC_C = (temp8 & 0x80) >> 7;
//  CC_V = CC_C ^ ((temp8 & 0x40) >> 6);
//  temp8 = (temp8 << 1) | postbyte;
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Dec_D()
//{ //A
//  temp16 = DPADDRESS(PC_REG++);
//  temp8 = MemRead8(temp16) - 1;
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  CC_V = temp8 == 0x7F;
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Tim_D()
//{	//B 6309 Untested wcreate
//  postbyte = MemRead8(PC_REG++);
//  temp8 = MemRead8(DPADDRESS(PC_REG++));
//  postbyte &= temp8;
//  CC_N = NTEST8(postbyte);
//  CC_Z = ZTEST(postbyte);
//  CC_V = 0;
//  instance->CycleCounter += 6;
//}
//
//void Inc_D()
//{ //C
//  temp16 = (DPADDRESS(PC_REG++));
//  temp8 = MemRead8(temp16) + 1;
//  CC_Z = ZTEST(temp8);
//  CC_V = temp8 == 0x80;
//  CC_N = NTEST8(temp8);
//  MemWrite8(temp8, temp16);
//  instance->CycleCounter += instance->NatEmuCycles65;
//}
//
//void Tst_D()
//{ //D
//  temp8 = MemRead8(DPADDRESS(PC_REG++));
//  CC_Z = ZTEST(temp8);
//  CC_N = NTEST8(temp8);
//  CC_V = 0;
//  instance->CycleCounter += instance->NatEmuCycles64;
//}
//
//void Jmp_D()
//{	//E
//  PC_REG = ((DP_REG | MemRead8(PC_REG)));
//  instance->CycleCounter += instance->NatEmuCycles32;
//}
//
//void Clr_D()
//{	//F
//  MemWrite8(0, DPADDRESS(PC_REG++));
//  CC_Z = 1;
//  CC_N = 0;
//  CC_V = 0;
//  CC_C = 0;
//  instance->CycleCounter += instance->NatEmuCycles65;
//}

void Band(void)
{ //1130 6309 untested
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) == 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) &= ~(1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() & ~(1 << Dest));
      break;
    }
  }

  // Else nothing changes
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Biand(void)
{ //1131 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) != 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) &= ~(1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() & ~(1 << Dest));
      break;
    }
  }

  // Else do nothing
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Bor(void)
{ //1132 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) != 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) |= (1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() | (1 << Dest));
      break;
    }
  }

  // Else do nothing
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Bior(void)
{ //1133 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) == 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) |= (1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() | (1 << Dest));
      break;
    }
  }

  // Else do nothing
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Beor(void)
{ //1134 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) != 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) ^= (1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() ^ (1 << Dest));
      break;
    }
  }

  instance->CycleCounter += instance->NatEmuCycles76;
}

void Bieor(void)
{ //1135 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) == 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) ^= (1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() ^ (1 << Dest));
      break;
    }
  }

  instance->CycleCounter += instance->NatEmuCycles76;
}

void Ldbt(void)
{ //1136 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = MemRead8(DPADDRESS(PC_REG++));
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  if ((temp8 & (1 << Source)) != 0)
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) |= (1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() | (1 << Dest));
      break;
    }
  }
  else
  {
    switch (postbyte)
    {
    case 0: // A Reg
    case 1: // B Reg
      PUR(postbyte) &= ~(1 << Dest);
      break;

    case 2: // CC Reg
      HD6309_setcc(HD6309_getcc() & ~(1 << Dest));
      break;
    }
  }

  instance->CycleCounter += instance->NatEmuCycles76;
}

void Stbt(void)
{ //1137 6309
  postbyte = MemRead8(PC_REG++);
  temp16 = DPADDRESS(PC_REG++);
  temp8 = MemRead8(temp16);
  Source = (postbyte >> 3) & 7;
  Dest = (postbyte) & 7;
  postbyte >>= 6;

  if (postbyte == 3)
  {
    InvalidInsHandler();
    return;
  }

  switch (postbyte)
  {
  case 0: // A Reg
  case 1: // B Reg
    postbyte = PUR(postbyte);
    break;

  case 2: // CC Reg
    postbyte = HD6309_getcc();
    break;
  }

  if ((postbyte & (1 << Source)) != 0)
  {
    temp8 |= (1 << Dest);
  }
  else
  {
    temp8 &= ~(1 << Dest);
  }

  MemWrite8(temp8, temp16);
  instance->CycleCounter += instance->NatEmuCycles87;
}

void Tfm1(void)
{ //1138 TFM R+,R+ 6309
  if (W_REG == 0)
  {
    instance->CycleCounter += 6;
    PC_REG++;
    return;
  }

  postbyte = MemRead8(PC_REG);
  Source = postbyte >> 4;
  Dest = postbyte & 15;

  if (Source > 4 || Dest > 4)
  {
    InvalidInsHandler();
    return;
  }

  temp8 = MemRead8(PXF(Source));
  MemWrite8(temp8, PXF(Dest));
  (PXF(Dest))++;
  (PXF(Source))++;
  W_REG--;
  instance->CycleCounter += 3;
  PC_REG -= 2;
}

void Tfm2(void)
{ //1139 TFM R-,R- Phase 3 6309
  if (W_REG == 0)
  {
    instance->CycleCounter += 6;
    PC_REG++;
    return;
  }

  postbyte = MemRead8(PC_REG);
  Source = postbyte >> 4;
  Dest = postbyte & 15;

  if (Source > 4 || Dest > 4)
  {
    InvalidInsHandler();
    return;
  }

  temp8 = MemRead8(PXF(Source));
  MemWrite8(temp8, PXF(Dest));
  (PXF(Dest))--;
  (PXF(Source))--;
  W_REG--;
  instance->CycleCounter += 3;
  PC_REG -= 2;
}

void Tfm3(void)
{ //113A 6309 TFM R+,R 6309
  if (W_REG == 0)
  {
    instance->CycleCounter += 6;
    PC_REG++;
    return;
  }

  postbyte = MemRead8(PC_REG);
  Source = postbyte >> 4;
  Dest = postbyte & 15;

  if (Source > 4 || Dest > 4)
  {
    InvalidInsHandler();
    return;
  }

  temp8 = MemRead8(PXF(Source));
  MemWrite8(temp8, PXF(Dest));
  (PXF(Source))++;
  W_REG--;
  PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
  instance->CycleCounter += 3;
}

void Tfm4(void)
{ //113B TFM R,R+ 6309 
  if (W_REG == 0)
  {
    instance->CycleCounter += 6;
    PC_REG++;
    return;
  }

  postbyte = MemRead8(PC_REG);
  Source = postbyte >> 4;
  Dest = postbyte & 15;

  if (Source > 4 || Dest > 4)
  {
    InvalidInsHandler();
    return;
  }

  temp8 = MemRead8(PXF(Source));
  MemWrite8(temp8, PXF(Dest));
  (PXF(Dest))++;
  W_REG--;
  PC_REG -= 2; //Hit the same instruction on the next loop if not done copying
  instance->CycleCounter += 3;
}

void Bitmd_M(void)
{ //113C  6309
  postbyte = MemRead8(PC_REG++) & 0xC0;
  temp8 = HD6309_getmd() & postbyte;
  CC_Z = ZTEST(temp8);

  if (temp8 & 0x80) MD_ZERODIV = 0;
  if (temp8 & 0x40) MD_ILLEGAL = 0;

  instance->CycleCounter += 4;
}

void Ldmd_M(void)
{ //113D DONE 6309
  instance->mdbits = MemRead8(PC_REG++) & 0x03;
  HD6309_setmd(instance->mdbits);
  instance->CycleCounter += 5;
}

void Swi3_I(void)
{ //113F
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

  if (MD_NATIVE6309)
  {
    MemWrite8((F_REG), --S_REG);
    MemWrite8((E_REG), --S_REG);
    instance->CycleCounter += 2;
  }

  MemWrite8(B_REG, --S_REG);
  MemWrite8(A_REG, --S_REG);
  MemWrite8(HD6309_getcc(), --S_REG);
  PC_REG = MemRead16(VSWI3);
  instance->CycleCounter += 20;
}

void Come_I(void)
{ //1143 6309 Untested
  E_REG = 0xFF - E_REG;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_C = 1;
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Dece_I(void)
{ //114A 6309
  E_REG--;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = E_REG == 0x7F;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Ince_I(void)
{ //114C 6309
  E_REG++;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = E_REG == 0x80;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Tste_I(void)
{ //114D 6309 Untested TESTED NITRO
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Clre_I(void)
{ //114F 6309
  E_REG = 0;
  CC_C = 0;
  CC_V = 0;
  CC_N = 0;
  CC_Z = 1;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Comf_I(void)
{ //1153 6309 Untested
  F_REG = 0xFF - F_REG;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_C = 1;
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Decf_I(void)
{ //115A 6309
  F_REG--;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = F_REG == 0x7F;
  instance->CycleCounter += instance->NatEmuCycles21;
}

void Incf_I(void)
{ //115C 6309 Untested
  F_REG++;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = F_REG == 0x80;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Tstf_I(void)
{ //115D 6309
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Clrf_I(void)
{ //115F 6309 Untested wcreate
  F_REG = 0;
  CC_C = 0;
  CC_V = 0;
  CC_N = 0;
  CC_Z = 1;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Sube_M(void)
{ //1180 6309 Untested
  postbyte = MemRead8(PC_REG++);
  temp16 = E_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  instance->CycleCounter += 3;
}

void Cmpe_M(void)
{ //1181 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = E_REG - postbyte;
  CC_C = temp8 > E_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, E_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 3;
}

void Cmpu_M(void)
{ //1183
  postword = IMMADDRESS(PC_REG);
  temp16 = U_REG - postword;
  CC_C = temp16 > U_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, U_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Lde_M(void)
{ //1186 6309
  E_REG = MemRead8(PC_REG++);
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += 3;
}

void Adde_M(void)
{ //118B 6309
  postbyte = MemRead8(PC_REG++);
  temp16 = E_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((E_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_N = NTEST8(E_REG);
  CC_Z = ZTEST(E_REG);
  instance->CycleCounter += 3;
}

void Cmps_M(void)
{ //118C
  postword = IMMADDRESS(PC_REG);
  temp16 = S_REG - postword;
  CC_C = temp16 > S_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, S_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Divd_M(void)
{ //118D 6309
  postbyte = MemRead8(PC_REG++);

  if (postbyte == 0)
  {
    instance->CycleCounter += 3;
    DivbyZero();
    return;
  }

  postword = D_REG;
  stemp16 = (signed short)postword / (signed char)postbyte;

  if ((stemp16 > 255) || (stemp16 < -256)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += 17;
    return;
  }

  A_REG = (unsigned char)((signed short)postword % (signed char)postbyte);
  B_REG = (unsigned char)stemp16;

  if ((stemp16 > 127) || (stemp16 < -128))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(B_REG);
    CC_N = NTEST8(B_REG);
    CC_V = 0;
  }

  CC_C = B_REG & 1;
  instance->CycleCounter += 25;
}

void Divq_M(void)
{ //118E 6309
  postword = MemRead16(PC_REG);
  PC_REG += 2;

  if (postword == 0)
  {
    instance->CycleCounter += 4;
    DivbyZero();
    return;
  }

  temp32 = Q_REG;
  stemp32 = (signed int)temp32 / (signed short int)postword;

  if ((stemp32 > 65535) || (stemp32 < -65536)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += 34 - 21;
    return;
  }

  D_REG = (unsigned short)((signed int)temp32 % (signed short int)postword);
  W_REG = stemp32;

  if ((stemp16 > 32767) || (stemp16 < -32768))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(W_REG);
    CC_N = NTEST16(W_REG);
    CC_V = 0;
  }

  CC_C = B_REG & 1;
  instance->CycleCounter += 34;
}

void Muld_M(void)
{ //118F Phase 5 6309
  Q_REG = (signed short)D_REG * (signed short)IMMADDRESS(PC_REG);
  CC_C = 0;
  CC_Z = ZTEST(Q_REG);
  CC_V = 0;
  CC_N = NTEST32(Q_REG);
  PC_REG += 2;
  instance->CycleCounter += 28;
}

void Sube_D(void)
{ //1190 6309 Untested HERE
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = E_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpe_D(void)
{ //1191 6309 Untested
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp8 = E_REG - postbyte;
  CC_C = temp8 > E_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, E_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpu_D(void)
{ //1193
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp16 = U_REG - postword;
  CC_C = temp16 > U_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, U_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Lde_D(void)
{ //1196 6309
  E_REG = MemRead8(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ste_D(void)
{ //1197 Phase 5 6309
  MemWrite8(E_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Adde_D(void)
{ //119B 6309 Untested
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = E_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((E_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_N = NTEST8(E_REG);
  CC_Z = ZTEST(E_REG);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmps_D(void)
{ //119C
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp16 = S_REG - postword;
  CC_C = temp16 > S_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, S_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Divd_D(void)
{ //119D 6309 02292008
  postbyte = MemRead8(DPADDRESS(PC_REG++));

  if (postbyte == 0)
  {
    instance->CycleCounter += 3;
    DivbyZero();
    return;
  }

  postword = D_REG;
  stemp16 = (signed short)postword / (signed char)postbyte;

  if ((stemp16 > 255) || (stemp16 < -256)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += 19;
    return;
  }

  A_REG = (unsigned char)((signed short)postword % (signed char)postbyte);
  B_REG = (unsigned char)stemp16;

  if ((stemp16 > 127) || (stemp16 < -128))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(B_REG);
    CC_N = NTEST8(B_REG);
    CC_V = 0;
  }

  CC_C = B_REG & 1;
  instance->CycleCounter += 27;
}

void Divq_D(void)
{ //119E 6309
  postword = MemRead16(DPADDRESS(PC_REG++));

  if (postword == 0)
  {
    instance->CycleCounter += 4;
    DivbyZero();
    return;
  }

  temp32 = Q_REG;
  stemp32 = (signed int)temp32 / (signed short int)postword;

  if ((stemp32 > 65535) || (stemp32 < -65536)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += instance->NatEmuCycles3635 - 21;
    return;
  }

  D_REG = (unsigned short)((signed int)temp32 % (signed short int)postword);
  W_REG = stemp32;

  if ((stemp16 > 32767) || (stemp32 < -32768))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(W_REG);
    CC_N = NTEST16(W_REG);
    CC_V = 0;
  }

  CC_C = B_REG & 1;
  instance->CycleCounter += instance->NatEmuCycles3635;
}

void Muld_D(void)
{ //119F 6309 02292008
  Q_REG = (signed short)D_REG * (signed short)MemRead16(DPADDRESS(PC_REG++));
  CC_C = 0;
  CC_Z = ZTEST(Q_REG);
  CC_V = 0;
  CC_N = NTEST32(Q_REG);
  instance->CycleCounter += instance->NatEmuCycles3029;
}

void Sube_X(void)
{ //11A0 6309 Untested
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = E_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  instance->CycleCounter += 5;
}

void Cmpe_X(void)
{ //11A1 6309
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp8 = E_REG - postbyte;
  CC_C = temp8 > E_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, E_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 5;
}

void Cmpu_X(void)
{ //11A3
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp16 = U_REG - postword;
  CC_C = temp16 > U_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, U_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Lde_X(void)
{ //11A6 6309
  E_REG = MemRead8(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Ste_X(void)
{ //11A7 6309
  MemWrite8(E_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Adde_X(void)
{ //11AB 6309 Untested
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = E_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((E_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_N = NTEST8(E_REG);
  CC_Z = ZTEST(E_REG);
  instance->CycleCounter += 5;
}

void Cmps_X(void)
{  //11AC
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp16 = S_REG - postword;
  CC_C = temp16 > S_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, S_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Divd_X(void)
{ //11AD wcreate  6309
  postbyte = MemRead8(INDADDRESS(PC_REG++));

  if (postbyte == 0)
  {
    instance->CycleCounter += 3;
    DivbyZero();
    return;
  }

  postword = D_REG;
  stemp16 = (signed short)postword / (signed char)postbyte;

  if ((stemp16 > 255) || (stemp16 < -256)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += 19;
    return;
  }

  A_REG = (unsigned char)((signed short)postword % (signed char)postbyte);
  B_REG = (unsigned char)stemp16;

  if ((stemp16 > 127) || (stemp16 < -128))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(B_REG);
    CC_N = NTEST8(B_REG);
    CC_V = 0;
  }
  CC_C = B_REG & 1;
  instance->CycleCounter += 27;
}

void Divq_X(void)
{ //11AE Phase 5 6309 CHECK
  postword = MemRead16(INDADDRESS(PC_REG++));

  if (postword == 0)
  {
    instance->CycleCounter += 4;
    DivbyZero();
    return;
  }

  temp32 = Q_REG;
  stemp32 = (signed int)temp32 / (signed short int)postword;

  if ((stemp32 > 65535) || (stemp32 < -65536)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += instance->NatEmuCycles3635 - 21;
    return;
  }

  D_REG = (unsigned short)((signed int)temp32 % (signed short int)postword);
  W_REG = stemp32;

  if ((stemp16 > 32767) || (stemp16 < -32768))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(W_REG);
    CC_N = NTEST16(W_REG);
    CC_V = 0;
  }

  CC_C = B_REG & 1;
  instance->CycleCounter += instance->NatEmuCycles3635;
}

void Muld_X(void)
{ //11AF 6309 CHECK
  Q_REG = (signed short)D_REG * (signed short)MemRead16(INDADDRESS(PC_REG++));
  CC_C = 0;
  CC_Z = ZTEST(Q_REG);
  CC_V = 0;
  CC_N = NTEST32(Q_REG);
  instance->CycleCounter += 30;
}

void Sube_E(void)
{ //11B0 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = E_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Cmpe_E(void)
{ //11B1 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp8 = E_REG - postbyte;
  CC_C = temp8 > E_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, E_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Cmpu_E(void)
{ //11B3
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp16 = U_REG - postword;
  CC_C = temp16 > U_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, U_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Lde_E(void)
{ //11B6 6309
  E_REG = MemRead8(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Ste_E(void)
{ //11B7 6309
  MemWrite8(E_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(E_REG);
  CC_N = NTEST8(E_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Adde_E(void)
{ //11BB 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = E_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((E_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, E_REG);
  E_REG = (unsigned char)temp16;
  CC_N = NTEST8(E_REG);
  CC_Z = ZTEST(E_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Cmps_E(void)
{ //11BC
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp16 = S_REG - postword;
  CC_C = temp16 > S_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, S_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Divd_E(void)
{ //11BD 6309 02292008 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  PC_REG += 2;

  if (postbyte == 0)
  {
    instance->CycleCounter += 3;
    DivbyZero();
    return;
  }

  postword = D_REG;
  stemp16 = (signed short)postword / (signed char)postbyte;

  if ((stemp16 > 255) || (stemp16 < -256)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += 17;
    return;
  }

  A_REG = (unsigned char)((signed short)postword % (signed char)postbyte);
  B_REG = (unsigned char)stemp16;

  if ((stemp16 > 127) || (stemp16 < -128))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(B_REG);
    CC_N = NTEST8(B_REG);
    CC_V = 0;
  }
  CC_C = B_REG & 1;
  instance->CycleCounter += 25;
}

void Divq_E(void)
{ //11BE Phase 5 6309 CHECK
  postword = MemRead16(IMMADDRESS(PC_REG));
  PC_REG += 2;

  if (postword == 0)
  {
    instance->CycleCounter += 4;
    DivbyZero();
    return;
  }

  temp32 = Q_REG;
  stemp32 = (signed int)temp32 / (signed short int)postword;

  if ((stemp32 > 65535) || (stemp32 < -65536)) //Abort
  {
    CC_V = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_C = 0;
    instance->CycleCounter += instance->NatEmuCycles3635 - 21;
    return;
  }

  D_REG = (unsigned short)((signed int)temp32 % (signed short int)postword);
  W_REG = stemp32;

  if ((stemp16 > 32767) || (stemp16 < -32768))
  {
    CC_V = 1;
    CC_N = 1;
  }
  else
  {
    CC_Z = ZTEST(W_REG);
    CC_N = NTEST16(W_REG);
    CC_V = 0;
  }
  CC_C = B_REG & 1;
  instance->CycleCounter += instance->NatEmuCycles3635;
}

void Muld_E(void)
{ //11BF 6309
  Q_REG = (signed short)D_REG * (signed short)MemRead16(IMMADDRESS(PC_REG));
  PC_REG += 2;
  CC_C = 0;
  CC_Z = ZTEST(Q_REG);
  CC_V = 0;
  CC_N = NTEST32(Q_REG);
  instance->CycleCounter += instance->NatEmuCycles3130;
}

void Subf_M(void)
{ //11C0 6309 Untested
  postbyte = MemRead8(PC_REG++);
  temp16 = F_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  instance->CycleCounter += 3;
}

void Cmpf_M(void)
{ //11C1 6309
  postbyte = MemRead8(PC_REG++);
  temp8 = F_REG - postbyte;
  CC_C = temp8 > F_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, F_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 3;
}

void Ldf_M(void)
{ //11C6 6309
  F_REG = MemRead8(PC_REG++);
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += 3;
}

void Addf_M(void)
{ //11CB 6309 Untested
  postbyte = MemRead8(PC_REG++);
  temp16 = F_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((F_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_N = NTEST8(F_REG);
  CC_Z = ZTEST(F_REG);
  instance->CycleCounter += 3;
}

void Subf_D(void)
{ //11D0 6309 Untested
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = F_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpf_D(void)
{ //11D1 6309 Untested
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp8 = F_REG - postbyte;
  CC_C = temp8 > F_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, F_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldf_D(void)
{ //11D6 6309 Untested wcreate
  F_REG = MemRead8(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Stf_D(void)
{ //11D7 Phase 5 6309
  MemWrite8(F_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Addf_D(void)
{ //11DB 6309 Untested
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = F_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((F_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_N = NTEST8(F_REG);
  CC_Z = ZTEST(F_REG);
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Subf_X(void)
{ //11E0 6309 Untested
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = F_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  instance->CycleCounter += 5;
}

void Cmpf_X(void)
{ //11E1 6309 Untested
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp8 = F_REG - postbyte;
  CC_C = temp8 > F_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, F_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 5;
}

void Ldf_X(void)
{ //11E6 6309
  F_REG = MemRead8(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Stf_X(void)
{ //11E7 Phase 5 6309
  MemWrite8(F_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Addf_X(void)
{ //11EB 6309 Untested
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = F_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((F_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_N = NTEST8(F_REG);
  CC_Z = ZTEST(F_REG);
  instance->CycleCounter += 5;
}

void Subf_E(void)
{ //11F0 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = F_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Cmpf_E(void)
{ //11F1 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp8 = F_REG - postbyte;
  CC_C = temp8 > F_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, F_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Ldf_E(void)
{ //11F6 6309
  F_REG = MemRead8(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Stf_E(void)
{ //11F7 Phase 5 6309
  MemWrite8(F_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(F_REG);
  CC_N = NTEST8(F_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Addf_E(void)
{ //11FB 6309 Untested
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = F_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((F_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, F_REG);
  F_REG = (unsigned char)temp16;
  CC_N = NTEST8(F_REG);
  CC_Z = ZTEST(F_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

//NOTE: Ported
void ErrorVector()
{
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

  if (MD_NATIVE6309)
  {
    MemWrite8((F_REG), --S_REG);
    MemWrite8((E_REG), --S_REG);

    instance->CycleCounter += 2;
  }

  MemWrite8(B_REG, --S_REG);
  MemWrite8(A_REG, --S_REG);
  MemWrite8(HD6309_getcc(), --S_REG);

  PC_REG = MemRead16(VTRAP);

  instance->CycleCounter += (12 + instance->NatEmuCycles54);	//One for each byte +overhead? Guessing from PSHS
}

//NOTE: Ported, but used inside some OpCodes
void InvalidInsHandler()
{
  MD_ILLEGAL = 1;
  instance->mdbits = HD6309_getmd();

  ErrorVector();
}

void DivbyZero()
{
  MD_ZERODIV = 1;

  instance->mdbits = HD6309_getmd();

  ErrorVector();
}

extern "C" __declspec(dllexport) void __cdecl Page_3(unsigned char opcode);

void(*JmpVec3[256])(void) = {
  ___,		// 00
  ___,		// 01
  ___,		// 02
  ___,		// 03
  ___,		// 04
  ___,		// 05
  ___,		// 06
  ___,		// 07
  ___,		// 08
  ___,		// 09
  ___,		// 0A
  ___,		// 0B
  ___,		// 0C
  ___,		// 0D
  ___,		// 0E
  ___,		// 0F
  ___,		// 10
  ___,		// 11
  ___,		// 12
  ___,		// 13
  ___,		// 14
  ___,		// 15
  ___,		// 16
  ___,		// 17
  ___,		// 18
  ___,		// 19
  ___,		// 1A
  ___,		// 1B
  ___,		// 1C
  ___,		// 1D
  ___,		// 1E
  ___,		// 1F
  ___,		// 20
  ___,		// 21
  ___,		// 22
  ___,		// 23
  ___,		// 24
  ___,		// 25
  ___,		// 26
  ___,		// 27
  ___,		// 28
  ___,		// 29
  ___,		// 2A
  ___,		// 2B
  ___,		// 2C
  ___,		// 2D
  ___,		// 2E
  ___,		// 2F
  Band,		// 30
  Biand,		// 31
  Bor,		// 32
  Bior,		// 33
  Beor,		// 34
  Bieor,		// 35
  Ldbt,		// 36
  Stbt,		// 37
  Tfm1,		// 38
  Tfm2,		// 39
  Tfm3,		// 3A
  Tfm4,		// 3B
  Bitmd_M,	// 3C
  Ldmd_M,		// 3D
  ___,		// 3E
  Swi3_I,		// 3F
  ___,		// 40
  ___,		// 41
  ___,		// 42
  Come_I,		// 43
  ___,		// 44
  ___,		// 45
  ___,		// 46
  ___,		// 47
  ___,		// 48
  ___,		// 49
  Dece_I,		// 4A
  ___,		// 4B
  Ince_I,		// 4C
  Tste_I,		// 4D
  ___,		// 4E
  Clre_I,		// 4F
  ___,		// 50
  ___,		// 51
  ___,		// 52
  Comf_I,		// 53
  ___,		// 54
  ___,		// 55
  ___,		// 56
  ___,		// 57
  ___,		// 58
  ___,		// 59
  Decf_I,		// 5A
  ___,		// 5B
  Incf_I,		// 5C
  Tstf_I,		// 5D
  ___,		// 5E
  Clrf_I,		// 5F
  ___,		// 60
  ___,		// 61
  ___,		// 62
  ___,		// 63
  ___,		// 64
  ___,		// 65
  ___,		// 66
  ___,		// 67
  ___,		// 68
  ___,		// 69
  ___,		// 6A
  ___,		// 6B
  ___,		// 6C
  ___,		// 6D
  ___,		// 6E
  ___,		// 6F
  ___,		// 70
  ___,		// 71
  ___,		// 72
  ___,		// 73
  ___,		// 74
  ___,		// 75
  ___,		// 76
  ___,		// 77
  ___,		// 78
  ___,		// 79
  ___,		// 7A
  ___,		// 7B
  ___,		// 7C
  ___,		// 7D
  ___,		// 7E
  ___,		// 7F
  Sube_M,		// 80
  Cmpe_M,		// 81
  ___,		// 82
  Cmpu_M,		// 83
  ___,		// 84
  ___,		// 85
  Lde_M,		// 86
  ___,		// 87
  ___,		// 88
  ___,		// 89
  ___,		// 8A
  Adde_M,		// 8B
  Cmps_M,		// 8C
  Divd_M,		// 8D
  Divq_M,		// 8E
  Muld_M,		// 8F
  Sube_D,		// 90
  Cmpe_D,		// 91
  ___,		// 92
  Cmpu_D,		// 93
  ___,		// 94
  ___,		// 95
  Lde_D,		// 96
  Ste_D,		// 97
  ___,		// 98
  ___,		// 99
  ___,		// 9A
  Adde_D,		// 9B
  Cmps_D,		// 9C
  Divd_D,		// 9D
  Divq_D,		// 9E
  Muld_D,		// 9F
  Sube_X,		// A0
  Cmpe_X,		// A1
  ___,		// A2
  Cmpu_X,		// A3
  ___,		// A4
  ___,		// A5
  Lde_X,		// A6
  Ste_X,		// A7
  ___,		// A8
  ___,		// A9
  ___,		// AA
  Adde_X,		// AB
  Cmps_X,		// AC
  Divd_X,		// AD
  Divq_X,		// AE
  Muld_X,		// AF
  Sube_E,		// B0
  Cmpe_E,		// B1
  ___,		// B2
  Cmpu_E,		// B3
  ___,		// B4
  ___,		// B5
  Lde_E,		// B6
  Ste_E,		// B7
  ___,		// B8
  ___,		// B9
  ___,		// BA
  Adde_E,		// BB
  Cmps_E,		// BC
  Divd_E,		// BD
  Divq_E,		// BE
  Muld_E,		// BF
  Subf_M,		// C0
  Cmpf_M,		// C1
  ___,		// C2
  ___,		// C3
  ___,		// C4
  ___,		// C5
  Ldf_M,		// C6
  ___,		// C7
  ___,		// C8
  ___,		// C9
  ___,		// CA
  Addf_M,		// CB
  ___,		// CC
  ___,		// CD
  ___,		// CE
  ___,		// CF
  Subf_D,		// D0
  Cmpf_D,		// D1
  ___,		// D2
  ___,		// D3
  ___,		// D4
  ___,		// D5
  Ldf_D,		// D6
  Stf_D,		// D7
  ___,		// D8
  ___,		// D9
  ___,		// DA
  Addf_D,		// DB
  ___,		// DC
  ___,		// DD
  ___,		// DE
  ___,		// DF
  Subf_X,		// E0
  Cmpf_X,		// E1
  ___,		// E2
  ___,		// E3
  ___,		// E4
  ___,		// E5
  Ldf_X,		// E6
  Stf_X,		// E7
  ___,		// E8
  ___,		// E9
  ___,		// EA
  Addf_X,		// EB
  ___,		// EC
  ___,		// ED
  ___,		// EE
  ___,		// EF
  Subf_E,		// F0
  Cmpf_E,		// F1
  ___,		// F2
  ___,		// F3
  ___,		// F4
  ___,		// F5
  Ldf_E,		// F6
  Stf_E,		// F7
  ___,		// F8
  ___,		// F9
  ___,		// FA
  Addf_E,		// FB
  ___,		// FC
  ___,		// FD
  ___,		// FE
  ___,		// FF
};

extern "C" {
  __declspec(dllexport) void __cdecl Page_3(unsigned char opCode) //11
  {
    JmpVec3[opCode]();
  }
}

void ___() {
  MessageBox(0, "Halt", "Halt", 0);
}
