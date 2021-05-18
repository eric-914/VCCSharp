/*****************************************************************************/
/*
  Copyright 2015 by Joseph Forgione
  This file is part of VCC (Virtual Color Computer).

  VCC (Virtual Color Computer) is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  VCC (Virtual Color Computer) is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with VCC (Virtual Color Computer).  If not, see <http://www.gnu.org/licenses/>.
*/

#include <windows.h>

#include "keyboardlayout.h"

const int MAX_COCO = 80;
const int MAX_NATURAL = 89;
const int MAX_COMPACT = 84;
const int MAX_CUSTOM = 89;

KeyTranslationEntry keyTranslationsCoCo[MAX_COCO + 1];
KeyTranslationEntry keyTranslationsNatural[MAX_NATURAL + 1];
KeyTranslationEntry keyTranslationsCompact[MAX_COMPACT+ 1];
KeyTranslationEntry keyTranslationsCustom[MAX_CUSTOM + 1];

extern "C" __declspec(dllexport) KeyTranslationEntry * __cdecl GetKeyTranslationsCoCo(void) {
  return keyTranslationsCoCo;
}

extern "C" __declspec(dllexport) void __cdecl SetKeyTranslationsCoCo(KeyTranslationEntry * value) {
  for (int i = 0; i < MAX_COCO; i++) {
    keyTranslationsCoCo[i] = value[i];
  }

  keyTranslationsCoCo[MAX_COCO] = { 0, 0, 0, 0, 0, 0 }; // terminator
}

extern "C" __declspec(dllexport) KeyTranslationEntry * __cdecl GetKeyTranslationsNatural(void) {
  return keyTranslationsNatural;
}

extern "C" __declspec(dllexport) void __cdecl SetKeyTranslationsNatural(KeyTranslationEntry * value) {
  for (int i = 0; i < MAX_NATURAL; i++) {
    keyTranslationsNatural[i] = value[i];
  }

  keyTranslationsNatural[MAX_NATURAL] = { 0, 0, 0, 0, 0, 0 }; // terminator
}

extern "C" __declspec(dllexport) KeyTranslationEntry * __cdecl GetKeyTranslationsCompact(void) {
  return keyTranslationsCompact;
}

extern "C" __declspec(dllexport) void __cdecl SetKeyTranslationsCompact(KeyTranslationEntry * value) {
  for (int i = 0; i < MAX_COMPACT; i++) {
    keyTranslationsCompact[i] = value[i];
  }

  keyTranslationsCompact[MAX_COMPACT] = { 0, 0, 0, 0, 0, 0 }; // terminator
}

extern "C" __declspec(dllexport) KeyTranslationEntry * __cdecl GetKeyTranslationsCustom(void) {
  return keyTranslationsCustom;
}

extern "C" __declspec(dllexport) void __cdecl SetKeyTranslationsCustom(KeyTranslationEntry * value) {
  for (int i = 0; i < MAX_CUSTOM; i++) {
    keyTranslationsCustom[i] = value[i];
  }

  keyTranslationsCustom[MAX_CUSTOM] = { 0, 0, 0, 0, 0, 0 }; // terminator
}
