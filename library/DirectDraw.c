//--This is just a wrapper of DirectX used
#include "di.version.h"
#include <ddraw.h>

#include "../resources/resource.h"

#include "DirectDraw.h"
#include "DirectDrawInternal.h"

#include "ProcessMessage.h"

DirectDrawState* InitializeInstance(DirectDrawState*);

static DirectDrawState* instance = InitializeInstance(new DirectDrawState());

extern "C" {
  __declspec(dllexport) DirectDrawState* __cdecl GetDirectDrawState() {
    return instance;
  }
}

DirectDrawState* InitializeInstance(DirectDrawState* p) {
  p->hWndStatusBar = NULL;

  p->StatusBarHeight = 0;
  p->InfoBand = 1;
  p->ForceAspect = 1;
  p->Color = 0;

  strcpy(p->StatusText, "");

  return p;
}

/*--------------------------------------------------------------------------*/
// The Window Procedure
LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
  ProcessMessage(hWnd, message, wParam, lParam);

  return DefWindowProc(hWnd, message, wParam, lParam);
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
    
extern "C" {
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindow(HINSTANCE hInstance, HICON hIcon, HCURSOR hCursor, HBRUSH hBrush, UINT style, LPCSTR lpszClassName, LPCSTR lpszMenuName)
  {
    //WNDPROC lpfnWndProc = WndProc; 

    return RegisterWcex(
      hInstance,
      WndProc, 
      lpszClassName,
      lpszMenuName, 
      style,
      hIcon,
      hCursor,
      hBrush
    );
  }
}
