#include "di.version.h"
#include <ddraw.h>

#include <windows.h>
#include "../resources/resource.h"

//--Stuff from wingdi.h

extern "C" {
  __declspec(dllexport) void __cdecl GDIWriteTextOut(HDC hdc, unsigned short x, unsigned short y, const char* message) 
  {
    TextOut(hdc, x, y, message, (int)strlen(message));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDISetBkColor(HDC hdc, COLORREF color)
  {
    SetBkColor(hdc, color);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDISetTextColor(HDC hdc, COLORREF color)
  {
    SetTextColor(hdc, color);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GDITextOut(HDC hdc, int x, int y, char* text, int textLength)
  {
    TextOut(hdc, x, y, text, textLength);
  }
}

extern "C" {
  __declspec(dllexport) LRESULT CALLBACK __cdecl WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
  {
    return DefWindowProc(hWnd, message, wParam, lParam);
  }
}
extern "C" {
  __declspec(dllexport) HBRUSH __cdecl GDIGetBrush() {
    return (HBRUSH)GetStockObject(BLACK_BRUSH);
  }
}

extern "C" {
  __declspec(dllexport) HCURSOR __cdecl GDIGetCursor(unsigned char fullscreen) {
    return fullscreen ? LoadCursor(NULL, MAKEINTRESOURCE(IDC_NONE)) : LoadCursor(NULL, IDC_ARROW);
  }
}

extern "C" {
  __declspec(dllexport) HICON __cdecl GDIGetIcon(HINSTANCE resources) {
    return LoadIcon(resources, (LPCTSTR)IDI_COCO3);
  }
}
