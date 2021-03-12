#pragma once

#include "GraphicsState.h"
#include "GraphicsSurfaces.h"
#include "GraphicsColors.h"

#include "EmuState.h"

extern "C" __declspec(dllexport) GraphicsState* __cdecl GetGraphicsState();
extern "C" __declspec(dllexport) GraphicsSurfaces* __cdecl GetGraphicsSurfaces();
extern "C" __declspec(dllexport) GraphicsColors* __cdecl GetGraphicsColors();

extern "C" __declspec(dllexport) unsigned char __cdecl CheckState(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetMonitorType(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetScanLines(EmuState*, unsigned char);

extern "C" __declspec(dllexport) void MakeRGBPalette(void);

extern "C" __declspec(dllexport) void __cdecl InvalidateBorder();
extern "C" __declspec(dllexport) void __cdecl MakeCMPPalette(int);
extern "C" __declspec(dllexport) void __cdecl SetCompatMode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeBorderColor(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeHorzOffset(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimePalette(unsigned char, unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeVdgMode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeVdgMode2(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeVdgOffset(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeVmode(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetGimeVres(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetPaletteType();
extern "C" __declspec(dllexport) void __cdecl SetPaletteType();
extern "C" __declspec(dllexport) void __cdecl SetVerticalOffsetRegister(unsigned short);
extern "C" __declspec(dllexport) void __cdecl SetVideoBank(unsigned char);
extern "C" __declspec(dllexport) void __cdecl SetupDisplay(void);
