﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>115F/CLRF/INHERENT</code>
/// Clear Accumulator <c>F</c>
/// <code>F’ ← 0</code>
/// </summary>
/// <remarks>
/// The <c>CLRF</c> instruction clears (sets to zero) the <c>F</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        0 1 0 0]
/// 
/// The Condition Code flags are also modified as follows:
///         N The Negative flag is cleared.
///         Z The Zero flag is set.
///         V The Overflow flag is cleared.
///         C The Carry flag is cleared.
/// 
/// The CLRA and CLRB instructions provide the smallest, fastest way to clear the Carry flag in the CC register.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: CLR (memory), LD
internal class _115F_Clrf_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        F = 0;

        CC_N = false;
        CC_Z = true;
        CC_V = false;
        CC_C = false;
    }
}
