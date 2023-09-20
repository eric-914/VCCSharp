using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>109B/ADDW/DIRECT</code>
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
/// Cycles (7 / 5)
/// Byte Count (4)
/// 
/// See Also: ADD (8-bit), ADDR
internal class _109B_Addw_D : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._75;

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        ushort value = M16[address];

        var fn = Add(W, value);

        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        W = (ushort)fn.Result;

        return CycleCount;
    }
}
