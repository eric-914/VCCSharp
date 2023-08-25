using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>49/ROLA/INHERENT</code>
/// Rotate 8-Bit Accumulator A Left through Carry
/// </summary>
/// <remarks>
/// The <c>ROLA</c> instruction rotates the contents of the <c>A</c> accumulator to the left by one bit, through the Carry bit of the <c>CC</c> register (effectively a 9-bit rotation). 
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
/// The 6309 does not provide variants of ROL to operate on the E and F accumulators.
/// However, you can achieve the same functionality using the ADCR instruction. 
/// The instructions ADCR E,E and ADCR F,F will perform a left-rotate operation on the E and F accumulators respectively.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ADCR, ROL (16-bit)
internal class _49_Rola_I : OpCode, IOpCode
{
    internal _49_Rola_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte bit = CC_C.ToByte();

        byte result = (byte)((A << 1) | bit);

        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = A.Bit7() ^ A.Bit6();
        CC_C = A.Bit7();

        A = result;

        return Cycles._21;
    }
}
