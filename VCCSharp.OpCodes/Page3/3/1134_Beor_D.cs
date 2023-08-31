using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1134/BEOR/DIRECT</code>
/// Exclusive-OR Register Bit with Memory Bit
/// <code>
/// 
/// r.dstBit’ ← r.dstBit ⨁ (DPM).srcBit
/// </code>
/// </summary>
/// <remarks>
/// The <c>BEOR</c> instruction logically XORs the value of a specified bit in either the A, B or CC registers with a specified bit in memory. 
/// The resulting value is placed back into the register bit. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit may be affected. 
/// The usefulness of the BEOR instruction is limited by the fact that only Direct Addressing is permitted.
/// ──────────────────────────────────────────────────────────────────────────────────
///               Accumulator A                      Memory Location $0040
///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 │ $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴───┴─┬─┴───┴───┴───┴───┴───╯
///  │                          ╭───╮                     │
///  │                     EOR  │ 1 │ ◀───────────────────╯
///  │                          ╰───╯
///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ 1 │ $0D   BEOR A,6,1,$40
///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
/// ──────────────────────────────────────────────────────────────────────────────────
/// The figure above shows an example of the BEOR instruction where bit 1 of Accumulator A is Exclusively ORed with bit 6 of the byte in memory at address $0040 (DP = 0).
/// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
/// Since the Condition Code flags are not affected by the operation, additional instructions would be needed to test the result for conditional branching.
/// 
/// The object code format for the BEOR instruction is:
/// ╭─────┬─────┬─────────-┬────────────-╮
/// │ $11 │ $30 │ POSTBYTE │ ADDRESS LSB │
/// ╰─────┴─────┴─────────-┴────────────-╯
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                 POSTBYTE FORMAT
///       7   6   5   4   3   2   1   0                                                  ╭────────┬─────────-╮
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮                                                │  Code  │ Register │   
///     │   │   │   │   │   │   │   │   │                                                ├────────┼──────────┤   
///     ╰─┬─┴─┬─┴─┬─┴───┴─┬─┴─┬─┴───┴─┬─╯                                                │  0 0   │    CC    │   
///       ╰─┬─╯   ╰───┬───╯   ╰───┬───╯                                                  │  0 1   │    A     │   
///         │         │           ╰-─────── Destination (register) Bit Number (0 - 7)    │  1 0   │    B     │   
///         │         ╰-─────────────────── Source (memory) Bit Number (0 - 7)           │  1 1   │ Invalid  │   
///         ╰────────────────────────────── Register Code                                ╰────────┴─────────-╯   
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// 
/// BEOR r,sBit,dBit,addr
/// 
/// Cycles (7 / 6)
/// Byte Count (4)
/// 
/// See Also: BAND, BIAND, BIEOR, BIOR, BOR, LDBT, STBT
internal class _1134_Beor_D : OpCode6309, IOpCode
{
    internal _1134_Beor_D(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];
        ushort address = DIRECT[PC++];
        byte mask = M8[address];

        byte destination = (byte)(value & 7);
        byte source = (byte)((value >> 3) & 7);
        byte register = (byte)(value >> 6);

        if (register == 3)
        {
            return Exceptions.IllegalInstruction();
        }

        byte sBit = (byte)(1 << source);
        byte dBit = (byte)~(1 << destination);

        if ((mask & sBit) == 0)
        {
            R8[register] ^= dBit;
        }

        // Else nothing changes
        return Cycles._76;
    }
}
