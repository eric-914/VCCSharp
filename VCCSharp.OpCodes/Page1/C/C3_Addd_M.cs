using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>C3/ADDD/IMMEDIATE</code>
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
/// Cycles (4 / 3)
/// Byte Count (3)
/// 
/// See Also: ADD (8-bit), ADDR
internal class _C3_Addd_M : OpCode, IOpCode
{
    internal _C3_Addd_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort value = M16[PC += 2];

        var sum = Add(A, value);

        //CC_H = sum.H; //--Not applicable
        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        D = (ushort)sum.Result;

        return Cycles._43;
    }
}
