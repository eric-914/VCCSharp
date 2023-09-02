using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>09/ROL/DIRECT</code>
/// Rotate 8-Bit Memory byte Left through Carry
/// <code>C’ ← b7 ← ... ← b0 ← C</code>
/// </summary>
/// <remarks>
/// The <c>ROL</c> instruction rotates the contents of the specified byte in memory to the left by one bit, through the Carry bit of the CC register (effectively a 9-bit rotation). 
/// </remarks>
/// 
///      ╭───────────────────────────────────╮
///    ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──╮    │
///    │  │ ◀── │  │  │  │  │  │  │  │  │ ◀──╯
///    ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──╯
///     C        b7 ────────────────▶ b0       
///     
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
/// 
/// Bit 0 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 7.
///         N The Negative flag is set equal to the new value of bit 7.
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         V The Overflow flag is set equal to the XOR of the original values of bits 6 and 7.
///         C The Carry flag receives the value shifted out of bit 7.
///         
/// The ROL instructions can be used for subsequent bytes of a multi-byte shift to bring in the carry bit from previous shift or rotate instructions. 
/// Other uses include conversion of data from serial to parallel and vise-versa.
/// 
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: ADCR, ROL (16-bit)
internal class _09_Rol_D : OpCode, IOpCode
{
    internal _09_Rol_D(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte bit = CC_C.ToBit();

        byte result = (byte)((value << 1) | bit);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value.Bit7() ^ value.Bit6();
        CC_C = value.Bit7();

        M8[address] = result;

        return DynamicCycles._65;
    }
}
