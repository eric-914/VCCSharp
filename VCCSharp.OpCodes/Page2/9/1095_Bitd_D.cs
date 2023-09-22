﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1095/BITD/DIRECT</code>
/// Bit Test Accumulator <c>D</c> with Memory Byte Value
/// <code>TEMP ← D AND (M)</code>
/// </summary>
/// <remarks>
/// The <c>BITD</c> instruction logically ANDs the contents of a word in memory with Accumulator <c>D</c>.
/// The 16-bit result is tested to set or clear the appropriate flags in the CC register. 
/// Neither Accumulator D nor the memory bytes are modified.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to bit 15 of the resulting value.
///         Z The Zero flag is set if the resulting value was zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
/// 
/// The BITD instruction differs from ANDD only in that Accumulator D is not modified.
/// 
/// Cycles (5 / 4)
/// Byte Count (4)
/// 
/// See Also: ANDD, BIT (8-bit), BITMD
internal class _1095_Bitd_D : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._75;

    public void Exec()
    {
        ushort address = DIRECT[PC++];
        ushort value = M16[address];

        ushort result = (ushort)(D & value);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = false;
    }
}
