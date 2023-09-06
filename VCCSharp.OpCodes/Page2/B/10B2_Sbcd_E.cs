using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10B2/SBCD/EXTENDED</code>
/// Subtract Memory Byte and Carry (borrow) from Accumulator <c>D</c>
/// <code>D’ ← D - IMM8|(M:M+1) - C</code>
/// </summary>
/// <remarks>
/// The <c>SBCD</c> instruction subtracts the 16-bit immediate value and the value of the Carry flag from the <c>D</c> accumulator. 
/// The 16-bit result is placed back into the <c>D</c> accumulator. 
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
///         Z The Zero flag is set if the new value of Accumulator D is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit 15 was needed; cleared otherwise.
///         
/// The SBCD instruction subtracts either a 16-bit immediate value or the contents of a double-byte value in memory, plus the value of the Carry flag from the D accumulator.
/// The 16-bit result is placed back into Accumulator D. 
/// 
/// The SBCD instruction is most often used to perform subtraction of subsequent words of a multi-byte subtraction. 
/// This allows the borrow from a previous SUB or SBC instruction to be included when doing subtraction for the next higher-order word.
/// 
/// The following instruction sequence is an example showing how 32-bit subtraction can be performed on a 6309 microprocessor:
///         LDQ VAL1ADR     ; Q = 32-bit minuend
///         SUBW VAL2ADR+2  ; Subtract lower half of subtrahend
///         SBCD VAL2ADR    ; Subtract upper half of subtrahend
///         STQ RESULT      ; Store difference
///         
/// Cycles (8 / 6)
/// Byte Count (4)
///         
/// See Also: SBC (8-bit), SBCR
internal class _10B2_Sbcd_E : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;
        ushort value = M16[address];

        var sum = Subtract(D, value, CC_C);

        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        D = (ushort)sum.Result;

        return DynamicCycles._86;
    }
}
