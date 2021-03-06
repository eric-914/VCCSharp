﻿using System.Collections.Generic;
using System.Linq;

namespace VCCSharp.Models.Keyboard
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

      PC Keyboard:
      +---------------------------------------------------------------------------------+
      | [Esc]   [F1][F2][F3][F4][F5][F6][F7][F8][F9][F10][F11][F12]   [Prnt][Scr][Paus] |
      |                                                                                 |
      | [`~][1!][2@][3#][4$][5%][6^][7&][8*][9(][0]][-_][=+][BkSpc]   [Inst][Hom][PgUp] |
      | [  Tab][Qq][Ww][Ee][Rr][Tt][Yy][Uu][Ii][Oo][Pp][[{][]}][\|]   [Dlet][End][PgDn] |
      | [  Caps][Aa][Ss][Dd][Ff][Gg][Hh][Jj][Kk][Ll][;:]['"][Enter]                     |
      | [  Shift ][Zz][Xx][Cc][Vv][Bb][Nn][Mm][,<][.>][/?][ Shift ]         [UpA]       |
      | [Cntl][Win][Alt][        Space       ][Alt][Win][Prp][Cntl]   [LftA][DnA][RgtA] |
      +---------------------------------------------------------------------------------+

      TODO: explain and add link or reference to CoCo 'scan codes' for each key
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
    }
}
