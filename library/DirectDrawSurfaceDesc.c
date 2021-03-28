#include "di.version.h"
#include <ddraw.h>

//--Some wrappers around the DDSURFACEDESC struct

static DDSURFACEDESC ddsd;

extern "C" {
  __declspec(dllexport) DDSURFACEDESC* __cdecl DDSDCreate()
  {
    ddsd = DDSURFACEDESC();

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0
    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure

    return &ddsd;
  }
}

extern "C" {
  __declspec(dllexport) unsigned long __cdecl DDSDRGBBitCount(DDSURFACEDESC* ddsd)
  {
    return ddsd->ddpfPixelFormat.dwRGBBitCount;
  }
}

extern "C" {
  __declspec(dllexport) unsigned long __cdecl DDSDPitch(DDSURFACEDESC* ddsd)
  {
    return ddsd->lPitch;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSDHasSurface(DDSURFACEDESC* ddsd)
  {
    return ddsd->lpSurface != NULL;
  }
}
