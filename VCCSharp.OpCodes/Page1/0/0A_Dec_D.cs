using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>0A/DEC/DIRECT</code>
/// Decrement a byte in Memory
/// <code>(M)’ ← (M) - 1</code>
/// </summary>
/// <remarks>
/// The <c>DEC</c> instruction subtracts 1 from the value contained in a memory byte. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of bit 7.
///         Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
///         V The Overflow flag is set if the original value of the memory byte was $80; cleared otherwise.
///         
/// Because the DEC instruction does not affect the Carry flag, it can be used to implement a loop counter within a multiple precision computation.
/// 
/// When used to decrement an unsigned value, only the BEQ and BNE branches will always behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: DEC (accumulator), INC, SUB
internal class _0A_Dec_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte result = (byte)(value - 1);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value == 0x80;

        M8[address] = result;

        return DynamicCycles._65;
    }
}
