#pragma once

#include "di.version.h"
#include <ddraw.h>

extern "C" __declspec(dllexport) DDSURFACEDESC* __cdecl DDSDCreate();
extern "C" __declspec(dllexport) unsigned long __cdecl DDSDRGBBitCount(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) unsigned long __cdecl DDSDPitch(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) BOOL __cdecl DDSDHasSurface(DDSURFACEDESC* ddsd);
