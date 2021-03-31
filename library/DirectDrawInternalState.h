#pragma once

#include "di.version.h"
#include <ddraw.h>

typedef struct {
  //Global Variables for Direct Draw functions
  LPDIRECTDRAW        DD;             // The DirectDraw object
  LPDIRECTDRAWCLIPPER DDClipper;      // Clipper for primary surface
  LPDIRECTDRAWSURFACE DDSurface;      // Primary surface
  LPDIRECTDRAWSURFACE DDBackSurface;  // Back surface
} DirectDrawInternalState;
