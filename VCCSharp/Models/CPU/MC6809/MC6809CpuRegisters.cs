using System;

namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public class MC6809CpuRegisters
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo

        public MC6809CpuRegister pc { get; } = new();
        public MC6809CpuRegister d { get; } = new();
        public MC6809CpuRegister x { get; } = new();
        public MC6809CpuRegister y { get; } = new();
        public MC6809CpuRegister u { get; } = new();
        public MC6809CpuRegister s { get; } = new();
        public MC6809CpuRegister dp { get; } = new();

        public byte ccbits;

        public bool[] cc = new bool[8];

        public Reg8 ureg8 { get; }
        public Reg16 xfreg16 { get; }

        public MC6809CpuRegisters()
        {
            ureg8 = new Reg8(this);
            xfreg16 = new Reg16(this);
        }

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
                    switch (index)
                    {
                        case 0: return _cpu.d.msb;
                        case 1: return _cpu.d.lsb;
                        case 2: return _cpu.ccbits;
                        case 3: return _cpu.dp.msb;
                        case 4: return _cpu.dp.msb;
                        case 5: return _cpu.dp.msb;
                        case 6: return _cpu.dp.msb;
                        case 7: return _cpu.dp.msb;
                    }

                    throw new NotImplementedException();
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
                            _cpu.ccbits = value;
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
                    switch (index)
                    {
                        case 0: return _cpu.d.Reg; // &D_REG;
                        case 1: return _cpu.x.Reg; // &X_REG;
                        case 2: return _cpu.y.Reg; // &Y_REG;
                        case 3: return _cpu.u.Reg; // &U_REG;
                        case 4: return _cpu.s.Reg; // &S_REG;
                        case 5: return _cpu.pc.Reg; // &PC_REG;
                    }

                    throw new NotImplementedException();
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
    }
}
