using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

/// ╭────────┬─────────-╮╭────────┬─────────-╮
/// │  Code  │ Register ││  Code  │ Register │
/// ├────────┼──────────┤├────────┼──────────┤
/// │  1000  │    A     ││  1100  │ invalid  │
/// │  1001  │    B     ││  1101  │ invalid  │
/// │  1010  │    CC    ││  1110  │ invalid  │
/// │  1011  │    DP    ││  1111  │ invalid  │
/// ╰────────┴─────────-╯╰────────┴─────────-╯
internal class Registers8Bit<T> : IRegisters8Bit
    where T : IRegisterCC, IRegisterA, IRegisterB, IRegisterDP
{
    private readonly T _cpu;

    private readonly Func<byte>[] _getter;
    private readonly Action<byte>[] _setter;

    public Registers8Bit(T cpu)
    {
        _cpu = cpu;

        _getter = new Func<byte>[8] { () => A, () => B, () => CC, () => DP, invalid, invalid, invalid, invalid };
        _setter = new Action<byte>[8] { v => A = v, v => B = v, v => CC = v, v => DP = v, nop, nop, nop, nop };
    }

    public byte this[int index]
    {
        get => _getter[index]();
        set => _setter[index](value);
    }

    private byte A { get => _cpu.A; set => _cpu.A = value; }
    private byte B { get => _cpu.B; set => _cpu.B = value; }
    private byte CC { get => _cpu.CC; set => _cpu.CC = value; }
    private byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    byte invalid() => 0xFF;
    void nop(byte _) { }
}
