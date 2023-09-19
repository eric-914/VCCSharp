using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>B9/ADCA/EXTENDED</code>
/// Add Memory Byte plus Carry with Accumulator <c>A</c>
/// <code>A’ ← A + (M) + C</code>
/// </summary>
/// <remarks>
/// The <c>ADCA</c> instruction adds the contents of a byte in memory plus the contents of the Carry flag with Accumulator <c>A</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ↕   ↕ ↕ ↕ ↕]
/// 
/// The 8-bit result is placed back into the specified accumulator.
///     H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
///     N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///     Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///     V The Overflow flag is set if an overflow occurred; cleared otherwise.
///     C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
///     
/// The ADC instruction is most often used to perform addition of the subsequent bytes of a multi-byte addition. 
/// This allows the carry from a previous ADD or ADC instruction to be included when doing addition for the next higher-order byte.
/// Since the 6x09 provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
/// 
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: ADCD, ADCR
internal class _B9_Adca_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;
        byte value = M8[address];

        var fn = Add(A, value, CC_C);

        CC_H = fn.H;
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        A = (byte)fn.Result;

        return DynamicCycles._54;
    }
}
