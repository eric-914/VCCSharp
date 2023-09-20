using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>59/ROLB/INHERENT</code>
/// Rotate 8-Bit Accumulator <c>A</c> Left through Carry
/// <code>C’ ← b7 ← ... ← b0 ← C</code>
/// </summary>
/// <remarks>
/// The <c>ROLB</c> instruction rotates the contents of the <c>C</c> accumulator to the left by one bit, through the Carry bit of the <c>CC</c> register (effectively a 9-bit rotation). 
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
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ADCR, ROL (16-bit)
internal class _59_Rolb_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public int Exec()
    {
        byte bit = CC_C.ToBit();

        byte result = (byte)((B << 1) | bit);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = B.Bit7() ^ B.Bit6();
        CC_C = B.Bit7();

        B = result;

        return CycleCount;
    }
}
