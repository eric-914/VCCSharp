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
  __declspec(dllexport) unsigned long __cdecl DDSDGetRGBBitCount(DDSURFACEDESC* ddsd)
  {
    return ddsd->ddpfPixelFormat.dwRGBBitCount;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetRGBBitCount(DDSURFACEDESC* ddsd, unsigned long value)
  {
    ddsd->ddpfPixelFormat.dwRGBBitCount = value;
  }
}

extern "C" {
  __declspec(dllexport) unsigned long __cdecl DDSDGetPitch(DDSURFACEDESC* ddsd)
  {
    return ddsd->lPitch;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetPitch(DDSURFACEDESC* ddsd, unsigned long value)
  {
    ddsd->lPitch = value;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSDHasSurface(DDSURFACEDESC* ddsd)
  {
    return ddsd->lpSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) void* __cdecl DDSDGetSurface(DDSURFACEDESC* ddsd)
  {
    return ddsd->lpSurface;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwCaps(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->ddsCaps.dwCaps = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwWidth(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwWidth = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwHeight(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwHeight = value;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSDSetdwFlags(DDSURFACEDESC* ddsd, DWORD value)
  {
    ddsd->dwFlags = value;
  }
}
