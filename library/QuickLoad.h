#pragma once

#include "EmuState.h"

#define QL_OK                 0
#define QL_NO_ACTION          1
#define QL_FILE_NOT_FOUND     2
#define QL_CANNOT_OPEN_FILE   3
#define QL_OUT_OF_MEMORY      4
#define QL_INVALID_TRANSFER   5
#define QL_INVALID_FILE_TYPE  6
#define QL_UNKNOWN            255

extern "C" __declspec(dllexport) int __cdecl QuickStart(EmuState*, char*);
