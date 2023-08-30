﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>8B/ADDA/IMMEDIATE</code>
/// Add Memory Byte to 8-Bit Accumulator <c>A</c>
/// <code>A’ ← A + (M)</code>
/// </summary>
/// <remarks>
/// The <c>ADDA</c> instruction adds the contents of a byte in memory with the 8-bit <c>A</c> accumulator.
/// The 8-bit result is placed back into the <c>A</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ↕   ↕ ↕ ↕ ↕]
/// 
///         H The Half-Carry flag is set if a carry into bit 4 occurred; cleared otherwise.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a carry out of bit 7 occurred; cleared otherwise.
///     
/// The 8-bit ADD instructions are used for single-byte addition, and for addition of the least-significant byte in multi-byte additions. 
/// Since the 6x09 also provides a 16-bit ADD instruction, it is not necessary to use the 8-bit ADD and ADC instructions for performing 16-bit addition.
/// 
/// Cycles (2)
/// Byte Count (2)
/// 
/// See Also: ADD (16-bit), ADDR
internal class _8B_Adda_M : OpCode, IOpCode
{
    internal _8B_Adda_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];

        var sum = Sum(A, value);

        CC_H = sum.H;
        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        A = (byte)sum.Result;

        return 2;
    }
}