﻿using System;
using System.Collections.Generic;
using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Keyboard;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp.Modules
{
    public interface IKeyboard
    {
        void KeyboardHandleKey(Key key, KeyStates keyState);
        void KeyboardHandleKey(byte scanCode, KeyStates keyState);
        void KeyboardBuildRuntimeTable(KeyboardLayouts keyMapIndex);
        void GimeSetKeyboardInterruptState(byte state);
        byte KeyboardGetScan(byte column);
        void SwapKeyboardLayout(KeyboardLayouts newLayout);
        void ResetKeyboardLayout();

        void SendSavedKeyEvents();
    }

    public class Keyboard : IKeyboard
    {
        private readonly IModules _modules;
        private readonly IKeyScanMapper _keyScanMapper;

        public bool KeyboardInterruptEnabled { get; set; }

        public KeyboardLayouts CurrentKeyBoardLayout { get; private set; }
        public KeyboardLayouts PreviousKeyBoardLayout { get; private set; }

        /** run-time 'rollover' table to pass to the MC6821 when a key is pressed */
        public byte[] RolloverTable = new byte[8];	// CoCo 'keys' for emulator

        /** track all keyboard scan codes state (up/down) */
        public int[] ScanTable = new int[256];

        private KeyTranslationEntry[] _keyTransTable = new KeyTranslationEntry[Define.KBTABLE_ENTRY_COUNT];

        //--------------------------------------------------------------------------
        // When the main window is about to lose keyboard focus there are one
        // or two keys down in the emulation that must be raised.  These routines
        // track the last two key down events so they can be raised when needed.
        //--------------------------------------------------------------------------
        private byte _scSave1;
        private byte _scSave2;
        private bool _keySaveToggle;

        public Keyboard(IModules modules, IKeyScanMapper keyScanMapper)
        {
            _modules = modules;
            _keyScanMapper = keyScanMapper;
        }

        /*
          Get CoCo 'scan' code

          Only called from MC6821.c to read the keyboard/joystick state

          should be a push instead of a pull?
        */
        public byte KeyboardGetScan(byte column)
        {
            byte temp = (byte)~column; //Get column
            byte mask = 1;
            byte retVal = 0;

            IJoystick joystick = _modules.Joystick;
            IMC6821 mc6821 = _modules.MC6821;

            //JoystickStates joystickState = _modules.Joystick.States;

            for (byte x = 0; x < 8; x++)
            {
                if ((temp & mask) != 0) // Found an active column scan
                {
                    retVal |= RolloverTable[x];
                }

                mask <<= 1;
            }

            retVal = (byte)(127 - retVal);

            //Collect CA2 and CB2 from the PIA (1of4 Multiplexer)
            _modules.Joystick.StickValue = (ushort)joystick.get_pot_value(mc6821.GetMuxState());

            if (_modules.Joystick.StickValue != 0)		//OS9 joyin routine needs this (koronis rift works now)
            {
                if (_modules.Joystick.StickValue >= mc6821.DACState())		// Set bit of stick >= DAC output $FF20 Bits 7-2
                {
                    retVal |= 0x80;
                }
            }

            if (_modules.Joystick.Left.Button1 == 1)
            {
                //Left Joystick Button 1 Down?
                retVal &= 0xFD;
            }

            if (_modules.Joystick.Right.Button1 == 1)
            {
                //Right Joystick Button 1 Down?
                retVal &= 0xFE;
            }

            if (_modules.Joystick.Left.Button2 == 1)
            {
                //Left Joystick Button 2 Down?
                retVal &= 0xF7;
            }

            if (_modules.Joystick.Right.Button2 == 1)
            {
                //Right Joystick Button 2 Down?
                retVal &= 0xFB;
            }

            #region // no noticeable change when this is disabled

            //                // TODO: move to MC6821/GIME
            //                {
            //                    /** another keyboard IRQ flag - this should really be in the GIME code*/
            //                    static unsigned char IrqFlag = 0;
            //                    if ((ret_val & 0x7F) != 0x7F)
            //                    {
            //                        if ((IrqFlag == 0) & GimeGetKeyboardInterruptState())
            //                        {
            //                            GimeAssertKeyboardInterrupt();
            //                            IrqFlag = 1;
            //                        }
            //                    }
            //                    else
            //                    {
            //                        IrqFlag = 0;
            //                    }
            //                }

            #endregion

            return retVal;
        }

        public void GimeSetKeyboardInterruptState(byte state)
        {
            KeyboardInterruptEnabled = state != 0;
        }

        public void SwapKeyboardLayout(KeyboardLayouts newLayout)
        {
            PreviousKeyBoardLayout = CurrentKeyBoardLayout;

            KeyboardBuildRuntimeTable(newLayout);
        }

        public void ResetKeyboardLayout()
        {
            SwapKeyboardLayout(PreviousKeyBoardLayout);
        }

        /*
          Rebuilds the run-time keyboard translation lookup table based on the
          current keyboard layout.

          The entries are sorted.  Any SHIFT + [char] entries need to be placed first
        */
        public void KeyboardBuildRuntimeTable(KeyboardLayouts keyBoardLayout)
        {
            CurrentKeyBoardLayout = keyBoardLayout;

            KeyTranslationEntry[] keyTranslationTable = KeyboardLayout.GetKeyboardLayout(keyBoardLayout);

            KeyboardClear();

            KeyTranslationEntry keyTransEntry = new KeyTranslationEntry();

            int index2 = 0;
            for (var index1 = 0; ; index1++)
            {
                if (keyTranslationTable == null)
                {
                    throw new NullReferenceException("Missing Key Translation Table");
                }

                keyTransEntry.Col1 = keyTranslationTable[index1].Col1;
                keyTransEntry.Col2 = keyTranslationTable[index1].Col2;
                keyTransEntry.Row1 = keyTranslationTable[index1].Row1;
                keyTransEntry.Row2 = keyTranslationTable[index1].Row2;
                keyTransEntry.ScanCode1 = keyTranslationTable[index1].ScanCode1;
                keyTransEntry.ScanCode2 = keyTranslationTable[index1].ScanCode2;

                //
                // Change entries to what the code expects
                //
                // Make sure ScanCode1 is never 0
                // If the key combo uses SHIFT, put it in ScanCode1
                // Completely clear unused entries (ScanCode1+2 are 0)
                //

                //
                // swaps ScanCode1 with ScanCode2 if ScanCode2 == DIK_LSHIFT
                //
                if (keyTransEntry.ScanCode2 == Define.DIK_LSHIFT)
                {
                    keyTransEntry.ScanCode2 = keyTransEntry.ScanCode1;
                    keyTransEntry.ScanCode1 = Define.DIK_LSHIFT;
                }

                //
                // swaps ScanCode1 with ScanCode2 if ScanCode1 is zero
                //
                if ((keyTransEntry.ScanCode1 == 0) && (keyTransEntry.ScanCode2 != 0))
                {
                    keyTransEntry.ScanCode1 = keyTransEntry.ScanCode2;
                    keyTransEntry.ScanCode2 = 0;
                }

                // check for terminating entry
                if (keyTransEntry.ScanCode1 == 0 && keyTransEntry.ScanCode2 == 0)
                {
                    break;
                }

                _keyTransTable[index2].Col1 = keyTransEntry.Col1;
                _keyTransTable[index2].Col2 = keyTransEntry.Col2;
                _keyTransTable[index2].Row1 = keyTransEntry.Row1;
                _keyTransTable[index2].Row2 = keyTransEntry.Row2;
                _keyTransTable[index2].ScanCode1 = keyTransEntry.ScanCode1;
                _keyTransTable[index2].ScanCode2 = keyTransEntry.ScanCode2;

                index2++;
            }

            KeyboardSort();

            #region DEBUG

#if DEBUG
            ////
            //// Debug dump the table
            ////
            //for (int index1 = 0; index1 < Define.KBTABLE_ENTRY_COUNT; index1++)
            //{
            //    // check for null entry
            //    if (instance->KeyTransTable[index1].ScanCode1 == 0 && instance->KeyTransTable[index1].ScanCode2 == 0)
            //    {
            //        // done
            //        break;
            //    }
            //}
#endif

            #endregion

            //XTRACE("Key: %3d - 0x%02X (%3d) 0x%02X (%3d) - %2d %2d  %2d %2d\n",
            //  Index1,
            //  KeyTransTable[Index1].ScanCode1,
            //  KeyTransTable[Index1].ScanCode1,
            //  KeyTransTable[Index1].ScanCode2,
            //  KeyTransTable[Index1].ScanCode2,
            //  KeyTransTable[Index1].Row1,
            //  KeyTransTable[Index1].Col1,
            //  KeyTransTable[Index1].Row2,
            //  KeyTransTable[Index1].Col2
            //);
        }

        private void KeyboardClear()
        {
            _keyTransTable = new KeyTranslationEntry[Define.KBTABLE_ENTRY_COUNT];
        }

        private void KeyboardSort()
        {
            //
            // Sort the key translation table
            //
            // Since the table is searched from beginning to end each
            // time a key is pressed, we want them to be in the correct 
            // order.
            //

            IComparer<KeyTranslationEntry> comparer = new KeyTranslationEntryIComparer();

            Array.Sort(_keyTransTable, comparer);
        }

        public void KeyboardHandleKey(Key key, KeyStates keyState)
        {
            //--Substituting the backslash for the "@" character.
            if (key == Key.Oem5)
            {
                SwapKeyboardLayout(KeyboardLayouts.Natural); //Natural (OS9)

                //--On standard keyboard, @ is Shift-2
                KeyboardHandleKey(Define.DIK_2, keyState);
                KeyboardHandleKey(Define.DIK_LSHIFT, keyState);

                ResetKeyboardLayout();
            }
            else
            {
                KeyboardHandleKey(_keyScanMapper.ToScanCode(key), keyState);
            }
        }

        /*
          Dispatch keyboard event to the emulator.

          Called from system. eg. WndProc : WM_KEYDOWN/WM_SYSKEYDOWN/WM_SYSKEYUP/WM_KEYUP

          @param key Windows virtual key code (VK_XXXX - not used)
          @param ScanCode keyboard scan code (DIK_XXXX - DirectInput)
          @param Status Key status - Down/Up
        */
        public void KeyboardHandleKey(byte scanCode, KeyStates keyState)
        {
            //System.Diagnostics.Debug.WriteLine($"scan={scanCode}, state={(keyState == KeyStates.Up ? "up" : "down")}");

            // check for shift key
            // Left and right shift generate different scan codes
            if (scanCode == Define.DIK_RSHIFT)
            {
                scanCode = Define.DIK_LSHIFT;
            }

            switch (keyState)
            {
                // Key Down
                case KeyStates.Down:
                    KeyboardHandleKeyDown(scanCode);
                    break;

                // Key Up
                case KeyStates.Up:
                    KeyboardHandleKeyUp(scanCode);
                    break;
            }
        }

        private void KeyboardHandleKeyDown(byte scanCode)
        {
            // Save key down in case focus is lost
            SaveLastTwoKeyDownEvents(scanCode);

            JoystickModel left = _modules.Joystick.GetLeftJoystick();
            JoystickModel right = _modules.Joystick.GetRightJoystick();

            if (left.InputSource == JoystickDevices.Keyboard || right.InputSource == JoystickDevices.Keyboard)
            {
                scanCode = _modules.Joystick.SetMouseStatus(scanCode, 1);
            }

            // track key is down
            ScanTable[scanCode] = Define.KEY_DOWN;

            KeyboardUpdateRolloverTable();

            if (KeyboardInterruptEnabled)
            {
                _modules.TC1014.GimeAssertKeyboardInterrupt();
            }
        }

        private void KeyboardHandleKeyUp(byte scanCode)
        {
            JoystickModel left = _modules.Joystick.GetLeftJoystick();
            JoystickModel right = _modules.Joystick.GetRightJoystick();

            if (left.InputSource == JoystickDevices.Keyboard || right.InputSource == JoystickDevices.Keyboard)
            {
                scanCode = _modules.Joystick.SetMouseStatus(scanCode, 0);
            }

            // reset key (released)
            ScanTable[scanCode] = Define.KEY_UP;

            // TODO: verify this is accurate emulation
            // Clean out rollover table on shift release
            if (scanCode == Define.DIK_LSHIFT)
            {
                for (int index = 0; index < Define.KBTABLE_ENTRY_COUNT; index++)
                {
                    ScanTable[index] = Define.KEY_UP;
                }
            }

            KeyboardUpdateRolloverTable();
        }

        private void KeyboardUpdateRolloverTable()
        {
            byte lockOut = 0;

            // clear the rollover table
            for (int index = 0; index < 8; index++)
            {
                RolloverTable[index] = 0;
            }

            // set rollover table based on ScanTable key status
            for (int index = 0; index < Define.KBTABLE_ENTRY_COUNT; index++)
            {
                KeyTranslationEntry entry = _keyTransTable[index];
                byte scanCode1 = entry.ScanCode1;
                byte scanCode2 = entry.ScanCode2;

                // stop at last entry
                if (scanCode1 == 0 && scanCode2 == 0)
                {
                    break;
                }

                if (lockOut != scanCode1)
                {
                    // Single input key 
                    if (scanCode1 != 0 && scanCode2 == 0)
                    {
                        // check if key pressed
                        if (ScanTable[scanCode1] == Define.KEY_DOWN)
                        {
                            int col = entry.Col1;

                            //assert(col >= 0 && col < 8);

                            RolloverTable[col] |= entry.Row1;

                            col = entry.Col2;

                            //assert(col >= 0 && col < 8);

                            RolloverTable[col] |= entry.Row2;
                        }
                    }

                    // Double Input Key
                    if (scanCode1 != 0 && scanCode2 != 0)
                    {
                        // check if both keys pressed
                        if (ScanTable[scanCode1] == Define.KEY_DOWN && ScanTable[scanCode2] == Define.KEY_DOWN)
                        {
                            int col = entry.Col1;

                            //assert(col >= 0 && col < 8);

                            RolloverTable[col] |= entry.Row1;

                            col = entry.Col2;

                            //assert(col >= 0 && col < 8);

                            RolloverTable[col] |= entry.Row2;

                            //--TODO: This implies that lockOut should be static?  But seems to work ok as is.
                            // always SHIFT
                            //lockOut = scanCode1;

                            break;
                        }
                    }
                }
            }
        }

        //--------------------------------------------------------------------------
        // When the main window is about to lose keyboard focus there are one
        // or two keys down in the emulation that must be raised.  These routines
        // track the last two key down events so they can be raised when needed.
        //--------------------------------------------------------------------------

        // Save last two key down events
        public void SaveLastTwoKeyDownEvents(byte oemScan)
        {
            // Ignore zero scan code
            if (oemScan == 0)
            {
                return;
            }

            // Remember it
            _keySaveToggle = !_keySaveToggle;

            if (_keySaveToggle)
            {
                _scSave1 = oemScan;
            }
            else
            {
                _scSave2 = oemScan;
            }
        }

        // Force keys up if main widow keyboard focus is lost.  Otherwise
        // down keys will cause issues with OS-9 on return
        // Send key up events to keyboard handler for saved keys
        public void SendSavedKeyEvents()
        {
            if (_scSave1 != 0)
            {
                _modules.Keyboard.KeyboardHandleKey(_scSave1, KeyStates.Up);
            }

            if (_scSave2 != 0)
            {
                _modules.Keyboard.KeyboardHandleKey(_scSave2, KeyStates.Up);
            }

            _scSave1 = 0;
            _scSave2 = 0;
        }
    }
}
