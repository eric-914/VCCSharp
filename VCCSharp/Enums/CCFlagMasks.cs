using System.ComponentModel;

namespace VCCSharp.Enums;

/// <summary>
/// The condition code register contains the condition codes and the interrupt masks.
/// https://www.maddes.net/m6809pm/sections.htm
/// Section 1.10
/// </summary>
public enum CCFlagMasks
{
    /// <summary>
    /// This bit is used to indicate how many registers were stacked. When set, all the registers were stacked during the last interrupt stacking operation. When clear, only the program counter and condition code registers were stacked during the last interrupt.
    /// The state of the E bit in the stacked condition code register is used by the return from interrupt (RTI) instruction to determine the number of registers to be unstacked.
    /// </summary>
    [Description("Entire Flag")]
    E = 7,

    /// <summary>
    /// This bit is used to mask (disable) any fast interrupt request line (!FIRQ). This bit is set automatically by a hardware reset or after recognition of another interrupt. Execution of certain instructions such as SWI will also inhibit recognition of a !FIRQ input.
    /// </summary>
    [Description("FIRQ Mask")]
    F = 6,
    
    /// <summary>
    /// This bit is used to indicate that a carry was generated from bit three in the arithmetic logic unit as a result of an 8-bit addition. This bit is undefined in all subtract-like instructions. The decimal addition adjust (DAA) instruction uses the state of this bit to perform the adjust operation.
    /// </summary>
    [Description("Half Carry")]
    H = 5,

    /// <summary>
    /// This bit is used to mask (disable) any interrupt request input (!IRQ). This bit is set automatically by a hardware reset or after recognition of another interrupt. Execution of certain instructions such as SWI will also inhibit recognition of an !IRQ input.
    /// </summary>
    [Description("IRQ Mask")]
    I = 4,
    
    /// <summary>
    /// This bit contains the value of the most-significant bit of the result of the previous data operation.
    /// </summary>
    [Description("Negative")]
    N = 3,
    
    /// <summary>
    /// This bit is used to indicate that the result of the previous operation was zero.
    /// </summary>
    [Description("Zero")]
    Z = 2,
    
    /// <summary>
    /// This bit is used to indicate that the previous operation caused a signed arithmetic overflow.
    /// </summary>
    [Description("Overflow")]
    V = 1,

    /// <summary>
    /// This bit is used to indicate that a carry or a borrow was generated from bit seven in the arithmetic logic unit as a result of an 8-bit mathematical operation.
    /// </summary>
    [Description("Carry")]
    C = 0
}
