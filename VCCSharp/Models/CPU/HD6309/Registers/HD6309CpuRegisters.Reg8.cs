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
                2 => _cpu.cc.bits,
                3 => _cpu.dp.msb,
                4 => _cpu.z.msb, //ZERO
                5 => _cpu.z.lsb, //ZERO
                6 => _cpu.q.mswmsb, //E
                7 => _cpu.q.mswlsb, //F
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
                    _cpu.cc.bits = value;
                    break;
                case 3:
                    _cpu.dp.msb = value;
                    break;
                case 4:
                    _cpu.z.msb = value; //ZERO
                    break;
                case 5:
                    _cpu.z.lsb = value; //ZERO
                    break;
                case 6:
                    _cpu.q.mswmsb = value; //E
                    break;
                case 7:
                    _cpu.q.mswlsb = value; //F
                    break;
            }
        }
    }
}
