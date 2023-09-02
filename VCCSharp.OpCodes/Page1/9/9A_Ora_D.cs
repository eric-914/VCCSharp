﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>9A/ORA/DIRECT</code>
/// Logically OR Accumulator <c>A</c> with a Byte from Memory
/// <code>A’ ← A OR IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>ORA</c> instruction logically ORs the contents of Accumulator A with the contents of a memory byte. 
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
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: BIOR, BOR, OIM, ORCC, ORD, ORR
internal class _9A_Ora_D : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = DIRECT[PC++];

        A |= M8[address];

        CC_N = A.Bit7();
        CC_Z = A == 0;
        CC_V = false;

        return DynamicCycles._43;
    }
}
