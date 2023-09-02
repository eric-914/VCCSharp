using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>3D/MUL/INHERENT</code>
/// Unsigned Multiply of Accumulator <c>A</c> and Accumulator <c>B</c>
/// <code>D’ ← A × B</code>
/// </summary>
/// <remarks>
/// This instruction multiplies the unsigned 8-bit value in Accumulator <c>A</c> by the unsigned 8-bit value in Accumulator <c>B</c>.
/// The 16-bit unsigned product is placed into Accumulator <c>D</c>.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [          ↕   ↕]
/// 
/// Only two Condition Code flags are affected:
///         Z The Zero flag is set if the 16-bit result is zero; cleared otherwise.
///         C The Carry flag is set equal to the new value of bit 7 in Accumulator B.
///         
/// The Carry flag is set equal to bit 7 of the least-significant byte so that rounding of the most-significant byte can be accomplished by executing:
///         ADCA #0
///         
/// Cycles (11 / 10)
/// Byte Count (1)
/// 
/// See Also: ADCA, MULD
internal class _3D_Mul_I : OpCode, IOpCode
{
    internal _3D_Mul_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        D = (ushort)(A * B);

        CC_Z = D == 0;
        CC_C = B.Bit7();

        return DynamicCycles._1110;
    }
}
