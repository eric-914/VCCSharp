namespace VCCSharp.OpCodes.Definitions;

internal enum CC_bits
{
    /// <summary>
    /// CONDITION CODE REGISTER: BIT 0 (C)
    /// </summary>
    /// <remarks>
    /// Bit is the carry flag and is usually the carry from the binary ALU. 
    /// C is also used to represent a "borrow" from subtract like instructions (CMP, NEG, SUB, SBC) and is the complement of the carry from the binary ALU.
    /// </remarks>
    C = 0,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 1 (V)
    /// </summary>
    /// <remarks>
    /// Bit 1 is the overflow flag and is set to a one by an operation which causes a signed twos complement arithmetic overflow. 
    /// This overflow is detected in an operation in which the carry from the MSB in the ALU does not match the carry from the MSB-1.
    /// </remarks>
    V = 1,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 2 (Z)
    /// </summary>
    /// <remarks>
    /// Bit 2 is the zero flag and is set to a one if the result of the previous operation was identically zero.
    /// </remarks>
    Z = 2,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 3 (N)
    /// </summary>
    /// <remarks>
    /// Bit 3 is the negative flag, which contains exactly the value of the MSB of the result of the preceding operation. 
    /// Thus, a negative twos complement result will leave N set to a one.
    /// </remarks>
    N = 3,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 4 (I)
    /// </summary>
    /// <remarks>
    /// Bit 4 is the ~IRQ mask bit. 
    /// The processor will not recognize interrupts from the ~IRQ line if this bit is set to a one. 
    /// ~NMI, ~FIRQ, ~IRQ, ~RESET and SWI all set I to a one.
    /// SWI2 and SWI3 do not affect I.
    /// </remarks>
    I = 4,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 5 (H)
    /// </summary>
    /// <remarks>
    /// Bit 5 is the half-carry bit, and is used to indicate a carry from bit 3 in the ALU as a result of an 8-bit addition only (ADC or ADD). 
    /// This bit is used by the DAA instruction to perform a BCD decimal add adjust operation. 
    /// The state of this flag is undefined in all subtract-like instructions.
    /// </remarks>
    H = 5,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 6 (F)
    /// </summary>
    /// <remarks>
    /// Bit 6 is the ~FIRQ mask bit. 
    /// The processor will not recognize interrupts from the ~FIRQ line if this bit is a one. 
    /// ~NMI, ~FIRQ, SWI, and ~RESET all set F to a one. 
    /// ~IRQ, SWI2, and SWI3 do not affect F.
    /// </remarks>
    F = 6,

    /// <summary>
    /// CONDITION CODE REGISTER: BIT 7 (E)
    /// </summary>
    /// <remarks>
    /// Bit 7 is the entire flag, and when set to a one indicates that the complete machine state (all the registers) was stacked, as opposed to the subset state (PC and CC). 
    /// The E bit of the stacked CC is used on a return from interrupt (RTI) to determine the extent of the unstacking. 
    /// Therefore, the current E left in the condition code register represents past action.
    /// </remarks>
    E = 7
}
