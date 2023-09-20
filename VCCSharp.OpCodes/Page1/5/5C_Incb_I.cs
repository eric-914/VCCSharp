using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>5C/INCB/INHERENT</code>
/// Increment Accumulator <c>B</c>
/// <code>B’ ← B + 1</code>
/// </summary>
/// <remarks>
/// The <c>INCB</c> instruction adds 1 to the contents of the <c>B</c> accumulator. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x7F (8-bit); cleared otherwise.
///         
/// It is important to note that the INC instructions do not affect the Carry flag. 
/// This means that it is not always possible to optimize code by simply replacing an ADDB #1 instruction with a corresponding INCB.
/// 
/// When used to increment an unsigned value, only the BEQ and BNE branches will consistently behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ADD, DEC, INC (memory)
internal class _5C_Incb_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public int Exec()
    {
        byte result = (byte)(B + 1);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = B == 0x7F;

        B = result;

        return CycleCount;
    }
}
