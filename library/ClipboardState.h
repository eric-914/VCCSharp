#pragma once

#include <string>

#include "ConfigModel.h"

typedef struct
{
  bool CodePaste;
  bool PasteWithNew;
  int CurrentKeyMap;

  string ClipboardText;
  ConfigModel ClipConfig;
} ClipboardState;
