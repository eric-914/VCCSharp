using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>B7/STA/EXTENDED</code>
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
/// Cycles (5 / 4)
/// Byte Count (3)
/// 
/// See Also: ST (16-bit), STQ
internal class _B7_Sta_E : OpCode, IOpCode
{
    internal _B7_Sta_E(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC+=2];

        M8[address] = A;

        CC_Z = A == 0;
        CC_N = A.Bit7();
        CC_V = false;

        return Cycles._54;
    }
}
