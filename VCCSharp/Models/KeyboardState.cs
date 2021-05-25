namespace VCCSharp.Models
{
    public struct KeyboardState
    {
        /** run-time 'rollover' table to pass to the MC6821 when a key is pressed */
        public unsafe fixed byte RolloverTable[8];	// CoCo 'keys' for emulator

        /** track all keyboard scan codes state (up/down) */
        public unsafe fixed int ScanTable[256];

        /** run-time key translation table - convert key up/down messages to 'rollover' codes */
        // run-time keyboard layout table (key(s) to keys(s) translation)
        public unsafe KeyTranslationEntry* KeyTransTable;
        //KeyTranslationEntry KeyTransTable[KBTABLE_ENTRY_COUNT];	
    }
}
