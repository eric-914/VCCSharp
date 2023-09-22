using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1046/RORD/INHERENT</code>
/// Rotate 16-Bit Accumulator <c>D</c> Right through Carry
/// <code>C → b15 → ... → b0 → C’</code>
/// </summary>
/// <remarks>
/// The <c>RORD</c> instruction rotates the contents of the <c>D</c> accumulator to the right by one bit, through the Carry (<c>C</c>) bit of the CC register (effectively a 17-bit rotation). 
/// Bit 15 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 0.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///     ╭───────────────────────────────────────────────────────────────╮
///     │   ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮  │
///     ╰─▶ │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ──▶ │  │ -╯
///         ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
///         b15 ──────────────────────────────────────-─▶ b0        C
///         
/// [E F H I N Z V C]
/// [        ↕ ↕   ↕]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of bit 15 (original value of Carry).
///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
///         C The Carry flag receives the value shifted out of bit 0.
/// 
/// The ROR instructions can be used for subsequent words of a multi-byte shift to bring in the carry bit from a previous shift or rotate instruction. 
/// Other uses include conversion of data from serial to parallel and vise-versa.
/// 
/// A right rotate of the 32-bit Q accumulator can be achieved by executing RORD immediately followed by RORW.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: ROR (8-bit)
internal class _1046_Rord_I : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._32;

    public void Exec()
    {
        var result = (ushort)((D >> 1) | (CC_C.ToBit() << 15));

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_C = D.Bit0();

        D = result;
    }
}
