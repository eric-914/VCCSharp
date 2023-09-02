﻿using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.HD6309;

internal class Registers8Bit<T> : IRegisters8Bit
    where T : IRegisterCC, IRegisterA, IRegisterB, IRegisterDP, IRegisterE, IRegisterF, IRegisterZ
{
    private readonly T _cpu;

    private readonly Func<byte>[] _getter;
    private readonly Action<byte>[] _setter;

    public Registers8Bit(T cpu)
    {
        _cpu = cpu;

        _getter = new Func<byte>[8] { () => A, () => B, () => CC, () => DP, () => Z, () => Z, () => E, () => F };
        _setter = new Action<byte>[8] { v => A = v, v => B = v, v => CC = v, v => DP = v, _ => { }, _ => { }, v => E = v, v => F = v };
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
    private byte E { get => _cpu.E; set => _cpu.E = value; }
    private byte F { get => _cpu.F; set => _cpu.F = value; }

    private byte Z { get => 0; } //--ZERO Register
}
