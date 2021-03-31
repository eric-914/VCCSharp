#include "DirectDrawInternalState.h"

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState*);

static DirectDrawInternalState* instance = InitializeInternal(new DirectDrawInternalState());

static WNDCLASSEX _wcex;

extern "C" {
  __declspec(dllexport) DirectDrawInternalState* __cdecl GetDirectDrawInternalState() {
    return instance;
  }
}

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState* p) {
  p->DD = NULL;
  p->DDClipper = NULL;
  p->DDSurface = NULL;
  p->DDBackSurface = NULL;

  return p;
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl LockDDBackSurface(DDSURFACEDESC* ddsd) {
    return instance->DDBackSurface->Lock(NULL, ddsd, DDLOCK_WAIT | DDLOCK_SURFACEMEMORYPTR, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl UnlockDDBackSurface() {
    return instance->DDBackSurface->Unlock(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetDDBackSurfaceDC(HDC* hdc) {
    instance->DDBackSurface->GetDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReleaseDDBackSurfaceDC(HDC hdc) {
    instance->DDBackSurface->ReleaseDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceFlip()
  {
    return instance->DDSurface->Flip(NULL, DDFLIP_NOVSYNC | DDFLIP_DONOTWAIT); //DDFLIP_WAIT
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasDDBackSurface()
  {
    return instance->DDBackSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasDDSurface()
  {
    return instance->DDSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceBlt(RECT* rcDest, RECT* rcSrc)
  {
    return instance->DDSurface->Blt(rcDest, instance->DDBackSurface, rcSrc, DDBLT_WAIT, NULL); // DDBLT_WAIT
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDRelease()
  {
    if (instance->DD != NULL) {
      instance->DD->Release();	//Destroy the current Window
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDUnregisterClass()
  {
    UnregisterClass(_wcex.lpszClassName, _wcex.hInstance);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSurfaceSetClipper()
  {
    return instance->DDSurface->SetClipper(instance->DDClipper);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDClipperSetHWnd(HWND hWnd)
  {
    return instance->DDClipper->SetHWnd(0, hWnd);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateClipper()
  {
    return instance->DD->CreateClipper(0, &(instance->DDClipper), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDGetDisplayMode(DDSURFACEDESC* ddsd)
  {
    return instance->DD->GetDisplayMode(ddsd);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateBackSurface(DDSURFACEDESC* ddsd)
  {
    return instance->DD->CreateSurface(ddsd, &(instance->DDBackSurface), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreateSurface(DDSURFACEDESC* ddsd)
  {
    return instance->DD->CreateSurface(ddsd, &(instance->DDSurface), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSetCooperativeLevel(HWND hWnd, DWORD value)
  {
    return instance->DD->SetCooperativeLevel(hWnd, value);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDCreate()
  {
    return DirectDrawCreate(NULL, &(instance->DD), NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl DDSetDisplayMode(DWORD x, DWORD y, DWORD depth)
  {
    return instance->DD->SetDisplayMode(x, y, depth);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSurfaceGetAttachedSurface(DDSCAPS* ddsCaps)
  {
    instance->DDSurface->GetAttachedSurface(ddsCaps, &(instance->DDBackSurface));
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDSurfaceIsLost()
  {
    return instance->DDSurface->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl DDBackSurfaceIsLost()
  {
    return instance->DDBackSurface->IsLost() == DDERR_SURFACELOST;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSurfaceRestore()
  {
    instance->DDSurface->Restore();
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDBackSurfaceRestore()
  {
    instance->DDBackSurface->Restore();
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl RegisterWcex(HINSTANCE hInstance, WNDPROC lpfnWndProc, LPCSTR lpszClassName, LPCSTR lpszMenuName, UINT style, HICON hIcon, HCURSOR hCursor, HBRUSH hbrBackground)
  {
    _wcex.cbSize = sizeof(WNDCLASSEX);	//And Rebuilt it from scratch
    _wcex.hInstance = hInstance;
    _wcex.lpfnWndProc = lpfnWndProc;
    _wcex.style = style;
    _wcex.hIcon = hIcon;
    _wcex.hIconSm = hIcon;
    _wcex.hbrBackground = hbrBackground;
    _wcex.lpszClassName = lpszClassName;
    _wcex.lpszMenuName = lpszMenuName;
    _wcex.hCursor = hCursor;
    _wcex.cbClsExtra = 0;
    _wcex.cbWndExtra = 0;

    return RegisterClassEx(&_wcex);
  }
}

extern "C" {
  __declspec(dllexport) IDirectDrawPalette* __cdecl DDCreatePalette(DWORD caps, PALETTEENTRY* pal)
  {
    IDirectDrawPalette* ddPalette;		  //Needed for 8bit Palette mode

    instance->DD->CreatePalette(caps, pal, &ddPalette, NULL);

    return ddPalette;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DDSurfaceSetPalette(IDirectDrawPalette* ddPalette)
  {
    instance->DDSurface->SetPalette(ddPalette);
  }
}
