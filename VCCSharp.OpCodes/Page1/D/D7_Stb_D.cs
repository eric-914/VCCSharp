using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>D7/STB/DIRECT</code>
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
/// Cycles (4 / 3)
/// Byte Count (2)
///         
/// See Also: ST (16-bit), STQ
internal class _D7_Stb_D : OpCode, IOpCode
{
    internal _D7_Stb_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];

        M8[address]=B;

        CC_N = B.Bit7();
        CC_Z = B == 0;
        CC_V = false;

        return Cycles._43;
    }
}
