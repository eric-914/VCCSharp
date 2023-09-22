using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1021/LBRN/RELATIVE</code>
/// Branch never
/// <code>PC’ ← PC + IMM</code>
/// </summary>
/// <remarks>
/// The <c>LBRN</c> instruction is essentially a no-operation; that is, the CPU never branches but merely advances the Program Counter to the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// None of the Condition Code flags are affected.
/// 
/// The LBRN instruction provides a 4-byte no-op that consumes 5 bus cycles, whereas NOP is a single-byte instruction that consumes either 1 or 2 bus cycles. 
/// In addition, there is the BRN instruction which provides a 2-byte no-op that consumes 3 bus cycles.
/// 
/// Since the branch is never taken, the third and fourth bytes of the instruction do not serve any purpose and may contain any value. 
/// These bytes could contain program code or data that is accessed by some other instruction(s).
/// 
/// Cycles (5)
/// Byte Count (4)
/// 
/// See Also: BRN, LBRA, NOP
internal class _1021_LBrn_R : OpCode, IOpCode
{
    public int CycleCount => 5;

    public void Exec()
    {
        PC += 2;
    }
}
