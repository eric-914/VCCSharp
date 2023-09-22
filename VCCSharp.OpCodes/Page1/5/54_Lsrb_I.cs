using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>54/LSRB/INHERENT</code>
/// Logical Shift Right of 8-Bit Accumulator <c>B</c>
/// <code>0 → b7 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>LSRB</c> instructions logically shifts the contents of the <c>B</c> accumulator to the right by one bit, clearing bit 7. 
/// </remarks>
/// 
///          ╭──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
///    0 ──▶ │  │  │  │  │  │  │  │  │ ──▶ │  │
///          ╰──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
///           b7 ────────────────▶ b0       C
///           
/// [E F H I N Z V C]
/// [        0 ↕   ↕]
///   
/// Bit 0 is shifted into the Carry flag of the Condition Codes register.
///         N The Negative flag is cleared by these instructions.
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
///        
/// The LSR instruction can be used in simple division routines on unsigned values (a single right-shift divides the value by 2).
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: LSR (16-bit)
internal class _54_Lsrb_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public void Exec()
    {
        byte result = (byte)(B >> 1);

        CC_N = false;
        CC_Z = result == 0;
        CC_C = B.Bit0();

        B = result;
    }
}
