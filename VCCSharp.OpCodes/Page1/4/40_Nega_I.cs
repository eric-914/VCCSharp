using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>40/NEGA/INHERENT</code>
/// Negation (Twos-Complement) of Accumulator <c>A</c>
/// <code>A’ ← 0 - A</code>
/// </summary>
/// <remarks>
/// The <c>NEGA</c> instruction changes the value of the <c>A</c> accumulator to that of it’s twos-complement; that is the value which when added to the original value produces a sum of zero. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x80 (8-bit); cleared otherwise.
///         C The Carry flag is cleared if the original value was 0; set otherwise.
/// 
/// The operation performed by the NEG instruction can be expressed as:
///         result = 0 - value
///         
/// The Carry flag represents a Borrow for this operation and is therefore always set unless the accumulator’s original value was zero.
/// 
/// If the original value of the accumulator is 0x80 then the Overflow flag (V) is set and the accumulator’s value is not modified.
/// 
/// This instruction performs a twos-complement operation. 
/// A ones-complement can be achieved with the COM instruction.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: COM, NEG (memory)
internal class _40_Nega_I : OpCode, IOpCode
{
    public int Exec()
    {
        byte value = (byte)(0 - A);

        CC_N = value.Bit7();
        CC_Z = value == 0;
        CC_V = A == 0x80;
        CC_C = !(A == 0);

        A = value;

        return DynamicCycles._21;
    }
}
