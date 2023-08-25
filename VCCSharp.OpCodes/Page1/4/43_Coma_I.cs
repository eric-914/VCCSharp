using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>43/COMA/INHERENT</code>
/// Complement accumulator A
/// <code>A’ ← ~A</code>
/// </summary>
/// <remarks>
/// The <c>COMA</c> instructions change the value of the <c>A</c> accumulator to that of it’s logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0 1]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         C The Carry flag is always set.
/// 
/// This instruction performs a ones-complement operation. 
/// A twos-complement can be achieved with the NEG instruction.
/// 
/// Complementing the Q accumulator requires executing both COMW and COMD.
/// 
/// The COMA and COMB instructions provide the smallest, fastest way to set the Carry flag in the CC register.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: COM (memory), NEG
internal class _43_Coma_I : OpCode, IOpCode
{
    internal _43_Coma_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte result = (byte)(0xFF - A);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;
        CC_C = true;

        A = result;

        return Cycles._21;
    }
}
