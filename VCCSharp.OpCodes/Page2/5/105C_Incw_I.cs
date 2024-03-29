﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>105C/INCW/INHERENT</code>
/// Increment Accumulator <c>W</c>
/// <code>W’ ← W + 1</code>
/// </summary>
/// <remarks>
/// The <c>INCW</c> instruction adds 1 to the contents of the <c>W</c> accumulator. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕  ]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
///         V The Overflow flag is set if the original value was $7FFF (16-bit); cleared otherwise.
///         
/// It is important to note that the INC instructions do not affect the Carry flag. 
/// This means that it is not always possible to optimize code by simply replacing an ADDr #1 instruction with a corresponding INCr.
/// 
/// When used to increment an unsigned value, only the BEQ and BNE branches will consistently behave as expected. 
/// When operating on a signed value, all of the signed conditional branch instructions will behave as expected.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: ADD, DEC, INC (memory)
internal class _105C_Incw_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        ushort result = (ushort)(W + 1);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = W == 0x7FFF;

        W = result;
    }
}
