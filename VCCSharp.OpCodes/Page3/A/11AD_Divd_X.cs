using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11AD/DIVD/INDEXED</code>
/// Signed Divide of Accumulator D by 8-bit value in Memory
/// <code>
/// B’ ← D ÷ (M)
/// A’ ← D MOD (M)
/// </code>
/// </summary>
/// <remarks>
/// The <c>DIVD</c> instruction divides the 16-bit value in Accumulator <c>D</c> (the dividend) by an 8-bit value contained in a memory byte (the divisor). 
/// The operation is performed using two’s complement binary arithmetic. 
/// The 16-bit result consists of the 8-bit quotient placed in Accumulator <c>B</c> and the 8-bit remainder placed in Accumulator <c>A</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The sign of the remainder is always the same as the sign of the dividend unless the remainder is zero.
///         N The Negative flag is set equal to the new value of bit 7 in Accumulator B.
///         Z The Zero flag is set if the new value of Accumulator B is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if the quotient in Accumulator B is odd; cleared if even.
///         
/// When the value of the specified memory byte (divisor) is zero, a Division-By-Zero exception is triggered. 
/// This causes the CPU to set bit 7 in the MD register, stack the machine state and jump to the address taken from the Illegal Instruction vector at $FFF0.
/// Two types of overflow may occur when the DIVD instruction is executed:
/// 
/// • A two’s complement overflow occurs when the sign of the resulting quotient is
///   incorrect. For example, when 300 is divided by 2, the result of 150 can be represented
///   in 8 bits only as an unsigned value. Since DIVD performs a signed operation, it
///   interprets the result as -106 and sets the Negative (N) and Overflow (V) flags.
///   
/// • A range overflow occurs when the quotient is larger than can be represented in 8 bits.
///   For example, when 900 is divided by 3, the result of 300 exceeds the 8-bit range. In
///   this case, the CPU aborts the operation, leaving the accumulators unmodified while
///   setting the Overflow flag (V) and clearing the N, Z and C flags.
///   
/// Cycles (27+•)
/// Byte Count (3+)
/// • If a two’s complement overflow occurs, the DIVD instruction uses one fewer cycle than what is shown in the table. 
///   If a range overflow occurs, DIVD uses 13 fewer cycles than what is shown in the table.
/// 
/// See Also: DIVQ
internal class _11AD_Divd_X : OpCode6309, IOpCode
{
    public int Exec()
    {
        Cycles = 27;

        ushort address = INDEXED[PC++];

        short numerator = (short)D;
        sbyte denominator = (sbyte)M8[address];

        var fn = Divide(numerator, denominator, Cycles);

        if (fn.Error == DivisionErrors.DivideByZero)
        {
            return Cycles - 24 + Exceptions.DivideByZero(); // (27-24) 3 + INDEXED cycles to read byte and increment PC and compare to zero.
        }

        if (fn.Error == DivisionErrors.None)
        {
            A = (byte)fn.Remainder;
            B = (byte)fn.Result;
        }

        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        return fn.Cycles;
    }
}
