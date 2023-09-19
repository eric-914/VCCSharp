using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11EB/ADDE/INDEXED</code>
/// Add Memory Byte to 8-Bit Accumulator <c>F</c>
/// <code>F’ ← F + (M)</code>
/// </summary>
/// <remarks>
/// The <c>ADDF</c> instruction adds the contents of a byte in memory with the 8-bit <c>F</c> accumulator.
/// The 8-bit result is placed back into the <c>F</c> accumulator.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ↕   ↕ ↕ ↕ ↕]
/// 
///         H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
///         
/// The 8-bit ADD instructions are used for single-byte addition, and for addition of the least-significant byte in multi-byte additions. 
/// Since the 6x09 also provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
/// 
/// Cycles (5+)
/// Byte Count (3+)
/// 
/// See Also: ADD (16-bit), ADDR
internal class _11EB_Addf_X : OpCode6309, IOpCode
{
    public int Exec()
    {
        Cycles = 5;

        ushort address = INDEXED[PC++];
        byte value = M8[address];

        var fn = Add(F, value);

        CC_H = fn.H;
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        F = (byte)fn.Result;

        return Cycles;
    }
}
