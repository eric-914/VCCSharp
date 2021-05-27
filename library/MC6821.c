#include "MC6821.h"
#include "MC6821State.h"

#include "PAKInterface.h"

MC6821State* InitializeInstance(MC6821State*);

static MC6821State* instance = InitializeInstance(new MC6821State());

extern "C" {
  __declspec(dllexport) MC6821State* __cdecl GetMC6821State() {
    return instance;
  }
}

MC6821State* InitializeInstance(MC6821State* p) {
  p->CartInserted = 0;

  return p;
}

extern "C" {
  __declspec(dllexport) HANDLE __cdecl MC6821_OpenPrintFile(char* filename)
  {
    return CreateFile(filename, GENERIC_READ | GENERIC_WRITE, 0, 0, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, 0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetCart(unsigned char cart)
  {
    instance->CartInserted = cart;
  }
}
