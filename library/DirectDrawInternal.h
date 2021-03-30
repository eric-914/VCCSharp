#pragma once

#include "DirectDrawInternalState.h"

extern "C" __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState();

extern "C" __declspec(dllexport) BOOL __cdecl DDBackSurfaceIsLost();
extern "C" __declspec(dllexport) BOOL __cdecl DDSurfaceIsLost();
extern "C" __declspec(dllexport) BOOL __cdecl HasDDBackSurface();
extern "C" __declspec(dllexport) BOOL __cdecl HasDDSurface();
extern "C" __declspec(dllexport) HRESULT __cdecl DDClipperSetHWnd(HWND hWnd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreate();
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateBackSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateClipper();
extern "C" __declspec(dllexport) HRESULT __cdecl DDCreateSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDGetDisplayMode(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSetCooperativeLevel(HWND hWnd, DWORD value);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSetDisplayMode(DWORD x, DWORD y, DWORD depth);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceBlt(RECT* rcDest, RECT* rcSrc);
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceFlip();
extern "C" __declspec(dllexport) HRESULT __cdecl DDSurfaceSetClipper();
extern "C" __declspec(dllexport) HRESULT __cdecl LockDDBackSurface(DDSURFACEDESC* ddsd);
extern "C" __declspec(dllexport) HRESULT __cdecl UnlockDDBackSurface();
extern "C" __declspec(dllexport) RECT __cdecl DDGetWindowDefaultSize();
extern "C" __declspec(dllexport) RECT __cdecl GetWindowDefaultSize();
extern "C" __declspec(dllexport) void __cdecl DDBackSurfaceRestore();
extern "C" __declspec(dllexport) void __cdecl DDRelease();
extern "C" __declspec(dllexport) void __cdecl DDSurfaceGetAttachedSurface(DDSCAPS* ddsCaps);
extern "C" __declspec(dllexport) void __cdecl DDSurfaceRestore();
extern "C" __declspec(dllexport) void __cdecl DDUnregisterClass();
extern "C" __declspec(dllexport) void __cdecl GetDDBackSurfaceDC(HDC* hdc);
extern "C" __declspec(dllexport) void __cdecl ReleaseDDBackSurfaceDC(HDC hdc);

extern "C" __declspec(dllexport) BOOL __cdecl RegisterWcex(HINSTANCE hInstance, WNDPROC lpfnWndProc, LPCSTR lpszClassName, LPCSTR lpszMenuName, UINT style, HICON hIcon, HCURSOR hCursor, HBRUSH hbrBackground);
extern "C" __declspec(dllexport) IDirectDrawPalette* __cdecl DDCreatePalette(DWORD caps, PALETTEENTRY* pal);
extern "C" __declspec(dllexport) void __cdecl DDSurfaceSetPalette(IDirectDrawPalette* ddPalette);
