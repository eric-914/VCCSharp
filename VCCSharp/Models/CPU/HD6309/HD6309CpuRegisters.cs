using System;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    public class HD6309CpuRegisters
    {
        // ReSharper disable InconsistentNaming
        // ReSharper disable IdentifierTypo

        public HD6309CpuRegister pc { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister x { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister y { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister u { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister s { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister dp { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister v { get; } = new HD6309CpuRegister();
        public HD6309CpuRegister z { get; } = new HD6309CpuRegister();

        public HD6309WideRegister q { get; } = new HD6309WideRegister();

        public byte ccbits;
        public byte mdbits;

        public byte[] cc = new byte[8];
        public uint[] md = new uint[8];

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
                    switch (index)
                    {
                        case 0: return _cpu.q.lswmsb;
                        case 1: return _cpu.q.lswlsb;
                        case 2: return _cpu.ccbits;
                        case 3: return _cpu.dp.msb;
                        case 4: return _cpu.z.msb;
                        case 5: return _cpu.z.lsb;
                        case 6: return _cpu.q.mswmsb;
                        case 7: return _cpu.q.mswlsb;
                    }

                    throw new NotImplementedException();
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
                    switch (index)
                    {
                        case 0: return _cpu.q.lsw;
                        case 1: return _cpu.x.Reg;
                        case 2: return _cpu.y.Reg;
                        case 3: return _cpu.u.Reg;
                        case 4: return _cpu.s.Reg;
                        case 5: return _cpu.pc.Reg;
                        case 6: return _cpu.q.msw;
                        case 7: return _cpu.v.Reg;
                    }

                    throw new NotImplementedException();
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
}
