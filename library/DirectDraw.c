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
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindowFullScreen(EmuState* emuState, DDSURFACEDESC* ddsd)
  {
    const unsigned char ColorValues[4] = { 0, 85, 170, 255 };

    HRESULT hr;
    PALETTEENTRY pal[256];
    IDirectDrawPalette* ddPalette;		  //Needed for 8bit Palette mode

    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    DDSDSetPitch(ddsd, 0);
    DDSDSetRGBBitCount(ddsd, 0);

    emuState->WindowHandle = CreateWindow(instance->AppNameText, NULL, WS_POPUP | WS_VISIBLE, 0, 0, instance->WindowSize.x, instance->WindowSize.y, NULL, NULL, instance->hInstance, NULL);

    if (!emuState->WindowHandle) {
      return FALSE;
    }

    GetWindowRect(emuState->WindowHandle, &(ddState->WindowDefaultSize));
    ShowWindow(emuState->WindowHandle, SW_SHOWMAXIMIZED);
    UpdateWindow(emuState->WindowHandle);

    hr = DirectDrawCreate(NULL, &(ddState->DD), NULL);		// Initialize DirectDraw

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DD->SetCooperativeLevel(emuState->WindowHandle, DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN | DDSCL_NOWINDOWCHANGES);

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DD->SetDisplayMode(instance->WindowSize.x, instance->WindowSize.y, 32);	// Set 640x480x32 Bit full-screen mode

    if (FAILED(hr)) {
      return FALSE;
    }

    ddsd->dwFlags = DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
    ddsd->ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE | DDSCAPS_COMPLEX | DDSCAPS_FLIP;
    ddsd->dwBackBufferCount = 1;

    hr = ddState->DD->CreateSurface(ddsd, &(ddState->DDSurface), NULL);

    if (FAILED(hr)) {
      return FALSE;
    }

    ddsd->ddsCaps.dwCaps = DDSCAPS_BACKBUFFER;

    ddState->DDSurface->GetAttachedSurface(&(ddsd->ddsCaps), &(ddState->DDBackSurface));

    hr = ddState->DD->GetDisplayMode(ddsd);

    if (FAILED(hr)) {
      return FALSE;
    }

    for (unsigned short i = 0; i <= 63; i++)
    {
      pal[i + 128].peBlue = ColorValues[(i & 8) >> 2 | (i & 1)];
      pal[i + 128].peGreen = ColorValues[(i & 16) >> 3 | (i & 2) >> 1];
      pal[i + 128].peRed = ColorValues[(i & 32) >> 4 | (i & 4) >> 2];
      pal[i + 128].peFlags = PC_RESERVED | PC_NOCOLLAPSE;
    }

    ddState->DD->CreatePalette(DDPCAPS_8BIT | DDPCAPS_ALLOW256, pal, &ddPalette, NULL);
    ddState->DDSurface->SetPalette(ddPalette); // Set pallete for Primary surface

    return true;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindow(HINSTANCE resources, unsigned char fullscreen)
  {
    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    ddState->Wcex.cbSize = sizeof(WNDCLASSEX);	//And Rebuilt it from scratch
    ddState->Wcex.style = CS_HREDRAW | CS_VREDRAW;
    ddState->Wcex.lpfnWndProc = (WNDPROC)WndProc;
    ddState->Wcex.cbClsExtra = 0;
    ddState->Wcex.cbWndExtra = 0;
    ddState->Wcex.hInstance = instance->hInstance;
    ddState->Wcex.hIcon = LoadIcon(resources, (LPCTSTR)IDI_COCO3);
    ddState->Wcex.hIconSm = LoadIcon(resources, (LPCTSTR)IDI_COCO3);
    ddState->Wcex.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
    ddState->Wcex.lpszClassName = instance->AppNameText;
    ddState->Wcex.lpszMenuName = NULL;	//Menu is set on WM_CREATE

    if (!fullscreen)
    {
      ddState->Wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    }
    else {
      ddState->Wcex.hCursor = LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE));
    }

    return RegisterClassEx(&(ddState->Wcex));
  }
}

// Checks if the memory associated with surfaces is lost and restores if necessary.
extern "C" {
  __declspec(dllexport) void __cdecl CheckSurfaces()
  {
    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    if (ddState->DDSurface) {	// Check the primary surface
      if (ddState->DDSurface->IsLost() == DDERR_SURFACELOST) {
        ddState->DDSurface->Restore();
      }
    }

    if (ddState->DDBackSurface) {	// Check the back buffer
      if (ddState->DDBackSurface->IsLost() == DDERR_SURFACELOST) {
        ddState->DDBackSurface->Restore();
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetStatusBarText(char* textBuffer)
  {
    SendMessage(instance->hWndStatusBar, WM_SETTEXT, strlen(textBuffer), (LPARAM)(LPCSTR)textBuffer);
    SendMessage(instance->hWndStatusBar, WM_SIZE, 0, 0);
  }
}
