#pragma once

#include "di.version.h"
#include <ddraw.h>

extern "C" __declspec(dllexport) void __cdecl GDIWriteTextOut(HDC hdc, unsigned short x, unsigned short y, const char* message);
extern "C" __declspec(dllexport) void __cdecl GDISetBkColor(HDC hdc, COLORREF color);
extern "C" __declspec(dllexport) void __cdecl GDISetTextColor(HDC hdc, COLORREF color);
extern "C" __declspec(dllexport) void __cdecl GDITextOut(HDC hdc, int x, int y, char* text, int textLength);
