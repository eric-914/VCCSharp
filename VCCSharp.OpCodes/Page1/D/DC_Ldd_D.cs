using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>DC/LDD/DIRECT</code>
/// Load Data into 16-Bit Register <c>D</c>
/// <code>D’ ← IMM16|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>LDD</c> instruction loads the contents from a pair of memory bytes (in big-endian order) into the 16-bit <c>D</c> accumulator.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of the register.
///         Z The Zero flag is set if the new register value is zero; cleared otherwise.
///         V The Overflow flag is always cleared.
///         
/// Cycles (5 / 4)
/// Byte Count (2)
///         
/// See Also: LD (8-bit), LDQ, LEA
internal class _DC_Ldd_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._54;

    public void Exec()
    {
        ushort address = DIRECT[PC++];

        D = M16[address];

        CC_N = D.Bit15();
        CC_Z = D == 0;
        CC_V = false;
    }
}
