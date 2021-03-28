#include "DirectDrawInternalState.h"

DirectDrawInternalState* InitializeInternal(DirectDrawInternalState*);

static DirectDrawInternalState* instance = InitializeInternal(new DirectDrawInternalState());

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
  __declspec(dllexport) HRESULT __cdecl LockSurface(DDSURFACEDESC* ddsd) {
    return instance->DDBackSurface->Lock(NULL, ddsd, DDLOCK_WAIT | DDLOCK_SURFACEMEMORYPTR, NULL);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl UnlockSurface() {
    return instance->DDBackSurface->Unlock(NULL);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GetSurfaceDC(HDC* hdc) {
    instance->DDBackSurface->GetDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ReleaseSurfaceDC(HDC hdc) {
    instance->DDBackSurface->ReleaseDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl SurfaceFlip()
  {
    return instance->DDSurface->Flip(NULL, DDFLIP_NOVSYNC | DDFLIP_DONOTWAIT); //DDFLIP_WAIT
  }
}

extern "C" {
  __declspec(dllexport) RECT __cdecl GetWindowDefaultSize()
  {
    return instance->WindowDefaultSize;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl HasBackSurface()
  {
    return instance->DDBackSurface != NULL;
  }
}

extern "C" {
  __declspec(dllexport) HRESULT __cdecl SurfaceBlt(RECT* rcDest, RECT* rcSrc)
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
    UnregisterClass(instance->Wcex.lpszClassName, instance->Wcex.hInstance);
  }
}

