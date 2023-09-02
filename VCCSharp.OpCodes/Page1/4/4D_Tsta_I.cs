using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>4D/TSTA/INHERENT</code>
/// Test Value in Accumulator <c>A</c>
/// <code>TEMP ← A</code>
/// </summary>
/// <remarks>
/// The <c>TSTA</c> instruction tests the value in a accumulator <c>A</c> to setup the Condition Codes register with minimal status for that value. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The accumulator itself is not modified by these instructions.
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
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: CMP, STQ, TST (memory)
internal class _4D_Tsta_I : OpCode, IOpCode
{
    public int Exec()
    {
        CC_N = A.Bit7();
        CC_Z = A == 0;
        CC_V = false;

        return DynamicCycles._21;
    }
}
