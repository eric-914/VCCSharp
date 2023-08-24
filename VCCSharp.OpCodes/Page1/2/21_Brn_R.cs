using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>21/BRN/RELATIVE</code>
/// Branch never
/// </summary>
/// <remarks>
/// This instruction is essentially a no-operation; that is, the CPU never branches but merely advances to the next instruction in sequence. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// No Condition Code flags are affected. 
/// 
/// BRN is effectively the equivalent of BRA *+2
/// 
/// The BRN instruction provides a 2-byte no-op that consumes 3 bus cycles, whereas NOP is a single-byte instruction that consumes either 1 or 2 bus cycles. 
/// In addition, there is the LBRN instruction which provides a 4-byte no-op that consumes 5 bus cycles.
/// 
/// Since the branch is never taken, the second byte of the instruction does not serve any purpose and may contain any value. 
/// This permits an optimization technique in which a BRN opcode can be used to skip over some other single byte instruction. 
/// In this technique, the second byte of the BRN instruction contains the opcode of the instruction which is to be skipped. 
/// The two code examples shown below both perform identically.
/// The difference is that Example 2 uses a BRN opcode to reduce the code size by one byte.
/// 
/// Example 1 - conventional:
///         CMPA #$40
///         BLO @1
///         SUBA #$20
///         BRA @2 ; SKIP NEXT INSTRUCTION
///         @1 CLRA
///         @2 STA RESULT
/// Example 2 - use BRN opcode ($21) to reduce code size:
///         CMPA #$40
///         BLO @1
///         SUBA #$20
///         FCB $21 ; SKIP NEXT INSTRUCTION
///         @1 CLRA
///         STA RESULT
///         
/// Cycles (3)
/// Byte Count (2)
/// 
/// See Also: BRA, NOP, LBRN
internal class _21_Brn_R : OpCode, IOpCode
{
    internal _21_Brn_R(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        PC++;

        return 3;
    }
}
