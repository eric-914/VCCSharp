#pragma once

#include "DirectDrawInternalState.h"

extern "C" __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState();

extern "C" __declspec(dllexport) BOOL __cdecl HasDDBackSurface();
extern "C" __declspec(dllexport) HRESULT __cdecl DDClipperSetHWnd(HWND hWnd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateClipper();
extern "C" __declspec(dllexport) HRESULT __cdecl DDGetDisplayMode(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceBlt(RECT* rcDest, RECT* rcSrc);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceFlip();
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceSetClipper();
extern "C" __declspec(dllexport) HRESULT __cdecl LockDDBackSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl UnlockDDBackSurface();
extern "C" __declspec(dllexport) RECT __cdecl GetWindowDefaultSize();
extern "C" __declspec(dllexport) void __cdecl DDRelease();
extern "C" __declspec(dllexport) void __cdecl DDUnregisterClass();
extern "C" __declspec(dllexport) void __cdecl GetDDBackSurfaceDC(HDC* hdc);
extern "C" __declspec(dllexport) void __cdecl ReleaseDDBackSurfaceDC(HDC hdc);

extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateBackSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSetCooperativeLevel(HWND hWnd, DWORD value);
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreate();
extern "C" __declspec(dllexport) RECT __cdecl DDGetWindowDefaultSize();
