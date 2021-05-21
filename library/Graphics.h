#pragma once

#include "GraphicsState.h"
#include "GraphicsSurfaces.h"
#include "GraphicsColors.h"

#include "EmuState.h"

extern "C" __declspec(dllexport) GraphicsState* __cdecl GetGraphicsState();
extern "C" __declspec(dllexport) GraphicsSurfaces* __cdecl GetGraphicsSurfaces();
extern "C" __declspec(dllexport) GraphicsColors* __cdecl GetGraphicsColors();

extern "C" __declspec(dllexport) void __cdecl SetupDisplay();
