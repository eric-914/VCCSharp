﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>90/SUBA/DIRECT</code>
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
/// Cycles (4 / 3)
/// Byte Count (2)
/// 
/// See Also: SUB (16-bit), SUBR
internal class _90_Suba_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._43;

    public void Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        var fn = Subtract(A, value);

        //CC_H = undefined
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;

        A = (byte)fn.Result;
    }
}
