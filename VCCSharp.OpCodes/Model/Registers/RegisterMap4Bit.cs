namespace VCCSharp.OpCodes.Model.Registers;

/// <summary>
/// Abstraction of <c>(r0)</c> → <c>(r1)</c> actions where <c>r0</c> and <c>r1</c> each represent a 4-bit lookup to a 8/16 bit register.
/// </summary>
/// ───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///                                                      [6809]                                             [6309]
///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮         ╭────────┬─────────-╮╭────────┬─────────-╮
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register │         │  Code  │ Register ││  Code  │ Register │
/// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤         ├────────┼──────────┤├────────┼──────────┤
///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     │         │  0000  │    D     ││  1000  │    A     │
///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     │         │  0001  │    X     ││  1001  │    B     │
///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    │         │  0010  │    Y     ││  1010  │    CC    │
///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    │         │  0011  │    U     ││  1011  │    DP    │
///                                                      │  0100  │    S     ││  1100  │ invalid  │         │  0100  │    S     ││▒▒1100▒▒│▒▒▒▒0▒▒▒▒▒│
///                                                      │  0101  │    PC    ││  1101  │ invalid  │         │  0101  │    PC    ││▒▒1101▒▒│▒▒▒▒0▒▒▒▒▒│
///                                                      │  0110  │ invalid  ││  1110  │ invalid  │         │▒▒0110▒▒│▒▒▒▒W▒▒▒▒▒││▒▒1110▒▒│▒▒▒▒E▒▒▒▒▒│
///                                                      │  0111  │ invalid  ││  1111  │ invalid  │         │▒▒0111▒▒│▒▒▒▒V▒▒▒▒▒││▒▒1111▒▒│▒▒▒▒F▒▒▒▒▒│
///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯         ╰────────┴─────────-╯╰────────┴─────────-╯
/// ───────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
internal class RegisterMap4Bit
{
    private const byte MASK = 0x07;

    //--16-bit registers
    public const byte D = 0x00;  // 0000
    public const byte X = 0x01;  // 0001
    public const byte Y = 0x02;  // 0010
    public const byte U = 0x03;  // 0011
    public const byte S = 0x04;  // 0100
    public const byte PC = 0x05; // 0101
    public const byte W = 0x06;  // 0110    🚫 6309 ONLY 🚫
    public const byte V = 0x07;  // 0111    🚫 6309 ONLY 🚫

    //--8-bit registers & MASK
    public const byte A = 0x00;  // 1000
    public const byte B = 0x01;  // 1001
    public const byte CC = 0x02; // 1010
    public const byte DP = 0x03; // 1011
    public const byte Z0 = 0x04; // 1100    🚫 6309 ONLY 🚫
    public const byte Z1 = 0x05; // 1101    🚫 6309 ONLY 🚫
    public const byte E = 0x06;  // 1101    🚫 6309 ONLY 🚫
    public const byte F = 0x07;  // 1101    🚫 6309 ONLY 🚫

    //--The differrent paths that can be taken
    public Action<byte, byte> _16_16 { get; set; } = Undefined;
    public Action<byte, byte> _16_8 { get; set; } = Undefined;
    public Action<byte, byte> _8_16 { get; set; } = Undefined;
    public Action<byte, byte> _8_8 { get; set; } = Undefined;

    private readonly Action<byte, byte>[] _paths;

    public RegisterMap4Bit()
    {
        _paths = new Action<byte, byte>[] { _16_16, _16_8, _8_16, _8_8 };
    }

    public void Execute(byte operand)
    {
        byte source = (byte)(operand >> 4);
        byte destination = (byte)(operand & 0x0F);

        byte path = (byte)(((operand >> 6) & 0x02) | ((operand >> 3) & 0x01));

        _paths[path]((byte)(source & MASK), (byte)(destination & MASK));
    }

    private static void Undefined(byte a, byte b) => throw new NotImplementedException();
}
