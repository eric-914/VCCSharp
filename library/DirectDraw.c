//--This is just a wrapper of DirectX used
#include "di.version.h"
#include <stdio.h>
#include <ddraw.h>
#include <commctrl.h>	// Windows common controls

#include "../resources/resource.h"
#include "resource.h"

#include "DirectDraw.h"
#include "Throttle.h"
#include "Config.h"
#include "VCC.h"
#include "Graphics.h"
#include "Audio.h"
#include "MenuCallbacks.h"

#include "EmuState.h"

static POINT WindowSize;

DirectDrawState* InitializeInstance(DirectDrawState*);

static DirectDrawState* instance = InitializeInstance(new DirectDrawState());

extern "C" {
  __declspec(dllexport) DirectDrawState* __cdecl GetDirectDrawState() {
    return instance;
  }
}

DirectDrawState* InitializeInstance(DirectDrawState* p) {
  p->DD = NULL;
  p->DDClipper = NULL;
  p->DDSurface = NULL;
  p->DDBackSurface = NULL;
  p->hWndStatusBar = NULL;

  p->StatusBarHeight = 0;
  p->InfoBand = 1;
  p->ForceAspect = 1;
  p->Color = 0;

  strcpy(p->StatusText, "");

  return p;
}

extern "C" {
  __declspec(dllexport) bool __cdecl CreateDirectDrawWindow(EmuState* emuState, WNDPROC WndProc)
  {
    const unsigned char ColorValues[4] = { 0, 85, 170, 255 };

    HRESULT hr;
    DDSURFACEDESC ddsd;				// A structure to describe the surfaces we want
    RECT rStatBar = RECT();
    RECT rc = RECT();
    PALETTEENTRY pal[256];
    IDirectDrawPalette* ddPalette;		  //Needed for 8bit Palette mode

    if (GetRememberSize()) {
      POINT pp = GetIniWindowSize();

      WindowSize.x = pp.x;
      WindowSize.y = pp.y;
    }
    else {
      WindowSize.x = 640;
      WindowSize.y = 480;
    }

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0

    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure
    rc.top = 0;
    rc.left = 0;

    rc.right = WindowSize.x;
    rc.bottom = WindowSize.y;

    if (emuState->WindowHandle != NULL) //If its go a value it must be a mode switch
    {
      if (instance->DD != NULL) {
        instance->DD->Release();	//Destroy the current Window
      }

      DestroyWindow(emuState->WindowHandle);

      UnregisterClass(instance->Wcex.lpszClassName, instance->Wcex.hInstance);
    }

    instance->Wcex.cbSize = sizeof(WNDCLASSEX);	//And Rebuilt it from scratch
    instance->Wcex.style = CS_HREDRAW | CS_VREDRAW;
    instance->Wcex.lpfnWndProc = (WNDPROC)WndProc;
    instance->Wcex.cbClsExtra = 0;
    instance->Wcex.cbWndExtra = 0;
    instance->Wcex.hInstance = instance->hInstance;
    instance->Wcex.hIcon = LoadIcon(emuState->Resources, (LPCTSTR)IDI_COCO3);
    instance->Wcex.hIconSm = LoadIcon(emuState->Resources, (LPCTSTR)IDI_COCO3);
    instance->Wcex.hbrBackground = (HBRUSH)GetStockObject(BLACK_BRUSH);
    instance->Wcex.lpszClassName = instance->AppNameText;
    instance->Wcex.lpszMenuName = NULL;	//Menu is set on WM_CREATE

    if (!emuState->FullScreen)
    {
      instance->Wcex.hCursor = LoadCursor(NULL, IDC_ARROW);
    }
    else {
      instance->Wcex.hCursor = LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE));
    }

    if (!RegisterClassEx(&(instance->Wcex))) {
      return FALSE;
    }

    switch (emuState->FullScreen)
    {
    case 0: //Windowed Mode
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

      GetWindowRect(emuState->WindowHandle, &(instance->WindowDefaultSize));	// And save the Final size of the Window 
      ShowWindow(emuState->WindowHandle, SW_SHOWDEFAULT);
      UpdateWindow(emuState->WindowHandle);

      // Create an instance of a DirectDraw object
      hr = DirectDrawCreate(NULL, &(instance->DD), NULL);

      if (FAILED(hr)) {
        return FALSE;
      }

      // Initialize the DirectDraw object
      hr = instance->DD->SetCooperativeLevel(emuState->WindowHandle, DDSCL_NORMAL);	// Set DDSCL_NORMAL to use windowed mode
      if (FAILED(hr)) {
        return FALSE;
      }

      ddsd.dwFlags = DDSD_CAPS;
      ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE;

      // Create our Primary Surface
      hr = instance->DD->CreateSurface(&ddsd, &(instance->DDSurface), NULL);

      if (FAILED(hr)) {
        return FALSE;
      }

      ddsd.dwFlags = DDSD_WIDTH | DDSD_HEIGHT | DDSD_CAPS;
      ddsd.dwWidth = WindowSize.x;								// Make our off-screen surface 
      ddsd.dwHeight = WindowSize.y;
      ddsd.ddsCaps.dwCaps = DDSCAPS_VIDEOMEMORY;				// Try to create back buffer in video RAM
      hr = instance->DD->CreateSurface(&ddsd, &(instance->DDBackSurface), NULL);

      if (FAILED(hr)) {													// If not enough Video Ram 			
        ddsd.ddsCaps.dwCaps = DDSCAPS_SYSTEMMEMORY;			// Try to create back buffer in System RAM
        hr = instance->DD->CreateSurface(&ddsd, &(instance->DDBackSurface), NULL);

        if (FAILED(hr)) {
          return FALSE;								//Giving Up
        }

        MessageBox(0, "Creating Back Buffer in System Ram!\nThis will be slower", "Performance Warning", 0);
      }

      hr = instance->DD->GetDisplayMode(&ddsd);

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DD->CreateClipper(0, &(instance->DDClipper), NULL);		// Create the clipper using the DirectDraw object

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DDClipper->SetHWnd(0, emuState->WindowHandle);	// Assign your window's HWND to the clipper

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DDSurface->SetClipper(instance->DDClipper);					      // Attach the clipper to the primary surface

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DDBackSurface->Lock(NULL, &ddsd, DDLOCK_WAIT, NULL);

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DDBackSurface->Unlock(NULL);						// Unlock surface

      if (FAILED(hr)) {
        return FALSE;
      }

      break;

    case 1:	//Full Screen Mode
      ddsd.lPitch = 0;
      ddsd.ddpfPixelFormat.dwRGBBitCount = 0;

      emuState->WindowHandle = CreateWindow(instance->AppNameText, NULL, WS_POPUP | WS_VISIBLE, 0, 0, WindowSize.x, WindowSize.y, NULL, NULL, instance->hInstance, NULL);

      if (!emuState->WindowHandle) {
        return FALSE;
      }

      GetWindowRect(emuState->WindowHandle, &(instance->WindowDefaultSize));
      ShowWindow(emuState->WindowHandle, SW_SHOWMAXIMIZED);
      UpdateWindow(emuState->WindowHandle);

      hr = DirectDrawCreate(NULL, &(instance->DD), NULL);		// Initialize DirectDraw

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DD->SetCooperativeLevel(emuState->WindowHandle, DDSCL_EXCLUSIVE | DDSCL_FULLSCREEN | DDSCL_NOWINDOWCHANGES);

      if (FAILED(hr)) {
        return FALSE;
      }

      hr = instance->DD->SetDisplayMode(WindowSize.x, WindowSize.y, 32);	// Set 640x480x32 Bit full-screen mode

      if (FAILED(hr)) {
        return FALSE;
      }

      ddsd.dwFlags = DDSD_CAPS | DDSD_BACKBUFFERCOUNT;
      ddsd.ddsCaps.dwCaps = DDSCAPS_PRIMARYSURFACE | DDSCAPS_COMPLEX | DDSCAPS_FLIP;
      ddsd.dwBackBufferCount = 1;

      hr = instance->DD->CreateSurface(&ddsd, &(instance->DDSurface), NULL);

      if (FAILED(hr)) {
        return FALSE;
      }

      ddsd.ddsCaps.dwCaps = DDSCAPS_BACKBUFFER;

      instance->DDSurface->GetAttachedSurface(&ddsd.ddsCaps, &(instance->DDBackSurface));

      hr = instance->DD->GetDisplayMode(&ddsd);

      if (FAILED(hr)) {
        return FALSE;
      }

      for (unsigned short i = 0;i <= 63;i++)
      {
        pal[i + 128].peBlue = ColorValues[(i & 8) >> 2 | (i & 1)];
        pal[i + 128].peGreen = ColorValues[(i & 16) >> 3 | (i & 2) >> 1];
        pal[i + 128].peRed = ColorValues[(i & 32) >> 4 | (i & 4) >> 2];
        pal[i + 128].peFlags = PC_RESERVED | PC_NOCOLLAPSE;
      }

      instance->DD->CreatePalette(DDPCAPS_8BIT | DDPCAPS_ALLOW256, pal, &ddPalette, NULL);
      instance->DDSurface->SetPalette(ddPalette); // Set pallete for Primary surface

      break;
    }

    emuState->WindowSize.x = WindowSize.x;
    emuState->WindowSize.y = WindowSize.y;

    return TRUE;
  }
}

// Checks if the memory associated with surfaces is lost and restores if necessary.
extern "C" {
  __declspec(dllexport) void __cdecl CheckSurfaces()
  {
    if (instance->DDSurface) {	// Check the primary surface
      if (instance->DDSurface->IsLost() == DDERR_SURFACELOST) {
        instance->DDSurface->Restore();
      }
    }

    if (instance->DDBackSurface) {	// Check the back buffer
      if (instance->DDBackSurface->IsLost() == DDERR_SURFACELOST) {
        instance->DDBackSurface->Restore();
      }
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl SetStatusBarText(char* textBuffer, EmuState* emuState)
  {
    if (!emuState->FullScreen)
    {
      SendMessage(instance->hWndStatusBar, WM_SETTEXT, strlen(textBuffer), (LPARAM)(LPCSTR)textBuffer);
      SendMessage(instance->hWndStatusBar, WM_SIZE, 0, 0);
    }
    else {
      strcpy(instance->StatusText, textBuffer);
    }
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl ClearScreen()
  {
    instance->Color = 0;
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetInfoBand(unsigned char infoBand)
  {
    if (infoBand != QUERY) {
      instance->InfoBand = infoBand;
    }

    return(instance->InfoBand);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl SetAspect(unsigned char forceAspect)
  {
    if (forceAspect != QUERY) {
      instance->ForceAspect = forceAspect;
    }

    return(instance->ForceAspect);
  }
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl LockScreen(EmuState* emuState)
  {
    HRESULT	hr;
    DDSURFACEDESC ddsd;				      // A structure to describe the surfaces we want

    GraphicsState* graphicsState = GetGraphicsState();

    memset(&ddsd, 0, sizeof(ddsd));	// Clear all members of the structure to 0
    ddsd.dwSize = sizeof(ddsd);		  // The first parameter of the structure must contain the size of the structure

    CheckSurfaces();

    // Lock entire surface, wait if it is busy, return surface memory pointer
    hr = instance->DDBackSurface->Lock(NULL, &ddsd, DDLOCK_WAIT | DDLOCK_SURFACEMEMORYPTR, NULL);

    if (FAILED(hr))
    {
      return(1);
    }

    switch (ddsd.ddpfPixelFormat.dwRGBBitCount)
    {
    case 8:
      emuState->SurfacePitch = ddsd.lPitch;
      emuState->BitDepth = BIT_8;
      break;

    case 15:
    case 16:
      emuState->SurfacePitch = ddsd.lPitch / 2;
      emuState->BitDepth = BIT_16;
      break;

    case 24:
      MessageBox(0, "24 Bit color is currnetly unsupported", "Ok", 0);

      exit(0);

      emuState->SurfacePitch = ddsd.lPitch;
      emuState->BitDepth = BIT_24;
      break;

    case 32:
      emuState->SurfacePitch = ddsd.lPitch / 4;
      emuState->BitDepth = BIT_32;
      break;

    default:
      MessageBox(0, "Unsupported Color Depth!", "Error", 0);
      return 1;
    }

    if (ddsd.lpSurface == NULL) {
      MessageBox(0, "Returning NULL!!", "ok", 0);
    }

    graphicsState->pSurface8 = (unsigned char*)ddsd.lpSurface;
    graphicsState->pSurface16 = (unsigned short*)ddsd.lpSurface;
    graphicsState->pSurface32 = (unsigned int*)ddsd.lpSurface;

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DisplayFlip(EmuState* emuState)	// Double buffering flip
  {
    using namespace std;

    static HRESULT hr;
    static RECT    rcSrc;  // source blit rectangle
    static RECT    rcDest; // destination blit rectangle
    static RECT	   rect;
    static POINT   p = POINT();

    if (emuState->FullScreen) {	// if we're windowed do the blit, else just Flip
      hr = instance->DDSurface->Flip(NULL, DDFLIP_NOVSYNC | DDFLIP_DONOTWAIT); //DDFLIP_WAIT
    }
    else
    {
      p.x = 0; p.y = 0;

      // The ClientToScreen function converts the client-area coordinates of a specified point to screen coordinates.
      // in other word the client rectangle of the main windows 0, 0 (upper-left corner) 
      // in a screen x,y coords which is put back into p  
      ClientToScreen(emuState->WindowHandle, &p);  // find out where on the primary surface our window lives

      // get the actual client rectangle, which is always 0,0 - w,h
      GetClientRect(emuState->WindowHandle, &rcDest);

      // The OffsetRect function moves the specified rectangle by the specified offsets
      // add the delta screen point we got above, which gives us the client rect in screen coordinates.
      OffsetRect(&rcDest, p.x, p.y);

      // our destination rectangle is going to be 
      SetRect(&rcSrc, 0, 0, WindowSize.x, WindowSize.y);

      //if (instance->Resizeable)
      if (1) //--Currently, this is fixed at always resizable
      {
        rcDest.bottom -= instance->StatusBarHeight;

        if (instance->ForceAspect) // Adjust the Aspect Ratio if window is resized
        {
          float srcWidth = (float)WindowSize.x;
          float srcHeight = (float)WindowSize.y;
          float srcRatio = srcWidth / srcHeight;

          // change this to use the existing rcDest and the calc, w = right-left & h = bottom-top, 
          //                         because rcDest has already been converted to screen cords, right?   
          static RECT rcClient;

          GetClientRect(emuState->WindowHandle, &rcClient);  // x,y is always 0,0 so right, bottom is w,h

          rcClient.bottom -= instance->StatusBarHeight;

          float clientWidth = (float)rcClient.right;
          float clientHeight = (float)rcClient.bottom;
          float clientRatio = clientWidth / clientHeight;

          float dstWidth = 0, dstHeight = 0;

          if (clientRatio > srcRatio)
          {
            dstWidth = srcWidth * clientHeight / srcHeight;
            dstHeight = clientHeight;
          }
          else
          {
            dstWidth = clientWidth;
            dstHeight = srcHeight * clientWidth / srcWidth;
          }

          float dstX = (clientWidth - dstWidth) / 2;
          float dstY = (clientHeight - dstHeight) / 2;

          static POINT pDstLeftTop = POINT();

          pDstLeftTop.x = (long)dstX; pDstLeftTop.y = (long)dstY;

          ClientToScreen(emuState->WindowHandle, &pDstLeftTop);

          static POINT pDstRightBottom = POINT();

          pDstRightBottom.x = (long)(dstX + dstWidth); pDstRightBottom.y = (long)(dstY + dstHeight);

          ClientToScreen(emuState->WindowHandle, &pDstRightBottom);

          SetRect(&rcDest, pDstLeftTop.x, pDstLeftTop.y, pDstRightBottom.x, pDstRightBottom.y);
        }
      }
      else
      {
        // this does not seem ideal, it lets you begin to resize and immediately resizes it back ... causing a lot of flicker.
        rcDest.right = rcDest.left + WindowSize.x;
        rcDest.bottom = rcDest.top + WindowSize.y;

        GetWindowRect(emuState->WindowHandle, &rect);
        MoveWindow(emuState->WindowHandle, rect.left, rect.top, instance->WindowDefaultSize.right - instance->WindowDefaultSize.left, instance->WindowDefaultSize.bottom - instance->WindowDefaultSize.top, 1);
      }

      if (instance->DDBackSurface == NULL) {
        MessageBox(0, "Odd", "Error", 0); // yes, odd error indeed!! (??) especially since we go ahead and use it below!
      }

      hr = instance->DDSurface->Blt(&rcDest, instance->DDBackSurface, &rcSrc, DDBLT_WAIT, NULL); // DDBLT_WAIT
    }

    //--Store the updated WindowSizeX/Y for configuration, later.
    static RECT windowSize;

    GetClientRect(emuState->WindowHandle, &windowSize);

    emuState->WindowSize.x = (int)windowSize.right;
    emuState->WindowSize.y = (int)windowSize.bottom - instance->StatusBarHeight;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl UnlockScreen(EmuState* emuState)
  {
    static HRESULT hr;
    static size_t index = 0;
    static HDC hdc;

    if (emuState->FullScreen & instance->InfoBand) //Put StatusText for full screen here
    {
      instance->DDBackSurface->GetDC(&hdc);
      SetBkColor(hdc, RGB(0, 0, 0));
      SetTextColor(hdc, RGB(255, 255, 255));

      for (index = strlen(instance->StatusText); index < 132; index++) {
        instance->StatusText[index] = 32;
      }

      instance->StatusText[index] = 0;

      TextOut(hdc, 0, 0, instance->StatusText, 132);

      instance->DDBackSurface->ReleaseDC(hdc);
    }

    hr = instance->DDBackSurface->Unlock(NULL);

    DisplayFlip(emuState);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl DoCls(EmuState* emuState)
  {
    unsigned short x = 0, y = 0;

    GraphicsState* graphicsState = GetGraphicsState();

    if (LockScreen(emuState)) {
      return;
    }

    switch (emuState->BitDepth)
    {
    case BIT_8:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++) {
          graphicsState->pSurface8[x + (y * emuState->SurfacePitch)] = instance->Color | 128;
        }
      }
      break;

    case BIT_16:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++) {
          graphicsState->pSurface16[x + (y * emuState->SurfacePitch)] = instance->Color;
        }
      }
      break;

    case BIT_24:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++)
        {
          graphicsState->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = (instance->Color & 0xFF0000) >> 16;
          graphicsState->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = (instance->Color & 0x00FF00) >> 8;
          graphicsState->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = (instance->Color & 0xFF);
        }
      }
      break;

    case BIT_32:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++) {
          graphicsState->pSurface32[x + (y * emuState->SurfacePitch)] = instance->Color;
        }
      }
      break;

    default:
      return;
    }

    UnlockScreen(emuState);
  }
}

extern "C" {
  __declspec(dllexport) float __cdecl Static(EmuState* emuState)
  {
    unsigned short x = 0;
    static unsigned short y = 0;
    unsigned char temp = 0;
    static unsigned short textX = 0, textY = 0;
    static unsigned char counter = 0, counter1 = 32;
    static char phase = 1;
    static char message[] = " Signal Missing! Press F9";
    static unsigned char greyScales[4] = { 128, 135, 184, 191 };
    HDC hdc;

    GraphicsState* graphicsState = GetGraphicsState();

    LockScreen(emuState);

    if (graphicsState->pSurface32 == NULL) {
      return(0);
    }

    switch (emuState->BitDepth)
    {
    case BIT_8:
      for (y = 0; y < 480; y += 2) {
        for (x = 0; x < 160; x++) {
          temp = rand() & 3;

          graphicsState->pSurface32[x + (y * emuState->SurfacePitch >> 2)] = greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24);
          graphicsState->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 2)] = greyScales[temp] | (greyScales[temp] << 8) | (greyScales[temp] << 16) | (greyScales[temp] << 24);
        }
      }
      break;

    case BIT_16:
      for (y = 0; y < 480; y += 2) {
        for (x = 0; x < 320; x++) {
          temp = rand() & 31;

          graphicsState->pSurface32[x + (y * emuState->SurfacePitch >> 1)] = temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27);
          graphicsState->pSurface32[x + ((y + 1) * emuState->SurfacePitch >> 1)] = temp | (temp << 6) | (temp << 11) | (temp << 16) | (temp << 22) | (temp << 27);
        }
      }
      break;

    case BIT_24:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++) {
          graphicsState->pSurface8[(x * 3) + (y * emuState->SurfacePitch)] = temp;
          graphicsState->pSurface8[(x * 3) + 1 + (y * emuState->SurfacePitch)] = temp << 8;
          graphicsState->pSurface8[(x * 3) + 2 + (y * emuState->SurfacePitch)] = temp << 16;
        }
      }
      break;

    case BIT_32:
      for (y = 0; y < 480; y++) {
        for (x = 0; x < 640; x++) {
          temp = rand() & 255;

          graphicsState->pSurface32[x + (y * emuState->SurfacePitch)] = temp | (temp << 8) | (temp << 16);
        }
      }
      break;

    default:
      return(0);
    }

    instance->DDBackSurface->GetDC(&hdc);

    SetBkColor(hdc, 0);
    SetTextColor(hdc, RGB(counter1 << 2, counter1 << 2, counter1 << 2));

    TextOut(hdc, textX, textY, message, (int)strlen(message));

    counter++;
    counter1 += phase;

    if ((counter1 == 60) || (counter1 == 20)) {
      phase = -phase;
    }

    counter %= 60; //about 1 seconds

    if (!counter)
    {
      textX = rand() % 580;
      textY = rand() % 470;
    }

    instance->DDBackSurface->ReleaseDC(hdc);

    UnlockScreen(emuState);

    return(CalculateFPS());
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

extern "C" {
  __declspec(dllexport) void __cdecl FullScreenToggle(WNDPROC WndProc)
  {
    VccState* vccState = GetVccState();
    EmuState* emuState = GetEmuState();

    PauseAudio(true);

    if (!CreateDirectDrawWindow(emuState, WndProc))
    {
      MessageBox(0, "Can't rebuild primary Window", "Error", 0);

      exit(0);
    }

    InvalidateBorder();
    RefreshDynamicMenu(emuState);

    emuState->ConfigDialog = NULL;

    PauseAudio(false);
  }
}
