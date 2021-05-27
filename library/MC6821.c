#include "MC6821.h"
#include "MC6821State.h"

#include "PAKInterface.h"
#include "fileoperations.h"

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

