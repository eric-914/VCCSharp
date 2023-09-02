using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>39/RTS/INHERENT</code>
/// Return from Subroutine
/// <code>
/// PC’ ← (S:S+1)
///  S’ ← S + 2
/// </code>
/// </summary>
/// <remarks>
/// This instruction pulls the double-byte value pointed to by the hardware stack pointer <c>(S)</c> and places it into the <c>PC</c> register. 
/// </remarks>
/// 
/// [E F H I N Z V C]
/// [               ]
/// 
/// This instruction pulls the double-byte value pointed to by the hardware stack pointer (S) and places it into the PC register. 
/// No condition code flags are affected. 
/// The effective result is the same as would be achieved using a PULS PC instruction.
/// 
/// RTS is typically used to exit from a subroutine that was called via a BSR or JSR instruction. 
/// Note, however, that a subroutine which preserves registers on entry by pushing them onto the stack, may opt to use a single PULS instruction to both restore the registers and return to the caller, as in:
///     ENTRY   PSHS A,B,X      ; Preserve registers
///             ...
///             ...
///             PULS A,B,X,PC   ; Restore registers and return
///         
/// Cycles (5 / 4)
/// Byte Count (1)
/// 
/// See Also: BSR, JSR, PULS, RTI
internal class _39_Rts_I : OpCode, IOpCode
{
    public int Exec()
    {
        PC_H = M8[S++];
        PC_L = M8[S++];

        return DynamicCycles._54;
    }
}
