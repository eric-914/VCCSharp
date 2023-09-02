using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>108B/ADDW/IMMEDIATE</code>
/// Add Memory Word to 16-Bit Accumulator <c>W</c>
/// <code>W’ ← W + (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>ADDW</c> instruction adds the contents of a double-byte value in memory with the 16-bit <c>W</c> accumulator.
/// The 16-bit result is placed back into the <c>W</c> accumulator. 
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
///         C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
///     
/// The 16-bit ADDW instruction is used for double-byte addition, and for addition of the least-significant word of multi-byte additions. 
/// See the description of the ADCD instruction for an example of how 32-bit addition can be performed on a 6309 processor.
/// 
/// Cycles (5 / 4)
/// Byte Count (4)
/// 
/// See Also: ADD (8-bit), ADDR
internal class _108B_Addw_M : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort value = M16[PC += 2];

        var sum = Add(W, value);

        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        W = (ushort)sum.Result;

        return DynamicCycles._54;
    }
}
