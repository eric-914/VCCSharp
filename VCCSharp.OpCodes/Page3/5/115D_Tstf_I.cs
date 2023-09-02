using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>115D/TSTF/INHERENT</code>
/// Test Value in Accumulator <c>F</c>
/// <code>TEMP ← F</code>
/// </summary>
/// <remarks>
/// The <c>TSTF</c> instruction tests the value in a accumulator <c>F</c> to setup the Condition Codes register with minimal status for that value. 
/// <code>🚫 6309 ONLY 🚫</code>
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
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: CMP, STQ, TST (memory)
internal class _115D_Tstf_I : OpCode6309, IOpCode
{
    public int Exec()
    {
        CC_N = F.Bit7();
        CC_Z = F == 0;
        CC_V = false;

        return DynamicCycles._32;
    }
}
