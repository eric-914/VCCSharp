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
  unsigned char PaletteIndex;
  unsigned char Stretch;
  unsigned char TextBGColor;
  unsigned char TextBGPalette;
  unsigned char TextFGColor;
  unsigned char TextFGPalette;
  unsigned char VertCenter;
  unsigned char VresIndex;

  unsigned short PixelsperLine;
  unsigned short TagY;
  unsigned short VerticalOffsetRegister;
  unsigned short VPitch;

  unsigned long DistoOffset;
  unsigned long NewStartofVidram;
  unsigned long StartofVidram;
  unsigned long VidMask;

  unsigned char  BorderColor8;
  unsigned short BorderColor16;
  unsigned long  BorderColor32;

  unsigned char  Lpf[4];
  unsigned char  VcenterTable[4];
} GraphicsState;
