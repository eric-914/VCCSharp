using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>F7/STB/EXTENDED</code>
/// Store 8-Bit Accumulator <c>B</c> to Memory
/// <code>(M)’ ← B</code>
/// </summary>
/// <remarks>
/// The <c>STB</c> instructions store the contents of the 8-bit <c>B</c> accumulators into a byte in memory. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the value of bit 7 of the accumulator.
///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: ST (16-bit), STQ
internal class _F7_Stb_E : OpCode, IOpCode
{
    public int Exec()
    {
        ushort address = M16[PC]; PC += 2;

        M8[address] = B;

        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;

        return DynamicCycles._54;
    }
}
