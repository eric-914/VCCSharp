using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11BF/MULD/EXTENDED</code>
/// Signed Multiply of Accumulator D and Memory Word
/// <code>Q’ ← D x IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// This instruction multiplies the signed 16-bit value in Accumulator <c>D</c> by either a 16-bit immediate value or the contents of a double-byte value from memory. 
/// The signed 32-bit product is placed into Accumulator <c>Q</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕    ]
/// 
/// Only two Condition Code flags are affected:
///         N The Negative flag is set if the twos complement result is negative; cleared otherwise.
///         Z The Zero flag is set if the 32-bit result is zero; cleared otherwise.
/// 
/// Cycles (31 / 30)
/// Byte Count (4)
/// 
/// See Also: MUL
internal class _11BF_Muld_E : OpCode6309, IOpCode
{
    internal _11BF_Muld_E(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC+=2];
        short value = (short)M16[address];

        Q = (uint)((short)D * value);

        CC_N = Q.Bit31();
        CC_Z = Q == 0;

        return DynamicCycles._3130;
    }
}
