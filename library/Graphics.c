#include "macros.h" //ARRAYCOPY

static const unsigned char  Lpf[4] = { 192, 199, 225, 225 }; // #2 is really undefined but I gotta put something here.
static const unsigned char  VcenterTable[4] = { 29, 23, 12, 12 };

typedef struct
{
  //--Still used in CoCo.SetLinesperScreen
  unsigned char  Lpf[4];
  unsigned char  VcenterTable[4];
} GraphicsState;

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
