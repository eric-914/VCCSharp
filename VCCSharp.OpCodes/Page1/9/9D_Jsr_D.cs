﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>9D/JSR/DIRECT</code>
/// Jump to subroutine
/// <code>
///      S’ ← S - 2
/// (S:S+1) ← PC
///     PC’ ← EA
/// </code>
/// </summary>
/// <remarks>
/// The <c>JSR</c> instruction pushes the value of the <c>PC</c> register (after the <c>JSR</c> instruction bytes have been fetched) onto the hardware stack and then performs an unconditional jump.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
///   
/// None of the Condition Code flags are affected.
/// 
/// By pushing the PC value onto the stack, the called subroutine can "return" to this address after it has completed.
/// 
/// The JSR instruction is similar in function to that of the BSR and LBSR instructions. 
/// The primary difference is that BSR and LBSR use only the Relative Addressing mode, whereas JSR uses only the Direct, Indexed or Extended modes.
/// 
/// Unlike most other instructions which use the Direct, Indexed and Extended addressing modes, the operand value used by the JSR instruction is the Effective Address itself, rather than the memory contents stored at that address (unless Indirect Indexing is used).
/// 
/// Here are some examples:
///         JSR $4000   ; Calls to address $4000
///         JSR [$4000] ; Calls to the address stored at $4000
///         JSR ,X      ; Calls to the address in X
///         JSR [B,X]   ; Calls to the address stored at X + B
///         
/// Indexed operands are useful in that they provide the ability to compute the subroutine address at run-time. 
/// The use of an Indirect Indexing mode is frequently used to call subroutines through a jump-table in memory.
/// Using Direct or Extended operands with the JSR instruction should be avoided in position-independent code unless the destination address is within non-relocatable code (such as a ROM routine).
/// 
/// Cycles (7 / 6)
/// Byte Count (2)
/// 
/// See Also: BSR, JMP, LBSR, PULS, RTS
internal class _9D_Jsr_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._76;

    public void Exec()
    {
        ushort address = DIRECT[PC++];

        Push(PC);

        PC = address;
    }
}
