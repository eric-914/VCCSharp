//--This is just a wrapper of DirectX used
#include "di.version.h"
#include <stdio.h>
#include <ddraw.h>
#include <commctrl.h>	// Windows common controls

#include "../resources/resource.h"
#include "resource.h"

#include "DirectDraw.h"
#include "Config.h"
#include "Graphics.h"
#include "Audio.h"
#include "MenuCallbacks.h"
#include "Emu.h"

#include "DirectDrawInternal.h"
#include "DirectDrawSurfaceDesc.h"
#include "GDI.h"

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
    UINT style = CS_HREDRAW | CS_VREDRAW;
    WNDPROC lpfnWndProc = (WNDPROC)WndProc;
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
