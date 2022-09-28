namespace VCCSharp.Models.CPU.HD6309.Registers;

// ReSharper disable once InconsistentNaming
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
