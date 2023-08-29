using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1049/ROLW/INHERENT</code>
/// Rotate 16-Bit <c>W</c> Accumulator Left through Carry
/// <code>C’ ← b15 ← ... ← b0 ← C</code>
/// </summary>
/// <remarks>
/// The <c>RORW</c> instruction rotates the contents of the <c>W</c> accumulator to the left by one bit, through the Carry (<c>C</c>) bit of the CC register (effectively a 17-bit rotation). 
/// Bit 0 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 15.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///   ╭────────────────────────────────────────────────────────────────╮
///   │  ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮    │
///   ╰─ │  │ ◀── │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ◀──╯
///      ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯
///       C       b15 ────────────────────────────────────────▶ b0       
///       
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of bit 15.
///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
///         V The Overflow flag is set equal to the XOR of the original values of bits 14 and 15.
///         C The Carry flag receives the value shifted out of bit 15.
/// 
/// The ROL instructions can be used for subsequent words of a multi-byte shift to bring in the carry bit from a previous shift or rotate instruction. 
/// Other uses include conversion of data from serial to parallel and vise-versa.
/// 
/// A left rotate of the 32-bit Q accumulator can be achieved by executing ROLW immediately followed by ROLD.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: ROL (8-bit)
internal class _1059_Rolw_I : OpCode6309, IOpCode
{
    internal _1059_Rolw_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort result = (ushort)((W << 1) | CC_C.ToBit());;

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = W.Bit14() ^ W.Bit15();
        CC_C = W.Bit15();

        W = result;

        return Cycles._32;
    }
}
