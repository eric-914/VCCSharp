using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Keyboard;

namespace VCCSharp.Modules
{
    public interface IKeyboard
    {
        unsafe KeyboardState* GetKeyboardState();
        void vccKeyboardHandleKeyDown(byte key, byte scanCode);
        void vccKeyboardHandleKeyUp(byte key, byte scanCode);
        void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
        void SetPaste(bool flag);
        void GimeSetKeyboardInterruptState(byte state);
        byte vccKeyboardGetScan(byte column);
        void SetKeyTranslations();
        void vccKeyboardHandleKey(byte key, byte scanCode, KeyStates keyState);
    }

    public class Keyboard : IKeyboard
    {
        private readonly IModules _modules;

        public Keyboard(IModules modules)
        {
            _modules = modules;
        }

        public unsafe KeyboardState* GetKeyboardState()
        {
            return Library.Keyboard.GetKeyBoardState();
        }

        public void vccKeyboardHandleKeyDown(byte key, byte scanCode)
        {
            vccKeyboardHandleKey(key, scanCode, KeyStates.kEventKeyDown);
        }

        public void vccKeyboardHandleKeyUp(byte key, byte scanCode)
        {
            vccKeyboardHandleKey(key, scanCode, KeyStates.kEventKeyUp);
        }

        public void SetPaste(bool flag)
        {
            unsafe
            {
                KeyboardState* keyboardState = GetKeyboardState();

                keyboardState->Pasting = flag ? Define.TRUE : Define.FALSE;
            }
        }

        /*
          Get CoCo 'scan' code

          Only called from MC6821.c to read the keyboard/joystick state

          should be a push instead of a pull?
        */
        public byte vccKeyboardGetScan(byte column)
        {
            byte temp = (byte)~column; //Get column
            byte mask = 1;
            byte ret_val = 0;

            unsafe
            {
                IJoystick joystick = _modules.Joystick;
                IMC6821 mc6821 = _modules.MC6821;

                KeyboardState* instance = GetKeyboardState();
                JoystickState* joystickState = joystick.GetJoystickState();

                for (byte x = 0; x < 8; x++)
                {
                    if ((temp & mask) != 0) // Found an active column scan
                    {
                        ret_val |= instance->RolloverTable[x];
                    }

                    mask <<= 1;
                }

                ret_val = (byte)(127 - ret_val);

                //Collect CA2 and CB2 from the PIA (1of4 Multiplexer)
                joystickState->StickValue = joystick.get_pot_value(mc6821.MC6821_GetMuxState());

                if (joystickState->StickValue != 0)		//OS9 joyin routine needs this (koronis rift works now)
                {
                    if (joystickState->StickValue >= mc6821.MC6821_DACState())		// Set bit of stick >= DAC output $FF20 Bits 7-2
                    {
                        ret_val |= 0x80;
                    }
                }

                if (joystickState->LeftButton1Status == 1)
                {
                    //Left Joystick Button 1 Down?
                    ret_val &= 0xFD;
                }

                if (joystickState->RightButton1Status == 1)
                {
                    //Right Joystick Button 1 Down?
                    ret_val &= 0xFE;
                }

                if (joystickState->LeftButton2Status == 1)
                {
                    //Left Joystick Button 2 Down?
                    ret_val &= 0xF7;
                }

                if (joystickState->RightButton2Status == 1)
                {
                    //Right Joystick Button 2 Down?
                    ret_val &= 0xFB;
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

                return ret_val;
            }
        }

        public void SetKeyTranslations()
        {
            Library.Keyboard.SetKeyTranslationsCoCo(KeyboardLayout.GetKeyTranslationsCoCo());
            Library.Keyboard.SetKeyTranslationsNatural(KeyboardLayout.GetKeyTranslationsNatural());
            Library.Keyboard.SetKeyTranslationsCompact(KeyboardLayout.GetKeyTranslationsCompact());
            Library.Keyboard.SetKeyTranslationsCustom(KeyboardLayout.GetKeyTranslationsCustom());
        }

        public void GimeSetKeyboardInterruptState(byte state)
        {
            Library.Keyboard.GimeSetKeyboardInterruptState(state);
        }

        /*
          Rebuilds the run-time keyboard translation lookup table based on the
          current keyboard layout.

          The entries are sorted.  Any SHIFT + [char] entries need to be placed first
        */
        public void vccKeyboardBuildRuntimeTable(byte keyMapIndex)
        {
            unsafe
            {
                KeyboardState* instance = GetKeyboardState();

                //int index1 = 0;
                //int index2 = 0;
                KeyTranslationEntry* keyLayoutTable = null;
                KeyboardLayouts keyBoardLayout = (KeyboardLayouts)keyMapIndex;

                switch (keyBoardLayout)
                {
                    case KeyboardLayouts.kKBLayoutCoCo:
                        keyLayoutTable = GetKeyTranslationsCoCo();
                        break;

                    case KeyboardLayouts.kKBLayoutNatural:
                        keyLayoutTable = GetKeyTranslationsNatural();
                        break;

                    case KeyboardLayouts.kKBLayoutCompact:
                        keyLayoutTable = GetKeyTranslationsCompact();
                        break;

                    case KeyboardLayouts.kKBLayoutCustom:
                        keyLayoutTable = GetKeyTranslationsCustom();
                        break;
                }

                //XTRACE("Building run-time key table for layout # : %d - %s\n", keyBoardLayout, k_keyboardLayoutNames[keyBoardLayout]);

                vccKeyboardClear();

                KeyTranslationEntry keyTransEntry = new KeyTranslationEntry();

                int index2 = 0;
                for (var index1 = 0; ; index1++)
                {

                    vccKeyboardCopyKeyTranslationEntry(&keyTransEntry, &keyLayoutTable[index1]);

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

                    vccKeyboardCopy(&keyTransEntry, index2);
                    //vccKeyboardCopyKeyTranslationEntry(&(instance->KeyTransTable[index2]), &keyTransEntry);

                    index2++;
                }

                vccKeyboardSort();

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
            }

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

        public unsafe KeyTranslationEntry* GetKeyTranslationsCoCo()
        {
            return Library.Keyboard.GetKeyTranslationsCoCo();
        }

        public unsafe KeyTranslationEntry* GetKeyTranslationsNatural()
        {
            return Library.Keyboard.GetKeyTranslationsNatural();
        }

        public unsafe KeyTranslationEntry* GetKeyTranslationsCompact()
        {
            return Library.Keyboard.GetKeyTranslationsCompact();
        }

        public unsafe KeyTranslationEntry* GetKeyTranslationsCustom()
        {
            return Library.Keyboard.GetKeyTranslationsCustom();
        }

        public void vccKeyboardClear()
        {
            // copy the selected keyboard layout to the run-time table
            Library.Keyboard.vccKeyboardClear();
        }

        public void vccKeyboardSort()
        {
            //
            // Sort the key translation table
            //
            // Since the table is searched from beginning to end each
            // time a key is pressed, we want them to be in the correct 
            // order.
            //

            Library.Keyboard.vccKeyboardSort();
        }

        public unsafe void vccKeyboardCopyKeyTranslationEntry(KeyTranslationEntry* target, KeyTranslationEntry* source)
        {
            Library.Keyboard.vccKeyboardCopyKeyTranslationEntry(target, source);
        }

        public unsafe void vccKeyboardCopy(KeyTranslationEntry* keyTransEntry, int index)
        {
            Library.Keyboard.vccKeyboardCopy(keyTransEntry, index);
        }

        /*
          Dispatch keyboard event to the emulator.

          Called from system. eg. WndProc : WM_KEYDOWN/WM_SYSKEYDOWN/WM_SYSKEYUP/WM_KEYUP

          @param key Windows virtual key code (VK_XXXX - not used)
          @param ScanCode keyboard scan code (DIK_XXXX - DirectInput)
          @param Status Key status - kEventKeyDown/kEventKeyUp
        */
        public void vccKeyboardHandleKey(byte key, byte scanCode, KeyStates keyState)
        {
            char c = key == 0 ? '0' : (char)key;

            //Key  : S ( 83 / 0x53)  Scan : 31 / 0x1F
            Debug.WriteLine($">>Key  : {c} ({key:###} / 0x{key:X})  Scan : {scanCode:###} / 0x{scanCode:X}", c, c, c, scanCode, scanCode);

            Library.Keyboard.vccKeyboardHandleKey(key, scanCode, keyState);
        }
    }
}
