namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// CONDITION CODE REGISTER
/// </summary>
/// <documentation>
/// The condition code register defines the state of the processor at any given time.
/// ╭───┬───┬───┬───┬───┬───┬───┬───╮
/// │ E │ F │ H │ I │ N │ Z │ V │ C │
/// ╰─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─┴─┬─╯
///   │   │   │   │   │   │   │   ╰─ Carry
///   │   │   │   │   │   │   ╰───── Overflow
///   │   │   │   │   │   ╰───────── Zero
///   │   │   │   │   ╰───────────── Negative
///   │   │   │   ╰───────────────── IRQ Mask
///   │   │   ╰───────────────────── Half Carry
///   │   ╰───────────────────────── FIRQ Mask
///   ╰───────────────────────────── Entire Flag
/// </documentation>
public interface IRegisterCC
{
    byte CC { get; set; }
}
