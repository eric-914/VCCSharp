using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10B0/SUBW/EXTENDED</code>
/// Subtract from value in 16-Bit Accumulator <c>W</c>
/// <code>W’ ← W - IMM8|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>SUBW</c> instruction subtracts a 16-bit immediate value from one of the 16-bit accumulator <c>W</c>.
/// The 16-bit result is placed back into the accumulator <c>W</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow out of bit 7 was needed; cleared otherwise.
///         
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
/// 
/// The 16-bit SUB instructions are used for 16-bit subtraction, and for subtraction of the least-significant word of multi-byte subtractions. 
/// See the description of the SBCD instruction for an example of how 32-bit subtraction can be performed on a 6309.
/// 
/// Cycles (8 / 6)
/// Byte Count (4)
/// 
/// See Also: SUB (8-bit), SUBR
internal class _10B0_Subw_E : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._86;

    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;
        ushort value = M16[address];

        var fn = Subtract(W, value);

        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        W = (ushort)fn.Result;

        return CycleCount;
    }
}
