#pragma once

typedef struct {
  unsigned char  Afacts8[2][4];
  unsigned short Afacts16[2][4];
  unsigned int   Afacts32[2][4];

  unsigned char  ColorTable16Bit[4];
  unsigned char  ColorTable32Bit[4];

  unsigned char  Palette[16];       //Coco 3 6 bit colors
  unsigned char  Palette8Bit[16];
  unsigned short Palette16Bit[16];  //Color values translated to 16bit 32BIT
  unsigned int   Palette32Bit[16];  //Color values translated to 24/32 bits

  unsigned char  PaletteLookup8[2][64];	  //0 = RGB 1=comp 8BIT
  unsigned short PaletteLookup16[2][64];	//0 = RGB 1=comp 16BIT
  unsigned int   PaletteLookup32[2][64];	//0 = RGB 1=comp 32BIT
} GraphicsColors;
