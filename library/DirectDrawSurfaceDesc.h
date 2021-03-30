#pragma once

#include "di.version.h"
#include <ddraw.h>

extern "C" __declspec(dllexport) DDSURFACEDESC * __cdecl DDSDCreate();
extern "C" __declspec(dllexport) unsigned long __cdecl DDSDGetRGBBitCount(DDSURFACEDESC * ddsd);
extern "C" __declspec(dllexport) void __cdecl DDSDSetRGBBitCount(DDSURFACEDESC * ddsd, unsigned long value);
extern "C" __declspec(dllexport) unsigned long __cdecl DDSDGetPitch(DDSURFACEDESC * ddsd);
extern "C" __declspec(dllexport) void __cdecl DDSDSetPitch(DDSURFACEDESC * ddsd, unsigned long value);
extern "C" __declspec(dllexport) BOOL __cdecl DDSDHasSurface(DDSURFACEDESC * ddsd);
extern "C" __declspec(dllexport) void* __cdecl DDSDGetSurface(DDSURFACEDESC * ddsd);
extern "C" __declspec(dllexport) void __cdecl DDSDSetdwCaps(DDSURFACEDESC * ddsd, DWORD value);

extern "C" __declspec(dllexport) void __cdecl DDSDSetdwWidth(DDSURFACEDESC* ddsd, DWORD value);
extern "C" __declspec(dllexport) void __cdecl DDSDSetdwHeight(DDSURFACEDESC* ddsd, DWORD value);
extern "C" __declspec(dllexport) void __cdecl DDSDSetdwFlags(DDSURFACEDESC* ddsd, DWORD value);
extern "C" __declspec(dllexport) void __cdecl DDSDSetdwBackBufferCount(DDSURFACEDESC* ddsd, DWORD value);
extern "C" __declspec(dllexport) DDSCAPS __cdecl DDSDGetddsCaps(DDSURFACEDESC* ddsd);
