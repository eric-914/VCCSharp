﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1084/ANDD/IMMEDIATE</code>
/// Logically AND Memory Byte with Accumulator <c>D</c>
/// <code>D’ ← D AND (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>ANDD</c> instruction logically ANDs the contents of a byte in memory with Accumulator <c>D</c>. 
/// The 16-bit result is placed back into Accumulator <c>D</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///     N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
///     Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
///     V The Overflow flag is cleared by this instruction.
/// 
/// The ANDD instruction logically ANDs the contents of a double-byte value in memory with the contents of Accumulator D. 
/// 
/// One use for the ANDD instruction is to truncate bits of an address value. 
/// For example:
///         ANDD #$E000 ;Convert address to that of its 8K page
///     
/// For testing bits, it is often preferable to use the BITD instruction instead, since it performs the same logical AND operation without modifying the contents of Accumulator D.
/// 
/// Cycles (5 / 4)
/// Byte Count (4)
/// 
/// See Also: AND (8-bit), ANDCC, ANDR, BITD
internal class _1084_Andd_M : OpCode6309, IOpCode
{
    internal _1084_Andd_M(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort value = M16[PC += 2];

        ushort result = (ushort)(D & value);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = false;

        D = result;

        return DynamicCycles._54;
    }
}
