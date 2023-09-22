using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>08/ASL/DIRECT</code>
/// Arithmetic Shift Left of 8-Bit Memory byte
/// <code>08/LSL/DIRECT</code>
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
/// Cycles (6 / 5)
/// Byte Count (2)
/// 
/// See Also: ASLD
/// See Also: LSLD
internal class _08_Asl_D : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._65;

    public void Exec()
    {
        ushort address = DIRECT[PC++];
        byte value = M8[address];

        byte result = (byte)(value << 1);

        //CC_H = undefined;
        CC_N = result.Bit7();
        CC_Z = result == 0;
        CC_V = value.Bit7() ^ value.Bit6();
        CC_C = value.Bit7();

        M8[address] = result;
    }
}
