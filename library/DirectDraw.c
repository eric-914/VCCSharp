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
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindow(HINSTANCE resources, unsigned char fullscreen)
  {
    WNDPROC proc;
    proc = WndProc;
    //proc = DefWindowProcA;

    UINT style = CS_HREDRAW | CS_VREDRAW;
    WNDPROC lpfnWndProc = proc; 
    HINSTANCE hInstance = instance->hInstance;
    HICON hIcon = LoadIcon(resources, (LPCTSTR)IDI_COCO3);
    HBRUSH hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
    LPCSTR lpszClassName = instance->AppNameText;
    LPCSTR lpszMenuName = NULL; //Menu is set on WM_CREATE
    HCURSOR hCursor = fullscreen ? LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE)) : LoadCursor(NULL, IDC_ARROW);

    //And Rebuilt it from scratch
    return RegisterWcex(hInstance, lpfnWndProc, lpszClassName, lpszMenuName, style, hIcon, hCursor, hbrBackground);
  }
}
