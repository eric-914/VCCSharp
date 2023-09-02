using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1132/BOR/DIRECT</code>
/// Logically OR Register Bit with Memory Bit
/// <code>
/// 
/// r.dstBit’ ← r.dstBit OR (DPM).srcBit
/// </code>
/// </summary>
/// <remarks>
/// The <c>BOR</c> instruction logically ORs the value of a specified bit in either the A, B or CC registers with a specified bit in memory. 
/// The resulting value is placed back into the register bit. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit may be affected.
/// The usefulness of the BOR instruction is limited by the fact that only Direct Addressing is permitted. 
/// ──────────────────────────────────────────────────────────────────────────────────
///               Accumulator A                      Memory Location $0040
///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 0 │ 0 │ 0 │ $08   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴─┬─┴───┴───┴───┴───┴───┴───╯
///  │                          ╭───╮                 │
///  │                       OR │ 1 │ ◀──────────────-╯
///  │                          ╰───╯   
///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 0 │ 1 │ 0 │ $0A   BOR A,6,1,$40
///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
/// ──────────────────────────────────────────────────────────────────────────────────
/// 
/// The figure above shows an example of the BOR instruction where bit 1 of Accumulator A is ORed with bit 6 of the byte in memory at address $0040 (DP = 0).
/// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
/// Since the Condition Code flags are not affected by the operation, additional instructions would be needed to test the result for conditional branching.
/// 
/// The object code format for the BOR instruction is:
/// ╭─────┬─────┬─────────-┬────────────-╮
/// │ $11 │ $32 │ POSTBYTE │ ADDRESS LSB │
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
/// BOR r,sBit,dBit,addr
/// 
/// Cycles (7 / 6)
/// Byte Count (4)
/// 
/// See Also: BAND, BEOR, BIAND, BIEOR, BIOR, LDBT, STBT
internal class _1132_Bor_D : OpCode6309, IOpCode
{
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
            R8[register] |= dBit;
        }

        // Else nothing changes
        return DynamicCycles._76;
    }
}
