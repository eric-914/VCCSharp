#pragma once

typedef struct
{
  unsigned char BlinkState;
  unsigned char BorderChange;
  unsigned char Bpp;
  unsigned char BytesperRow;
  unsigned char CC2Offset;
  unsigned char CC2VDGMode;
  unsigned char CC2VDGPiaMode;
  unsigned char CC3BorderColor;
  unsigned char CC3Vmode;
  unsigned char CC3Vres;
  unsigned char ColorInvert;
  unsigned char CompatMode;
  unsigned char ExtendedText;
  unsigned char GraphicsMode;
  unsigned char Hoffset;
  unsigned char HorzCenter;
  unsigned char HorzOffsetReg;
  unsigned char InvertAll;
  unsigned char LinesperRow;
  unsigned char LinesperScreen;
  unsigned char LowerCase;
  unsigned char MasterMode;
  unsigned char MonType;
  unsigned char PalleteIndex;
  unsigned char Stretch;
  unsigned char TextBGColor;
  unsigned char TextBGPallete;
  unsigned char TextFGColor;
  unsigned char TextFGPallete;
  unsigned char VertCenter;
  unsigned char VresIndex;

  unsigned short PixelsperLine;
  unsigned short TagY;
  unsigned short VerticalOffsetRegister;
  unsigned short VPitch;

  unsigned int DistoOffset;
  unsigned int NewStartofVidram;
  unsigned int StartofVidram;
  unsigned int VidMask;

  unsigned char  BorderColor8;
  unsigned short BorderColor16;
  unsigned int   BorderColor32;

  unsigned char  Afacts8[2][4];
  unsigned short Afacts16[2][4];
  unsigned int   Afacts32[2][4];

  unsigned char  ColorTable16Bit[4];
  unsigned char  ColorTable32Bit[4];

  unsigned char  Pallete[16];       //Coco 3 6 bit colors
  unsigned char  Pallete8Bit[16];
  unsigned short Pallete16Bit[16];  //Color values translated to 16bit 32BIT
  unsigned int   Pallete32Bit[16];  //Color values translated to 24/32 bits

  unsigned char  PalleteLookup8[2][64];	  //0 = RGB 1=comp 8BIT
  unsigned short PalleteLookup16[2][64];	//0 = RGB 1=comp 16BIT
  unsigned int   PalleteLookup32[2][64];	//0 = RGB 1=comp 32BIT

  unsigned char  Lpf[4];
  unsigned char  VcenterTable[4];

  unsigned char* pSurface8;
  unsigned short* pSurface16;
  unsigned int* pSurface32;
} GraphicsState;
