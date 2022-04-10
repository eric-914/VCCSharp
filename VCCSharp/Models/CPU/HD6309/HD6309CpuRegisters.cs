#pragma warning disable IDE1006

namespace VCCSharp.Models.CPU.HD6309;

// ReSharper disable once InconsistentNaming
public class HD6309CpuRegisters
{
    // ReSharper disable InconsistentNaming
    // ReSharper disable IdentifierTypo

    public HD6309CpuRegister pc { get; } = new();
    public HD6309CpuRegister x { get; } = new();
    public HD6309CpuRegister y { get; } = new();
    public HD6309CpuRegister u { get; } = new();
    public HD6309CpuRegister s { get; } = new();
    public HD6309CpuRegister dp { get; } = new();
    public HD6309CpuRegister v { get; } = new();
    public HD6309CpuRegister z { get; } = new();

    public HD6309WideRegister q { get; } = new();

    public byte ccbits;
    public byte mdbits;

    public bool[] cc = new bool[8];
    public bool[] md = new bool[8];

    public Reg8 ureg8 { get; }
    public Reg16 xfreg16 { get; }

    public HD6309CpuRegisters()
    {
        ureg8 = new Reg8(this);
        xfreg16 = new Reg16(this);
    }

    public class Reg8
    {
        private readonly HD6309CpuRegisters _cpu;

        public Reg8(HD6309CpuRegisters cpu)
        {
            _cpu = cpu;
        }

        public byte this[int index]
        {
            get
            {
                return index switch
                {
                    0 => _cpu.q.lswmsb,
                    1 => _cpu.q.lswlsb,
                    2 => _cpu.ccbits,
                    3 => _cpu.dp.msb,
                    4 => _cpu.z.msb,
                    5 => _cpu.z.lsb,
                    6 => _cpu.q.mswmsb,
                    7 => _cpu.q.mswlsb,
                    _ => throw new NotImplementedException()
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        _cpu.q.lswmsb = value;
                        break;
                    case 1:
                        _cpu.q.lswlsb = value;
                        break;
                    case 2:
                        _cpu.ccbits = value;
                        break;
                    case 3:
                        _cpu.dp.msb = value;
                        break;
                    case 4:
                        _cpu.z.msb = value;
                        break;
                    case 5:
                        _cpu.z.lsb = value;
                        break;
                    case 6:
                        _cpu.q.mswmsb = value;
                        break;
                    case 7:
                        _cpu.q.mswlsb = value;
                        break;
                }
            }
        }
    }

    public class Reg16
    {
        private readonly HD6309CpuRegisters _cpu;

        public Reg16(HD6309CpuRegisters cpu)
        {
            _cpu = cpu;
        }

        public ushort this[int index]
        {
            get
            {
                return index switch
                {
                    0 => _cpu.q.lsw,
                    1 => _cpu.x.Reg,
                    2 => _cpu.y.Reg,
                    3 => _cpu.u.Reg,
                    4 => _cpu.s.Reg,
                    5 => _cpu.pc.Reg,
                    6 => _cpu.q.msw,
                    7 => _cpu.v.Reg,
                    _ => throw new NotImplementedException()
                };
            }
            set
            {
                switch (index)
                {
                    case 0:
                        _cpu.q.lsw = value;
                        break;
                    case 1:
                        _cpu.x.Reg = value;
                        break;
                    case 2:
                        _cpu.y.Reg = value;
                        break;
                    case 3:
                        _cpu.u.Reg = value;
                        break;
                    case 4:
                        _cpu.s.Reg = value;
                        break;
                    case 5:
                        _cpu.pc.Reg = value;
                        break;
                    case 6:
                        _cpu.q.msw = value;
                        break;
                    case 7:
                        _cpu.v.Reg = value;
                        break;
                }
            }
        }
    }
}
