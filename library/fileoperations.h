#pragma once

extern "C" __declspec(dllexport) void __cdecl FilePathStripPath(char* path);
extern "C" __declspec(dllexport) BOOL __cdecl FilePathRemoveFileSpec(char* path);

extern "C" __declspec(dllexport) HANDLE __cdecl FileCreateFile(char* filename, long desiredAccess);
extern "C" __declspec(dllexport) DWORD __cdecl FileSetFilePointer(HANDLE handle, DWORD moveMethod);
extern "C" __declspec(dllexport) BOOL __cdecl FileReadFile(HANDLE handle, unsigned char* buffer, unsigned long size, unsigned long* moved);
extern "C" __declspec(dllexport) BOOL __cdecl FileWriteFile(HANDLE handle, unsigned char* buffer, int size);