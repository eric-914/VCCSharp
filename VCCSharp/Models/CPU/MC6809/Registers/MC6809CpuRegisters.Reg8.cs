namespace VCCSharp.Models.CPU.MC6809.Registers;

// ReSharper disable once InconsistentNaming
public class Reg8
{
    private readonly MC6809CpuRegisters _cpu;

    public Reg8(MC6809CpuRegisters cpu)
    {
        _cpu = cpu;
    }

    public byte this[int index]
    {
        get
        {
            return index switch
            {
                0 => _cpu.d.msb,
                1 => _cpu.d.lsb,
                2 => _cpu.cc.bits,
                3 => _cpu.dp.msb,
                4 => _cpu.dp.msb,
                5 => _cpu.dp.msb,
                6 => _cpu.dp.msb,
                7 => _cpu.dp.msb,
                _ => throw new NotImplementedException()
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    _cpu.d.msb = value;
                    break;
                case 1:
                    _cpu.d.lsb = value;
                    break;
                case 2:
                    _cpu.cc.bits = value;
                    break;
                case 3:
                    _cpu.dp.msb = value;
                    break;
                case 4:
                    _cpu.dp.msb = value;
                    break;
                case 5:
                    _cpu.dp.msb = value;
                    break;
                case 6:
                    _cpu.dp.msb = value;
                    break;
                case 7:
                    _cpu.dp.msb = value;
                    break;
            }
        }
    }
}
