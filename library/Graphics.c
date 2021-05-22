#include "GraphicsState.h"

#include "macros.h" //ARRAYCOPY

static const unsigned char  Lpf[4] = { 192, 199, 225, 225 }; // #2 is really undefined but I gotta put something here.
static const unsigned char  VcenterTable[4] = { 29, 23, 12, 12 };

GraphicsState* InitializeInstance(GraphicsState*);

static GraphicsState* instance = InitializeInstance(new GraphicsState());

GraphicsState* InitializeInstance(GraphicsState* p) {
  p->PixelsperLine = 0;
  p->TagY = 0;
  p->VerticalOffsetRegister = 0;
  p->VPitch = 32;

  p->DistoOffset = 0;
  p->NewStartofVidram = 0;
  p->StartofVidram = 0;
  p->VidMask = 0x1FFFF;

  p->BorderColor8 = 0;
  p->BorderColor16 = 0;
  p->BorderColor32 = 0;

  ARRAYCOPY(Lpf);
  ARRAYCOPY(VcenterTable);

  return p;
}

extern "C" {
  __declspec(dllexport) GraphicsState* __cdecl GetGraphicsState() {
    return instance;
  }
}
