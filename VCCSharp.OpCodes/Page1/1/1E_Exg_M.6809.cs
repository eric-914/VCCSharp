using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Registers;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1E/EXG/IMMEDIATE</code>
/// Exchange Register to Register: 
/// <code>{ A, B, CC, DP, D, X, Y, S, U, PC }</code>
/// <code>r0 ↔ r1</code>
/// </summary>
/// <remarks>
/// This instruction exchanges the contents of two registers.
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6809 IMPLEMENTATION │
/// ╰─────────────────────╯
/// 
/// None of the Condition Code flags are affected unless CC is one of the registers involved in the exchange.
/// 
/// Program flow can be altered by specifying PC as one of the registers. 
/// When this occurs, the other register is set to the address of the instruction that follows EXG.
/// 
/// Any of the 6809 registers may be used in the exchange. 
/// When exchanging registers of the same size, the order in which they are specified is irrelevant. 
/// For example, EXG A,B will operate exactly the same as EXG B,A although the object code will be different.
/// 
/// When exchanging registers of different sizes, a 6809 operates differently than a 6309.
/// The 8-bit register is always exchanged with the lower half of the 16-bit register, and the the upper half of the 16-bit register is then set to the value shown in the table below.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///         ╭───────────────┬─────────────────────┬────────────────────────────────-╮
///         │ Operand Order │ 8-bit Register Used │ 16-bit Register’s MSB after EXG │
///         ├───────────────┼─────────────────────┼─────────────────────────────────┤
///         │    16 , 8     │         Any         │              0xFF *             │
///         │     8 , 16    │        A or B       │              0xFF *             │
///         │     8 , 16    │       CC or DP      │           Same as LSB           │
///         ╰───────────────┴─────────────────────┴────────────────────────────────-╯
///         *The one exception is for EXG A,D which produces exactly the same result as EXG A,B
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// The EXG instruction requires a postbyte in which the two registers are encoded into the upper and lower nibbles.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
/// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││  1100  │ invalid  │
///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││  1101  │ invalid  │
///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││  0110  │ invalid  ││  1110  │ invalid  │
///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││  0111  │ invalid  ││  1111  │ invalid  │
///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// If an invalid register encoding is specified for either register, a constant value of 0xFF or 0xFFFF is used for the exchange. 
/// The invalid register encodings have valid meanings on 6309 processors, and should be avoided in code intended to run on both CPU’s.
/// 
/// EXG r0,r1
/// Cycles (8)
/// Byte Count (2)
/// 
/// See Also: EXG (6309 implementation), TFR
internal class _1E_Exg_M_6809 : OpCode, IOpCode
{
    private new const byte A = RegisterMap4Bit.A;
    private new const byte B = RegisterMap4Bit.B;
    private new const byte CC = RegisterMap4Bit.CC;
    private new const byte DP = RegisterMap4Bit.DP;

    private readonly RegisterMap4Bit _map;

    internal _1E_Exg_M_6809()
    {
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

        return 8;
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
        byte r0 = (byte)(R16[source] & 0xFF);
        ushort r1 = (ushort)(0xFF00 | R8[destination]);

        R16[source] = r1;
        R8[destination] = r0;
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
        byte r0 = R8[source];
        ushort r1 = R16[destination];

        ushort Convert(byte r)
        {
            if (destination == A || destination == B)
            {
                return (ushort)(0xFF00 | r);
            }
            else if (destination == CC || destination == DP)
            {
                return (ushort)(r << 8 | r);
            }

            return r;
        }


        R8[source] = (byte)(r1 & 0xFF);
        R16[destination] = Convert(r0);
    }
}