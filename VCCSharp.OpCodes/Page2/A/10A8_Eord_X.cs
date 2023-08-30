using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10A8/EORD/INDEXED</code>
/// Exclusive-OR (XOR) Memory Word with Accumulator <c>D</c>
/// <code>D’ ← D ⨁ (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>EORD</c> instruction XORs the contents of a byte in memory with Accumulator <c>D</c>.
/// The 16-bit result is placed back into Accumulator <c>D</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
///         Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///         
/// The EORD instruction produces a result containing '1' bits in the positions where the corresponding bits in the two operands have different values. 
/// Exclusive-OR logic is often used in parity functions.
/// 
/// EOR can also be used to perform "bit-flipping" since a '1' bit in the source operand will invert the value of the corresponding bit in the destination operand. 
/// For example:
///         EORD #$8080 ;Invert values of bits 7 and 15 in D
///         
/// Cycles (7+ / 6+)
/// Byte Count (3+)
///         
/// See Also: EOR (8-bit), EORR
internal class _10A8_Eord_X : OpCode6309, IOpCode
{
    internal _10A8_Eord_X(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = INDEXED[PC++];
        ushort value = M16[address];

        ushort result = (ushort)(D ^ value);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = false;

        D = result;

        return Cycles._76;
    }
}
