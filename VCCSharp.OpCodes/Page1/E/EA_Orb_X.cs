﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>EA/ORB/INDEXED</code>
/// Logically OR Accumulator <c>B</c> with a Byte from Memory
/// <code>B’ ← B OR IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>ORB</c> instruction logically ORs the contents of Accumulator <c>B</c> with the contents of a memory byte. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The 8-bit result is then placed back in the specified accumulator.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///         
/// The OR instructions are commonly used for setting specific bits in an accumulator to '1' while leaving other bits unchanged. 
/// Consider the following examples:
///         ORA #%00010000  ;Sets bit 4 in A
///         ORB #$7F        ;Sets all bits in B except bit 7
///         
/// Cycles (4+ / 2+)
/// Byte Count (2)
/// 
/// See Also: BIOR, BOR, OIM, ORCC, ORD, ORR
internal class _EA_Orb_X : OpCode, IOpCode
{
    public int CycleCount => 4;

    public void Exec()
    {
        ushort address = INDEXED[PC++];
        byte value = M8[address];

        byte result = (byte)(B | value);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = false;

        B = result;
    }
}
