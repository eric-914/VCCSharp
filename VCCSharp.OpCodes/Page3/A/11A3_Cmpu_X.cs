using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11A3/CMPU/INDEXED</code>
/// Compare Memory Word from 16-Bit <c>U</c> Register
/// <code>TEMP ← U - (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>COMPU</c> instruction subtracts the contents of a double-byte value in memory from the value contained in the 16-bit <c>U</c> accumulator and sets the Condition Codes accordingly. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// Neither the memory bytes nor the register are modified unless an auto-increment / auto-decrement addressing mode is used with the same register.
///         N The Negative flag is set equal to the value of bit 15 of the result.
///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit 15 was needed; cleared otherwise
/// 
/// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
/// 
/// The 16-bit CMP instructions for accumulators perform exactly the same operation as the 16-bit SUB instructions, with the exception that the value in the accumulator is not changed. 
/// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
/// 
/// Cycles (7+ / 6+)
/// Byte Count (3+)
/// 
/// See Also: CMP (8-bit), CMPR
internal class _11A3_Cmpu_X : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = INDEXED[PC++];
        ushort value = M16[address];

        var sum = Subtract(U, value);

        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        return DynamicCycles._76;
    }
}
