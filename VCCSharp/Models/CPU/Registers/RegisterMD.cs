using VCCSharp.Enums;

namespace VCCSharp.Models.CPU.Registers
{
    /// <summary>
    /// The MD register is a mode and error register and works much in the same way as the CC register.The bit definitions are as follows:
    ///  Write bits
    ///     Bit 0 - Execution mode of the 6309.
    ///         If clear( 0 ), the cpu is in 6809 emulation mode.
    ///         If set ( 1 ), the cpu is in 6309 native mode.
    ///     Bit 1 - FIRQ mode
    ///         If clear ( 0 ), the FIRQ will occur normally.
    ///         If set( 1 ) , the FIRQ will operate the same as the IRQ
    ///         
    ///  Bits 2 to 5 are unused
    ///  
    ///  Read bits - One of these bits is set when the 6309 traps an error
    ///     Bit 6 - This bit is set( 1 ) if an illegal instruction is encountered
    ///     Bit 7 - This bit is set( 1 ) if a zero division occurs.
    /// </summary>
    /// <url>
    /// https://colorcomputerarchive.com/repo/Documents/Microprocessors/HD6309/HD63B09EP%20Technical%20Reference%20Guide.pdf
    /// </url>
    public class RegisterMD
    {
        private readonly byte _NATIVE6309 = 1 << (byte)MDFlagMasks.NATIVE6309;
        private readonly byte _FIRQMODE = 1 << (byte)MDFlagMasks.FIRQMODE;
        private readonly byte _UNDEF2 = 1 << (byte)MDFlagMasks.MD_UNDEF2;
        private readonly byte _UNDEF3 = 1 << (byte)MDFlagMasks.MD_UNDEF3;
        private readonly byte _UNDEF4 = 1 << (byte)MDFlagMasks.MD_UNDEF4;
        private readonly byte _UNDEF5 = 1 << (byte)MDFlagMasks.MD_UNDEF5;
        private readonly byte _ILLEGAL = 1 << (byte)MDFlagMasks.ILLEGAL;
        private readonly byte _ZERODIV = 1 << (byte)MDFlagMasks.ZERODIV;

        public byte bits;

        public bool NATIVE6309 { get { return Get(_NATIVE6309); } set { Set(_NATIVE6309, value); } }
        public bool FIRQMODE { get { return Get(_FIRQMODE); } set { Set(_FIRQMODE, value); } }
        public bool MD_UNDEF2 { get { return Get(_UNDEF2); } set { Set(_UNDEF2, value); } }
        public bool MD_UNDEF3 { get { return Get(_UNDEF3); } set { Set(_UNDEF3, value); } }
        public bool MD_UNDEF4 { get { return Get(_UNDEF4); } set { Set(_UNDEF4, value); } }
        public bool MD_UNDEF5 { get { return Get(_UNDEF5); } set { Set(_UNDEF5, value); } }
        public bool ILLEGAL { get { return Get(_ILLEGAL); } set { Set(_ILLEGAL, value); } }
        public bool ZERODIV { get { return Get(_ZERODIV); } set { Set(_ZERODIV, value); } }

        private bool Get(byte mask)
        {
            return (bits & mask) != 0;
        }

        private void Set(byte mask, bool value)
        {
            if (value)
                bits |= mask;
            else
                bits &= (byte)~mask;
        }

    }
}
