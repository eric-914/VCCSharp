using System.Windows.Input;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Keyboard;
using VCCSharp.Models.Keyboard.Definitions;
using VCCSharp.Models.Keyboard.Layouts;
using KeyStates = VCCSharp.Enums.KeyStates;

namespace VCCSharp.Modules;

public interface IKeyboard : IModule
{
    void KeyboardHandleKey(Key key, KeyStates keyState);
    void KeyboardHandleKey(byte scanCode, KeyStates keyState);
    void KeyboardBuildRuntimeTable();
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

    private IConfiguration Configuration => _modules.Configuration;

    private bool KeyboardInterruptEnabled { get; set; }

    private KeyboardLayouts CurrentKeyBoardLayout { get; set; }
    private KeyboardLayouts PreviousKeyBoardLayout { get; set; }

    /** run-time 'rollover' table to pass to the MC6821 when a key is pressed */
    private byte[] _rolloverTable = new byte[8];	// CoCo 'keys' for emulator

    /** track all keyboard scan codes state (up/down) */
    private int[] _scanTable = new int[256];

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

        IJoysticks joysticks = _modules.Joysticks;
        IMC6821 mc6821 = _modules.MC6821;

        //JoystickStates joystickState = _modules.Joysticks.States;

        for (byte x = 0; x < 8; x++)
        {
            if ((temp & mask) != 0) // Found an active column scan
            {
                retVal |= _rolloverTable[x];
            }

            mask <<= 1;
        }

        retVal = (byte)(127 - retVal);

        //Collect CA2 and CB2 from the PIA (1 of 4 Multiplexer)
        _modules.Joysticks.StickValue = (ushort)joysticks.GetPotValue((Pots)mc6821.GetMuxState());

        if (_modules.Joysticks.StickValue != 0)		//OS9 joyin routine needs this (koronis rift works now)
        {
            if (_modules.Joysticks.StickValue >= mc6821.DACState())		// Set bit of stick >= DAC output $FF20 Bits 7-2
            {
                retVal |= 0x80;
            }
        }

        if (_modules.Joysticks.Left.Button1)
        {
            //Left Joystick Button 1 Down?
            retVal &= 0xFD;
        }

        if (_modules.Joysticks.Right.Button1)
        {
            //Right Joysticks Button 1 Down?
            retVal &= 0xFE;
        }

        if (_modules.Joysticks.Left.Button2)
        {
            //Left Joysticks Button 2 Down?
            retVal &= 0xF7;
        }

        if (_modules.Joysticks.Right.Button2)
        {
            //Right Joysticks Button 2 Down?
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

    public void KeyboardBuildRuntimeTable()
    {
        KeyboardBuildRuntimeTable(Configuration.Keyboard.Layout.Value);
    }

    /*
      Rebuilds the run-time keyboard translation lookup table based on the
      current keyboard layout.

      The entries are sorted.  Any SHIFT + [char] entries need to be placed first
    */
    private void KeyboardBuildRuntimeTable(KeyboardLayouts keyBoardLayout)
    {
        CurrentKeyBoardLayout = keyBoardLayout;

        KeyTranslationEntry[] keyTranslationTable = KeyboardLayout.GetKeyboardLayout(keyBoardLayout);

        KeyboardClear();

        int index2 = 0;
        for (var index1 = 0; ; index1++)
        {
            if (keyTranslationTable == null)
            {
                throw new NullReferenceException("Missing Key Translation Table");
            }

            KeyTranslationEntry keyTransEntry = new()
            {
                Col1 = keyTranslationTable[index1].Col1,
                Col2 = keyTranslationTable[index1].Col2,
                Row1 = keyTranslationTable[index1].Row1,
                Row2 = keyTranslationTable[index1].Row2,
                ScanCode1 = keyTranslationTable[index1].ScanCode1,
                ScanCode2 = keyTranslationTable[index1].ScanCode2
            };

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
            if (keyTransEntry.ScanCode2 == DIK.DIK_LSHIFT)
            {
                keyTransEntry.ScanCode2 = keyTransEntry.ScanCode1;
                keyTransEntry.ScanCode1 = DIK.DIK_LSHIFT;
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

            _keyTransTable[index2] = keyTransEntry;
            //_keyTransTable[index2].Col1 = keyTransEntry.Col1;
            //_keyTransTable[index2].Col2 = keyTransEntry.Col2;
            //_keyTransTable[index2].Row1 = keyTransEntry.Row1;
            //_keyTransTable[index2].Row2 = keyTransEntry.Row2;
            //_keyTransTable[index2].ScanCode1 = keyTransEntry.ScanCode1;
            //_keyTransTable[index2].ScanCode2 = keyTransEntry.ScanCode2;

            index2++;
        }

        //--Get rid of null entries
        _keyTransTable = _keyTransTable.Where(x => x != null).ToArray();

        KeyboardSort();
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
            SwapKeyboardLayout(KeyboardLayouts.PC);

            //--On standard keyboard, @ is Shift-2
            KeyboardHandleKey(DIK.DIK_2, keyState);
            KeyboardHandleKey(DIK.DIK_LSHIFT, keyState);

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

        switch (scanCode)
        {
            case DIK.DIK_NONE:
                return;

            // check for shift key
            // Left and right shift generate different scan codes
            case DIK.DIK_RSHIFT:
                scanCode = DIK.DIK_LSHIFT;
                break;
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

        //--See if the scan-code is a key reserved for joystick movement.
        //--If it is, the joystick state will be updated and the scan code will come back as zero.
        scanCode = _modules.Joysticks.SetJoystickFromKeyboard(scanCode, true);

        // track key is down
        _scanTable[scanCode] = Define.KEY_DOWN;

        KeyboardUpdateRolloverTable();

        if (KeyboardInterruptEnabled)
        {
            _modules.TCC1014.GimeAssertKeyboardInterrupt();
        }
    }

    private void KeyboardHandleKeyUp(byte scanCode)
    {
        //--See if the scan-code is a key reserved for joystick movement.
        //--If it is, the joystick state will be updated and the scan code will come back as zero.
        scanCode = _modules.Joysticks.SetJoystickFromKeyboard(scanCode, false);

        // reset key (released)
        _scanTable[scanCode] = Define.KEY_UP;

        // TODO: verify this is accurate emulation
        // Clean out rollover table on shift release
        if (scanCode == DIK.DIK_LSHIFT)
        {
            for (int index = 0; index < Define.KBTABLE_ENTRY_COUNT; index++)
            {
                _scanTable[index] = Define.KEY_UP;
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
            _rolloverTable[index] = 0;
        }

        // set rollover table based on ScanTable key status
        for (int index = 0; index < _keyTransTable.Length; index++)
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
                    if (_scanTable[scanCode1] == Define.KEY_DOWN)
                    {
                        int col = entry.Col1;

                        //assert(col >= 0 && col < 8);

                        _rolloverTable[col] |= entry.Row1;

                        col = entry.Col2;

                        //assert(col >= 0 && col < 8);

                        _rolloverTable[col] |= entry.Row2;
                    }
                }

                // Double Input Key
                if (scanCode1 != 0 && scanCode2 != 0)
                {
                    // check if both keys pressed
                    if (_scanTable[scanCode1] == Define.KEY_DOWN && _scanTable[scanCode2] == Define.KEY_DOWN)
                    {
                        int col = entry.Col1;

                        //assert(col >= 0 && col < 8);

                        _rolloverTable[col] |= entry.Row1;

                        col = entry.Col2;

                        //assert(col >= 0 && col < 8);

                        _rolloverTable[col] |= entry.Row2;

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
    private void SaveLastTwoKeyDownEvents(byte oemScan)
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
        _modules.Keyboard.KeyboardHandleKey(_scSave1, KeyStates.Up);
        _modules.Keyboard.KeyboardHandleKey(_scSave2, KeyStates.Up);

        _scSave1 = 0;
        _scSave2 = 0;
    }

    public void ModuleReset()
    {
        CurrentKeyBoardLayout = KeyboardLayouts.CoCo;
        PreviousKeyBoardLayout = KeyboardLayouts.CoCo;
        _rolloverTable = new byte[8];
        _scanTable = new int[256];

        _keyTransTable = new KeyTranslationEntry[Define.KBTABLE_ENTRY_COUNT];

        _scSave1 = 0;
        _scSave2 = 0;
        _keySaveToggle = false;

        KeyboardBuildRuntimeTable();
    }
}
