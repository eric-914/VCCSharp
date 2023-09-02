using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>98/EORA/DIRECT</code>
/// Exclusive-OR (XOR) Memory Byte with Accumulator <c>A</c>
/// <code>A’ ← A ⨁ (M)</code>
/// </summary>
/// <remarks>
/// The <c>EORA</c> instruction XORs the contents of a byte in memory with Accumulator <c>A</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The 8-bit result is then placed in the specified accumulator.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///         
/// The EOR instruction produces a result containing '1' bits in the positions where the corresponding bits in the two operands have different values. 
/// Exclusive-OR logic is often used in parity functions.
/// 
/// EOR can also be used to perform "bit-flipping" since a '1' bit in the source operand will invert the value of the corresponding bit in the destination operand. 
/// For example:
///         EORA #%00000100 ;Invert value of bit 2 in Accumulator A
///         
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: BEOR, BIEOR, EIM, EORD, EORR
internal class _98_Eora_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];

        A ^= M8[address];

        CC_N = A.Bit7();
        CC_Z = A == 0;
        CC_V = false;

        return DynamicCycles._43;
    }
}
