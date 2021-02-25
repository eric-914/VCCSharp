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

extern "C" __declspec(dllexport) LRESULT CALLBACK DialogBoxAboutCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);

extern "C" __declspec(dllexport) BOOL CALLBACK DirectSoundEnumerateCallback(LPGUID lpGuid, LPCSTR lpcstrDescription, LPCSTR lpcstrModule, LPVOID lpContext);

extern "C" __declspec(dllexport) LRESULT CALLBACK CreateMainConfigDialogCallback(HWND hDlg, UINT message, WPARAM wParam, LPARAM lParam);
