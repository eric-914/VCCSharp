#include "di.version.h"
#include <dinput.h>

const int MAX_COCO = 80;
const int MAX_NATURAL = 89;
const int MAX_COMPACT = 84;
const int MAX_CUSTOM = 89;

#define KBTABLE_ENTRY_COUNT 100

typedef enum KeyStates
{
  kEventKeyUp = 0,
  kEventKeyDown = 1
} KeyStates;

typedef struct KeyTranslationEntry
{
  unsigned char ScanCode1;
  unsigned char ScanCode2;
  unsigned char Row1;
  unsigned char Col1;
  unsigned char Row2;
  unsigned char Col2;
} KeyTranslationEntry;

typedef struct {
  unsigned char RolloverTable[8];

  int ScanTable[256];

  KeyTranslationEntry KeyTransTable[KBTABLE_ENTRY_COUNT];
} KeyboardState;

KeyTranslationEntry keyTranslationsCoCo[MAX_COCO + 1];
KeyTranslationEntry keyTranslationsNatural[MAX_NATURAL + 1];
KeyTranslationEntry keyTranslationsCompact[MAX_COMPACT + 1];
KeyTranslationEntry keyTranslationsCustom[MAX_CUSTOM + 1];

static KeyboardState* instance = new KeyboardState();

//--Spelled funny because there's a GetKeyboardState() in User32.dll
extern "C" {
  __declspec(dllexport) KeyboardState* __cdecl GetKeyBoardState() {
    return instance;
  }
}

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

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardClear() {
    memset(instance->KeyTransTable, 0, sizeof(instance->KeyTransTable));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardCopyKeyTranslationEntry(KeyTranslationEntry* target, KeyTranslationEntry* source) {
    memcpy(target, source, sizeof(KeyTranslationEntry));
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardCopy(KeyTranslationEntry* keyTransEntry, int index) {
    vccKeyboardCopyKeyTranslationEntry(&(instance->KeyTransTable[index]), keyTransEntry);
  }
}

extern "C" {
  __declspec(dllexport) KeyTranslationEntry __cdecl vccKeyTranslationEntry(int index) {
    return instance->KeyTransTable[index];
  }
}

/**
  Key translation table compare function for sorting (with qsort)
*/
extern "C" {
  __declspec(dllexport) int __cdecl KeyTransCompare(const void* e1, const void* e2)
  {
    KeyTranslationEntry* entry1 = (KeyTranslationEntry*)e1;
    KeyTranslationEntry* entry2 = (KeyTranslationEntry*)e2;
    int result = 0;

    // empty listing push to end
    if (entry1->ScanCode1 == 0 && entry1->ScanCode2 == 0 && entry2->ScanCode1 != 0) return 1;
    if (entry2->ScanCode1 == 0 && entry2->ScanCode2 == 0 && entry1->ScanCode1 != 0) return -1;

    // push shift/alt/control by themselves to the end
    if (entry1->ScanCode2 == 0 && (entry1->ScanCode1 == DIK_LSHIFT || entry1->ScanCode1 == DIK_LMENU || entry1->ScanCode1 == DIK_LCONTROL)) return 1;
    // push shift/alt/control by themselves to the end
    if (entry2->ScanCode2 == 0 && (entry2->ScanCode1 == DIK_LSHIFT || entry2->ScanCode1 == DIK_LMENU || entry2->ScanCode1 == DIK_LCONTROL)) return -1;

    // move double key combos in front of single ones
    if (entry1->ScanCode2 == 0 && entry2->ScanCode2 != 0) return 1;

    // move double key combos in front of single ones
    if (entry2->ScanCode2 == 0 && entry1->ScanCode2 != 0) return -1;

    result = entry1->ScanCode1 - entry2->ScanCode1;

    if (result == 0) result = entry1->Row1 - entry2->Row1;

    if (result == 0) result = entry1->Col1 - entry2->Col1;

    if (result == 0) result = entry1->Row2 - entry2->Row2;

    if (result == 0) result = entry1->Col2 - entry2->Col2;

    return result;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardSort() {
    qsort(instance->KeyTransTable, KBTABLE_ENTRY_COUNT, sizeof(KeyTranslationEntry), KeyTransCompare);
  }
}
