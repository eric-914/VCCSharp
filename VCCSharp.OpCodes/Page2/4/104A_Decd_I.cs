using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>104A/DECD/INHERENT</code>
/// Decrement Accumulator <c>D</c>
/// <code>D’ ← D - 1</code>
/// </summary>
/// <remarks>
/// The <c>DECD</c> instruction subtracts 1 from the <c>D</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x8000 (16-bit); cleared otherwise.
///         
/// It is important to note that the DEC instructions do not affect the Carry flag. 
/// This means that it is not always possible to optimize code by simply replacing a SUBr #1 instruction with a corresponding DECr. 
/// Because the DEC instructions do not affect the Carry flag, they can be used to implement loop counters within multiple precision computations.
/// 
/// When used to decrement an unsigned value, only the BEQ and BNE branches will always behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: DEC (memory), INC, SUB
internal class _104A_Decd_I : OpCode6309, IOpCode
{
    internal _104A_Decd_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort result = (ushort)(D - 1);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = D == 0x8000;

        D = result;

        return Cycles._32;
    }
}
