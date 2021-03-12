#pragma once

#include "defines.h"

#include "CmdLineArguments.h"
#include "ConfigState.h"
#include "EmuState.h"

extern "C" __declspec(dllexport) ConfigState * __cdecl GetConfigState();
extern "C" __declspec(dllexport) ConfigModel * __cdecl GetConfigModel();
extern "C" __declspec(dllexport) JoystickModel * __cdecl GetLeftJoystick();
extern "C" __declspec(dllexport) JoystickModel * __cdecl GetRightJoystick();

extern "C" __declspec(dllexport) POINT __cdecl GetIniWindowSize();

extern "C" __declspec(dllexport) int __cdecl GetCurrentKeyboardLayout();
extern "C" __declspec(dllexport) int __cdecl GetPaletteType();
extern "C" __declspec(dllexport) int __cdecl GetRememberSize();
extern "C" __declspec(dllexport) int __cdecl SelectSerialCaptureFile(EmuState*, char*);

extern "C" __declspec(dllexport) unsigned char __cdecl TranslateDisplay2Scan(LRESULT);
extern "C" __declspec(dllexport) unsigned char __cdecl TranslateScan2Display(int);
extern "C" __declspec(dllexport) void __cdecl WriteIniFile(EmuState*);

extern "C" __declspec(dllexport) void __cdecl SetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl UpdateTapeDialog(unsigned int, unsigned char);

extern "C" __declspec(dllexport) unsigned char __cdecl GetSoundCardIndex(char* soundCardName);

extern "C" __declspec(dllexport) void __cdecl SaveConfig();

extern "C" __declspec(dllexport) void __cdecl ValidateModel(ConfigModel* model);
