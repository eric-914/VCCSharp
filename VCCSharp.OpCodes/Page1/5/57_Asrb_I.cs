using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>57/ASR/INHERENT</code>
/// Arithmetic shift of accumulator <c>B</c>
/// <code>⤿b7 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>ASRB</c> instruction arithmetically shifts the contents of the <c>B</c> accumulator to the right by one bit. 
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
internal class _57_Asrb_I : OpCode, IOpCode
{
    public int Exec()
    {
        byte result = (byte)((B & 0x80) | (B >> 1));

        //CC_H = undefined;
        CC_C = B.Bit0();
        CC_Z = result == 0;
        CC_N = result.Bit7();

        B = result;

        return DynamicCycles._21;
    }
}
