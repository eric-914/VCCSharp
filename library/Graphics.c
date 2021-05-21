#include <windows.h>
#include <math.h>

#include "GraphicsState.h"
#include "GraphicsSurfaces.h"
#include "GraphicsColors.h"

#include "EmuState.h"

#include "CoCo.h"
#include "Config.h"
#include "DirectDraw.h"

#include "macros.h"

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

extern "C" {
  __declspec(dllexport) void __cdecl SetupDisplay()
  {
    static unsigned char CC2Bpp[8] = { 1,0,1,0,1,0,1,0 };
    static unsigned char CC2LinesperRow[8] = { 12,3,3,2,2,1,1,1 };
    static unsigned char CC3LinesperRow[8] = { 1,1,2,8,9,10,11,200 };
    static unsigned char CC2BytesperRow[8] = { 16,16,32,16,32,16,32,32 };
    static unsigned char CC3BytesperRow[8] = { 16,20,32,40,64,80,128,160 };
    static unsigned char CC3BytesperTextRow[8] = { 32,40,32,40,64,80,64,80 };
    static unsigned char CC2PaletteSet[4] = { 8,0,10,4 };
    static unsigned char CCPixelsperByte[4] = { 8,4,2,2 };
    static unsigned char ColorSet = 0, Temp1;

    instance->ExtendedText = 1;

    switch (instance->CompatMode)
    {
    case 0:		//Color Computer 3 Mode
      instance->NewStartofVidram = instance->VerticalOffsetRegister * 8;
      instance->GraphicsMode = (instance->CC3Vmode & 128) >> 7;
      instance->VresIndex = (instance->CC3Vres & 96) >> 5;
      CC3LinesperRow[7] = instance->LinesperScreen;	// For 1 pixel high modes
      instance->Bpp = instance->CC3Vres & 3;
      instance->LinesperRow = CC3LinesperRow[instance->CC3Vmode & 7];
      instance->BytesperRow = CC3BytesperRow[(instance->CC3Vres & 28) >> 2];
      instance->PaletteIndex = 0;

      if (!instance->GraphicsMode)
      {
        if (instance->CC3Vres & 1) {
          instance->ExtendedText = 2;
        }

        instance->Bpp = 0;
        instance->BytesperRow = CC3BytesperTextRow[(instance->CC3Vres & 28) >> 2];
      }

      break;

    case 1:					//Color Computer 2 Mode
      instance->CC3BorderColor = 0;	//Black for text modes
      instance->BorderChange = 3;
      instance->NewStartofVidram = (512 * instance->CC2Offset) + (instance->VerticalOffsetRegister & 0xE0FF) * 8;
      instance->GraphicsMode = (instance->CC2VDGPiaMode & 16) >> 4; //PIA Set on graphics clear on text
      instance->VresIndex = 0;
      instance->LinesperRow = CC2LinesperRow[instance->CC2VDGMode];

      if (instance->GraphicsMode)
      {
        ColorSet = (instance->CC2VDGPiaMode & 1);

        if (ColorSet == 0) {
          instance->CC3BorderColor = 18; //18 Bright Green
        }
        else {
          instance->CC3BorderColor = 63; //63 White 
        }

        instance->BorderChange = 3;
        instance->Bpp = CC2Bpp[(instance->CC2VDGPiaMode & 15) >> 1];
        instance->BytesperRow = CC2BytesperRow[(instance->CC2VDGPiaMode & 15) >> 1];
        Temp1 = (instance->CC2VDGPiaMode & 1) << 1 | (instance->Bpp & 1);
        instance->PaletteIndex = CC2PaletteSet[Temp1];
      }
      else
      {	//Setup for 32x16 text Mode
        instance->Bpp = 0;
        instance->BytesperRow = 32;
        instance->InvertAll = (instance->CC2VDGPiaMode & 4) >> 2;
        instance->LowerCase = (instance->CC2VDGPiaMode & 2) >> 1;
        ColorSet = (instance->CC2VDGPiaMode & 1);
        Temp1 = ((ColorSet << 1) | instance->InvertAll);

        switch (Temp1)
        {
        case 0:
          instance->TextFGPalette = 12;
          instance->TextBGPalette = 13;
          break;

        case 1:
          instance->TextFGPalette = 13;
          instance->TextBGPalette = 12;
          break;

        case 2:
          instance->TextFGPalette = 14;
          instance->TextBGPalette = 15;
          break;

        case 3:
          instance->TextFGPalette = 15;
          instance->TextBGPalette = 14;
          break;
        }
      }

      break;
    }

    //gs->ColorInvert = (gs->CC3Vmode & 32) >> 5;
    instance->LinesperScreen = instance->Lpf[instance->VresIndex];

    SetLinesperScreen(instance->VresIndex);

    instance->VertCenter = instance->VcenterTable[instance->VresIndex] - 4; //4 unrendered top lines
    instance->PixelsperLine = instance->BytesperRow * CCPixelsperByte[instance->Bpp];

    if (instance->PixelsperLine % 40)
    {
      instance->Stretch = (512 / instance->PixelsperLine) - 1;
      instance->HorzCenter = 64;
    }
    else
    {
      instance->Stretch = (640 / instance->PixelsperLine) - 1;
      instance->HorzCenter = 0;
    }

    instance->VPitch = instance->BytesperRow;

    if (instance->HorzOffsetReg & 128) {
      instance->VPitch = 256;
    }

    instance->BorderColor8 = ((instance->CC3BorderColor & 63) | 128);
    instance->BorderColor16 = colors->PaletteLookup16[instance->MonType][instance->CC3BorderColor & 63];
    instance->BorderColor32 = colors->PaletteLookup32[instance->MonType][instance->CC3BorderColor & 63];

    instance->NewStartofVidram = (instance->NewStartofVidram & instance->VidMask) + instance->DistoOffset; //DistoOffset for 2M configuration
    instance->MasterMode = (instance->GraphicsMode << 7) | (instance->CompatMode << 6) | ((instance->Bpp & 3) << 4) | (instance->Stretch & 15);
  }
}
