using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1047/ASRD/INHERENT</code>
/// Arithmetic Shift Right of Accumulator <c>D</c>
/// <code>⤿b15 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>ASRD</c> instruction shifts the contents of Accumulator <c>D</c> to the right by one bit. 
/// Bit 0 is shifted into the Carry (<c>C</c>) flag of the Condition Codes register. 
/// The value of bit 15 is not changed.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///    ╭───╮
///    │  ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
///    ╰─▶│  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ──▶ │  │
///       ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
///        b15 ───────────────────────────────────────▶ b0       C
///        
/// [E F H I N Z V C]
/// [        ↕ ↕   ↕]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the value of bit 15.
///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
///     
/// The ASRD instruction can be used in simple division routines (a single right-shift divides the value by 2). 
/// Be careful here, as a right-shift is not the same as a division when the value is negative; it rounds in the wrong direction. 
/// For example, -5 (FFFB16) divided by 2 should be -2 but, when arithmetically shifted right, is -3 (FFFD16).
/// 
/// The 6309 does not provide a variant of ASR to operate on the W accumulator, although it does provide the LSRW instruction for performing a logical shift.
/// An arithmetic right-shift of the 32-bit Q accumulator can be achieved as follows:
///     ASRD ; Shift Hi-word, Low-bit into Carry
///     RORW ; Shift Low-word, Carry into Hi-bit
///     
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: ASR (8-bit), ROR (16-bit)
internal class _1047_Asrd_I : OpCode6309, IOpCode
{
    internal _1047_Asrd_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort result = (ushort)((D & 0x8000) | (D >> 1)); ;

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_C = D.Bit0();

        D = result;

        return DynamicCycles._32;
    }
}
