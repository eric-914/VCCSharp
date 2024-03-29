﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1043/COMW/INHERENT</code>
/// Complement Accumulator <c>W</c>
/// <code>W’ ← ~W</code>
/// </summary>
/// <remarks>
/// The <c>COMW</c> instruction changes the value of the <c>W</c> accumulator to that of its logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1.
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
/// Complementing the Q accumulator requires executing both COMW and COMD.
/// 
/// The COMA and COMB instructions provide the smallest, fastest way to set the Carry flag in the CC register.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: COM (memory), NEG
internal class _1053_Comw_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        var result = (ushort)(0xFFFF - W);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = false;
        CC_C = true;

        W = result;
    }
}
