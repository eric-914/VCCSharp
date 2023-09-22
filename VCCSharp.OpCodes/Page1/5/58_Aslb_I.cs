using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>58/ASLB/INHERENT</code>
/// Arithmetic Shift Left of 8-Bit Accumulator <c>B</c>
/// <code>58/LSLB/INHERENT</code>
/// Logical Shift Left of 8-Bit Accumulator <c>B</c>
/// <code>C ← b7 ← ... ← b0 ← 0</code>
/// </summary>
/// <remarks>
/// The <c>ASLB/LSLB</c> instruction shifts the contents of the <c>B</c> accumulator to the left by one bit, clearing bit 0. 
/// </remarks>    
/// 
///    ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──╮     
///    │  │ ◀── │  │  │  │  │  │  │  │  │ ◀── 0
///    ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──╯     
///     C        b7 ◀──────────────── b0      
/// 
/// [E F H I N Z V C]
/// [    ~   ↕ ↕ ↕ ↕]
/// 
/// Bit 7 is shifted into the Carry flag of the Condition Codes register.
///         H The affect on the Half-Carry flag is undefined for these instructions.
///         N The Negative flag is set equal to the new value of bit 7; previously bit 6.
///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
///         V The Overflow flag is set to the XOR of the original values of bits 6 and 7.
///         C The Carry flag receives the value shifted out of bit 7.
///     
/// The ASLB/LSLB instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
/// Other uses include conversion of data from serial to parallel and viseversa.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ASLD
/// See Also: LSLD
internal class _58_Aslb_I : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._21;

    public void Exec()
    {
        byte result = (byte)(B << 1);

        //CC_H = undefined;
        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = B.Bit7() ^ B.Bit6();
        CC_C = B.Bit7();

        B = result;
    }
}
