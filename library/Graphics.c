#include "GraphicsState.h"

#include "macros.h" //ARRAYCOPY

static const unsigned char  Lpf[4] = { 192, 199, 225, 225 }; // #2 is really undefined but I gotta put something here.
static const unsigned char  VcenterTable[4] = { 29, 23, 12, 12 };

GraphicsState* InitializeInstance(GraphicsState*);

static GraphicsState* instance = InitializeInstance(new GraphicsState());

GraphicsState* InitializeInstance(GraphicsState* p) {
  ARRAYCOPY(Lpf);
  ARRAYCOPY(VcenterTable);

  return p;
}

extern "C" {
  __declspec(dllexport) GraphicsState* __cdecl GetGraphicsState() {
    return instance;
  }
}
