using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>A7/STA/INDEXED</code>
/// Store 8-Bit Accumulator <c>A</c> to Memory
/// <code>(M)’ ← A</code>
/// </summary>
/// <remarks>
/// The <c>STA</c> instruction stores the contents of the 8-bit <c>A</c> accumulator to a byte in memory.
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
/// Cycles (4+)
/// Byte Count (2+)
/// 
/// See Also: ST (16-bit), STQ
internal class _A7_Sta_X : OpCode, IOpCode
{
    public int CycleCount => 4;

    public int Exec()
    {
        Cycles = CycleCount;

        ushort address = INDEXED[PC++];

        M8[address] = A;

        CC_Z = A == 0;
        CC_N = A.Bit7();
        CC_V = false;

        return Cycles;
    }
}
