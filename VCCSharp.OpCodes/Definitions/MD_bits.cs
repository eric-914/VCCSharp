namespace VCCSharp.OpCodes.Definitions;

internal enum MD_bits
{
    /// <summary>
    /// EXECUTION MODE REGISTER: BIT 0
    /// </summary>
    /// <remarks>
    /// Execution mode of the 6309. 
    /// If clear (0), the cpu is in 6809 emulation mode. 
    /// If set (1), the cpu is in 6309 native mode.
    /// </remarks>
    NATIVE6309 = 0,

    /// <summary>
    /// FIRQ MODE REGISTER: BIT 1
    /// </summary>
    /// <remarks>
    /// FIRQ mode. 
    /// If clear (0), the FIRQ will occur normally. 
    /// If set (1) , the FIRQ will operate the same as the IRQ.
    /// </remarks>
    /// <notes>
    /// This isn't used by any opcodes
    /// </notes>
    FIRQMODE = 1,

    /// <summary>
    /// ILLEGAL INSTRUCTION MODE REGISTER: BIT 6
    /// </summary>
    /// <remarks>
    /// This bit is set (1) if an illegal instruction is encountered.
    /// </remarks>
    ILLEGAL = 6,

    /// <summary>
    /// DIVIDE BY ZERO MODE REGISTER: BIT 7
    /// </summary>
    /// <remarks>
    /// This bit is set (1) if a zero division occurs.
    /// </remarks>
    ZERODIV = 7
}
