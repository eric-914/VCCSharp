﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>11A7/STE/EXTENDED</code>
/// Store 8-Bit Accumulator <c>E</c> to Memory
/// <code>(M)’ ← E</code>
/// </summary>
/// <remarks>
/// The <c>STE</c> instruction stores the contents of the 8-bit <c>E</c> accumulator to a byte in memory.
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
/// Cycles (6 / 5)
/// Byte Count (4)
///         
/// See Also: ST (16-bit), STQ
internal class _11B7_Ste_E : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._65;

    public void Exec()
    {
        ushort address = M16[PC]; PC += 2;

        M8[address] = E;

        CC_N = E.Bit7();
        CC_Z = E == 0;
        CC_V = false;
    }
}
