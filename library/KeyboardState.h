#pragma once

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
  unsigned char KeyboardInterruptEnabled;
  BOOL Pasting;

  unsigned char RolloverTable[8];

  int ScanTable[256];

  KeyTranslationEntry KeyTransTable[KBTABLE_ENTRY_COUNT];
} KeyboardState;