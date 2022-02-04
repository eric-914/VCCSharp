using System;
using System.Collections.Generic;
using VCCSharp.Models;
using VCCSharp.Models.Keyboard.Definitions;

namespace VCCSharp.Modules
{
    public class KeyTranslationEntryIComparer : IComparer<KeyTranslationEntry>
    {
        public int Compare(KeyTranslationEntry entry1, KeyTranslationEntry entry2)
        {
            if (entry1 == null) return 1;
            if (entry2 == null) return -1;

            // empty listing push to end
            if (entry1.ScanCode1 == 0 && entry1.ScanCode2 == 0 && entry2.ScanCode1 != 0) return 1;
            if (entry2.ScanCode1 == 0 && entry2.ScanCode2 == 0 && entry1.ScanCode1 != 0) return -1;

            // push shift/alt/control by themselves to the end
            if (entry1.ScanCode2 == 0 && (entry1.ScanCode1 == DIK.DIK_LSHIFT || entry1.ScanCode1 == DIK.DIK_LMENU || entry1.ScanCode1 == DIK.DIK_LCONTROL)) return 1;
            // push shift/alt/control by themselves to the end
            if (entry2.ScanCode2 == 0 && (entry2.ScanCode1 == DIK.DIK_LSHIFT || entry2.ScanCode1 == DIK.DIK_LMENU || entry2.ScanCode1 == DIK.DIK_LCONTROL)) return -1;

            // move double key combos in front of single ones
            if (entry1.ScanCode2 == 0 && entry2.ScanCode2 != 0) return 1;

            // move double key combos in front of single ones
            if (entry2.ScanCode2 == 0 && entry1.ScanCode2 != 0) return -1;

            var result = entry1.ScanCode1 - entry2.ScanCode1;

            if (result == 0) result = entry1.Row1 - entry2.Row1;

            if (result == 0) result = entry1.Col1 - entry2.Col1;

            if (result == 0) result = entry1.Row2 - entry2.Row2;

            if (result == 0) result = entry1.Col2 - entry2.Col2;

            return result;
        }
    }
}