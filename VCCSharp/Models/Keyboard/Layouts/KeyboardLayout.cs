using System;
using System.Collections.Generic;
using System.Linq;
using VCCSharp.Enums;
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Models.Keyboard.Layouts
{
    /*****************************************************************************/
    /*
      Keyboard layout data

      key translation tables used to convert keyboard oem scan codes / key
      combinations into CoCo keyboard row/col values

      ScanCode1 and ScanCode2 are used to determine what actual
      key presses are translated into a specific CoCo key

      Keyboard ScanCodes are from dinput.h

      The code expects SHIFTed entries in the table to use DIK_LSHIFT (not DIK_RSHIFT)
      The key handling code turns DIK_RSHIFT into DIK_LSHIFT

      These do not need to be in any particular order,
      the code sorts them after they are copied to the run-time table
      each table is terminated at the first entry with ScanCode1+2 == 0


    */
    /*****************************************************************************/

    public partial class KeyboardLayout
    {
        //--Shared by all Keyboard Layouts
        private static KeyTranslationEntry CreateKeyTranslationEntry(byte[] bytes)
        {
            return new KeyTranslationEntry
            {
                ScanCode1 = bytes[0],
                ScanCode2 = bytes[1],
                Row1 = bytes[2],
                Col1 = bytes[3],
                Row2 = bytes[4],
                Col2 = bytes[5]
            };
        }

        private static KeyTranslationEntry[] ToArray(byte[][] raw)
        {
            var terminator = new KeyTranslationEntry
            {
                ScanCode1 = 0,
                ScanCode2 = 0,
                Row1 = 0,
                Col1 = 0,
                Row2 = 0,
                Col2 = 0
            };

            return raw.Select(CreateKeyTranslationEntry).Concat(new List<KeyTranslationEntry> { terminator }).ToArray();
        }

        public static KeyTranslationEntry[] GetKeyboardLayout(KeyboardLayouts keyBoardLayout)
        {
            var map = new Dictionary<KeyboardLayouts, Func<KeyTranslationEntry[]>>
            {
                { KeyboardLayouts.CoCo, GetKeyTranslationsCoCo },
                { KeyboardLayouts.CoCo3, GetKeyTranslationsCoCo3 },
                { KeyboardLayouts.PC, GetKeyTranslationsNatural }
            };

            return map[keyBoardLayout]();
        }

        //--Character isn't used, but included for debugging purposes
        private static byte[] Key(byte scanCode1, byte scanCode2, byte row1, byte col1, byte row2, byte col2, char character = Chr.None) 
            => new[] { scanCode1, scanCode2, row1, col1, row2, col2, (byte)character };

    }
}
