#pragma once

#include <windows.h>

extern "C" __declspec(dllexport) LRESULT CALLBACK CreateAudioConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateBitBangerConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateCpuConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateDisplayConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateInputConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateJoyStickConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateMiscConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
extern "C" __declspec(dllexport) LRESULT CALLBACK CreateTapeConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

extern "C" __declspec(dllexport) LRESULT CALLBACK CreateMainConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

extern "C" __declspec(dllexport) void SetDialogTapeCounter(HWND hDlg, unsigned int tapeCounter);
extern "C" __declspec(dllexport) void SetDialogTapeMode(HWND hDlg, unsigned char tapeMode);
extern "C" __declspec(dllexport) void SetDialogTapeFileName(HWND hDlg, char* tapeFileName);
extern "C" __declspec(dllexport) void SetDialogCpuMultiplier(HWND hDlg, unsigned char cpuMultiplier);
extern "C" __declspec(dllexport) void SetDialogAudioBars(HWND hDlg, unsigned short left, unsigned short right);
