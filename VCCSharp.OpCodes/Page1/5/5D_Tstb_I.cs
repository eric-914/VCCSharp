using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>5D/TSTB/INHERENT</code>
/// Test Value in Accumulator <c>B</c>
/// <code>TEMP ← B</code>
/// </summary>
/// <remarks>
/// The <c>TSTB</c> instruction tests the value in a accumulator <c>B</c> to setup the Condition Codes register with minimal status for that value. 
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
internal class _5D_Tstb_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public void Exec()
    {
        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;
    }
}
