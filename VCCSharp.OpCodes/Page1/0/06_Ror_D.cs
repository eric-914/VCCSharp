using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>06/ROR/DIRECT</code>
/// Rotate 8-Bit Memory Byte Right through Carry
/// <code>C → b7 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// These instructions rotate the contents of the A or B accumulator or a specified byte in memory to the right by one bit, through the Carry bit of the CC register (effectively a 9- bit rotation). 
/// </remarks>
///   
///    ╭───────────────────────────────╮
///    │  ╭──┬──┬──┬──┬──┬──┬──┬──╮   ╭──╮
///    ╰─▶│  │  │  │  │  │  │  │  │──▶│  │
///       ╰──┴──┴──┴──┴──┴──┴──┴──╯   ╰──╯
///        b7 ────────────────▶ b0     C
/// 
/// [E F H I N Z V C]
/// [        ↕ ↕   ↕]
/// 
/// These instructions rotate the contents of specified byte in memory to the right by one bit, through the Carry bit of the CC register (effectively a 9-bit rotation). 
/// Bit 7 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 0.
///         N The Negative flag is set equal to the new value of bit 7 (original value of Carry).
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
/// 
/// The ROR instructions can be used for subsequent bytes of a multi-byte shift to bring in the carry bit from previous shift or rotate instructions. 
/// Other uses include conversion of data from serial to parallel and vise-versa. 
/// 
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: ROR (16-bit)
internal class _06_Ror_D : OpCode, IOpCode
{
    internal _06_Ror_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte bit = CC_C.ToByte();

        byte result = (byte)((value >> 1) | (bit << 7));

        CC_N = result.Bit7();
        CC_Z = result != 0;
        CC_C = value.Bit0();

        M8[address] = result;

        return Cycles._65;
    }
}
