#pragma once

#include "defines.h"

#include "CmdLineArguments.h"
#include "ConfigState.h"
#include "EmuState.h"

extern "C" __declspec(dllexport) ConfigState* __cdecl GetConfigState();

extern "C" __declspec(dllexport) POINT __cdecl GetIniWindowSize();

extern "C" __declspec(dllexport) int __cdecl GetRememberSize();

extern "C" __declspec(dllexport) void __cdecl SetIniFilePath(char*);
extern "C" __declspec(dllexport) void __cdecl UpdateTapeDialog(unsigned int, unsigned char);

void MainCommandOk(HWND hDlg, ConfigModel* model);
void MainCommandCancel(HWND hDlg);
void MainCommandApply(ConfigModel* model);
void MainNotify(WPARAM wParam);
void MainInitDialog(HWND hDlg);
