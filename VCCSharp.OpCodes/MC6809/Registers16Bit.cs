using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

internal class Registers16Bit<T> : IRegisters16Bit
    where T : IRegisterD, IRegisterX, IRegisterY, IRegisterU, IRegisterS
{
    private readonly T _cpu;

    private readonly Func<ushort>[] _getter;
    private readonly Action<ushort>[] _setter;

    public Registers16Bit(T cpu)
    {
        _cpu = cpu;

        _getter = new Func<ushort>[8] { () => D, () => X, () => Y, () => U , () => S, invalid, invalid, invalid };
        _setter = new Action<ushort>[8] { v => D = v, v => X = v, v => Y = v, v => U = v, v => S = v, nop, nop, nop };
    }

    public ushort this[int index]
    {
        get => _getter[index]();
        set => _setter[index](value);
    }

    private ushort D { get => _cpu.D; set => _cpu.D = value; }
    private ushort X { get => _cpu.X; set => _cpu.X = value; }
    private ushort Y { get => _cpu.Y; set => _cpu.Y = value; }
    private ushort U { get => _cpu.U; set => _cpu.U = value; }
    private ushort S { get => _cpu.S; set => _cpu.S = value; }

    ushort invalid() => 0xFFFF;
    void nop(ushort _) { }
}
