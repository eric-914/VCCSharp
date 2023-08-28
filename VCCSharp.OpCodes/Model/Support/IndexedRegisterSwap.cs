using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.Support;

internal interface IIndexedRegisterSwap
{
    void SetFlags(IFlags f);

    ushort D { get; }
    ushort W { get; }
    byte CC { get; set; }
    byte DP { get; }

    IRegisters8Bit R8 { get; }
    IRegisters16Bit R16 { get; }
}

internal class IndexedRegisterSwap
{
    private readonly IIndexedRegisterSwap _cpu;
    private readonly bool _setTarget;

    public Func<byte, byte, IFunction> F8 { get; set; } = (_, _) => throw new NotImplementedException();
    public Func<ushort, ushort, IFunction> F16 { get; set; } = (_, _) => throw new NotImplementedException();

    public IndexedRegisterSwap(IIndexedRegisterSwap cpu, bool setTarget)
    {
        _cpu = cpu;
        _setTarget = setTarget;
    }

    public void Exec(byte value)
    {
        byte source = (byte)(value >> 4);
        byte destination = (byte)(value & 15);

        Action<byte, byte> target = Is8Bit(destination) ? _8 : _16;

        target(destination, source);
    }

    private void _8(byte destination, byte source)
    {
        destination &= 0x07;
        source &= 0x07;

        byte dest8 = _cpu.R8[destination];
        byte source8 = Is8Bit(source) ? _cpu.R8[source] : (byte)_cpu.R16[source];

        IFunction fn = F8(dest8, source8);

        _cpu.SetFlags(fn);

        if (_setTarget)
        {
            SetDestination(destination, (byte)fn.Result);
        }
    }

    private void _16(byte destination, byte source)
    {
        ushort dest16 = _cpu.R16[destination];
        ushort source16 = Is8Bit(source) ? GetSource(source) : _cpu.R16[source];

        IFunction fn = F16(dest16, source16);

        _cpu.SetFlags(fn);

        if (_setTarget)
        {
            _cpu.R16[destination] = (ushort)fn.Result;
        }
    }

    private void SetDestination(byte destination, byte result)
    {
        switch (destination)
        {
            case 2: _cpu.CC = result; break;
            case 4: case 5: break; // never assign to zero reg
            default: _cpu.R8[destination] = result; break;
        }
    }

    private ushort GetSource(byte source)
    {
        switch (source & 0x07)
        {
            case 0: case 1: return _cpu.D; // A & B Reg
            case 2: return _cpu.CC; // CC
            case 3: return _cpu.DP; // DP
            case 4: case 5: return 0; // Zero Reg
            case 6: case 7: return _cpu.W; // E & F Reg
        }

        throw new InvalidOperationException();
    }

    //--The 8-Bit register indexes are in the high range (1___) of the nibble, 16-bit register indexes in the low range (0___)
    private bool Is8Bit(byte value) => value.Bit3();
}
