#pragma once

#include "KeyboardState.h"

#define KEY_DOWN	1
#define KEY_UP		0

/**
  Keyboard layout names used to populate the
  layout selection pull-down in the config dialog

  This of course must match keyboardlayout_e above
*/
const char* const k_keyboardLayoutNames[] =
{
  "CoCo (DECB)",
  "Natural (OS-9)",
  "Compact (OS-9)",
  "Custom"
};

//--Spelled funny because there's a GetKeyboardState() in User32.dll
extern "C" __declspec(dllexport) KeyboardState * __cdecl GetKeyBoardState();

extern "C" __declspec(dllexport) int __cdecl KeyTransCompare(const void*, const void*);

extern "C" __declspec(dllexport) unsigned char __cdecl GimeGetKeyboardInterruptState();
extern "C" __declspec(dllexport) unsigned char __cdecl vccKeyboardGetScan(unsigned char);

extern "C" __declspec(dllexport) void __cdecl GimeSetKeyboardInterruptState(unsigned char);

extern "C" __declspec(dllexport) void __cdecl vccKeyboardBuildRuntimeTable(unsigned char keyMapIndex);
extern "C" __declspec(dllexport) void __cdecl vccKeyboardHandleKey(unsigned char, unsigned char, KeyStates);
extern "C" __declspec(dllexport) void __cdecl vccKeyboardUpdateRolloverTable();
