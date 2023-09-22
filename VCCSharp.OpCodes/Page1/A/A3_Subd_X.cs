using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>A3/SUBD/INDEXED</code>
/// Subtract from value in 16-Bit Accumulator <c>D</c>
/// <code>D’ ← D - IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>SUBD</c> instruction subtracts the contents of a doublebyte value in memory into the 16-bit <c>D</c>accumulator. 
/// The 16-bit result is placed back into the <c>D</c> accumulator. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
///         N The Negative flag is set equal to the new value of bit 15 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow out of bit 7 was needed; cleared otherwise.
///         
/// The 16-bit SUB instructions are used for 16-bit subtraction, and for subtraction of the least-significant word of multi-byte subtractions. 
/// 
/// Cycles (6+ / 5+)
/// Byte Count (2)
/// 
/// See Also: SUB (8-bit), SUBR
internal class _A3_Subd_X : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._65;

    public void Exec()
    {
        ushort address = INDEXED[PC++];
        ushort value = M16[address];

        var fn = Subtract(D, value);

        //CC_H = sum.H; //--Not applicable
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        D = (ushort)fn.Result;
    }
}
