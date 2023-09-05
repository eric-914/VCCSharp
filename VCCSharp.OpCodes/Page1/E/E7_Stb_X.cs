using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>E7/STB/INDEXED</code>
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
/// Cycles (4+ / 2+)
/// Byte Count (2)
/// 
/// See Also: ST (16-bit), STQ
internal class _E7_Stb_X : OpCode, IOpCode
{
    public int Exec()
    {
        Cycles = 4;

        ushort address = INDEXED[PC++];

        M8[address] = B;

        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;

        return Cycles;
    }
}
