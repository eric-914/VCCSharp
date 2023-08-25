using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>78/ASL/EXTENDED</code>
/// Arithmetic Shift Left of 8-Bit Memory byte
/// <code>78/LSL/EXTENDED</code>
/// Logical Shift Left of 8-Bit Memory byte
/// <code>C ← b7 ← ... ← b0 ← 0</code>
/// </summary>
/// <remarks>
/// The <c>ASL/LSL</c> instruction shifts the contents of the specified byte in memory to the left by one bit, clearing bit 0. 
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
/// The ASL/LSL instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
/// Other uses include conversion of data from serial to parallel and viseversa.
/// 
/// The ASL and LSL mnemonics are duplicates. Both produce the same object code.
/// 
/// Cycles (7 / 6)
/// Byte Count (3)
/// 
/// See Also: ASLD
/// See Also: LSLD
internal class _78_Asl_E : OpCode, IOpCode
{
    internal _78_Asl_E(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort address = M16[PC+=2];
        byte value = M8[address];

        byte result = (byte)(value << 1);

        //CC_H = undefined;
        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value.Bit7() ^ value.Bit6();
        CC_C = value.Bit7();

        M8[address] = result;

        return Cycles._76;
    }
}