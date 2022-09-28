namespace VCCSharp.Models.CPU.HD6309.Registers;

// ReSharper disable once InconsistentNaming
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
