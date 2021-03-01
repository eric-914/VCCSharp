#pragma once

using namespace std;

#include "ClipboardState.h"

extern "C" __declspec(dllexport) ClipboardState * __cdecl GetClipboardState();

extern "C" __declspec(dllexport) bool __cdecl ClipboardEmpty();
extern "C" __declspec(dllexport) int __cdecl GetCurrentKeyMap();
extern "C" __declspec(dllexport) char __cdecl PeekClipboard();
extern "C" __declspec(dllexport) void __cdecl PopClipboard();
extern "C" __declspec(dllexport) bool __cdecl SetClipboard(const char*);

extern "C" __declspec(dllexport) void __cdecl PasteText();
extern "C" __declspec(dllexport) void __cdecl PasteBASIC();
extern "C" __declspec(dllexport) void __cdecl PasteBASICWithNew();

extern "C" __declspec(dllexport) void CopyText();
