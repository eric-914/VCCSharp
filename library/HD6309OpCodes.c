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

void LBeq_R()
{ //1027
  if (CC_Z) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }
  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBrn_R()
{ //1021
  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBhi_R()
{ //1022
  if (!(CC_C | CC_Z)) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBls_R()
{ //1023
  if (CC_C | CC_Z) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBhs_R(void)
{ //1024
  if (!CC_C) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 6;
}

void LBcs_R(void)
{ //1025
  if (CC_C) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBne_R(void)
{ //1026
  if (!CC_Z) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBvc_R(void)
{ //1028
  if (!CC_V) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBvs_R(void)
{ //1029
  if (CC_V) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBpl_R(void)
{ //102A
  if (!CC_N) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBmi_R(void)
{ //102B
  if (CC_N) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBge_R(void)
{ //102C
  if (!(CC_N ^ CC_V)) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBlt_R(void)
{ //102D
  if (CC_V ^ CC_N) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBgt_R(void)
{ //102E
  if (!(CC_Z | (CC_N ^ CC_V))) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void LBle_R(void)
{	//102F
  if (CC_Z | (CC_N ^ CC_V)) {
    *spostword = IMMADDRESS(PC_REG);
    PC_REG += *spostword;
    instance->CycleCounter += 1;
  }

  PC_REG += 2;
  instance->CycleCounter += 5;
}

void Addr(void)
{ //1030 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) { // 8 bit dest
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) { // 8 bit source
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp16 = source8 + dest8;

    switch (Dest)
    {
    case 2: 				HD6309_setcc((unsigned char)temp16); break;
    case 4: case 5: break; // never assign to zero reg
    default: 				PUR(Dest) = (unsigned char)temp16; break;
    }

    CC_C = (temp16 & 0x100) >> 8;
    CC_V = OVERFLOW8(CC_C, source8, dest8, temp16);
    CC_N = NTEST8(PUR(Dest));
    CC_Z = ZTEST(PUR(Dest));
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0: case 1: source16 = D_REG; break; // A & B Reg
      case 2:	        source16 = (unsigned short)HD6309_getcc(); break; // CC
      case 3:	        source16 = (unsigned short)DP_REG; break; // DP
      case 4: case 5: source16 = 0; break; // Zero Reg
      case 6: case 7: source16 = W_REG; break; // E & F Reg
      }
    }

    temp32 = source16 + dest16;
    PXF(Dest) = (unsigned short)temp32;
    CC_C = (temp32 & 0x10000) >> 16;
    CC_V = OVERFLOW16(CC_C, source16, dest16, temp32);
    CC_N = NTEST16(PXF(Dest));
    CC_Z = ZTEST(PXF(Dest));
  }

  instance->CycleCounter += 4;
}

void Adcr(void)
{ //1031 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) { // 8 bit source
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp16 = source8 + dest8 + CC_C;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp16);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp16;
      break;
    }

    CC_C = (temp16 & 0x100) >> 8;
    CC_V = OVERFLOW8(CC_C, source8, dest8, temp16);
    CC_N = NTEST8(PUR(Dest));
    CC_Z = ZTEST(PUR(Dest));
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp32 = source16 + dest16 + CC_C;
    PXF(Dest) = (unsigned short)temp32;
    CC_C = (temp32 & 0x10000) >> 16;
    CC_V = OVERFLOW16(CC_C, source16, dest16, temp32);
    CC_N = NTEST16(PXF(Dest));
    CC_Z = ZTEST(PXF(Dest));
  }

  instance->CycleCounter += 4;
}

void Subr(void)
{ //1032 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) { // 8 bit source
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else { // 16 bit source - demote to 8 bit
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp16 = dest8 - source8;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp16);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp16;
      break;
    }

    CC_C = (temp16 & 0x100) >> 8;
    CC_V = CC_C ^ ((dest8 ^ PUR(Dest) ^ source8) >> 7);
    CC_N = PUR(Dest) >> 7;
    CC_Z = ZTEST(PUR(Dest));
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp32 = dest16 - source16;
    CC_C = (temp32 & 0x10000) >> 16;
    CC_V = !!((dest16 ^ source16 ^ temp32 ^ (temp32 >> 1)) & 0x8000);
    PXF(Dest) = (unsigned short)temp32;
    CC_N = (temp32 & 0x8000) >> 15;
    CC_Z = ZTEST(temp32);
  }

  instance->CycleCounter += 4;
}

void Sbcr(void)
{ //1033 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) // 8 bit source
    {
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp16 = dest8 - source8 - CC_C;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp16);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp16;
      break;
    }

    CC_C = (temp16 & 0x100) >> 8;
    CC_V = CC_C ^ ((dest8 ^ PUR(Dest) ^ source8) >> 7);
    CC_N = PUR(Dest) >> 7;
    CC_Z = ZTEST(PUR(Dest));
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp32 = dest16 - source16 - CC_C;
    CC_C = (temp32 & 0x10000) >> 16;
    CC_V = !!((dest16 ^ source16 ^ temp32 ^ (temp32 >> 1)) & 0x8000);
    PXF(Dest) = (unsigned short)temp32;
    CC_N = (temp32 & 0x8000) >> 15;
    CC_Z = ZTEST(temp32);
  }

  instance->CycleCounter += 4;
}

void Andr(void)
{ //1034 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) // 8 bit source
    {
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp8 = dest8 & source8;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp8);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp8;
      break;
    }

    CC_N = temp8 >> 7;
    CC_Z = ZTEST(temp8);
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;
      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp16 = dest16 & source16;
    PXF(Dest) = temp16;
    CC_N = temp16 >> 15;
    CC_Z = ZTEST(temp16);
  }
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Orr(void)
{ //1035 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) // 8 bit source
    {
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp8 = dest8 | source8;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp8);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp8;
      break;
    }

    CC_N = temp8 >> 7;
    CC_Z = ZTEST(temp8);
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp16 = dest16 | source16;
    PXF(Dest) = temp16;
    CC_N = temp16 >> 15;
    CC_Z = ZTEST(temp16);
  }

  CC_V = 0;
  instance->CycleCounter += 4;
}

void Eorr(void)
{ //1036 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) // 8 bit source
    {
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp8 = dest8 ^ source8;

    switch (Dest)
    {
    case 2:
      HD6309_setcc((unsigned char)temp8);
      break;

    case 4:
    case 5:
      break; // never assign to zero reg

    default:
      PUR(Dest) = (unsigned char)temp8;
      break;
    }

    CC_N = temp8 >> 7;
    CC_Z = ZTEST(temp8);
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp16 = dest16 ^ source16;
    PXF(Dest) = temp16;
    CC_N = temp16 >> 15;
    CC_Z = ZTEST(temp16);
  }

  CC_V = 0;
  instance->CycleCounter += 4;
}

void Cmpr(void)
{ //1037 6309 - WallyZ 2019
  unsigned char dest8 = 0, source8 = 0;
  unsigned short dest16 = 0, source16 = 0;
  temp8 = MemRead8(PC_REG++);
  Source = temp8 >> 4;
  Dest = temp8 & 15;

  if (Dest > 7) // 8 bit dest
  {
    Dest &= 7;

    if (Dest == 2) {
      dest8 = HD6309_getcc();
    }
    else {
      dest8 = PUR(Dest);
    }

    if (Source > 7) // 8 bit source
    {
      Source &= 7;

      if (Source == 2) {
        source8 = HD6309_getcc();
      }
      else {
        source8 = PUR(Source);
      }
    }
    else // 16 bit source - demote to 8 bit
    {
      Source &= 7;
      source8 = (unsigned char)PXF(Source);
    }

    temp16 = dest8 - source8;
    temp8 = (unsigned char)temp16;
    CC_C = (temp16 & 0x100) >> 8;
    CC_V = CC_C ^ ((dest8 ^ temp8 ^ source8) >> 7);
    CC_N = temp8 >> 7;
    CC_Z = ZTEST(temp8);
  }
  else // 16 bit dest
  {
    dest16 = PXF(Dest);

    if (Source < 8) // 16 bit source
    {
      source16 = PXF(Source);
    }
    else // 8 bit source - promote to 16 bit
    {
      Source &= 7;

      switch (Source)
      {
      case 0:
      case 1:
        source16 = D_REG;
        break; // A & B Reg

      case 2:
        source16 = (unsigned short)HD6309_getcc();
        break; // CC

      case 3:
        source16 = (unsigned short)DP_REG;
        break; // DP

      case 4:
      case 5:
        source16 = 0;
        break; // Zero Reg

      case 6:
      case 7:
        source16 = W_REG;
        break; // E & F Reg
      }
    }

    temp32 = dest16 - source16;
    CC_C = (temp32 & 0x10000) >> 16;
    CC_V = !!((dest16 ^ source16 ^ temp32 ^ (temp32 >> 1)) & 0x8000);
    CC_N = (temp32 & 0x8000) >> 15;
    CC_Z = ZTEST(temp32);
  }

  instance->CycleCounter += 4;
}

void Pshsw(void)
{ //1038 DONE 6309
  MemWrite8((F_REG), --S_REG);
  MemWrite8((E_REG), --S_REG);
  instance->CycleCounter += 6;
}

void Pulsw(void)
{	//1039 6309 Untested wcreate
  E_REG = MemRead8(S_REG++);
  F_REG = MemRead8(S_REG++);
  instance->CycleCounter += 6;
}

void Pshuw(void)
{ //103A 6309 Untested
  MemWrite8((F_REG), --U_REG);
  MemWrite8((E_REG), --U_REG);
  instance->CycleCounter += 6;
}

void Puluw(void)
{ //103B 6309 Untested
  E_REG = MemRead8(U_REG++);
  F_REG = MemRead8(U_REG++);
  instance->CycleCounter += 6;
}

void Swi2_I(void)
{ //103F
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
  PC_REG = MemRead16(VSWI2);
  instance->CycleCounter += 20;
}

void Negd_I(void)
{ //1040 Phase 5 6309
  D_REG = 0 - D_REG;
  CC_C = temp16 > 0;
  CC_V = D_REG == 0x8000;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Comd_I(void)
{ //1043 6309
  D_REG = 0xFFFF - D_REG;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_C = 1;
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Lsrd_I(void)
{ //1044 6309
  CC_C = D_REG & 1;
  D_REG = D_REG >> 1;
  CC_Z = ZTEST(D_REG);
  CC_N = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Rord_I(void)
{ //1046 6309 Untested
  postword = CC_C << 15;
  CC_C = D_REG & 1;
  D_REG = (D_REG >> 1) | postword;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Asrd_I(void)
{ //1047 6309 Untested TESTED NITRO MULTIVUE
  CC_C = D_REG & 1;
  D_REG = (D_REG & 0x8000) | (D_REG >> 1);
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Asld_I(void)
{ //1048 6309
  CC_C = D_REG >> 15;
  CC_V = CC_C ^ ((D_REG & 0x4000) >> 14);
  D_REG = D_REG << 1;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Rold_I(void)
{ //1049 6309 Untested
  postword = CC_C;
  CC_C = D_REG >> 15;
  CC_V = CC_C ^ ((D_REG & 0x4000) >> 14);
  D_REG = (D_REG << 1) | postword;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Decd_I(void)
{ //104A 6309
  D_REG--;
  CC_Z = ZTEST(D_REG);
  CC_V = D_REG == 0x7FFF;
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Incd_I(void)
{ //104C 6309
  D_REG++;
  CC_Z = ZTEST(D_REG);
  CC_V = D_REG == 0x8000;
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Tstd_I(void)
{ //104D 6309
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Clrd_I(void)
{ //104F 6309
  D_REG = 0;
  CC_C = 0;
  CC_V = 0;
  CC_N = 0;
  CC_Z = 1;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Comw_I(void)
{ //1053 6309 Untested
  W_REG = 0xFFFF - W_REG;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_C = 1;
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Lsrw_I(void)
{ //1054 6309 Untested
  CC_C = W_REG & 1;
  W_REG = W_REG >> 1;
  CC_Z = ZTEST(W_REG);
  CC_N = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Rorw_I(void)
{ //1056 6309 Untested
  postword = CC_C << 15;
  CC_C = W_REG & 1;
  W_REG = (W_REG >> 1) | postword;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Rolw_I(void)
{ //1059 6309
  postword = CC_C;
  CC_C = W_REG >> 15;
  CC_V = CC_C ^ ((W_REG & 0x4000) >> 14);
  W_REG = (W_REG << 1) | postword;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Decw_I(void)
{ //105A 6309
  W_REG--;
  CC_Z = ZTEST(W_REG);
  CC_V = W_REG == 0x7FFF;
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Incw_I(void)
{ //105C 6309
  W_REG++;
  CC_Z = ZTEST(W_REG);
  CC_V = W_REG == 0x8000;
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Tstw_I(void)
{ //105D Untested 6309 wcreate
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Clrw_I(void)
{ //105F 6309
  W_REG = 0;
  CC_C = 0;
  CC_V = 0;
  CC_N = 0;
  CC_Z = 1;
  instance->CycleCounter += instance->NatEmuCycles32;
}

void Subw_M(void)
{ //1080 6309 CHECK
  postword = IMMADDRESS(PC_REG);
  temp16 = W_REG - postword;
  CC_C = temp16 > W_REG;
  CC_V = OVERFLOW16(CC_C, temp16, W_REG, postword);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  W_REG = temp16;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpw_M(void)
{ //1081 6309 CHECK
  postword = IMMADDRESS(PC_REG);
  temp16 = W_REG - postword;
  CC_C = temp16 > W_REG;
  CC_V = OVERFLOW16(CC_C, temp16, W_REG, postword);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Sbcd_M(void)
{ //1082 6309
  postword = IMMADDRESS(PC_REG);
  temp32 = D_REG - postword - CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, D_REG, postword);
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpd_M(void)
{ //1083
  postword = IMMADDRESS(PC_REG);
  temp16 = D_REG - postword;
  CC_C = temp16 > D_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, D_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Andd_M(void)
{ //1084 6309
  D_REG &= IMMADDRESS(PC_REG);
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Bitd_M(void)
{ //1085 6309 Untested
  temp16 = D_REG & IMMADDRESS(PC_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldw_M(void)
{ //1086 6309
  W_REG = IMMADDRESS(PC_REG);
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Eord_M(void)
{ //1088 6309 Untested
  D_REG ^= IMMADDRESS(PC_REG);
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Adcd_M(void)
{ //1089 6309
  postword = IMMADDRESS(PC_REG);
  temp32 = D_REG + postword + CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, postword, temp32, D_REG);
  CC_H = ((D_REG ^ temp32 ^ postword) & 0x100) >> 8;
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ord_M(void)
{ //108A 6309 Untested
  D_REG |= IMMADDRESS(PC_REG);
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Addw_M(void)
{ //108B Phase 5 6309
  temp16 = IMMADDRESS(PC_REG);
  temp32 = W_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpy_M(void)
{ //108C
  postword = IMMADDRESS(PC_REG);
  temp16 = Y_REG - postword;
  CC_C = temp16 > Y_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, Y_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldy_M(void)
{ //108E
  Y_REG = IMMADDRESS(PC_REG);
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Subw_D(void)
{ //1090 Untested 6309
  temp16 = MemRead16(DPADDRESS(PC_REG++));
  temp32 = W_REG - temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Cmpw_D(void)
{ //1091 6309 Untested
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp16 = W_REG - postword;
  CC_C = temp16 > W_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, W_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Sbcd_D(void)
{ //1092 6309
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp32 = D_REG - postword - CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, D_REG, postword);
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Cmpd_D(void)
{ //1093
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp16 = D_REG - postword;
  CC_C = temp16 > D_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, D_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Andd_D(void)
{ //1094 6309 Untested
  postword = MemRead16(DPADDRESS(PC_REG++));
  D_REG &= postword;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Bitd_D(void)
{ //1095 6309 Untested
  temp16 = D_REG & MemRead16(DPADDRESS(PC_REG++));
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Ldw_D(void)
{ //1096 6309
  W_REG = MemRead16(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Stw_D(void)
{ //1097 6309
  MemWrite16(W_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Eord_D(void)
{ //1098 6309 Untested
  D_REG ^= MemRead16(DPADDRESS(PC_REG++));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Adcd_D(void)
{ //1099 6309
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp32 = D_REG + postword + CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, postword, temp32, D_REG);
  CC_H = ((D_REG ^ temp32 ^ postword) & 0x100) >> 8;
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Ord_D(void)
{ //109A 6309 Untested
  D_REG |= MemRead16(DPADDRESS(PC_REG++));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Addw_D(void)
{ //109B 6309
  temp16 = MemRead16(DPADDRESS(PC_REG++));
  temp32 = W_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Cmpy_D(void)
{	//109C
  postword = MemRead16(DPADDRESS(PC_REG++));
  temp16 = Y_REG - postword;
  CC_C = temp16 > Y_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, Y_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles75;
}

void Ldy_D(void)
{ //109E
  Y_REG = MemRead16(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Sty_D(void)
{ //109F
  MemWrite16(Y_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Subw_X(void)
{ //10A0 6309 MODDED
  temp16 = MemRead16(INDADDRESS(PC_REG++));
  temp32 = W_REG - temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Cmpw_X(void)
{ //10A1 6309
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp16 = W_REG - postword;
  CC_C = temp16 > W_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, W_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Sbcd_X(void)
{ //10A2 6309
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp32 = D_REG - postword - CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, postword, temp32, D_REG);
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Cmpd_X(void)
{ //10A3
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp16 = D_REG - postword;
  CC_C = temp16 > D_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, D_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Andd_X(void)
{ //10A4 6309
  D_REG &= MemRead16(INDADDRESS(PC_REG++));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Bitd_X(void)
{ //10A5 6309 Untested
  temp16 = D_REG & MemRead16(INDADDRESS(PC_REG++));
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Ldw_X(void)
{ //10A6 6309
  W_REG = MemRead16(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Stw_X(void)
{ //10A7 6309
  MemWrite16(W_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Eord_X(void)
{ //10A8 6309 Untested TESTED NITRO 
  D_REG ^= MemRead16(INDADDRESS(PC_REG++));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Adcd_X(void)
{ //10A9 6309 untested
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp32 = D_REG + postword + CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, postword, temp32, D_REG);
  CC_H = (((D_REG ^ temp32 ^ postword) & 0x100) >> 8);
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Ord_X(void)
{ //10AA 6309 Untested wcreate
  D_REG |= MemRead16(INDADDRESS(PC_REG++));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Addw_X(void)
{ //10AB 6309 Untested TESTED NITRO CHECK no Half carry?
  temp16 = MemRead16(INDADDRESS(PC_REG++));
  temp32 = W_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Cmpy_X(void)
{ //10AC
  postword = MemRead16(INDADDRESS(PC_REG++));
  temp16 = Y_REG - postword;
  CC_C = temp16 > Y_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, Y_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Ldy_X(void)
{ //10AE
  Y_REG = MemRead16(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Sty_X(void)
{ //10AF
  MemWrite16(Y_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Subw_E(void)
{ //10B0 6309 Untested
  temp16 = MemRead16(IMMADDRESS(PC_REG));
  temp32 = W_REG - temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Cmpw_E(void)
{ //10B1 6309 Untested
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp16 = W_REG - postword;
  CC_C = temp16 > W_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, W_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Sbcd_E(void)
{ //10B2 6309 Untested
  temp16 = MemRead16(IMMADDRESS(PC_REG));
  temp32 = D_REG - temp16 - CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, D_REG);
  D_REG = temp32;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Cmpd_E(void)
{ //10B3
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp16 = D_REG - postword;
  CC_C = temp16 > D_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, D_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Andd_E(void)
{ //10B4 6309 Untested
  D_REG &= MemRead16(IMMADDRESS(PC_REG));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Bitd_E(void)
{ //10B5 6309 Untested CHECK NITRO
  temp16 = D_REG & MemRead16(IMMADDRESS(PC_REG));
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Ldw_E(void)
{ //10B6 6309
  W_REG = MemRead16(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Stw_E(void)
{ //10B7 6309
  MemWrite16(W_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Eord_E(void)
{ //10B8 6309 Untested
  D_REG ^= MemRead16(IMMADDRESS(PC_REG));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Adcd_E(void)
{ //10B9 6309 untested
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp32 = D_REG + postword + CC_C;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, postword, temp32, D_REG);
  CC_H = (((D_REG ^ temp32 ^ postword) & 0x100) >> 8);
  D_REG = temp32;
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Ord_E(void)
{ //10BA 6309 Untested
  D_REG |= MemRead16(IMMADDRESS(PC_REG));
  CC_N = NTEST16(D_REG);
  CC_Z = ZTEST(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Addw_E(void)
{ //10BB 6309 Untested
  temp16 = MemRead16(IMMADDRESS(PC_REG));
  temp32 = W_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, W_REG);
  W_REG = temp32;
  CC_Z = ZTEST(W_REG);
  CC_N = NTEST16(W_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Cmpy_E(void)
{ //10BC
  postword = MemRead16(IMMADDRESS(PC_REG));
  temp16 = Y_REG - postword;
  CC_C = temp16 > Y_REG;
  CC_V = OVERFLOW16(CC_C, postword, temp16, Y_REG);
  CC_N = NTEST16(temp16);
  CC_Z = ZTEST(temp16);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles86;
}

void Ldy_E(void)
{ //10BE
  Y_REG = MemRead16(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Sty_E(void)
{ //10BF
  MemWrite16(Y_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(Y_REG);
  CC_N = NTEST16(Y_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Lds_I(void)
{  //10CE
  S_REG = IMMADDRESS(PC_REG);
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += 4;
}

void Ldq_D(void)
{ //10DC 6309
  Q_REG = MemRead32(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles87;
}

void Stq_D(void)
{ //10DD 6309
  MemWrite32(Q_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles87;
}

void Lds_D(void)
{ //10DE
  S_REG = MemRead16(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Sts_D(void)
{ //10DF 6309
  MemWrite16(S_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Ldq_X(void)
{ //10EC 6309
  Q_REG = MemRead32(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  instance->CycleCounter += 8;
}

void Stq_X(void)
{ //10ED 6309 DONE
  MemWrite32(Q_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  instance->CycleCounter += 8;
}

void Lds_X(void)
{ //10EE
  S_REG = MemRead16(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Sts_X(void)
{ //10EF 6309
  MemWrite16(S_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  instance->CycleCounter += 6;
}

void Ldq_E(void)
{ //10FC 6309
  Q_REG = MemRead32(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles98;
}

void Stq_E(void)
{ //10FD 6309
  MemWrite32(Q_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles98;
}

void Lds_E(void)
{ //10FE
  S_REG = MemRead16(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Sts_E(void)
{ //10FF 6309
  MemWrite16(S_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(S_REG);
  CC_N = NTEST16(S_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

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

void Subb_M(void)
{ //C0
  postbyte = MemRead8(PC_REG++);
  temp16 = B_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  instance->CycleCounter += 2;
}

void Cmpb_M(void)
{ //C1
  postbyte = MemRead8(PC_REG++);
  temp8 = B_REG - postbyte;
  CC_C = temp8 > B_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, B_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 2;
}

void Sbcb_M(void)
{ //C2
  postbyte = MemRead8(PC_REG++);
  temp16 = B_REG - postbyte - CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 2;
}

void Addd_M(void)
{ //C3
  temp16 = IMMADDRESS(PC_REG);
  temp32 = D_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, D_REG);
  D_REG = temp32;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Andb_M(void)
{ //C4 LOOK
  B_REG = B_REG & MemRead8(PC_REG++);
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 2;
}

void Bitb_M(void)
{ //C5
  temp8 = B_REG & MemRead8(PC_REG++);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  CC_V = 0;
  instance->CycleCounter += 2;
}

void Ldb_M(void)
{ //C6
  B_REG = MemRead8(PC_REG++);
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  instance->CycleCounter += 2;
}

void Eorb_M(void)
{ //C8
  B_REG = B_REG ^ MemRead8(PC_REG++);
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 2;
}

void Adcb_M(void)
{ //C9
  postbyte = MemRead8(PC_REG++);
  temp16 = B_REG + postbyte + CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  CC_H = ((B_REG ^ temp16 ^ postbyte) & 0x10) >> 4;
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 2;
}

void Orb_M(void)
{ //CA
  B_REG = B_REG | MemRead8(PC_REG++);
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 2;
}

void Addb_M(void)
{ //CB
  postbyte = MemRead8(PC_REG++);
  temp16 = B_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((B_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 2;
}

void Ldd_M(void)
{ //CC
  D_REG = IMMADDRESS(PC_REG);
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += 3;
}

void Ldq_M(void)
{ //CD 6309 WORK
  Q_REG = MemRead32(PC_REG);
  PC_REG += 4;
  CC_Z = ZTEST(Q_REG);
  CC_N = NTEST32(Q_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Ldu_M(void)
{ //CE
  U_REG = IMMADDRESS(PC_REG);
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += 3;
}

void Subb_D(void)
{ //D0
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = B_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Cmpb_D(void)
{ //D1
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp8 = B_REG - postbyte;
  CC_C = temp8 > B_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, B_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Sbcb_D(void)
{ //D2
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = B_REG - postbyte - CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Addd_D(void)
{ //D3
  temp16 = MemRead16(DPADDRESS(PC_REG++));
  temp32 = D_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, D_REG);
  D_REG = temp32;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles64;
}

void Andb_D(void)
{ //D4 
  B_REG = B_REG & MemRead8(DPADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Bitb_D(void)
{ //D5
  temp8 = B_REG & MemRead8(DPADDRESS(PC_REG++));
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Ldb_D(void)
{ //D6
  B_REG = MemRead8(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Stb_D(void)
{ //D7
  MemWrite8(B_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Eorb_D(void)
{ //D8	
  B_REG = B_REG ^ MemRead8(DPADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Adcb_D(void)
{ //D9
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = B_REG + postbyte + CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  CC_H = ((B_REG ^ temp16 ^ postbyte) & 0x10) >> 4;
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Orb_D(void)
{ //DA
  B_REG = B_REG | MemRead8(DPADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Addb_D(void)
{ //DB
  postbyte = MemRead8(DPADDRESS(PC_REG++));
  temp16 = B_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((B_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += instance->NatEmuCycles43;
}

void Ldd_D(void)
{ //DC
  D_REG = MemRead16(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Std_D(void)
{ //DD
  MemWrite16(D_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldu_D(void)
{ //DE
  U_REG = MemRead16(DPADDRESS(PC_REG++));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Stu_D(void)
{ //DF
  MemWrite16(U_REG, DPADDRESS(PC_REG++));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Subb_X(void)
{ //E0
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = B_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  instance->CycleCounter += 4;
}

void Cmpb_X(void)
{ //E1
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp8 = B_REG - postbyte;
  CC_C = temp8 > B_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, B_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  instance->CycleCounter += 4;
}

void Sbcb_X(void)
{ //E2
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = B_REG - postbyte - CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 4;
}

void Addd_X(void)
{ //E3 
  temp16 = MemRead16(INDADDRESS(PC_REG++));
  temp32 = D_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, D_REG);
  D_REG = temp32;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Andb_X(void)
{ //E4
  B_REG = B_REG & MemRead8(INDADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Bitb_X(void)
{ //E5 
  temp8 = B_REG & MemRead8(INDADDRESS(PC_REG++));
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Ldb_X(void)
{ //E6
  B_REG = MemRead8(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Stb_X(void)
{ //E7
  MemWrite8(B_REG, HD6309_CalculateEA(MemRead8(PC_REG++)));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Eorb_X(void)
{ //E8
  B_REG = B_REG ^ MemRead8(INDADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Adcb_X(void)
{ //E9
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = B_REG + postbyte + CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  CC_H = ((B_REG ^ temp16 ^ postbyte) & 0x10) >> 4;
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 4;
}

void Orb_X(void)
{ //EA 
  B_REG = B_REG | MemRead8(INDADDRESS(PC_REG++));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  instance->CycleCounter += 4;
}

void Addb_X(void)
{ //EB
  postbyte = MemRead8(INDADDRESS(PC_REG++));
  temp16 = B_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((B_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  instance->CycleCounter += 4;
}

void Ldd_X(void)
{ //EC
  D_REG = MemRead16(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Std_X(void)
{ //ED
  MemWrite16(D_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Ldu_X(void)
{ //EE
  U_REG = MemRead16(INDADDRESS(PC_REG++));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Stu_X(void)
{ //EF
  MemWrite16(U_REG, INDADDRESS(PC_REG++));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  instance->CycleCounter += 5;
}

void Subb_E(void)
{ //F0
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = B_REG - postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Cmpb_E(void)
{ //F1
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp8 = B_REG - postbyte;
  CC_C = temp8 > B_REG;
  CC_V = OVERFLOW8(CC_C, postbyte, temp8, B_REG);
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Sbcb_E(void)
{ //F2
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = B_REG - postbyte - CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Addd_E(void)
{ //F3
  temp16 = MemRead16(IMMADDRESS(PC_REG));
  temp32 = D_REG + temp16;
  CC_C = (temp32 & 0x10000) >> 16;
  CC_V = OVERFLOW16(CC_C, temp32, temp16, D_REG);
  D_REG = temp32;
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles76;
}

void Andb_E(void)
{  //F4
  B_REG = B_REG & MemRead8(IMMADDRESS(PC_REG));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Bitb_E(void)
{ //F5
  temp8 = B_REG & MemRead8(IMMADDRESS(PC_REG));
  CC_N = NTEST8(temp8);
  CC_Z = ZTEST(temp8);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldb_E(void)
{ //F6
  B_REG = MemRead8(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Stb_E(void)
{ //F7 
  MemWrite8(B_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(B_REG);
  CC_N = NTEST8(B_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Eorb_E(void)
{ //F8
  B_REG = B_REG ^ MemRead8(IMMADDRESS(PC_REG));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Adcb_E(void)
{ //F9
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = B_REG + postbyte + CC_C;
  CC_C = (temp16 & 0x100) >> 8;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  CC_H = ((B_REG ^ temp16 ^ postbyte) & 0x10) >> 4;
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Orb_E(void)
{ //FA
  B_REG = B_REG | MemRead8(IMMADDRESS(PC_REG));
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Addb_E(void)
{ //FB
  postbyte = MemRead8(IMMADDRESS(PC_REG));
  temp16 = B_REG + postbyte;
  CC_C = (temp16 & 0x100) >> 8;
  CC_H = ((B_REG ^ postbyte ^ temp16) & 0x10) >> 4;
  CC_V = OVERFLOW8(CC_C, postbyte, temp16, B_REG);
  B_REG = (unsigned char)temp16;
  CC_N = NTEST8(B_REG);
  CC_Z = ZTEST(B_REG);
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles54;
}

void Ldd_E(void)
{ //FC
  D_REG = MemRead16(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Std_E(void)
{ //FD
  MemWrite16(D_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(D_REG);
  CC_N = NTEST16(D_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Ldu_E(void)
{ //FE
  U_REG = MemRead16(IMMADDRESS(PC_REG));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
  PC_REG += 2;
  instance->CycleCounter += instance->NatEmuCycles65;
}

void Stu_E(void)
{ //FF
  MemWrite16(U_REG, IMMADDRESS(PC_REG));
  CC_Z = ZTEST(U_REG);
  CC_N = NTEST16(U_REG);
  CC_V = 0;
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

extern "C" __declspec(dllexport) void __cdecl Page_1(unsigned char opcode);
extern "C" __declspec(dllexport) void __cdecl Page_2(unsigned char opcode);
extern "C" __declspec(dllexport) void __cdecl Page_3(unsigned char opcode);

void(*JmpVec1[256])(void) = {
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
  ___,	// 15
  ___,		// 16
  ___,		// 17
  ___,	// 18
  ___,		// 19
  ___,		// 1A
  ___,	// 1B
  ___,	// 1C
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
  ___,		// 30
  ___,		// 31
  ___,		// 32
  ___,		// 33
  ___,		// 34
  ___,		// 35
  ___,		// 36
  ___,		// 37
  ___,	// 38
  ___,		// 39
  ___,		// 3A
  ___,		// 3B
  ___,		// 3C
  ___,		// 3D
  ___,		// 3E
  ___,		// 3F
  ___,		// 40
  ___,	// 41
  ___,	// 42
  ___,		// 43
  ___,		// 44
  ___,	// 45
  ___,		// 46
  ___,		// 47
  ___,		// 48
  ___,		// 49
  ___,		// 4A
  ___,	// 4B
  ___,		// 4C
  ___,		// 4D
  ___,	// 4E
  ___,		// 4F
  ___,		// 50
  ___,	// 51
  ___,	// 52
  ___,		// 53
  ___,		// 54
  ___,	// 55
  ___,		// 56
  ___,		// 57
  ___,		// 58
  ___,		// 59
  ___,		// 5A
  ___,	// 5B
  ___,		// 5C
  ___,		// 5D
  ___,	// 5E
  ___,		// 5F
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
  ___,		// 80
  ___,		// 81
  ___,		// 82
  ___,		// 83
  ___,		// 84
  ___,		// 85
  ___,		// 86
  ___,	// 87
  ___,		// 88
  ___,		// 89
  ___,		// 8A
  ___,		// 8B
  ___,		// 8C
  ___,		// 8D
  ___,		// 8E
  ___,	// 8F
  ___,		// 90
  ___,		// 91
  ___,		// 92
  ___,		// 93
  ___,		// 94
  ___,		// 95
  ___,		// 96
  ___,		// 97
  ___,		// 98
  ___,		// 99
  ___,		// 9A
  ___,		// 9B
  ___,		// 9C
  ___,		// 9D
  ___,		// 9E
  ___,		// 9A
  ___,		// A0
  ___,		// A1
  ___,		// A2
  ___,		// A3
  ___,		// A4
  ___,		// A5
  ___,		// A6
  ___,		// A7
  ___,		// a8
  ___,		// A9
  ___,		// AA
  ___,		// AB
  ___,		// AC
  ___,		// AD
  ___,		// AE
  ___,		// AF
  ___,		// B0
  ___,		// B1
  ___,		// B2
  ___,		// B3
  ___,		// B4
  ___,		// B5
  ___,		// B6
  ___,		// B7
  ___,		// B8
  ___,		// B9
  ___,		// BA
  ___,		// BB
  ___,		// BC
  ___,		// BD
  ___,		// BE
  ___,		// BF
  Subb_M,		// C0
  Cmpb_M,		// C1
  Sbcb_M,		// C2
  Addd_M,		// C3
  Andb_M,		// C4
  Bitb_M,		// C5
  Ldb_M,		// C6
  ___,		// C7
  Eorb_M,		// C8
  Adcb_M,		// C9
  Orb_M,		// CA
  Addb_M,		// CB
  Ldd_M,		// CC
  Ldq_M,		// CD
  Ldu_M,		// CE
  ___,		// CF
  Subb_D,		// D0
  Cmpb_D,		// D1
  Sbcb_D,		// D2
  Addd_D,		// D3
  Andb_D,		// D4
  Bitb_D,		// D5
  Ldb_D,		// D6
  Stb_D,		// D7
  Eorb_D,		// D8
  Adcb_D,		// D9
  Orb_D,		// DA
  Addb_D,		// DB
  Ldd_D,		// DC
  Std_D,		// DD
  Ldu_D,		// DE
  Stu_D,		// DF
  Subb_X,		// E0
  Cmpb_X,		// E1
  Sbcb_X,		// E2
  Addd_X,		// E3
  Andb_X,		// E4
  Bitb_X,		// E5
  Ldb_X,		// E6
  Stb_X,		// E7
  Eorb_X,		// E8
  Adcb_X,		// E9
  Orb_X,		// EA
  Addb_X,		// EB
  Ldd_X,		// EC
  Std_X,		// ED
  Ldu_X,		// EE
  Stu_X,		// EF
  Subb_E,		// F0
  Cmpb_E,		// F1
  Sbcb_E,		// F2
  Addd_E,		// F3
  Andb_E,		// F4
  Bitb_E,		// F5
  Ldb_E,		// F6
  Stb_E,		// F7
  Eorb_E,		// F8
  Adcb_E,		// F9
  Orb_E,		// FA
  Addb_E,		// FB
  Ldd_E,		// FC
  Std_E,		// FD
  Ldu_E,		// FE
  Stu_E,		// FF
};

void(*JmpVec2[256])(void) = {
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
  LBrn_R,		// 21
  LBhi_R,		// 22
  LBls_R,		// 23
  LBhs_R,		// 24
  LBcs_R,		// 25
  LBne_R,		// 26
  LBeq_R,		// 27
  LBvc_R,		// 28
  LBvs_R,		// 29
  LBpl_R,		// 2A
  LBmi_R,		// 2B
  LBge_R,		// 2C
  LBlt_R,		// 2D
  LBgt_R,		// 2E
  LBle_R,		// 2F
  Addr,		// 30
  Adcr,		// 31
  Subr,		// 32
  Sbcr,		// 33
  Andr,		// 34
  Orr,		// 35
  Eorr,		// 36
  Cmpr,		// 37
  Pshsw,		// 38
  Pulsw,		// 39
  Pshuw,		// 3A
  Puluw,		// 3B
  ___,		// 3C
  ___,		// 3D
  ___,		// 3E
  Swi2_I,		// 3F
  Negd_I,		// 40
  ___,		// 41
  ___,		// 42
  Comd_I,		// 43
  Lsrd_I,		// 44
  ___,		// 45
  Rord_I,		// 46
  Asrd_I,		// 47
  Asld_I,		// 48
  Rold_I,		// 49
  Decd_I,		// 4A
  ___,		// 4B
  Incd_I,		// 4C
  Tstd_I,		// 4D
  ___,		// 4E
  Clrd_I,		// 4F
  ___,		// 50
  ___,		// 51
  ___,		// 52
  Comw_I,		// 53
  Lsrw_I,		// 54
  ___,		// 55
  Rorw_I,		// 56
  ___,		// 57
  ___,		// 58
  Rolw_I,		// 59
  Decw_I,		// 5A
  ___,		// 5B
  Incw_I,		// 5C
  Tstw_I,		// 5D
  ___,		// 5E
  Clrw_I,		// 5F
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
  Subw_M,		// 80
  Cmpw_M,		// 81
  Sbcd_M,		// 82
  Cmpd_M,		// 83
  Andd_M,		// 84
  Bitd_M,		// 85
  Ldw_M,		// 86
  ___,		// 87
  Eord_M,		// 88
  Adcd_M,		// 89
  Ord_M,		// 8A
  Addw_M,		// 8B
  Cmpy_M,		// 8C
  ___,		// 8D
  Ldy_M,		// 8E
  ___,		// 8F
  Subw_D,		// 90
  Cmpw_D,		// 91
  Sbcd_D,		// 92
  Cmpd_D,		// 93
  Andd_D,		// 94
  Bitd_D,		// 95
  Ldw_D,		// 96
  Stw_D,		// 97
  Eord_D,		// 98
  Adcd_D,		// 99
  Ord_D,		// 9A
  Addw_D,		// 9B
  Cmpy_D,		// 9C
  ___,		// 9D
  Ldy_D,		// 9E
  Sty_D,		// 9F
  Subw_X,		// A0
  Cmpw_X,		// A1
  Sbcd_X,		// A2
  Cmpd_X,		// A3
  Andd_X,		// A4
  Bitd_X,		// A5
  Ldw_X,		// A6
  Stw_X,		// A7
  Eord_X,		// A8
  Adcd_X,		// A9
  Ord_X,		// AA
  Addw_X,		// AB
  Cmpy_X,		// AC
  ___,		// AD
  Ldy_X,		// AE
  Sty_X,		// AF
  Subw_E,		// B0
  Cmpw_E,		// B1
  Sbcd_E,		// B2
  Cmpd_E,		// B3
  Andd_E,		// B4
  Bitd_E,		// B5
  Ldw_E,		// B6
  Stw_E,		// B7
  Eord_E,		// B8
  Adcd_E,		// B9
  Ord_E,		// BA
  Addw_E,		// BB
  Cmpy_E,		// BC
  ___,		// BD
  Ldy_E,		// BE
  Sty_E,		// BF
  ___,		// C0
  ___,		// C1
  ___,		// C2
  ___,		// C3
  ___,		// C4
  ___,		// C5
  ___,		// C6
  ___,		// C7
  ___,		// C8
  ___,		// C9
  ___,		// CA
  ___,		// CB
  ___,		// CC
  ___,		// CD
  Lds_I,		// CE
  ___,		// CF
  ___,		// D0
  ___,		// D1
  ___,		// D2
  ___,		// D3
  ___,		// D4
  ___,		// D5
  ___,		// D6
  ___,		// D7
  ___,		// D8
  ___,		// D9
  ___,		// DA
  ___,		// DB
  Ldq_D,		// DC
  Stq_D,		// DD
  Lds_D,		// DE
  Sts_D,		// DF
  ___,		// E0
  ___,		// E1
  ___,		// E2
  ___,		// E3
  ___,		// E4
  ___,		// E5
  ___,		// E6
  ___,		// E7
  ___,		// E8
  ___,		// E9
  ___,		// EA
  ___,		// EB
  Ldq_X,		// EC
  Stq_X,		// ED
  Lds_X,		// EE
  Sts_X,		// EF
  ___,		// F0
  ___,		// F1
  ___,		// F2
  ___,		// F3
  ___,		// F4
  ___,		// F5
  ___,		// F6
  ___,		// F7
  ___,		// F8
  ___,		// F9
  ___,		// FA
  ___,		// FB
  Ldq_E,		// FC
  Stq_E,		// FD
  Lds_E,		// FE
  Sts_E,		// FF
};

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
  __declspec(dllexport) void __cdecl Page_1(unsigned char opcode) {
    JmpVec1[opcode]();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl Page_2(unsigned char opCode) //10
  {
    JmpVec2[opCode]();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl Page_3(unsigned char opCode) //11
  {
    JmpVec3[opCode]();
  }
}

void ___() {
  MessageBox(0, "Halt", "Halt", 0);
}
