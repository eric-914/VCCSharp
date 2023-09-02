using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>48/ASLA/INHERENT</code>
/// Arithmetic Shift Left of 8-Bit Accumulator <c>A</c>
/// <code>48/LSLA/INHERENT</code>
/// Logical Shift Left of 8-Bit Accumulator <c>A</c>
/// <code>C ← b7 ← ... ← b0 ← 0</code>
/// </summary>
/// <remarks>
/// The <c>ASLA/LSLA</c> instruction shifts the contents of the <c>A</c> accumulator to the left by one bit, clearing bit 0. 
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
/// The ASLA/LSLA instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
/// Other uses include conversion of data from serial to parallel and viseversa.
/// 
/// Cycles (2 / 1)
/// Byte Count (1)
/// 
/// See Also: ASLD
/// See Also: LSLD
internal class _48_Asla_I : OpCode, IOpCode
{
    internal _48_Asla_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte result = (byte)(A << 1);

        //CC_H = undefined;
        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = A.Bit7() ^ A.Bit6();
        CC_C = A.Bit7();

        A = result;

        return DynamicCycles._21;
    }
}
