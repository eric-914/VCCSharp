namespace VCCSharp.Models.CPU.MC6809.Registers;

// ReSharper disable once InconsistentNaming
public class Reg16
{
    private readonly MC6809CpuRegisters _cpu;

    public Reg16(MC6809CpuRegisters cpu)
    {
        _cpu = cpu;
    }

    public ushort this[int index]
    {
        get
        {
            return index switch
            {
                0 => _cpu.d.Reg, // &D_REG;
                1 => _cpu.x.Reg, // &X_REG;
                2 => _cpu.y.Reg, // &Y_REG;
                3 => _cpu.u.Reg, // &U_REG;
                4 => _cpu.s.Reg, // &S_REG;
                5 => _cpu.pc.Reg, // &PC_REG;
                _ => throw new NotImplementedException()
            };
        }
        set
        {
            switch (index)
            {
                case 0:
                    _cpu.d.Reg = value;
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
            }
        }
    }
}
