﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>E1/CMPB/EXTENDED</code>
/// Compare Memory Byte from 8-Bit Accumulator <c>B</c>
/// <code>TEMP ← B - (M)</code>
/// </summary>
/// <remarks>
/// The <c>CMPB</c> instruction subtracts the contents of a byte in memory from the value in the 8-bit <c>B</c> accumulator and set the Condition Codes accordingly.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [    ~   ↕ ↕ ↕ ↕]
///   
/// Neither the memory byte nor the accumulator are modified.
///         H The affect on the Half-Carry flag is undefined for these instructions.
///         N The Negative flag is set equal to the value of bit 7 of the result.
///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
///         C The Carry flag is set if a borrow into bit-7 was needed; cleared otherwise.
/// 
/// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
/// 
/// The 8-bit CMPB instruction performs exactly the same operation as the 8-bit SUBB instruction, with the exception that the value in the accumulator is not changed. 
/// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
/// 
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: CMP (16-bit), CMPR
internal class _F1_Cmpb_E : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._54;

    public void Exec()
    {
        ushort address = M16[PC]; PC += 2;
        byte value = M8[address];

        var fn = Subtract(B, value);

        //CC_H = undefined
        CC_N = fn.N;
        CC_Z = fn.Z;
        CC_V = fn.V;
        CC_C = fn.C;
    }
}
