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
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindowedMode(EmuState* emuState)
  {
    DDSURFACEDESC ddsd;				// A structure to describe the surfaces we want
    HRESULT hr;
    RECT rc = RECT();
    RECT rStatBar = RECT();

    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0
    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure

    rc.top = 0;
    rc.left = 0;
    rc.right = instance->WindowSize.x;
    rc.bottom = instance->WindowSize.y;

    // Calculates the required size of the window rectangle, based on the desired client-rectangle size
      // The window rectangle can then be passed to the CreateWindow function to create a window whose client area is the desired size.
    AdjustWindowRect(&rc, WS_OVERLAPPEDWINDOW, TRUE);

    // We create the Main window 
    emuState->WindowHandle = CreateWindow(instance->AppNameText, instance->TitleBarText, WS_OVERLAPPEDWINDOW, CW_USEDEFAULT, 0,
      rc.right - rc.left, rc.bottom - rc.top,
      NULL, NULL, instance->hInstance, NULL);

    if (!emuState->WindowHandle) {	// Can't create window
      return FALSE;
    }

    // Create the Status Bar Window at the bottom
    instance->hWndStatusBar = CreateStatusWindow(WS_CHILD | WS_VISIBLE | WS_CLIPSIBLINGS | CCS_BOTTOM, "Ready", emuState->WindowHandle, 2);

    if (!instance->hWndStatusBar) { // Can't create Status bar
      return FALSE;
    }

    // Retrieves the dimensions of the bounding rectangle of the specified window
    // The dimensions are given in screen coordinates that are relative to the upper-left corner of the screen.
    GetWindowRect(instance->hWndStatusBar, &rStatBar); // Get the size of the Status bar

    instance->StatusBarHeight = rStatBar.bottom - rStatBar.top; // Calculate its height

    // Get the size of main window and add height of status bar then resize Main
    // re-using rStatBar RECT even though it's the main window
    GetWindowRect(emuState->WindowHandle, &rStatBar);
    MoveWindow(emuState->WindowHandle, rStatBar.left, rStatBar.top, // using MoveWindow to resize 
      rStatBar.right - rStatBar.left, (rStatBar.bottom + instance->StatusBarHeight) - rStatBar.top,
      1);

    SendMessage(instance->hWndStatusBar, WM_SIZE, 0, 0); // Redraw Status bar in new position

    GetWindowRect(emuState->WindowHandle, &(ddState->WindowDefaultSize));	// And save the Final size of the Window 
    ShowWindow(emuState->WindowHandle, SW_SHOWDEFAULT);
    UpdateWindow(emuState->WindowHandle);

    // Create an instance of a DirectDraw object
    hr = DirectDrawCreate(NULL, &(ddState->DD), NULL);

    if (FAILED(hr)) {
      return FALSE;
    }

    // Initialize the DirectDraw object
    hr = ddState->DD->SetCooperativeLevel(emuState->WindowHandle, DDSCL_NORMAL);	// Set DDSCL_NORMAL to use windowed mode
    if (FAILED(hr)) {
      return FALSE;
    }

    ddsd.dwFlags = DDSD_CAPS;
    ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;

    // Create our Primary Surface
    hr = ddState->DD->CreateSurface(&ddsd, &(ddState->DDSurface), NULL);

    if (FAILED(hr)) {
      return FALSE;
    }

    ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_CAPS;
    ddsd.dwWidth = instance->WindowSize.x;								// Make our off-screen surface 
    ddsd.dwHeight = instance->WindowSize.y;
    ddsd.ddsCaps.dwCaps = DDSCAPS_VIDEOMEMORY;				// Try to create back buffer in video RAM
    hr = ddState->DD->CreateSurface(&ddsd, &(ddState->DDBackSurface), NULL);

    if (FAILED(hr)) {													// If not enough Video Ram 			
      ddsd.ddsCaps.dwCaps = DDSCAPS_SYSTEMMEMORY;			// Try to create back buffer in System RAM
      hr = ddState->DD->CreateSurface(&ddsd, &(ddState->DDBackSurface), NULL);

      if (FAILED(hr)) {
        return FALSE;								//Giving Up
      }

      MessageBox(0, "Creating Back Buffer in System Ram!\nThis will be slower", "Performance Warning", 0);
    }

    hr = ddState->DD->GetDisplayMode(&ddsd);

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DD->CreateClipper(0, &(ddState->DDClipper), NULL);		// Create the clipper using the DirectDraw object

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DDClipper->SetHWnd(0, emuState->WindowHandle);	// Assign your window's HWND to the clipper

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DDSurface->SetClipper(ddState->DDClipper);					      // Attach the clipper to the primary surface

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DDBackSurface->Lock(NULL, &ddsd, DDLOCK_WAIT, NULL);

    if (FAILED(hr)) {
      return FALSE;
    }

    hr = ddState->DDBackSurface->Unlock(NULL);						// Unlock surface

    if (FAILED(hr)) {
      return FALSE;
    }

    return TRUE;
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindowFullScreen(EmuState* emuState)
  {
    const unsigned char ColorValues[4] = { 0, 85, 170, 255 };

    DDSURFACEDESC ddsd;				// A structure to describe the surfaces we want
    HRESULT hr;
    PALETTEENTRY pal[256];
    IDirectDrawPalette* ddPalette;		  //Needed for 8bit Palette mode

    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0
    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure
    ddsd.lPitch = 0;
    ddsd.ddpfPixelFormat.dwRGBBitCount = 0;

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

    ddsd.dwFlags = DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
    ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE | DDSCAPS_COMPLEX | DDSCAPS_FLIP;
    ddsd.dwBackBufferCount = 1;

    hr = ddState->DD->CreateSurface(&ddsd, &(ddState->DDSurface), NULL);

    if (FAILED(hr)) {
      return FALSE;
    }

    ddsd.ddsCaps.dwCaps = DDSCAPS_BACKBUFFER;

    ddState->DDSurface->GetAttachedSurface(&ddsd.ddsCaps, &(ddState->DDBackSurface));

    hr = ddState->DD->GetDisplayMode(&ddsd);

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
  __declspec(dllexport) BOOL __cdecl CreateDirectDrawWindow(EmuState* emuState)
  {
    DDSURFACEDESC ddsd;				// A structure to describe the surfaces we want
    RECT rStatBar = RECT();
    RECT rc = RECT();

    DirectDrawInternalState* ddState = GetDirectDrawInternalState();

    if (GetRememberSize()) {
      POINT pp = GetIniWindowSize();

      instance->WindowSize.x = pp.x;
      instance->WindowSize.y = pp.y;
    }
    else {
      instance->WindowSize.x = 640;
      instance->WindowSize.y = 480;
    }

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0

    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure
    rc.top = 0;
    rc.left = 0;

    rc.right = instance->WindowSize.x;
    rc.bottom = instance->WindowSize.y;

    if (emuState->WindowHandle != NULL) //If its go a value it must be a mode switch
    {
      if (ddState->DD != NULL) {
        ddState->DD->Release();	//Destroy the current Window
      }

      DestroyWindow(emuState->WindowHandle);

      UnregisterClass(ddState->Wcex.lpszClassName, ddState->Wcex.hInstance);
    }

    ddState->Wcex.cbSize = sizeof(WNDCLASSEX);	//And Rebuilt it from scratch
    ddState->Wcex.style = CS_HREDRAW | CS_VREDRAW;
    ddState->Wcex.lpfnWndProc = (WNDPROC)WndProc;
    ddState->Wcex.cbClsExtra = 0;
    ddState->Wcex.cbWndExtra = 0;
    ddState->Wcex.hInstance = instance->hInstance;
    ddState->Wcex.hIcon = LoadIcon(emuState->Resources, (LPCTSTR)IDI_COCO3);
    ddState->Wcex.hIconSm = LoadIcon(emuState->Resources, (LPCTSTR)IDI_COCO3);
    ddState->Wcex.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
    ddState->Wcex.lpszClassName = instance->AppNameText;
    ddState->Wcex.lpszMenuName = NULL;	//Menu is set on WM_CREATE

    if (!emuState->FullScreen)
    {
      ddState->Wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    }
    else {
      ddState->Wcex.hCursor = LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE));
    }

    if (!RegisterClassEx(&(ddState->Wcex))) {
      return FALSE;
    }

    switch (emuState->FullScreen)
    {
    case 0: //Windowed Mode
      if (!CreateDirectDrawWindowedMode(emuState)) {
        return FALSE;
      }
      break;

    case 1:	//Full Screen Mode
      if (!CreateDirectDrawWindowFullScreen(emuState)) {
        return FALSE;
      }
      break;
    }

    emuState->WindowSize.x = instance->WindowSize.x;
    emuState->WindowSize.y = instance->WindowSize.y;

    return TRUE;
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

extern "C" {
  __declspec(dllexport) void __cdecl SetSurfaces(DDSURFACEDESC* ddsd)
  {
    GraphicsSurfaces* graphicsSurfaces = GetGraphicsSurfaces();

    graphicsSurfaces->pSurface8 = (unsigned char*)ddsd->lpSurface;
    graphicsSurfaces->pSurface16 = (unsigned short*)ddsd->lpSurface;
    graphicsSurfaces->pSurface32 = (unsigned int*)ddsd->lpSurface;
  }
}

//Put StatusText for full screen here
extern "C" {
  __declspec(dllexport) void __cdecl WriteStatusText(char* statusText)
  {
    static HDC hdc;

    int len = (int)strlen(statusText);
    for (int index = len; index < 132; index++) {
      statusText[index] = 32;
    }

    statusText[len + 1] = 0;

    GetSurfaceDC(&hdc);

    GDISetBkColor(hdc, RGB(0, 0, 0));
    GDISetTextColor(hdc, RGB(255, 255, 255));
    GDITextOut(hdc, 0, 0, statusText, 132);

    ReleaseSurfaceDC(hdc);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl InitDirectDraw(HINSTANCE hInstance, HINSTANCE hResources)
  {
    instance->hInstance = hInstance;

    ResourceAppTitle(hResources, instance->TitleBarText);
    ResourceAppTitle(hResources, instance->AppNameText);

    return TRUE;
  }
}
