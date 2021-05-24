#include "../resources/resource.h"

#include "DirectDrawState.h"

DirectDrawState* InitializeInstance(DirectDrawState*);

static DirectDrawState* instance = InitializeInstance(new DirectDrawState());

extern "C" {
  __declspec(dllexport) DirectDrawState* __cdecl GetDirectDrawState() {
    return instance;
  }
}

DirectDrawState* InitializeInstance(DirectDrawState* p) {
  strcpy(p->StatusText, "");

  return p;
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK __cdecl WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
  {
    return DefWindowProc(hWnd, message, wParam, lParam);
  }
}
extern "C" {
  __declspec(dllexport) HBRUSH __cdecl DDGetBrush() {
    return (HBRUSH)GetStockObject(BLACK_BRUSH);
  }
}

extern "C" {
  __declspec(dllexport) HCURSOR __cdecl DDGetCursor(unsigned char fullscreen) {
    return fullscreen ? LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE)) : LoadCursor(NULL, IDC_ARROW);
  }
}

extern "C" {
  __declspec(dllexport) HICON __cdecl DDGetIcon(HINSTANCE resources) {
    return LoadIcon(resources, (LPCTSTR)IDI_COCO3);
  }
}
