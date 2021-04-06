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

        // ReSharper restore IdentifierTypo
        // ReSharper restore InconsistentNaming

        //_cpu->ureg8[0] = (long)&(_cpu->q.lswmsb); //(byte*)&A_REG;
        //_cpu->ureg8[1] = (long)&(_cpu->q.lswlsb); //(byte*)&B_REG;
        //_cpu->ureg8[2] = (long)&(_cpu->ccbits);//(byte*)&(instance->ccbits);
        //_cpu->ureg8[3] = (long)&(_cpu->dp.msb);//(byte*)&DPA;
        //_cpu->ureg8[4] = (long)&(_cpu->z.msb);//(byte*)&Z_H;
        //_cpu->ureg8[5] = (long)&(_cpu->z.lsb);//(byte*)&Z_L;
        //_cpu->ureg8[6] = (long)&(_cpu->q.mswmsb);//(byte*)&E_REG;
        //_cpu->ureg8[7] = (long)&(_cpu->q.mswlsb);//(byte*)&F_REG;

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

        //_cpu->xfreg16[0] = (long)&(_cpu->q.lsw); //&D_REG;
        //_cpu->xfreg16[1] = (long)&(_cpu->x.Reg); //&X_REG;
        //_cpu->xfreg16[2] = (long)&(_cpu->y.Reg); //&Y_REG;
        //_cpu->xfreg16[3] = (long)&(_cpu->u.Reg); //&U_REG;
        //_cpu->xfreg16[4] = (long)&(_cpu->s.Reg); //&S_REG;
        //_cpu->xfreg16[5] = (long)&(_cpu->pc.Reg); //&PC_REG;
        //_cpu->xfreg16[6] = (long)&(_cpu->q.msw); //&W_REG;
        //_cpu->xfreg16[7] = (long)&(_cpu->v.Reg); //&V_REG;

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
