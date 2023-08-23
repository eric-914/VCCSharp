﻿using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

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

    private byte A { get => _cpu.A_REG; set => _cpu.A_REG = value; }
    private byte B { get => _cpu.B_REG; set => _cpu.B_REG = value; }
    private byte CC { get => _cpu.CC; set => _cpu.CC = value; }
    private byte DP { get => _cpu.DP; set => _cpu.DP = value; }

    byte invalid() => 0xFF;
    void nop(byte _) { }
}
