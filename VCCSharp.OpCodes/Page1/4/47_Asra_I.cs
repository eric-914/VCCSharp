using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>47/ASRA/INHERENT</code>
/// Arithmetic shift of accumulator A
/// <code>⤿b7 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>ASRA</c> instruction arithmetically shifts the contents of the <c>A</c> accumulator to the right by one bit. 
/// </remarks>
/// 
///    ╭───╮
///    │  ╭──┬──┬──┬──┬──┬──┬──┬──╮   ╭──╮
///    ╰─▶│  │  │  │  │  │  │  │  │──▶│  │
///       ╰──┴──┴──┴──┴──┴──┴──┴──╯   ╰──╯
///        b7 ────────────────▶ b0     C
///  
/// [E F H I N Z V C]
/// [    ~   ↕ ↕   ↕]
///   
/// The value of bit 7 is not changed.
///         H The affect on the Half-Carry flag is undefined for these instructions.
///         N The Negative flag is set equal to the value of bit 7.
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
///     
/// The ASR instruction can be used in simple division routines (a single right-shift divides the value by 2). 
/// Be careful here, as a right-shift is not the same as a division when the value is negative; it rounds in the wrong direction. 
/// For example, -5 (0xFB) divided by 2 should be -2 but, when arithmetically shifted right, is -3 (0xFD).
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// The 6309 does not provide variants of ASR to operate on the E and F accumulators.
internal class _47_Asra_I : OpCode, IOpCode
{
    internal _47_Asra_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte result = (byte)((A & 0x80) | (A >> 1));

        //CC_H = undefined;
        CC_C = A.Bit0();
        CC_Z = result == 0;
        CC_N = result.Bit7();

        A = result;

        return Cycles._21;
    }
}
