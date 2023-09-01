using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11BE/DIVQ/EXTENDED</code>
/// Signed Divide of Accumulator Q by 16-bit value in Memory
/// <code>
/// W’ ← Q ÷ (M:M+1)
/// D’ ← Q MOD (M:M+1)
/// </code>
/// </summary>
/// <remarks>
/// This instruction divides the 32-bit value in Accumulator <c>Q</c> (the dividend) by a 16-bit value contained in memory (the divisor). 
/// The operation is performed using two’s complement binary arithmetic. 
/// The 32-bit result consists of the 16-bit quotient placed in Accumulator <c>W</c> and the 16-bit remainder placed in Accumulator <c>D</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The sign of the remainder is always the same as the sign of the dividend unless the remainder is zero.
///         N The Negative flag is set equal to the new value of bit 15 in Accumulator W.
///         Z The Zero flag is set if the new value of Accumulator W is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if the quotient in Accumulator W is odd; cleared if even.    
/// 
/// When the value of the specified memory byte (divisor) is zero, a Division-By-Zero exception is triggered. 
/// This causes the CPU to set bit 7 in the MD register, stack the machine state and jump to the address taken from the Illegal Instruction vector at $FFF0.
/// 
/// Two types of overflow are possible when the DIVQ instruction is executed:
/// 
/// • A two’s complement overflow occurs when the sign of the resulting quotient is
///   incorrect. For example, when 80,000 is divided by 2, the result of 40,000 can be
///   represented in 16 bits only as an unsigned value. Since DIVQ is a signed operation, it
///   interprets the result as -25,536 and sets the Negative (N) and Overflow (V) flags.
/// 
/// • A range overflow occurs when the quotient is larger than can be represented in 16
///   bits. For example, when 210,000 is divided by 3, the result of 70,000 exceeds the 16-
///   bit range. In this case, the CPU aborts the operation, leaving the accumulators
///   unmodified while setting the Overflow flag (V) and clearing the N, Z and C flags.
///   
/// Cycles (37 / 36•)
/// Byte Count (4)
/// • When a range overflow occurs, the DIVQ instruction uses 21 fewer cycles than what is shown in the table.
///   
/// See Also: DIVD
internal class _11BE_Divq_E : OpCode6309, IOpCode
{
    internal _11BE_Divq_E(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        const ushort abort = 0xFFFF;
        const ushort overflow = 0x8000;

        ushort address = M16[PC+=2];

        short denominator = (short)M16[address];

        if (denominator == 0)
        {
            return Exceptions.DivideByZero();
        }

        int numerator = (int)Q;
        int result = numerator / denominator;

        if (result > abort || result < -abort) //Abort
        {
            CC_V = true;
            CC_N = false;
            CC_Z = false;
            CC_C = false;

            return Cycles._3635 - 21;
        }

        int remainder = numerator % denominator;

        D = (ushort)remainder;
        W = (ushort)result;

        CC_N = W.Bit15();
        CC_Z = W == 0;
        CC_V = result > ~overflow || result < overflow;
        CC_C = (B & 1) != 0;

        return Cycles._3635; //TODO: Doesn't match
    }
}
