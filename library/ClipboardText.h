#pragma once

extern "C" __declspec(dllexport) unsigned char __cdecl ClipboardEmpty();

extern "C" __declspec(dllexport) char __cdecl PeekClipboard();
extern "C" __declspec(dllexport) void __cdecl PopClipboard();

extern "C" __declspec(dllexport) void __cdecl SetClipboardText(const char* text);
