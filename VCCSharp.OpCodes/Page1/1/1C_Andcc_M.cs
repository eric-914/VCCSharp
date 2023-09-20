using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1C/ANDCC/IMMEDIATE</code>
/// Logically AND Immediate Value with the CC Register
/// <code>CC’ ← CC AND IMM</code>
/// </summary>
/// <remarks>
/// The <c>ANDCC</c> instruction logically ANDs the contents of the Condition Codes register with the immediate byte specified in the instruction with the result placed back into the CC register.
/// </remarks>
/// 
/// This instruction logically ANDs the contents of the Condition Codes register with the immediate byte specified in the instruction. 
/// The result is placed back into the Condition Codes register.
/// The ANDCC instruction provides a method to clear specific flags in the Condition Codes register. 
/// All flags that correspond to "0" bits in the immediate operand are cleared, while those corresponding with "1"s are left unchanged.
/// The bit numbers for each flag are shown below:
///     7 6 5 4 3 2 1 0
///     E F H I N Z V C
/// 
/// One of the more common uses for the ANDCC instruction is to clear the IRQ and FIRQ Interrupt Masks (I and F) at the completion of a routine that runs with interrupts disabled.
/// This is accomplished by executing:
///     ANDCC #$AF ; Clear bits 4 and 6 in CC
/// 
/// Some assemblers will accept a comma-delimited list of the bit names to be cleared as an alternative to the immediate expression. 
/// For instance, the example above might also be written as:
///     ANDCC I,F ; Clear bits 4 and 6 in CC  
/// 
/// This syntax is generally discouraged due to the confusion it can create as to whether it means clear the I and F bits, or clear all bits except I and F.    
/// 
/// More examples:
///     ANDCC #$FE ; Clear the Carry flag
///     ANDCC #1 ; Clear all flags except Carry
///     
/// Cycles (3)
/// Byte Count (2)
/// 
/// ANDCC #i8
/// I8 : 8-bit Immediate value
/// 
/// See Also: AND (8-bit), ANDD, ANDR, CWAI, ORCC
internal class _1C_Andcc_M : OpCode, IOpCode
{
    public int CycleCount => 3;

    public int Exec()
    {
        byte value = M8[PC++];

        byte result = (byte)(CC & value);

        CC = result;

        return CycleCount;
    }
}
