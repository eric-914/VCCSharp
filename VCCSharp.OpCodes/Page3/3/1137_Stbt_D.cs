using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>1137/STBT/DIRECT</code>
/// Store value of a Register Bit into Memory
/// <code>(DPM).dstBit’ ← r.srcBit</code>
/// </summary>
/// <remarks>
/// The <c>STBT</c> instruction stores the value of a specified bit in either the A, B or CC registers to a specified bit in memory. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
/// 
/// None of the Condition Code flags are affected by the operation. 
/// The usefulness of the STBT instruction is limited by the fact that only Direct Addressing is permitted.
/// ──────────────────────────────────────────────────────────────────────────────────
///           Memory Location $0040                       Accumulator A                      
///       7   6   5   4   3   2   1   0           7   6   5   4   3   2   1   0
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮       ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 1 │ 1 │ $0F   │ 1 │ 1 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ $C6
///  │  ╰───┴───┴───┴───┴───┴───┴───┴───╯       ╰───┴───┴─┬─┴───┴───┴───┴───┴───╯
///  │                                                    │
///  │                            ╭───────────────────────╯
///  │                            ▼
///  ▼  ╭───┬───┬───┬───┬───┬───┬───┬───╮
///     │ 0 │ 0 │ 0 │ 0 │ 1 │ 1 │ 0 │ 1 │ $0D   STBT A,5,1,$40
///     ╰───┴───┴───┴───┴───┴───┴───┴───╯
/// ──────────────────────────────────────────────────────────────────────────────────
/// The figure above shows an example of the STBT instruction where bit 5 from Accumulator A is stored into bit 1 of memory location $0040 (DP = 0).
/// The assembler syntax for this instruction can be confusing due to the ordering of the operands: destination register, source bit, destination bit, source address.
/// 
/// The object code format for the STBT instruction is:
/// ╭─────┬─────┬─────────-┬────────────-╮
/// │ $11 │ $37 │ POSTBYTE │ ADDRESS LSB │
/// ╰─────┴─────┴─────────-┴────────────-╯
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                 POSTBYTE FORMAT
///       7   6   5   4   3   2   1   0                                                  ╭────────┬─────────-╮
///     ╭───┬───┬───┬───┬───┬───┬───┬───╮                                                │  Code  │ Register │   
///     │   │   │   │   │   │   │   │   │                                                ├────────┼──────────┤   
///     ╰─┬─┴─┬─┴─┬─┴───┴─┬─┴─┬─┴───┴─┬─╯                                                │  0 0   │    CC    │   
///       ╰─┬─╯   ╰───┬───╯   ╰───┬───╯                                                  │  0 1   │    A     │   
///         │         │           ╰-─────── Destination (memory) Bit Number (0 - 7)      │  1 0   │    B     │   
///         │         ╰-─────────────────── Source (register) Bit Number (0 - 7)         │  1 1   │ Invalid  │   
///         ╰────────────────────────────── Register Code                                ╰────────┴─────────-╯   
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// 
/// STBT r,sBit,dBit,addr
/// 
/// Cycles (8 / 7)
/// Byte Count (4)
/// 
/// The object code format for the STBT instruction is:
internal class _1137_Stbt_D : OpCode6309, IOpCode
{
    internal _1137_Stbt_D(HD6309.IState cpu) : base(cpu) { }

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

        byte data = R8[register];

        byte sBit = (byte)(1 << source);

        if ((data & sBit) != 0)
        {
            mask |= destination.ToSetMask();
        }
        else
        {
            mask &= destination.ToClearMask();
        }

        M8[address] = mask;

        return DynamicCycles._87;
    }
}
