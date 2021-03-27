#include "di.version.h"
#include <ddraw.h>

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
