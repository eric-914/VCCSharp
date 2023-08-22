using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>07/ASR/DIRECT</code>
/// Arithmetic shift of accumulator or memory right 
/// <code>⤿b7 → ... → b0 → C</code>
/// </summary>
/// <remarks>
/// The <c>ASR</c> instruction arithmetically shift the contents of the A or B accumulator or a specified byte in memory to the right by one bit. 
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
/// The ASR instruction arithmetically shift the contents of the A or B accumulator or a specified byte in memory to the right by one bit. 
/// Bit 0 is shifted into the Carry flag of the Condition Codes register. 
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
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: ASRD
internal class _07_Asr_D : OpCode, IOpCode
{
    internal _07_Asr_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte result = (byte)((value & 0x80) | (value >> 1));

        //CC_H = undefined;
        CC_C = value.Bit0();
        CC_Z = result == 0;
        CC_N = result.Bit7();

        M8[address] = result;

        return Cycles._65;
    }
}