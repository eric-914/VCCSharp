#include "TC1014MMU.h"

#include "HD6309Macros.h"

typedef union
{
  unsigned short Reg;
  struct
  {
    unsigned char lsb, msb;
  } B;
} HD6309CpuRegister;

typedef union
{
  unsigned int Reg;
  struct
  {
    unsigned short msw, lsw;
  } Word;
  struct
  {
    unsigned char mswlsb, mswmsb, lswlsb, lswmsb;	//Might be backwards
  } Byte;
} HD6309WideRegister;

typedef struct {
  unsigned char NatEmuCycles65;
  unsigned char NatEmuCycles64;
  unsigned char NatEmuCycles32;
  unsigned char NatEmuCycles21;
  unsigned char NatEmuCycles54;
  unsigned char NatEmuCycles97;
  unsigned char NatEmuCycles85;
  unsigned char NatEmuCycles51;
  unsigned char NatEmuCycles31;
  unsigned char NatEmuCycles1110;
  unsigned char NatEmuCycles76;
  unsigned char NatEmuCycles75;
  unsigned char NatEmuCycles43;
  unsigned char NatEmuCycles87;
  unsigned char NatEmuCycles86;
  unsigned char NatEmuCycles98;
  unsigned char NatEmuCycles2726;
  unsigned char NatEmuCycles3635;
  unsigned char NatEmuCycles3029;
  unsigned char NatEmuCycles2827;
  unsigned char NatEmuCycles3726;
  unsigned char NatEmuCycles3130;
  unsigned char NatEmuCycles42;
  unsigned char NatEmuCycles53;

  //--Interrupt states
  unsigned char InsCycles[2][25];

  unsigned char* NatEmuCycles[24];

  HD6309CpuRegister pc, x, y, u, s, dp, v, z;
  HD6309WideRegister q;

  unsigned char ccbits;
  unsigned char mdbits;

  unsigned char cc[8];
  unsigned int md[8];

  unsigned char* ureg8[8];
  unsigned short* xfreg16[8];

} HD6309State;

HD6309State* InitializeInstance(HD6309State*);

//TODO: Startup doesn't initialize this instance in the expected order

static HD6309State* GetInstance();

static HD6309State* instance = GetInstance();

HD6309State* GetInstance() {
  return (instance ? instance : (instance = InitializeInstance(new HD6309State())));
}

extern "C" {
  __declspec(dllexport) HD6309State* __cdecl GetHD6309State() {
    return GetInstance();
  }
}

HD6309State* InitializeInstance(HD6309State* p) {
  p->NatEmuCycles65 = 6;
  p->NatEmuCycles64 = 6;
  p->NatEmuCycles32 = 3;
  p->NatEmuCycles21 = 2;
  p->NatEmuCycles54 = 5;
  p->NatEmuCycles97 = 9;
  p->NatEmuCycles85 = 8;
  p->NatEmuCycles51 = 5;
  p->NatEmuCycles31 = 3;
  p->NatEmuCycles1110 = 11;
  p->NatEmuCycles76 = 7;
  p->NatEmuCycles75 = 7;
  p->NatEmuCycles43 = 4;
  p->NatEmuCycles87 = 8;
  p->NatEmuCycles86 = 8;
  p->NatEmuCycles98 = 9;
  p->NatEmuCycles2726 = 27;
  p->NatEmuCycles3635 = 36;
  p->NatEmuCycles3029 = 30;
  p->NatEmuCycles2827 = 28;
  p->NatEmuCycles3726 = 37;
  p->NatEmuCycles3130 = 31;
  p->NatEmuCycles42 = 4;
  p->NatEmuCycles53 = 5;

  return p;
}
