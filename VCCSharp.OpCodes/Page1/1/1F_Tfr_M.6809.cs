using VCCSharp.OpCodes.Model.OpCodes;
using VCCSharp.OpCodes.Model.Registers;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>1F/TFR/IMMEDIATE</code>
/// Transfer Register to Register: 
/// <code>{ A, B, CC, DP, D, X, Y, S, U, PC }</code>
/// <code>r0 → r1</code>
/// </summary>
/// <remarks>
/// <c>TFR</c> copies the contents of a source register into a destination register. 
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6809 IMPLEMENTATION │
/// ╰─────────────────────╯
/// TFR copies the contents of a source register into a destination register. 
/// None of the Condition Code flags are affected unless CC is specified as the destination register.
/// The TFR instruction can be used to alter the flow of execution by specifying PC as the destination register.
/// 
/// Any of the 6809 registers may be specified as either the source, destination or both. 
/// Specifying the same register for both the source and destination produces an instruction which, like NOP, has no effect.
/// The table below explains how the destination register is affected when the source and destination sizes are different. 
/// This behavior differs from the 6309 implementation.
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///         ╭───────────┬─────────────────────┬─────────────────────────────────────────-╮
///         │ Operation │ 8-bit Register Used │                Results                   │
///         ├───────────┼─────────────────────┼──────────────────────────────────────────┤
///         │  16 → 8   │        Any          │ Destination = LSB from Source            │
///         │   8 → 16  │      A or B         │ MSB of Destination = 0xFF ; LSB = Source │
///         │   8 → 16  │     CC or DP        │ Both MSB and LSB of Destination = Source │
///         ╰───────────┴─────────────────────┴─────────────────────────────────────────-╯
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// The TFR instruction requires a postbyte in which the source and destination registers are encoded into the upper and lower nibbles respectively.
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
/// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││  1100  │ invalid  │
///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││  1101  │ invalid  │
///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││  0110  │ invalid  ││  1110  │ invalid  │
///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││  0111  │ invalid  ││  1111  │ invalid  │
///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
/// ─────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// If an invalid register encoding is used for the source, a constant value of 0xFF or 0xFFFF is transferred to the destination. 
/// If an invalid register encoding is used for the destination, then the instruction will have no effect. 
/// The invalid register encodings have valid meanings when executed on 6309 processors, and should be avoided in code that needs to work the same way on both CPU’s. 
/// 
/// Cycles (6)
/// Byte Count (2)
/// 
/// See Also: EXG, TFR (6309 implementation)
internal class _1F_Tfr_M_6809 : OpCode, IOpCode
{
    private new const byte A = RegisterMap4Bit.A;
    private new const byte B = RegisterMap4Bit.B;
    private new const byte CC = RegisterMap4Bit.CC;
    private new const byte DP = RegisterMap4Bit.DP;

    private readonly RegisterMap4Bit _map;

    internal _1F_Tfr_M_6809(MC6809.IState cpu) : base(cpu)
    {
        _map = new RegisterMap4Bit
        {
            _16_16 = _16Bit_to_16Bit,
            _16_8 = _16Bit_to_8Bit,
            _8_16 = _8Bit_to_16Bit,
            _8_8 = _8Bit_to_8Bit
        };
    }

    public int Exec()
    {
        byte value = M8[PC++];

        _map.Execute(value);

        return 6;
    }

    private void _16Bit_to_16Bit(byte source, byte destination) => R16[destination] = R16[source];

    private void _16Bit_to_8Bit(byte source, byte destination) => R8[destination] = (byte)(R16[source] & 0xFF);

    private void _8Bit_to_8Bit(byte source, byte destination) => R8[destination] = R8[source];

    private void _8Bit_to_16Bit(byte source, byte destination)
    {
        byte r8 = R8[source];
        ushort r16 = r8;

        if (destination == A || destination == B)
        {
            r16 = (ushort)(0xFF00 | r8);
        }
        else if (destination == CC || destination == DP)
        {
            r16 = (ushort)(r8 << 8 | r8);
        }

        R16[destination] = r16;
    }
}
