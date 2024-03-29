﻿using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Registers;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1E/EXG/IMMEDIATE</code>
/// Exchange Register to Register: 
/// <code>{ A, B, CC, DP, D, X, Y, S, U, PC, E, F, W, V, Z }</code>
/// <code>r0 ↔ r1</code>
/// </summary>
/// <remarks>
/// This instruction exchanges the contents of two registers.
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6309 IMPLEMENTATION │
/// ╰─────────────────────╯
/// 
/// None of the Condition Code flags are affected unless CC is one of the registers involved in the exchange.
/// 
/// Program flow can be altered by specifying PC as one of the registers. 
/// When this occurs, the other register is set to the address of the instruction that follows EXG.
/// 
/// Any of the 6309 registers except Q and MD may be used in the exchange. 
/// The order in which the two registers are specified is irrelevant. 
/// For example, EXG A,B will operate exactly the same as EXG B,A although the object code will be different.
/// 
/// When an 8-bit register is exchanged with a 16-bit register, the contents of the 8-bit register are placed into both halves of the 16-bit register. 
/// Conversely, only the upper or the lower half of the 16-bit register is placed into the 8-bit register. 
/// As illustrated in the diagram below, which half is transferred depends on which 8-bit register is involved.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
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
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// The EXG instruction requires a postbyte in which the two registers that are involved are encoded into the upper and lower nibbles.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
/// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││▒▒1100▒▒│▒▒▒▒0▒▒▒▒▒│
///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││▒▒1101▒▒│▒▒▒▒0▒▒▒▒▒│
///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││▒▒0110▒▒│▒▒▒▒W▒▒▒▒▒││▒▒1110▒▒│▒▒▒▒E▒▒▒▒▒│
///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││▒▒0111▒▒│▒▒▒▒V▒▒▒▒▒││▒▒1111▒▒│▒▒▒▒F▒▒▒▒▒│
///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
///                                                      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒Shaded encodings are invalid on 6809 microprocessors▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// 
/// EXG r0,r1
/// Cycles (8 / 5)
/// Byte Count (2)
/// 
/// See Also: EXG (6809 implementation), TFR
internal class _1E_Exg_M_6309 : OpCode6309, IOpCode
{
    private readonly Action<byte, byte>[] _mixed;
    private readonly RegisterMap4Bit _map;

    public int CycleCount => DynamicCycles._85;

    internal _1E_Exg_M_6309()
    {
        //--From mapping in above documentation
        var A = MSB;
        var B = LSB;
        var E = MSB;
        var F = LSB;
        var DP = MSB;
        var CC = LSB;

        _mixed = new Action<byte, byte>[8] { A, B, CC, DP, Z, Z, E, F }; //--Define handlers when exchanging registers of mixed sizes

        _map = new RegisterMap4Bit
        {
            _16_16 = _16Bit_and_16Bit,
            _16_8 = _16Bit_and_8Bit,
            _8_16 = _8Bit_and_16Bit,
            _8_8 = _8Bit_and_8Bit
        };
    }

    public void Exec()
    {
        byte value = M8[PC++];

        _map.Execute(value);
    }

    private void _16Bit_and_16Bit(byte source, byte destination)
    {
        ushort r0 = R16[source];
        ushort r1 = R16[destination];

        R16[source] = r1;
        R16[destination] = r0;
    }

    private void _16Bit_and_8Bit(byte source, byte destination)
    {
        //--The exchange handlers work with the first register being 8-bit and the second being 16-bit.
        //--The source is a 16-bit register so swap our registers to match.
        _mixed[destination](destination, source);
    }

    private void _8Bit_and_8Bit(byte source, byte destination)
    {
        byte r0 = R8[source];
        byte r1 = R8[destination];

        R8[source] = r1;
        R8[destination] = r0;
    }

    private void _8Bit_and_16Bit(byte source, byte destination)
    {
        _mixed[source](source, destination);
    }

    // Exchange ZERO: Exchange with the (16-bit) zero register which is fixed at zero.  So zero out 8-bit register.
    private void Z(byte r8, byte r16)
    {
        R8[r8] = 0;
    }

    // Exchange HIGH: Get high byte of 16 bit Dest
    private void MSB(byte r8, byte r16)
    {
        byte r1 = R8[r8];
        ushort r2 = R16[r16];

        ushort r1Word = (ushort)((r1 << 8) | r1);   // Place 8 bit source in both halves of 16 bit Dest
        byte r2Byte = (byte)(r2 >> 8);              // Get high byte of 16 bit Dest

        R8[r8] = r2Byte;
        R16[r16] = r1Word;
    }

    // Exchange LOW: Get low byte of 16 bit Dest
    private void LSB(byte r8, byte r16)
    {
        byte r1 = R8[r8];
        ushort r2 = R16[r16];

        ushort r1Word = (ushort)((r1 << 8) | r1);   // Place 8 bit source in both halves of 16 bit Dest
        byte r2Byte = (byte)(r2 & 0xFF);            // Get low byte of 16 bit Dest

        R8[r8] = r2Byte;
        R16[r16] = r1Word;
    }
}
