using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Registers;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1F/TFR/IMMEDIATE</code>
/// Transfer Register to Register: 
/// <code>{ A, B, CC, DP, D, X, Y, S, U, PC, E, F, W, V, Z }</code>
/// <code>r0 → r1</code>
/// </summary>
/// <remarks>
/// <c>TFR</c> copies the contents of a source register into a destination register. 
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6309 IMPLEMENTATION │
/// ╰─────────────────────╯
/// TFR copies the contents of a source register into a destination register. 
/// None of the Condition Code flags are affected unless CC is specified as the destination register.
/// 
/// Any of the 6309 registers except Q and MD may be specified as either the source, destination or both. 
/// Specifying the same register for both the source and destination produces an instruction which, like NOP, has no effect.
/// 
/// The TFR instruction can be used to alter the flow of execution by specifying PC as the destination register.
/// 
/// When an 8-bit source register is transferred to a 16-bit destination register, the contents of the 8-bit register are placed into both halves of the 16-bit register. 
/// When a 16-bit source register is transferred to an 8-bit destination register, only the upper or the lower half of the 16-bit register is transferred. 
/// As illustrated in the diagram below, which half is transferred depends on which 8-bit register is specified as the destination.
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                     b15  b8  b7   b0
///                                                    ╭───────╮╭───────╮
///     16-bit register ( D, X, Y, U, S, PC, W, V ):   │  MSB  ││  LSB  │
///                                                    ╰─┬─┬─┬─╯╰─┬─┬─┬─╯
///                                      ╭───────────────╯ │ │    │ │ ╰───────────────╮
///                                      │        ╭───────────────╯ │                 │
///                                      │        │        │ ╰──────│────────╮        │
///                                   ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮
///                 8-bit register:   │  A  │  │  B  │  │  E  │  │  F  │  │ DP  │  │ CC  │
///                                   ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// The TFR instruction requires a postbyte in which the source and destination registers are encoded into the upper and lower nibbles respectively.
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
/// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││▒▒1100▒▒│▒▒▒▒0▒▒▒▒▒│
///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││▒▒1101▒▒│▒▒▒▒0▒▒▒▒▒│
///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││▒▒0110▒▒│▒▒▒▒W▒▒▒▒▒││▒▒1110▒▒│▒▒▒▒E▒▒▒▒▒│
///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││▒▒0111▒▒│▒▒▒▒V▒▒▒▒▒││▒▒1111▒▒│▒▒▒▒F▒▒▒▒▒│
///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
///                                                      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒Shaded encodings are invalid on 6809 microprocessors▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// 
/// Cycles (6 / 4)
/// Byte Count (2)
/// 
/// See Also: EXG, TFR (6809 implementation)
internal class _1F_Tfr_M_6309 : OpCode6309, IOpCode
{
    private readonly Action<byte, byte, bool>[] _mixed;
    private readonly RegisterMap4Bit _map;

    internal _1F_Tfr_M_6309(HD6309.IState cpu) : base(cpu)
    {
        //--From mapping in above documentation
        var A = MSB;
        var B = LSB;
        var E = MSB;
        var F = LSB;
        var DP = MSB;
        var CC = LSB;

        _mixed = new Action<byte, byte, bool>[8] { A, B, CC, DP, Z, Z, E, F }; //--Define handlers when exchanging registers of mixed sizes

        _map = new RegisterMap4Bit
        {
            _16_16 = _16Bit_and_16Bit,
            _16_8 = _16Bit_and_8Bit,
            _8_16 = _8Bit_and_16Bit,
            _8_8 = _8Bit_and_8Bit
        };
    }

    public int Exec()
    {
        byte value = M8[PC++];

        _map.Execute(value);

        return DynamicCycles._64;
    }

    private void _16Bit_and_16Bit(byte source, byte destination) => R16[destination] = R16[source];

    //--The exhange handlers work with the first register being 8-bit and the second being 16-bit.
    //--The source is a 16-bit register so swap our registers to match.
    private void _16Bit_and_8Bit(byte source, byte destination) => _mixed[destination](destination, source, true);

    private void _8Bit_and_8Bit(byte source, byte destination) => R8[destination] = R8[source];

    private void _8Bit_and_16Bit(byte source, byte destination) => _mixed[source](source, destination, false);

    // Transfer ZERO: Transfer to/from the (16-bit) zero register which is fixed at zero.  So zero out 8-bit register if target.
    private void Z(byte r8, byte r16, bool reverse)
    {
        if (reverse) //--target: 8-bit
        {
            R8[r8] = 0;
        }
    }

    // Transfer HIGH: Get high byte of 16 bit Dest
    private void MSB(byte r8, byte r16, bool reverse)
    {
        if (reverse) //--target: 8-bit
        {
            ushort r2 = R16[r16];
            byte r2Byte = (byte)(r2 >> 8);              // Get high byte of 16 bit Dest
            R8[r8] = r2Byte;
        }
        else //--target: 16-bit
        {
            byte r1 = R8[r8];
            ushort r1Word = (ushort)((r1 << 8) | r1);   // Place 8 bit source in both halves of 16 bit Dest
            R16[r16] = r1Word;
        }
    }

    // Transfer LOW: Get low byte of 16 bit Dest
    private void LSB(byte r8, byte r16, bool reverse)
    {
        if (reverse) //--target: 8-bit
        {
            ushort r2 = R16[r16];
            byte r2Byte = (byte)(r2 & 0xFF);            // Get low byte of 16 bit Dest
            R8[r8] = r2Byte;
        }
        else //--target: 16-bit
        {
            byte r1 = R8[r8];
            ushort r1Word = (ushort)((r1 << 8) | r1);   // Place 8 bit source in both halves of 16 bit Dest
            R16[r16] = r1Word;
        }
    }
}
