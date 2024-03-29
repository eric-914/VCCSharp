﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>104D/TSTD/INHERENT</code>
/// Test Value in Accumulator <c>D</c>
/// <code>TEMP ← D</code>
/// </summary>
/// <remarks>
/// The <c>TSTD</c> instruction tests the value in accumulator <c>D</c> to setup the Condition Codes register with minimal status for that value. 
/// The accumulator itself is not modified by these instructions.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the value of the accumulator’s high-order bit (sign bit).
///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// For unsigned values, the only meaningful information provided is whether or not the value is zero. 
/// In this case, BEQ or BNE would typically follow such a test. 
/// 
/// For signed (twos complement) values, the information provided is sufficient to allow any of the signed conditional branches (BGE, BGT, BLE, BLT) to be used as though the accumulator’s value had been compared with zero. 
/// You can also use BMI and BPL to branch according to the sign of the value.
/// 
/// To determine the sign of a 16-bit or 32-bit value, you only need to test the high order byte. 
/// For example, TSTA is sufficient for determining the sign of a 32-bit twos complement value in accumulator Q. 
/// A full test of accumulator Q could be accomplished by storing it to a scratchpad RAM location (or ROM address). 
/// In a traditional stack environment, the instruction STQ -4,S may be acceptable.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: CMP, STQ, TST (memory)
internal class _104D_Tstd_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        CC_N = D.Bit15();
        CC_Z = D == 0;
        CC_V = false;
    }
}
