using System;
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
        void vccKeyboardHandleKeyDown(char key, char scanCode);
        void vccKeyboardHandleKeyUp(char key, char scanCode);
        void vccKeyboardBuildRuntimeTable(byte keyMapIndex);
        void SetPaste(bool flag);
        void GimeSetKeyboardInterruptState(byte state);
        byte vccKeyboardGetScan(byte column);
        void SetKeyTranslations();
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

        public void vccKeyboardHandleKeyDown(char key, char scanCode)
        {
            vccKeyboardHandleKey(key, scanCode, KeyStates.kEventKeyDown);
        }

        public void vccKeyboardHandleKeyUp(char key, char scanCode)
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

        public void vccKeyboardHandleKey(char key, char scanCode, KeyStates keyState)
        {
            Library.Keyboard.vccKeyboardHandleKey(key, scanCode, keyState);
        }

        public void GimeSetKeyboardInterruptState(byte state)
        {
            Library.Keyboard.GimeSetKeyboardInterruptState(state);
        }

        public void vccKeyboardBuildRuntimeTable(byte keyMapIndex)
        {
            unsafe
            {
                KeyboardState* instance = GetKeyboardState();

                //int index1 = 0;
                //int index2 = 0;
                KeyTranslationEntry* keyLayoutTable = null;
                //KeyTranslationEntry keyTransEntry;
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

                Library.Keyboard.vccKeyboardBuildRuntimeTable(keyMapIndex, keyLayoutTable);

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
            Library.Keyboard.vccKeyboardClear();
        }

        public void vccKeyboardSort()
        {
            Library.Keyboard.vccKeyboardSort();;
        }
    }
}
