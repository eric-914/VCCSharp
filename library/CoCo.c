#include "CoCo.h"

#include "defines.h"

#include "Graphics.h"
#include "MC6821.h"

CoCoState* InitializeInstance(CoCoState*);

static CoCoState* instance = InitializeInstance(new CoCoState());

extern "C" {
  __declspec(dllexport) CoCoState* __cdecl GetCoCoState() {
    return instance;
  }
}

CoCoState* InitializeInstance(CoCoState* p) {
  p->OverClock = 1;

  return p;
}
