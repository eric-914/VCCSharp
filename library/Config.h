#pragma once

#include <richedit.h>

#include "defines.h"

#include "CmdLineArguments.h"
#include "ConfigState.h"
#include "EmuState.h"

extern "C" __declspec(dllexport) ConfigState * __cdecl GetConfigState();

extern "C" __declspec(dllexport) POINT __cdecl GetIniWindowSize();

extern "C" __declspec(dllexport) char* __cdecl AppDirectory();
extern "C" __declspec(dllexport) char* __cdecl ExternalBasicImage(void);

extern "C" __declspec(dllexport) int __cdecl GetCurrentKeyboardLayout();
extern "C" __declspec(dllexport) int __cdecl GetPaletteType();
extern "C" __declspec(dllexport) int __cdecl GetRememberSize();
extern "C" __declspec(dllexport) int __cdecl SelectSerialCaptureFile(EmuState*, char*);

extern "C" __declspec(dllexport) unsigned char __cdecl TranslateDisplay2Scan(LRESULT);
extern "C" __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int);
extern "C" __declspec(dllexport) void __cdecl ReadIniFile(EmuState*);
extern "C" __declspec(dllexport) void __cdecl WriteIniFile(EmuState*);

extern "C" __declspec(dllexport) void __cdecl DecreaseOverclockSpeed(EmuState*);
extern "C" __declspec(dllexport) void __cdecl GetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl IncreaseOverclockSpeed(EmuState*);
extern "C" __declspec(dllexport) void __cdecl InitConfig(EmuState*, CmdLineArguments*);
extern "C" __declspec(dllexport) void __cdecl ConfigureJoysticks();
extern "C" __declspec(dllexport) void __cdecl SetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl SynchSystemWithConfig(EmuState*);
extern "C" __declspec(dllexport) void __cdecl UpdateSoundBar(unsigned short, unsigned short);
extern "C" __declspec(dllexport) void __cdecl UpdateTapeDialog(unsigned int, unsigned char);

unsigned char GetSoundCardIndex(char* soundCardName);

extern "C" __declspec(dllexport) void __cdecl LoadIniFile();
extern "C" __declspec(dllexport) void __cdecl SaveConfig();
