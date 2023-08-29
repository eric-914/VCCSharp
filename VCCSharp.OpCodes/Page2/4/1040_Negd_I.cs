using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1040/NEGD/INHERENT</code>
/// Negation (Twos-Complement) of Accumulator <c>D</c>
/// <code>D’ ← 0 - D</code>
/// </summary>
/// <remarks>
/// The <c>NEGD</c> instruction changes the value of the <c>D</c> accumulator to that of its twos-complement; that is the value which when added to the original value produces a sum of zero. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x8000 (16-bit); cleared otherwise.
///         C The Carry flag is cleared if the original value was 0; set otherwise.
/// 
/// The operation performed by the NEG instruction can be expressed as:
///         result = 0 - value
///         
/// The Carry flag represents a Borrow for this operation and is therefore always set unless the accumulator’s original value was zero.
/// 
/// If the original value of the accumulator is 0x8000 then the Overflow flag (V) is set and the accumulator’s value is not modified.
/// 
/// This instruction performs a twos-complement operation. 
/// A ones-complement can be achieved with the COMD instruction.
/// 
/// A 32-bit negation of Q can be achieved with the following instructions:
///         COMD
///         COMW
///         ADCR 0,W
///         ADCR 0,D
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: COM, NEG (memory)
internal class _1040_Negd_I : OpCode6309, IOpCode
{
    internal _1040_Negd_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort result = (ushort)(0 - D);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = D == 0x8000;
        CC_C = D == 0;

        D = result;

        return Cycles._32;
    }
}
