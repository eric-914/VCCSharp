using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1143/COME/INHERENT</code>
/// Complement accumulator <c>E</c>
/// <code>E’ ← ~E</code>
/// </summary>
/// <remarks>
/// The <c>COME</c> instructions change the value of the <c>E</c> accumulator to that of it’s logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1.
/// <code>🚫 6309 ONLY 🚫</code>
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
/// The COMA and COMB instructions provide the smallest, fastest way to set the Carry flag in the CC register.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: COM (memory), NEG
internal class _1143_Come_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        byte result = (byte)(0xFF - E);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;
        CC_C = true;

        E = result;
    }
}
