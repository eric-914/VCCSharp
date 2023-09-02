using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1A/ORCC/IMMEDIATE</code>
/// Logically OR the CC Register with an Immediate Value
/// <code>CC’ ← CC OR IMM8</code>
/// </summary>
/// <remarks>
/// This instruction logically ORs the contents of the Condition Codes register with the 8-bit immediate value specified in the operand with the result place back into the CC register.
/// </remarks>
/// 
/// This instruction logically ORs the contents of the Condition Codes register with the 8-bit immediate value specified in the operand. 
/// The result is placed back into the Condition Codes register.
/// 
/// The ORCC instruction provides a method to set specific flags in the Condition Codes register. 
/// All flags that correspond to '1' bits in the immediate operand are set, while those corresponding with '0's are left unchanged.
/// 
/// The bit numbers for each flag are shown below:
///     7 6 5 4 3 2 1 0
///     E F H I N Z V C
///     
/// One of the more common uses for the ORCC instruction is to set the IRQ and FIRQ Interrupt Masks (I and F) at the beginning of a routine that must run with interrupts disabled. 
/// This is accomplished by executing:
///         ORCC #$50 ; Set bits 4 and 6 in CC
///         
/// Some assemblers will accept a comma-delimited list of the bit names as an alternative to the immediate value. 
/// For instance, the example above might also be written as:
///         ORCC I,F  ; Set bits 4 and 6 in CC
///         
/// More examples:
///         ORCC #1   ; Set the Carry flag
///         ORCC #$80 ; Set the Entire flag
///         
/// Cycles (3)
/// Byte Count (2)
/// 
/// ORCC #i8
/// I8 : 8-bit Immediate value
/// 
/// See Also: ANDCC, OR (8-bit), ORD, ORR
internal class _1A_Orcc_M : OpCode, IOpCode
{
    internal _1A_Orcc_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];

        byte result = (byte)(CC | value);

        CC = result;

        return DynamicCycles._32;
    }
}
