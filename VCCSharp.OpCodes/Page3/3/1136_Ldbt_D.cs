using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1136/LDBT/DIRECT</code>
/// Load Memory Bit into Register Bit
/// <code>r.dstBit’ ← (DPM).srcBit</code>
/// </summary>
/// <remarks>
/// The <c>LDBT</c> instruction loads the value of a specified bit in memory into a specified bit of either the A, B or CC registers. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by the operation unless CC is specified as the register, in which case only the destination bit will be affected. 
/// The usefulness of the LDBT instruction is limited by the fact that only Direct Addressing is permitted.
/// ──────────────────────────────────────────────────────────────────────────────────
///               Accumulator A                      Memory Location $0040
///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 │ $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴───┴─┬─┴───┴───┴───┴───┴───╯
///  │                                                    │
///  │                            ╭───────────────────────╯
///  │                            ▼
///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ 1 │ $0D   LDBT A,5,1,$40
///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
/// ──────────────────────────────────────────────────────────────────────────────────
/// The figure above shows an example of the LDBT instruction where bit 1 of Accumulator A is Loaded with bit 5 of the byte in memory at address $0040 (DP = 0).
/// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
/// 
/// The object code format for the LDBT instruction is:
/// ╭─────┬─────┬─────────-┬────────────-╮
/// │ $11 │ $36 │ POSTBYTE │ ADDRESS LSB │
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
/// LDBT r,sBit,dBit,addr
///
/// Cycles (7 / 6)
/// Byte Count (4)
/// 
/// See Also: BAND, BEOR, BIAND, BIEOR, BIOR, BOR, STBT
internal class _1136_Ldbt_D : OpCode6309, IOpCode
{
    internal _1136_Ldbt_D(HD6309.IState cpu) : base(cpu) { }

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

        //TODO: Verify the following:

        byte data = M8[address];

        byte sBit = (byte)(1 << source);

        if ((data & sBit) != 0)
        {
            mask |= destination.ToSetMask();
        }
        else
        {
            mask &= destination.ToClearMask();
        }

        R8[register] = mask;

        return Cycles._76;
    }
}
