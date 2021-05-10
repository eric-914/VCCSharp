#include "MC6809.h"

extern "C" {
  __declspec(dllexport) MC6809State* __cdecl GetMC6809State() {
    return new MC6809State();
  }
}
