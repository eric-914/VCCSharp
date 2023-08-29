using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>1047/ASLD/INHERENT</code>
/// Arithmetic Shift Left of Accumulator <c>D</c>
/// <code>1047/LSLD/INHERENT</code>
/// Logical Shift Left of Accumulator <c>D</c>
/// <code>C ← b15 ← ... ← b0 ← 0</code>
/// </summary>
/// <remarks>
/// The <c>ALSD</c> instruction shifts the contents of Accumulator <c>D</c> to the left by one bit, clearing bit 0.
/// Bit 15 is shifted into the Carry (<c>C</c>) flag of the Condition Codes register.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
///    ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──┬──╮
///    │  │ ◀── │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │  │ ◀── 0
///    ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──┴──╯
///     C        b15 ◀─────────────────────────────────────── b0      
///     
/// [E F H I N Z V C]
/// [        ↕ ↕ ↕ ↕]
///   
/// The Condition Code flags are affected as follows:
///         N The Negative flag is set equal to the new value of bit 15; previously bit 14.
///         Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
///         V The Overflow flag is set to the XOR of the original values of bits 14 and 15.
///         C The Carry flag receives the value shifted out of bit 15.
///     
/// The ASL instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
/// Other uses include conversion of data from serial to parallel and viseversa.
/// 
/// The D accumulator is the only 16-bit register for which an ASL instruction has been provided. 
/// You can however achieve the same functionality using the ADDR instruction.
/// For example, ADDR W,W will perform the same left-shift operation on the W accumulator.
/// 
/// A left-shift of the 32-bit Q accumulator can be achieved as follows:
///     ADDR W,W ; Shift Low-word, Hi-bit into Carry
///     ROLD     ; Shift Hi-word, Carry into Low-bit
///     
/// The ASLD and LSLD mnemonics are duplicates. Both produce the same object code.
/// 
/// Cycles (3 / 2)
/// Byte Count (2)
/// 
/// See Also: ASL (8-bit), ROL (16-bit)
/// See Also: LSL (8-bit), ROL (16-bit)
internal class _1048_Asld_I : OpCode6309, IOpCode
{
    internal _1048_Asld_I(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        ushort result = (ushort)(D << 1);

        CC_N = result.Bit15();
        CC_Z = result == 0;
        CC_V = D.Bit14() ^ D.Bit15();
        CC_C = D.Bit15();

        D = result;

        return Cycles._32;
    }
}
