using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1044/LSRD/INHERENT</code>
/// Logical Shift Right of 16-Bit Accumulator <c>D</c>
/// <code>0 → b15 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>LSRD</c> instruction shifts the contents of Accumulator <c>D</c> to the right by one bit. 
/// Bit 0 is shifted into the Carry (<c>C</c>) flag of the Condition Codes register. 
/// The value of bit 15 is not changed.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///          ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
///    0 ──▶ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ──▶ │  │
///          ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
///           b15 ───────────────────────────────────────▶ b0       C
///           
/// [E F H I N Z V C]
/// [        0 ↕   ↕]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is cleared by these instructions.
///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
///         
/// These instructions can be used in simple division routines on unsigned values (a single right-shift divides the value by 2).
/// 
/// A logical right-shift of the 32-bit Q accumulator can be achieved as follows:
///         LSRD ; Shift Hi-word, Low-bit into Carry
///         RORW ; Shift Low-word, Carry into Hi-bit
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: LSR (8-bit), ROR (16-bit)
internal class _1044_Lsrd_I : OpCode6309, IOpCode
{
    public int Exec()
    {
        ushort result = (ushort)(D >> 1);

        CC_N = false;
        CC_Z = result == 0;
        CC_C = D.Bit0();

        D = result;

        return DynamicCycles._32;
    }
}
