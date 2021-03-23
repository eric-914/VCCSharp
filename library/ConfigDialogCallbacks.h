#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) LRESULT CALLBACK CreateTapeConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

extern "C" __declspec(dllexport) LRESULT CALLBACK CreateMainConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

extern "C" __declspec(dllexport) void SetDialogTapeCounter(HWND hDlg, unsigned int tapeCounter);
extern "C" __declspec(dllexport) void SetDialogTapeMode(HWND hDlg, unsigned char tapeMode);
extern "C" __declspec(dllexport) void SetDialogTapeFileName(HWND hDlg, char* tapeFileName);
