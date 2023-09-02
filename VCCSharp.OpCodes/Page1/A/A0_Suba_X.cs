﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>A0/SUBA/INDEXED</code>
/// Subtract from value in 8-Bit Accumulator <c>A</c>
/// <code>A’ ← A - IMM8|(M)</code>
/// </summary>
/// <remarks>
/// The <c>SUBA</c> instruction subtracts an 8-bit immediate value from one of the 8-bit accumulator <c>A</c>.
/// The 8-bit result is placed back into the specified accumulator
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ~   ↕ ↕ ↕ ↕]
/// 
/// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
///         H The value of Half-Carry flag is undefined after executing these instructions.
///         N The Negative flag is set equal to the new value of bit 7 of the accumulator.
///         Z The Zero flag is set if the new accumulator value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit 7 was needed; cleared otherwise.
///         
/// The 8-bit SUB instructions are used for single-byte subtraction, and for subtraction of the least-significant byte in multi-byte subtractions.
/// Since the 6809 and 6309 both provide 16-bit SUB instructions for the accumulators, it is not necessary to use the 8-bit SUB and SBC instructions to perform 16-bit subtraction.
/// 
/// Cycles (4+)
/// Byte Count (2+)
/// 
/// See Also: SUB (16-bit), SUBR
internal class _A0_Suba_X : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = INDEXED[PC++];
        byte value = M8[address];

        var sum = Subtract(A, value);

        CC_H = sum.H;
        CC_N = sum.N;
        CC_Z = sum.Z;
        CC_V = sum.V;
        CC_C = sum.C;

        A = (byte)sum.Result;

        return 4;
    }
}
