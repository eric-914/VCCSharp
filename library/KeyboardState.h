#pragma once

#define KBTABLE_ENTRY_COUNT 100	///< key translation table maximum size, (arbitrary) most of the layouts are < 80 entries

typedef enum keyevent_e
{
  kEventKeyUp = 0,
  kEventKeyDown = 1
} keyevent_e;

/**
  Keyboard layouts
*/
typedef enum keyboardlayout_e
{
  kKBLayoutCoCo = 0,
  kKBLayoutNatural,
  kKBLayoutCompact,
  kKBLayoutCustom,

  kKBLayoutCount
} keyboardlayout_e;

typedef struct keytranslationentry_t
{
  unsigned char ScanCode1;
  unsigned char ScanCode2;
  unsigned char Row1;
  unsigned char Col1;
  unsigned char Row2;
  unsigned char Col2;
} keytranslationentry_t;

typedef struct {
  /** track all keyboard scan codes state (up/down) */
  int ScanTable[256];

  /** run-time 'rollover' table to pass to the MC6821 when a key is pressed */
  unsigned char RolloverTable[8];	// CoCo 'keys' for emulator

  /** run-time key translation table - convert key up/down messages to 'rollover' codes */
  keytranslationentry_t KeyTransTable[KBTABLE_ENTRY_COUNT];	// run-time keyboard layout table (key(s) to keys(s) translation)

  unsigned char KeyboardInterruptEnabled;
  bool Pasting;  //Are the keyboard functions in the middle of a paste operation?
} KeyboardState;