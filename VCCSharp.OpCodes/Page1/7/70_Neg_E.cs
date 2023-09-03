using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>70/NEG/EXTENDED</code>
/// Negate (Twos Complement) a Byte in Memory
/// <code>(M)’ ← 0 - (M)</code>
/// </summary>
/// <remarks>
/// This instruction changes the value of a byte in memory to that of it’s twos-complement; that is the value which when added to the original value produces a sum of zero. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of bit 7.
///         Z The Zero flag is set if the new value is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was 0x80; cleared otherwise.
///         C The Carry flag is cleared if the original value was 0; set otherwise.
/// 
/// The operation performed by the NEG instruction can be expressed as:
///         result = 0 - value
///         
/// The Carry flag represents a Borrow for this operation and is therefore always set unless the memory byte’s original value was zero.
/// 
/// If the original value of the memory byte is 0x80 then the Overflow flag (V) is set and the byte’s value is not modified.
/// 
/// This instruction performs a twos-complement operation. 
/// A ones-complement can be achieved with the COM instruction.
/// 
/// Cycles (7 / 6)
/// Byte Count (3)
/// 
/// See Also: COM, NEG (accumulator)
internal class _70_Neg_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;
        byte value = M8[address];

        byte result = (byte)(0 - value);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value == 0x80;
        CC_C = value == 0;

        M8[address] = result;

        return DynamicCycles._76;
    }
}
