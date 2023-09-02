using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>0C/INC/DIRECT</code>
/// Increment a byte in Memory
/// <code>(M)’ ← (M) + 1</code>
/// </summary>
/// <remarks>
/// The <c>INC</c> instruction adds 1 to the contents of a memory byte. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of bit 7.
///         Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
///         V The Overflow flag is set if the original value of the memory byte was $7F; cleared otherwise.
///         
/// Because the INC instruction does not affect the Carry flag, it can be used to implement a loop counter within a multiple precision computation.
/// 
/// When used to increment an unsigned value, only the BEQ and BNE branches will consistently behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: ADD, DEC, INC (accumulator)
internal class _0C_Inc_D : OpCode, IOpCode
{
    internal _0C_Inc_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte result = (byte)(value + 1);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value == 0x7F;

        M8[address] = result;

        return DynamicCycles._65;
    }
}
