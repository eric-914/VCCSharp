﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>10B3/CMPD/EXTENDED</code>
/// Compare Memory Byte from 16-Bit Accumulator <c>D</c>
/// <code>TEMP ← D - (M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>CMPD</c> instruction subtracts the contents of a byte in memory from the value in the 16-bit <c>D</c> accumulator and set the Condition Codes accordingly.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the value of bit 15 of the result.
///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit 15 was needed; cleared otherwise
/// 
/// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
/// Neither the memory bytes nor the register are modified unless an auto-increment / auto-decrement addressing mode is used with the same register.
/// 
/// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
/// 
/// The 16-bit CMPD instruction performs exactly the same operation as the 16-bit SUBD instruction, with the exception that the value in the accumulator is not changed. 
/// 
/// Cycles (8 / 6)
/// Byte Count (4)
/// 
/// See Also: CMP (8-bit), CMPR
internal class _10B3_Cmpd_E : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._86;

    public void Exec()
    {
        ushort address = M16[PC]; PC += 2;
        ushort value = M16[address];

        var fn = Subtract(D, value);

        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;
    }
}
