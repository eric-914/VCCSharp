#include "di.version.h"
#include <dinput.h>
#include <assert.h>

#include "Keyboard.h"
#include "keyboardlayout.h"
#include "Joystick.h"
#include "MC6821.h"

#include "TC1014Registers.h"
#include "xDebug.h"

#define KEY_DOWN	1
#define KEY_UP		0

KeyboardState* InitializeInstance(KeyboardState*);

static KeyboardState* instance = InitializeInstance(new KeyboardState());

//--Spelled funny because there's a GetKeyboardState() in User32.dll
extern "C" {
  __declspec(dllexport) KeyboardState* __cdecl GetKeyBoardState() {
    return instance;
  }
}

KeyboardState* InitializeInstance(KeyboardState* p) {
  p->KeyboardInterruptEnabled = 0;
  p->Pasting = FALSE;

  return p;
}

extern "C" {
  __declspec(dllexport) unsigned char __cdecl GimeGetKeyboardInterruptState()
  {
    return instance->KeyboardInterruptEnabled;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl GimeSetKeyboardInterruptState(unsigned char state)
  {
    instance->KeyboardInterruptEnabled = !!state;
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
    if (entry1->ScanCode1 == 0 && entry1->ScanCode2 == 0 && entry2->ScanCode1 != 0)
    {
      return 1;
    }
    else {
      if (entry2->ScanCode1 == 0 && entry2->ScanCode2 == 0 && entry1->ScanCode1 != 0)
      {
        return -1;
      }
      else {
        // push shift/alt/control by themselves to the end
        if (entry1->ScanCode2 == 0 && (entry1->ScanCode1 == DIK_LSHIFT || entry1->ScanCode1 == DIK_LMENU || entry1->ScanCode1 == DIK_LCONTROL))
        {
          result = 1;
        }
        else {
          // push shift/alt/control by themselves to the end
          if (entry2->ScanCode2 == 0 && (entry2->ScanCode1 == DIK_LSHIFT || entry2->ScanCode1 == DIK_LMENU || entry2->ScanCode1 == DIK_LCONTROL))
          {
            result = -1;
          }
          else {
            // move double key combos in front of single ones
            if (entry1->ScanCode2 == 0 && entry2->ScanCode2 != 0)
            {
              result = 1;
            }
            else {
              // move double key combos in front of single ones
              if (entry2->ScanCode2 == 0 && entry1->ScanCode2 != 0)
              {
                result = -1;
              }
              else
              {
                result = entry1->ScanCode1 - entry2->ScanCode1;

                if (result == 0)
                {
                  result = entry1->Row1 - entry2->Row1;
                }

                if (result == 0)
                {
                  result = entry1->Col1 - entry2->Col1;
                }

                if (result == 0)
                {
                  result = entry1->Row2 - entry2->Row2;
                }

                if (result == 0)
                {
                  result = entry1->Col2 - entry2->Col2;
                }
              }
            }
          }
        }
      }
    }

    return result;
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardSort() {
    //
    // Sort the key translation table
    //
    // Since the table is searched from beginning to end each
    // time a key is pressed, we want them to be in the correct 
    // order.
    //
    qsort(instance->KeyTransTable, KBTABLE_ENTRY_COUNT, sizeof(KeyTranslationEntry), KeyTransCompare);
  }
}

extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardClear() {
    // copy the selected keyboard layout to the run-time table
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
  __declspec(dllexport) void __cdecl vccKeyboardUpdateRolloverTable()
  {
    unsigned char	lockOut = 0;

    // clear the rollover table
    for (int index = 0; index < 8; index++)
    {
      instance->RolloverTable[index] = 0;
    }

    // set rollover table based on ScanTable key status
    for (int index = 0; index < KBTABLE_ENTRY_COUNT; index++)
    {
      // stop at last entry
      if ((instance->KeyTransTable[index].ScanCode1 == 0) && (instance->KeyTransTable[index].ScanCode2 == 0))
      {
        break;
      }

      if (lockOut != instance->KeyTransTable[index].ScanCode1)
      {
        // Single input key 
        if ((instance->KeyTransTable[index].ScanCode1 != 0) && (instance->KeyTransTable[index].ScanCode2 == 0))
        {
          // check if key pressed
          if (instance->ScanTable[instance->KeyTransTable[index].ScanCode1] == KEY_DOWN)
          {
            int col = instance->KeyTransTable[index].Col1;

            assert(col >= 0 && col < 8);

            instance->RolloverTable[col] |= instance->KeyTransTable[index].Row1;

            col = instance->KeyTransTable[index].Col2;

            assert(col >= 0 && col < 8);

            instance->RolloverTable[col] |= instance->KeyTransTable[index].Row2;
          }
        }

        // Double Input Key
        if ((instance->KeyTransTable[index].ScanCode1 != 0) && (instance->KeyTransTable[index].ScanCode2 != 0))
        {
          // check if both keys pressed
          if ((instance->ScanTable[instance->KeyTransTable[index].ScanCode1] == KEY_DOWN) && (instance->ScanTable[instance->KeyTransTable[index].ScanCode2] == KEY_DOWN))
          {
            int col = instance->KeyTransTable[index].Col1;

            assert(col >= 0 && col < 8);

            instance->RolloverTable[col] |= instance->KeyTransTable[index].Row1;

            col = instance->KeyTransTable[index].Col2;

            assert(col >= 0 && col < 8);

            instance->RolloverTable[col] |= instance->KeyTransTable[index].Row2;

            // always SHIFT
            lockOut = instance->KeyTransTable[index].ScanCode1;

            break;
          }
        }
      }
    }
  }
}

/*
  Dispatch keyboard event to the emulator.

  Called from system. eg. WndProc : WM_KEYDOWN/WM_SYSKEYDOWN/WM_SYSKEYUP/WM_KEYUP

  @param key Windows virtual key code (VK_XXXX - not used)
  @param ScanCode keyboard scan code (DIK_XXXX - DirectInput)
  @param Status Key status - kEventKeyDown/kEventKeyUp
*/
extern "C" {
  __declspec(dllexport) void __cdecl vccKeyboardHandleKey(unsigned char key, unsigned char scanCode, KeyStates keyState)
  {
    XTRACE("Key  : %c (%3d / 0x%02X)  Scan : %d / 0x%02X\n", key == 0 ? '0' : key, key == 0 ? '0' : key, key == 0 ? '0' : key, scanCode, scanCode);

    JoystickState* joystickState = GetJoystickState();

    //If requested, abort pasting operation.
    if (scanCode == 0x01 || scanCode == 0x43 || scanCode == 0x3F) {
      instance->Pasting = FALSE;

      OutputDebugString("ABORT PASTING!!!\n");
    }

    // check for shift key
    // Left and right shift generate different scan codes
    if (scanCode == DIK_RSHIFT)
    {
      scanCode = DIK_LSHIFT;
    }

#if 0 // TODO: CTRL and/or ALT?
    // CTRL key - right -> left
    if (ScanCode == DIK_RCONTROL)
    {
      ScanCode = DIK_LCONTROL;
    }
    // ALT key - right -> left
    if (ScanCode == DIK_RMENU)
    {
      ScanCode = DIK_LMENU;
  }
#endif

    switch (keyState)
    {
    default:
      // internal error
      break;

      // Key Down
    case kEventKeyDown:
      if ((joystickState->Left->UseMouse == 0) || (joystickState->Right->UseMouse == 0))
      {
        scanCode = SetMouseStatus(scanCode, 1);
      }

      // track key is down
      instance->ScanTable[scanCode] = KEY_DOWN;

      vccKeyboardUpdateRolloverTable();

      if (GimeGetKeyboardInterruptState())
      {
        GimeAssertKeyboardInterrupt();
      }

      break;

      // Key Up
    case kEventKeyUp:
      if ((joystickState->Left->UseMouse == 0) || (joystickState->Right->UseMouse == 0))
      {
        scanCode = SetMouseStatus(scanCode, 0);
      }

      // reset key (released)
      instance->ScanTable[scanCode] = KEY_UP;

      // TODO: verify this is accurate emulation
      // Clean out rollover table on shift release
      if (scanCode == DIK_LSHIFT)
      {
        for (int Index = 0; Index < KBTABLE_ENTRY_COUNT; Index++)
        {
          instance->ScanTable[Index] = KEY_UP;
        }
      }

      vccKeyboardUpdateRolloverTable();

      break;
    }
}
}