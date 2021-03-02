#include <windows.h>
#include <stdio.h>

extern "C" {
  __declspec(dllexport) BOOL __cdecl FilePathRemoveFileSpec(char* path) {
    size_t index = strlen(path), length = index;

    if ((index == 0) || (index > MAX_PATH)) {
      return(false);
    }

    while ((index > 0) && (path[index] != '\\')) {
      index--;
    }

    while ((index > 0) && (path[index] == '\\')) {
      index--;
    }

    if (index == 0) {
      return(false);
    }

    path[index + 2] = 0;

    return(!(strlen(path) == length));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FilePathStripPath(char* path) {
    char buffer[MAX_PATH] = "";
    short index = (short)strlen(path);

    if ((index > MAX_PATH) || (index == 0)) { //Test for overflow
      return;
    }

    for (; index >= 0; index--) {
      if (path[index] == '\\') {
        break;
      }
    }

    if (index < 0) {	//delimiter not found
      return;
    }

    strcpy(buffer, &path[index + 1]);
    strcpy(path, buffer);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl FileValidatePath(char* path) {
    char exePath[MAX_PATH] = "";
    char tempPath[MAX_PATH] = "";

    strcpy(tempPath, path);
    GetModuleFileName(NULL, exePath, MAX_PATH);

    FilePathRemoveFileSpec(exePath);	  //Get path to executable
    FilePathRemoveFileSpec(tempPath);		//Get path to Incoming file

    if (!strcmp(tempPath, exePath)) {	  // If they match remove the Path
      FilePathStripPath(path);
    }
  }
}

extern "C" {
  __declspec(dllexport) int __cdecl FileCheckPath(char* path) {
    char TempPath[MAX_PATH] = "";
    HANDLE hr = NULL;

    if ((strlen(path) == 0) || (strlen(path) > MAX_PATH)) {
      return(1);
    }

    hr = CreateFile(path, 0, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

    if (hr == INVALID_HANDLE_VALUE) //File Doesn't exist
    {
      GetModuleFileName(NULL, TempPath, MAX_PATH);
      FilePathRemoveFileSpec(TempPath);

      if ((strlen(TempPath)) + (strlen(path)) > MAX_PATH) { //Resulting path is to large Bail.
        return(1);
      }

      strcat(TempPath, path);
      hr = CreateFile(TempPath, 0, FILE_SHARE_READ, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);

      if (hr == INVALID_HANDLE_VALUE) {
        return(1);
      }

      strcpy(path, TempPath);
    }

    CloseHandle(hr);

    return(0);
  }
}

extern "C" {
  __declspec(dllexport) BOOL __cdecl FilePathRemoveExtension(char* path) {
    size_t index = strlen(path), length = index;

    if ((index == 0) || (index > MAX_PATH)) {
      return(false);
    }

    while ((index > 0) && (path[index--] != '.'));

    path[index + 1] = 0;

    return(!(strlen(path) == length));
  }
}

extern "C" {
  __declspec(dllexport) char* __cdecl FilePathFindExtension(char* path) {
    size_t index = strlen(path), length = index;

    if ((index == 0) || (index > MAX_PATH)) {
      return(&path[strlen(path) + 1]);
    }

    while ((index > 0) && (path[index--] != '.'));

    return(&path[index + 1]);
  }
}

extern "C" {
  __declspec(dllexport) DWORD __cdecl FileWritePrivateProfileInt(LPCTSTR sectionName, LPCTSTR keyName, int keyValue, LPCTSTR iniFileName) {
    char buffer[32] = "";
    sprintf(buffer, "%i", keyValue);

    return(WritePrivateProfileString(sectionName, keyName, buffer, iniFileName));
  }
}