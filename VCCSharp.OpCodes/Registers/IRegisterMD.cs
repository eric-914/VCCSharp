namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// MODE AND ERROR REGISTER
/// </summary>
/// <remarks>
/// The MD register is a mode and error register and works much in the same way as the CC register. 
/// </remarks>
/// <documentation>
/// The bit definitions are as follows:
/// ╭───┬───┬───┬───┬───┬───┬───┬───╮
/// │ 7 │ 6 │ 5 │ 4 │ 3 │ 2 │ 1 │ 0 │
/// ╰─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─╯
///   │   │   │   │   │   │   │   ╰─ WRITE: Execution mode of the 6309. If clear (0), the cpu is in 6809 emulation mode. If set (1), the cpu is in 6309 native mode.
///   │   │   │   │   │   │   ╰───── WRITE: FIRQ mode. If clear (0), the FIRQ will occur normally. If set (1) , the FIRQ will operate the same as the IRQ.
///   │   │   │   │   │   │
///   │   │   │   │   │   ╰───────── Unused
///   │   │   │   │   ╰───────────── Unused
///   │   │   │   ╰───────────────── Unused
///   │   │   ╰───────────────────── Unused
///   │   │
///   │   │                          *** One of these bits is set when the 6309 traps an error *** 
///   │   ╰───────────────────────── READ: This bit is set (1) if an illegal instruction is encountered.
///   ╰───────────────────────────── READ: This bit is set (1) if a zero division occurs.
/// </documentation>
public interface IRegisterMD
{
    byte MD { get; set; }

    /// <summary>
    /// EXECUTION MODE REGISTER: BIT 0
    /// </summary>
    /// <remarks>
    /// Execution mode of the 6309. 
    /// If clear (0), the cpu is in 6809 emulation mode. 
    /// If set (1), the cpu is in 6309 native mode.
    /// </remarks>
    bool MD_NATIVE6309 { get; }

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
    bool MD_FIRQMODE { get; }

    /// <summary>
    /// ILLEGAL INSTRUCTION MODE REGISTER: BIT 6
    /// </summary>
    /// <remarks>
    /// This bit is set (1) if an illegal instruction is encountered.
    /// </remarks>
    bool MD_ILLEGAL { set; }

    /// <summary>
    /// DIVIDE BY ZERO MODE REGISTER: BIT 7
    /// </summary>
    /// <remarks>
    /// This bit is set (1) if a zero division occurs.
    /// </remarks>
    bool MD_ZERODIV { set; }
}

