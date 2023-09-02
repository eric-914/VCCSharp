﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>D6/LDB/DIRECT</code>
/// Load Data into 8-Bit Accumulator <c>B</c>
/// <code>B’ ← IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>LDB</c> instruction loads the contents of a memory byte into the 8-bit <c>B</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: LD (16-bit), LDQ
internal class _D6_Ldb_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];

        B = M8[address];

        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;

        return DynamicCycles._43;
    }
}
