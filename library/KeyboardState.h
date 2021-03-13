#pragma once

#define KBTABLE_ENTRY_COUNT 100	///< key translation table maximum size, (arbitrary) most of the layouts are < 80 entries

typedef enum KeyStates
{
  kEventKeyUp = 0,
  kEventKeyDown = 1
} KeyStates;

/**
  Keyboard layouts
*/
typedef enum KeyboardLayouts
{
  kKBLayoutCoCo = 0,
  kKBLayoutNatural,
  kKBLayoutCompact,
  kKBLayoutCustom,

  kKBLayoutCount
} KeyboardLayouts;

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
  BOOL Pasting;  //Are the keyboard functions in the middle of a paste operation?

  /** run-time 'rollover' table to pass to the MC6821 when a key is pressed */
  unsigned char RolloverTable[8];	// CoCo 'keys' for emulator

  /** track all keyboard scan codes state (up/down) */
  int ScanTable[256];

  /** run-time key translation table - convert key up/down messages to 'rollover' codes */
  KeyTranslationEntry KeyTransTable[KBTABLE_ENTRY_COUNT];	// run-time keyboard layout table (key(s) to keys(s) translation)
} KeyboardState;