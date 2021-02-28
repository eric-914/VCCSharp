#pragma once

#define CL_MAX_PATH 256

struct CmdLineArguments {
  char QLoadFile[CL_MAX_PATH];
  char IniFile[CL_MAX_PATH];
};
