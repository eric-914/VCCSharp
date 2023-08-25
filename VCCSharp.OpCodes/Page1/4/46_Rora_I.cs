using System.Net;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>46/RORA/INHERENT</code>
/// Rotate 8-Bit Accumulator <c>A</c> Right through Carry
/// <code>C → b7 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>RORA</c> instruction rotates the contents of the <c>A</c> accumulator to the right by one bit, through the Carry bit of the <c>CC</c> register (effectively a 9-bit rotation). 
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
/// Bit 7 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 0.
///         N The Negative flag is set equal to the new value of bit 7 (original value of Carry).
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
/// 
/// The ROR instructions can be used for subsequent bytes of a multi-byte shift to bring in the carry bit from previous shift or rotate instructions. 
/// Other uses include conversion of data from serial to parallel and vise-versa. 
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ROR (16-bit)
internal class _46_Rora_I : OpCode, IOpCode
{
    internal _46_Rora_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte bit = CC_C.ToByte();

        byte result = (byte)((A >> 1) | (bit << 7));

        CC_N = result.Bit7();
        CC_Z = !(result == 0);
        CC_C = A.Bit0();

        A = result;

        return Cycles._21;
    }
}
