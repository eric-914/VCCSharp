using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1131/BIAND/DIRECT</code>
/// Logically AND Register Bit with Inverted Memory Bit
/// <code>
///                          ＿＿＿＿＿＿＿
/// r.dstBit’ ← r.dstBit AND (DPM).srcBit
/// </code>
/// </summary>
/// <remarks>
/// The <c>BIAND</c> instruction logically ANDs the value of a specified bit in either the A, B or CC registers with the inverted value of a specified bit in memory. 
/// The resulting value is placed back into the register bit. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit may be affected. 
/// The usefulness of the BIAND instruction is limited by the fact that only Direct Addressing is permitted.
/// ──────────────────────────────────────────────────────────────────────────────────
///               Accumulator A                      Memory Location $0040
///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 │ $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
///  │  ╰───┴───┴───┴───┴─┬─┴───┴───┴───╯       ╰───┴───┴───┴───┴───┴───┴─┬─┴───╯
///  │                    │         ╭───╮       ╭───╮                     │
///  │                    ╰───────▶ │ 1 │  AND  │ 0 │ ◀──────INVERT──────-╯
///  │                              ╰───╯   │   ╰───╯
///  │                    ╭─────────────────╯
///  │                    ▼      
///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ $07   BIAND A,1,3,$40
///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
/// ──────────────────────────────────────────────────────────────────────────────────
/// The figure above shows an example of the BIAND instruction where bit 3 of Accumulator A is ANDed with the inverted value of bit 1 from the byte in memory at address $0040 (DP = 0).
/// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
/// Since the Condition Code flags are not affected by the operation, additional instructions would be needed to test the result for conditional branching.
/// 
/// The object code format for the BIAND instruction is:
/// ╭─────┬─────┬─────────-┬────────────-╮
/// │ $11 │ $31 │ POSTBYTE │ ADDRESS LSB │
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
/// BIAND r,sBit,dBit,addr
/// 
/// Cycles (7 / 6)
/// Byte Count (4)
/// 
/// See Also: BAND, BEOR, BIEOR, BIOR, BOR, LDBT, STBT
internal class _1131_Biand_D : OpCode6309, IOpCode
{
    public int CycleCount => DynamicCycles._76;

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

        if ((mask & sBit) != 0)
        {
            R8[register] &= dBit;
        }

        // Else do nothing
        return CycleCount;
    }
}
