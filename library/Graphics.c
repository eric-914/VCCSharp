#include "GraphicsState.h"
#include "GraphicsSurfaces.h"
#include "GraphicsColors.h"

#include "macros.h" //ARRAYCOPY

static const unsigned char  ColorTable16Bit[4] = { 0, 10, 21, 31 };	//Color brightness at 0 1 2 and 3 (2 bits)
static const unsigned char  ColorTable32Bit[4] = { 0, 85, 170, 255 };

static const unsigned char  Lpf[4] = { 192, 199, 225, 225 }; // #2 is really undefined but I gotta put something here.
static const unsigned char  VcenterTable[4] = { 29, 23, 12, 12 };

static const unsigned char  Afacts8[2][4] = { 0,     0xA4,     0x89,     0xBF, 0,      137,      164,      191 };
static const unsigned short Afacts16[2][4] = { 0,   0xF800,   0x001F,   0xFFFF, 0,   0x001F,   0xF800,   0xFFFF };
static const unsigned int   Afacts32[2][4] = { 0, 0xFF8D1F, 0x0667FF, 0xFFFFFF, 0, 0x0667FF, 0xFF8D1F, 0xFFFFFF };

GraphicsState* InitializeInstance(GraphicsState*);
GraphicsSurfaces* InitializeSurfaces(GraphicsSurfaces*);
GraphicsColors* InitializeColors(GraphicsColors*);

static GraphicsState* instance = InitializeInstance(new GraphicsState());
static GraphicsSurfaces* surfaces = InitializeSurfaces(new GraphicsSurfaces());
static GraphicsColors* colors = InitializeColors(new GraphicsColors());

GraphicsState* InitializeInstance(GraphicsState* p) {
  p->BlinkState = 1;
  p->BorderChange = 3;
  p->Bpp = 0;
  p->BytesperRow = 32;
  p->CC2Offset = 0;
  p->CC2VDGMode = 0;
  p->CC2VDGPiaMode = 0;
  p->CC3BorderColor = 0;
  p->CC3Vmode = 0;
  p->CC3Vres = 0;
  p->ColorInvert = 1;
  p->CompatMode = 0;
  p->ExtendedText = 1;
  p->GraphicsMode = 0;
  p->Hoffset = 0;
  p->HorzCenter = 0;
  p->HorzOffsetReg = 0;
  p->InvertAll = 0;
  p->LinesperRow = 1;
  p->LinesperScreen = 0;
  p->LowerCase = 0;
  p->MasterMode = 0;
  p->MonType = 1;
  p->PaletteIndex = 0;
  p->Stretch = 0;
  p->TextBGColor = 0;
  p->TextBGPalette = 0;
  p->TextFGColor = 0;
  p->TextFGPalette = 0;
  p->VertCenter = 0;
  p->VresIndex = 0;

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

GraphicsSurfaces* InitializeSurfaces(GraphicsSurfaces* p) {
  return p;
}

GraphicsColors* InitializeColors(GraphicsColors* p) {
  //ZEROARRAY(Palette);
  ARRAYCOPY(ColorTable16Bit);
  ARRAYCOPY(ColorTable32Bit);

  for (int i = 0; i < 16; i++) {
    p->Palette[i] = 0;
    p->Palette8Bit[i] = 0;
    p->Palette16Bit[i] = 0;
    p->Palette32Bit[i] = 0;
  }

  for (int i = 0; i < 2; i++) {
    for (int j = 0; j < 4; j++) {
      p->Afacts8[i][j] = Afacts8[i][j];
      p->Afacts16[i][j] = Afacts16[i][j];
      p->Afacts32[i][j] = Afacts32[i][j];
    }
  }

  for (int i = 0; i < 2; i++) {
    for (int j = 0; j < 64; j++) {
      p->PaletteLookup8[i][j] = 0;
      p->PaletteLookup16[i][j] = 0;
      p->PaletteLookup32[i][j] = 0;
    }
  }

  return p;
}

extern "C" {
  __declspec(dllexport) GraphicsState* __cdecl GetGraphicsState() {
    return instance;
  }
}

extern "C" {
  __declspec(dllexport) GraphicsSurfaces* __cdecl GetGraphicsSurfaces() {
    return surfaces;
  }
}

extern "C" {
  __declspec(dllexport) GraphicsColors* __cdecl GetGraphicsColors() {
    return colors;
  }
}
