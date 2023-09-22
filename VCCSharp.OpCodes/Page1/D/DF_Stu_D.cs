using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>DF/STU/DIRECT</code>
/// Store 16-Bit Register <c>U</c> to Memory
/// <code>(M:M+1)’ ← U</code>
/// </summary>
/// <remarks>
/// The <c>STU</c> instruction stores the contents of the 16-bit <c>U</c> accumulator to a pair of memory bytes in big-endian order.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
/// 
/// The Condition Codes are affected as follows:
///         N The Negative flag is set equal to the value in bit 15 of the register.
///         Z The Zero flag is set if the register value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5 / 4)
/// Byte Count (2)
///         
/// See Also: ST (8-bit), STQ
internal class _DF_Stu_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._54;

    public void Exec()
    {
        ushort address = DIRECT[PC++];

        M16[address] = U;

        CC_N = U.Bit15();
        CC_Z = U == 0;
        CC_V = false;
    }
}
