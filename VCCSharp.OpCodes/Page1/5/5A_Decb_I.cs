using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>5A/DECB/INHERENT</code>
/// Decrement Accumulator <c>B</c>
/// <code>B’ ← B - 1</code>
/// </summary>
/// <remarks>
/// The <c>DECB</c> instructions subtracts 1 from the <c>B</c> accumulator. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
/// 
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x80 (8-bit); cleared otherwise.
///         
/// It is important to note that the DEC instructions do not affect the Carry flag. 
/// This means that it is not always possible to optimize code by simply replacing a SUBB #1 instruction with a corresponding DECB. 
/// Because the DEC instructions do not affect the Carry flag, they can be used to implement loop counters within multiple precision computations.
/// 
/// When used to decrement an unsigned value, only the BEQ and BNE branches will always behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: DEC (memory), INC, SUB
internal class _5A_Decb_I : OpCode, IOpCode
{
    internal _5A_Decb_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte result = (byte)(B - 1);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = B == 0x80;

        B = result;

        return Cycles._21;
    }
}
