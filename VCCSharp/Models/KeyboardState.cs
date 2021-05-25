namespace VCCSharp.Models
{
    public struct KeyboardState
    {
        /** run-time key translation table - convert key up/down messages to 'rollover' codes */
        // run-time keyboard layout table (key(s) to keys(s) translation)
        public unsafe KeyTranslationEntry* KeyTransTable;
        //KeyTranslationEntry KeyTransTable[KBTABLE_ENTRY_COUNT];	
    }
}
