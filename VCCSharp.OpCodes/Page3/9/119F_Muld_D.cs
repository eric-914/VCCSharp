﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>119F/MULD/DIRECT</code>
/// Signed Multiply of Accumulator D and Memory Word
/// <code>Q’ ← D x IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// This instruction multiplies the signed 16-bit value in Accumulator <c>D</c> by either a 16-bit immediate value or the contents of a double-byte value from memory. 
/// The signed 32-bit product is placed into Accumulator <c>Q</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕    ]
/// 
/// Only two Condition Code flags are affected:
///         N The Negative flag is set if the twos complement result is negative; cleared otherwise.
///         Z The Zero flag is set if the 32-bit result is zero; cleared otherwise.
/// 
/// Cycles (30 / 29)
/// Byte Count (3)
/// 
/// See Also: MUL
internal class _119F_Muld_D : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._3029;

    public void Exec()
    {
        ushort address = DIRECT[PC++];
        ushort value = M16[address];

        var fn = Multiply(D, value);

        CC_N = fn.N;
        CC_Z = fn.Z;

        Q = (uint)fn.Result;
    }
}
