#include "HD6309.h"
#include "TC1014MMU.h"

#include "HD6309Macros.h"

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
  p->InInterrupt = 0;
  p->CycleCounter = 0;
  p->SyncWaiting = 0;

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

  p->IRQWaiter = 0;
  p->PendingInterrupts = 0;

  return p;
}

static unsigned char* NatEmuCycles[] =
{
  &(instance->NatEmuCycles65),
  &(instance->NatEmuCycles64),
  &(instance->NatEmuCycles32),
  &(instance->NatEmuCycles21),
  &(instance->NatEmuCycles54),
  &(instance->NatEmuCycles97),
  &(instance->NatEmuCycles85),
  &(instance->NatEmuCycles51),
  &(instance->NatEmuCycles31),
  &(instance->NatEmuCycles1110),
  &(instance->NatEmuCycles76),
  &(instance->NatEmuCycles75),
  &(instance->NatEmuCycles43),
  &(instance->NatEmuCycles87),
  &(instance->NatEmuCycles86),
  &(instance->NatEmuCycles98),
  &(instance->NatEmuCycles2726),
  &(instance->NatEmuCycles3635),
  &(instance->NatEmuCycles3029),
  &(instance->NatEmuCycles2827),
  &(instance->NatEmuCycles3726),
  &(instance->NatEmuCycles3130),
  &(instance->NatEmuCycles42),
  &(instance->NatEmuCycles53)
};

extern "C" {
  __declspec(dllexport) void __cdecl HD6309_setcc(unsigned char bincc)
  {
    instance->ccbits = bincc;

    CC_E = !!(bincc & (1 << E));
    CC_F = !!(bincc & (1 << F));
    CC_H = !!(bincc & (1 << H));
    CC_I = !!(bincc & (1 << I));
    CC_N = !!(bincc & (1 << N));
    CC_Z = !!(bincc & (1 << Z));
    CC_V = !!(bincc & (1 << V));
    CC_C = !!(bincc & (1 << C));
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl HD6309_getcc()
  {
    unsigned char bincc = 0;

#define TST(_CC, _F) if (_CC) { bincc |= (1 << _F); }

    TST(CC_E, E);
    TST(CC_F, F);
    TST(CC_H, H);
    TST(CC_I, I);
    TST(CC_N, N);
    TST(CC_Z, Z);
    TST(CC_V, V);
    TST(CC_C, C);

    return(bincc);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HD6309_setmd(unsigned char binmd)
  {
    MD_NATIVE6309 = !!(binmd & (1 << NATIVE6309));
    MD_FIRQMODE = !!(binmd & (1 << FIRQMODE));
    MD_UNDEFINED2 = !!(binmd & (1 << MD_UNDEF2));
    MD_UNDEFINED3 = !!(binmd & (1 << MD_UNDEF3));
    MD_UNDEFINED4 = !!(binmd & (1 << MD_UNDEF4));
    MD_UNDEFINED5 = !!(binmd & (1 << MD_UNDEF5));
    MD_ILLEGAL = !!(binmd & (1 << ILLEGAL));
    MD_ZERODIV = !!(binmd & (1 << ZERODIV));

    for (short i = 0; i < 24; i++)
    {
      *NatEmuCycles[i] = instance->InsCycles[MD_NATIVE6309][i];
    }
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl HD6309_getmd()
  {
    unsigned char binmd = 0;

#define TSM(_MD, _F) if (_MD) { binmd |= (1 << _F); } //--Can't use the same macro name

    TSM(MD_NATIVE6309, NATIVE6309);
    TSM(MD_FIRQMODE, FIRQMODE);
    TSM(MD_UNDEFINED2, MD_UNDEF2);
    TSM(MD_UNDEFINED3, MD_UNDEF3);
    TSM(MD_UNDEFINED4, MD_UNDEF4);
    TSM(MD_UNDEFINED5, MD_UNDEF5);
    TSM(MD_ILLEGAL, ILLEGAL);
    TSM(MD_ZERODIV, ZERODIV);

    return(binmd);
  }
}

extern "C" {
  __declspec(dllexport) unsigned short __cdecl HD6309_CalculateEA(unsigned char postbyte)
  {
    static unsigned short int ea = 0;
    static signed char byte = 0;
    static unsigned char reg;

    reg = ((postbyte >> 5) & 3) + 1;

    if (postbyte & 0x80)
    {
      switch (postbyte & 0x1F)
      {
      case 0: // Post inc by 1
        ea = PXF(reg);

        PXF(reg)++;

        instance->CycleCounter += instance->NatEmuCycles21;

        break;

      case 1: // post in by 2
        ea = PXF(reg);

        PXF(reg) += 2;

        instance->CycleCounter += instance->NatEmuCycles32;

        break;

      case 2: // pre dec by 1
        PXF(reg) -= 1;

        ea = PXF(reg);

        instance->CycleCounter += instance->NatEmuCycles21;

        break;

      case 3: // pre dec by 2
        PXF(reg) -= 2;

        ea = PXF(reg);

        instance->CycleCounter += instance->NatEmuCycles32;

        break;

      case 4: // no offset
        ea = PXF(reg);

        break;

      case 5: // B reg offset
        ea = PXF(reg) + ((signed char)B_REG);

        instance->CycleCounter += 1;

        break;

      case 6: // A reg offset
        ea = PXF(reg) + ((signed char)A_REG);

        instance->CycleCounter += 1;

        break;

      case 7: // E reg offset 
        ea = PXF(reg) + ((signed char)E_REG);

        instance->CycleCounter += 1;

        break;

      case 8: // 8 bit offset
        ea = PXF(reg) + (signed char)MemRead8(PC_REG++);

        instance->CycleCounter += 1;

        break;

      case 9: // 16 bit offset
        ea = PXF(reg) + IMMADDRESS(PC_REG);

        instance->CycleCounter += instance->NatEmuCycles43;

        PC_REG += 2;

        break;

      case 10: // F reg offset
        ea = PXF(reg) + ((signed char)F_REG);

        instance->CycleCounter += 1;

        break;

      case 11: // D reg offset 
        ea = PXF(reg) + D_REG; //Changed to unsigned 03/14/2005 NG Was signed

        instance->CycleCounter += instance->NatEmuCycles42;

        break;

      case 12: // 8 bit PC relative
        ea = (signed short)PC_REG + (signed char)MemRead8(PC_REG) + 1;

        instance->CycleCounter += 1;

        PC_REG++;

        break;

      case 13: // 16 bit PC relative
        ea = PC_REG + IMMADDRESS(PC_REG) + 2;

        instance->CycleCounter += instance->NatEmuCycles53;

        PC_REG += 2;

        break;

      case 14: // W reg offset
        ea = PXF(reg) + W_REG;

        instance->CycleCounter += 4;

        break;

      case 15: // W reg
        byte = (postbyte >> 5) & 3;

        switch (byte)
        {
        case 0: // No offset from W reg
          ea = W_REG;

          break;

        case 1: // 16 bit offset from W reg
          ea = W_REG + IMMADDRESS(PC_REG);

          PC_REG += 2;

          instance->CycleCounter += 2;

          break;

        case 2: // Post inc by 2 from W reg
          ea = W_REG;

          W_REG += 2;

          instance->CycleCounter += 1;

          break;

        case 3: // Pre dec by 2 from W reg
          W_REG -= 2;

          ea = W_REG;

          instance->CycleCounter += 1;

          break;
        }

        break;

      case 16: // W reg
        byte = (postbyte >> 5) & 3;

        switch (byte)
        {
        case 0: // Indirect no offset from W reg
          ea = MemRead16(W_REG);

          instance->CycleCounter += 3;

          break;

        case 1: // Indirect 16 bit offset from W reg
          ea = MemRead16(W_REG + IMMADDRESS(PC_REG));

          PC_REG += 2;

          instance->CycleCounter += 5;

          break;

        case 2: // Indirect post inc by 2 from W reg
          ea = MemRead16(W_REG);

          W_REG += 2;

          instance->CycleCounter += 4;

          break;

        case 3: // Indirect pre dec by 2 from W reg
          W_REG -= 2;

          ea = MemRead16(W_REG);

          instance->CycleCounter += 4;

          break;
        }
        break;

      case 17: // Indirect post inc by 2 
        ea = PXF(reg);

        PXF(reg) += 2;

        ea = MemRead16(ea);

        instance->CycleCounter += 6;

        break;

      case 18: // possibly illegal instruction
        instance->CycleCounter += 6;

        break;

      case 19: // Indirect pre dec by 2
        PXF(reg) -= 2;

        ea = MemRead16(PXF(reg));

        instance->CycleCounter += 6;

        break;

      case 20: // Indirect no offset 
        ea = MemRead16(PXF(reg));

        instance->CycleCounter += 3;

        break;

      case 21: // Indirect B reg offset
        ea = MemRead16(PXF(reg) + ((signed char)B_REG));

        instance->CycleCounter += 4;

        break;

      case 22: // indirect A reg offset
        ea = MemRead16(PXF(reg) + ((signed char)A_REG));

        instance->CycleCounter += 4;

        break;

      case 23: // indirect E reg offset
        ea = MemRead16(PXF(reg) + ((signed char)E_REG));

        instance->CycleCounter += 4;

        break;

      case 24: // indirect 8 bit offset
        ea = MemRead16(PXF(reg) + (signed char)MemRead8(PC_REG++));

        instance->CycleCounter += 4;

        break;

      case 25: // indirect 16 bit offset
        ea = MemRead16(PXF(reg) + IMMADDRESS(PC_REG));

        instance->CycleCounter += 7;

        PC_REG += 2;

        break;

      case 26: // indirect F reg offset
        ea = MemRead16(PXF(reg) + ((signed char)F_REG));

        instance->CycleCounter += 4;

        break;

      case 27: // indirect D reg offset
        ea = MemRead16(PXF(reg) + D_REG);

        instance->CycleCounter += 7;

        break;

      case 28: // indirect 8 bit PC relative
        ea = MemRead16((signed short)PC_REG + (signed char)MemRead8(PC_REG) + 1);

        instance->CycleCounter += 4;

        PC_REG++;

        break;

      case 29: //indirect 16 bit PC relative
        ea = MemRead16(PC_REG + IMMADDRESS(PC_REG) + 2);

        instance->CycleCounter += 8;

        PC_REG += 2;

        break;

      case 30: // indirect W reg offset
        ea = MemRead16(PXF(reg) + W_REG);

        instance->CycleCounter += 7;

        break;

      case 31: // extended indirect
        ea = MemRead16(IMMADDRESS(PC_REG));

        instance->CycleCounter += 8;

        PC_REG += 2;

        break;
      }
    }
    else // 5 bit offset
    {
      byte = (postbyte & 31);
      byte = (byte << 3);
      byte = byte / 8;

      ea = PXF(reg) + byte; //Was signed

      instance->CycleCounter += 1;
    }

    return(ea);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HD6309Reset()
  {
    for (char index = 0; index <= 6; index++) {		//Set all register to 0 except V
      PXF(index) = 0;
    }

    for (char index = 0; index <= 7; index++) {
      PUR(index) = 0;
    }

    CC_E = 0;
    CC_F = 1;
    CC_H = 0;
    CC_I = 1;
    CC_N = 0;
    CC_Z = 0;
    CC_V = 0;
    CC_C = 0;

    MD_NATIVE6309 = 0;
    MD_FIRQMODE = 0;
    MD_UNDEFINED2 = 0;  //UNDEFINED
    MD_UNDEFINED3 = 0;  //UNDEFINED
    MD_UNDEFINED4 = 0;  //UNDEFINED
    MD_UNDEFINED5 = 0;  //UNDEFINED
    MD_ILLEGAL = 0;
    MD_ZERODIV = 0;

    instance->mdbits = HD6309_getmd();

    instance->SyncWaiting = 0;

    DP_REG = 0;
    PC_REG = MemRead16(VRESET);	//PC gets its reset vector

    SetMapType(0);	//shouldn't be here
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl HD6309AssertInterrupt(unsigned char interrupt, unsigned char waiter) // 4 nmi 2 firq 1 irq
  {
    instance->SyncWaiting = 0;
    instance->PendingInterrupts |= (1 << (interrupt - 1));
    instance->IRQWaiter = waiter;
  }
}
