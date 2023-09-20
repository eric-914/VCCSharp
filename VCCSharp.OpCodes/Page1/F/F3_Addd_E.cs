using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>F3/ADDD/EXTENDED</code>
/// Add Memory Word to 16-Bit Accumulator <c>D</c>
/// <code>D’ ← D + (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>ADDD</c> instruction adds the contents of a double-byte value in memory with the 16-bit <c>D</c> accumulator.
/// The 16-bit result is placed back into the <c>D</c> accumulator. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
///         N The Negative flag is set equal to the new value of bit 15 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
///     
/// The 16-bit ADD instructions are used for double-byte addition, and for addition of the least-significant word of multi-byte additions. 
/// See the description of the ADCD instruction for an example of how 32-bit addition can be performed on a 6309 processor.
/// 
/// Cycles (7 / 5)
/// Byte Count (3)
/// 
/// See Also: ADD (8-bit), ADDR
internal class _F3_Addd_E : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;
        ushort value = M16[address];

        var fn = Add(D, value);

        //CC_H = sum.H; //--Not applicable
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        D = (ushort)fn.Result;

        return CycleCount;
    }
}
