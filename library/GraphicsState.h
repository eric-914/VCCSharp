#pragma once

typedef struct
{
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
