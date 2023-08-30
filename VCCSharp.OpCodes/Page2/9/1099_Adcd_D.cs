using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1099/ADCD/DIRECT</code>
/// Add Memory Byte plus Carry with Accumulator <c>D</c>
/// <code>D’ ← D + (M) + C</code>
/// </summary>
/// <remarks>
/// The <c>ADCD</c> instruction adds the contents of a byte in memory plus the contents of the Carry flag with Accumulator <c>D</c>.
/// The 16 bit result is placed back into Accumulator <c>D</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of the accumulator.
///         Z The Zero flag is set if the new Accumulator D value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a carry out of bit 15 occurred; cleared otherwise.
///         
/// The ADCD instruction is most often used to perform addition of the subsequent words of a multi-byte addition. 
/// This allows the carry from a previous ADD or ADC instruction to be included when doing addition for the next higher-order word.
/// The following instruction sequence is an example showing how 32-bit addition can be performed on a 6309 microprocessor:
///         LDQ VAL1    ; Q = first 32-bit value
///         ADDW VAL2+2 ; Add lower 16 bits of second value
///         ADCD VAL2   ; Add upper 16 bits plus Carry
///         STQ RESULT  ; Store 32-bit result    
///     
/// Cycles (7 / 5)
/// Byte Count (3)
/// 
/// See Also: ADC (8-bit), ADCR
internal class _1099_Adcd_D : OpCode6309, IOpCode
{
    internal _1099_Adcd_D(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        ushort value = M16[address];

        var sum = Add(D, value.Plus(CC_C));

        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        D = (ushort)sum.Result;

        return Cycles._75;
    }
}
