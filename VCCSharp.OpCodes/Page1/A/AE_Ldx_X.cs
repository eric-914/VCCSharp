﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>AE/LDX/INDEXED</code>
/// Load Data into 16-Bit Register <c>X</c>
/// <code>X’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDX</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>X</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of the register.
///         Z The Zero flag is set if the new register value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5+)
/// Byte Count (2+)
/// 
/// See Also: LD (8-bit), LDQ, LEA
internal class _AE_Ldx_X : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        ushort address = INDEXED[PC++];

        X = M16[address];

        CC_N = X.Bit15();
        CC_Z = X == 0;
        CC_V = false;
    }
}
