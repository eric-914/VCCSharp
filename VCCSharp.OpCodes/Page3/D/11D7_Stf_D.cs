﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11D7/STF/DIRECT</code>
/// Store 8-Bit Accumulator <c>F</c> to Memory
/// <code>(M)’ ← F</code>
/// </summary>
/// <remarks>
/// The <c>STF</c> instruction stores the contents of the 8-bit <c>F</c> accumulator to a byte in memory.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the value of bit 7 of the accumulator.
///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5 / 4)
/// Byte Count (3)
///         
/// See Also: ST (16-bit), STQ
internal class _11D7_Stf_D : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];

        M8[address] = E;

        CC_N = E.Bit7();
        CC_Z = E == 0;
        CC_V = false;

        return DynamicCycles._54;
    }
}
