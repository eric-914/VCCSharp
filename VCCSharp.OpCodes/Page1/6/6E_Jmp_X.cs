﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>6E/JMP/INDEXED</code>
/// Unconditional Jump
/// <code>PC’ ← EA</code>
/// </summary>
/// <remarks>
/// This instruction causes an unconditional jump. None of the Condition Code flags are affected by this instruction.
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// The JMP instruction is similar in function to the BRA and LBRA instructions in that it always causes execution to be transferred to the effective address specified by the operand. 
/// The primary difference is that BRA and LBRA use only the Relative Addressing mode, whereas JMP uses only the Direct, Indexed or Extended modes.
/// 
/// Unlike most other instructions which use the Direct, Indexed and Extended addressing modes, the operand value used by the JMP instruction is the Effective Address itself, rather than the memory contents stored at that address (unless Indirect Indexing is used).
/// 
/// Here are some examples:
///         JMP $4000   ; Jumps to address $4000
///         JMP [$4000] ; Jumps to address stored at $4000
///         JMP ,X      ; Jumps to the address in X
///         JMP B,X     ; Jumps to computed address X + B
///         JMP [B,X]   ; Jumps to address stored at X + B
///         JMP ＜$80   ; Jumps to address (DP * $100) + $80
///         
/// Indexed operands are useful in that they provide the ability to compute the destination address at run-time. 
/// The use of an Indirect Indexing mode is frequently used to call routines through a jump-table in memory.
/// 
/// Using Direct or Extended operands with the JMP instruction should be avoided in position-independent code unless the destination address is within non-relocatable code (such as a ROM routine).
/// 
/// Cycles (3+)
/// Byte Count (2+)
/// 
/// See Also: BRA, JSR, LBRA
internal class _6E_Jmp_X : OpCode, IOpCode
{
    internal _6E_Jmp_X(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = INDEXED[PC];

        PC = address;

        return 3;
    }
}