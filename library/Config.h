#pragma once

#include <richedit.h>

#include "defines.h"

#include "CmdLineArguments.h"
#include "ConfigState.h"
#include "SystemState.h"

extern "C" __declspec(dllexport) ConfigState * __cdecl GetConfigState();

extern "C" __declspec(dllexport) POINT __cdecl GetIniWindowSize();

extern "C" __declspec(dllexport) char* __cdecl AppDirectory();
extern "C" __declspec(dllexport) char* __cdecl BasicRomName(void);

extern "C" __declspec(dllexport) int __cdecl GetCurrentKeyboardLayout();
extern "C" __declspec(dllexport) int __cdecl GetPaletteType();
extern "C" __declspec(dllexport) int __cdecl GetRememberSize();
extern "C" __declspec(dllexport) int __cdecl SelectSerialCaptureFile(SystemState*, char*);

extern "C" __declspec(dllexport) unsigned char __cdecl GetProfileByte(LPCSTR, LPCSTR, int);
extern "C" __declspec(dllexport) unsigned char __cdecl ReadIniFile(SystemState*);
extern "C" __declspec(dllexport) unsigned char __cdecl TranslateDisplay2Scan(LRESULT);
extern "C" __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int);
extern "C" __declspec(dllexport) void __cdecl WriteIniFile();

extern "C" __declspec(dllexport) unsigned short __cdecl GetProfileShort(LPCSTR, LPCSTR, int);

extern "C" __declspec(dllexport) void __cdecl BuildTransDisp2ScanTable();
extern "C" __declspec(dllexport) void __cdecl DecreaseOverclockSpeed(SystemState*);
extern "C" __declspec(dllexport) void __cdecl GetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl GetProfileText(LPCSTR, LPCSTR, LPCSTR, LPSTR);
extern "C" __declspec(dllexport) void __cdecl IncreaseOverclockSpeed(SystemState*);
extern "C" __declspec(dllexport) void __cdecl LoadConfig(SystemState*, CmdLineArguments);
extern "C" __declspec(dllexport) void __cdecl RefreshJoystickStatus();
extern "C" __declspec(dllexport) void __cdecl SetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl SetProfileText(LPCSTR, LPCSTR, LPCSTR);
extern "C" __declspec(dllexport) void __cdecl UpdateConfig(SystemState*);
extern "C" __declspec(dllexport) void __cdecl UpdateSoundBar(unsigned short, unsigned short);
extern "C" __declspec(dllexport) void __cdecl UpdateTapeCounter(unsigned int, unsigned char);

unsigned char GetSoundCardIndex(char* soundCardName);
