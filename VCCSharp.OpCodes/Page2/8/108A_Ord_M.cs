using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>108A/ORD/IMMEDIATE</code>
/// Logically OR Accumulator <c>D</c> with a Word from Memory
/// <code>D’ ← D OR IMM8|(M:M+1)</code>
/// </summary>
/// <remarks>
/// The <c>ORD</c> instruction logically ORs the contents of Accumulator <c>D</c> with the contents of a memory word.
/// The 16-bit result is placed back into Accumulator <c>D</c>.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕ 0  ]
///   
/// The Condition Codes are affected as follows.
///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
///         Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
///         V The Overflow flag is cleared by this instruction.
///         
/// The ORD instruction is commonly used for setting specific bits in the accumulator to '1' while leaving other bits unchanged.
/// 
/// When using an immediate operand, it is possible to optimize code by determining if the value will only affect half of the accumulator. 
/// For example: 
///         ORD #$1E00
///         
/// could be replaced with:
///         ORA #$1E
///         
/// To ensure that the Negative (N) condition code is set correctly, this optimization must not be made if it would result in an ORB instruction that sets bit 7.
/// 
/// Cycles (5 / 4)
/// Byte Count (4)
/// 
/// See Also: BIOR, BOR, OIM, OR (8-bit), ORCC, ORR
internal class _108A_Ord_M : OpCode6309, IOpCode
{
    internal _108A_Ord_M(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort value = M16[PC += 2];

        ushort result = (ushort)(D | value);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = false;

        D = result;

        return DynamicCycles._54;
    }
}
