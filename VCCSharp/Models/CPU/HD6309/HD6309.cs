using VCCSharp.Enums;
using VCCSharp.IoC;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    public interface IHD6309 : IProcessor { }

    // ReSharper disable once InconsistentNaming
    public partial class HD6309 : IHD6309
    {
        private readonly IModules _modules;

        private readonly HD6309CpuRegisters _cpu = new();
        private readonly HD6309NatEmuCycles _instance = new();

        private byte _inInterrupt;
        private int _cycleCounter;
        private uint _syncWaiting;
        private int _gCycleFor;

        //--Interrupt states
        private byte _irqWaiter;
        private byte _pendingInterrupts;

        public HD6309(IModules modules)
        {
            _modules = modules;

            InitializeJmpVectors();
        }

        public void Init()
        {
            //--The classes initialize themselves now.
        }

        public void ForcePc(ushort address)
        {
            _cpu.pc.Reg = address;

            _pendingInterrupts = 0;
            _syncWaiting = 0;
        }

        public void DeAssertInterrupt(byte irq)
        {
            _pendingInterrupts &= (byte)~(1 << (irq - 1));
            _inInterrupt = 0;
        }

        public void AssertInterrupt(byte irq, byte flag)
        {
            _syncWaiting = 0;
            _pendingInterrupts |= (byte)(1 << (irq - 1));
            _irqWaiter = flag;
        }

        public void Reset()
        {
            for (byte index = 0; index <= 6; index++)
            {
                //Set all register to 0 except V
                PXF(index, 0);
            }

            for (byte index = 0; index <= 7; index++)
            {
                PUR(index, 0);
            }

            CC_E = false;
            CC_F = true;
            CC_H = false;
            CC_I = true;
            CC_N = false;
            CC_Z = false;
            CC_V = false;
            CC_C = false;

            MD_NATIVE6309 = false;
            MD_FIRQMODE = false;
            MD_UNDEFINED2 = false;  //UNDEFINED
            MD_UNDEFINED3 = false;  //UNDEFINED
            MD_UNDEFINED4 = false;  //UNDEFINED
            MD_UNDEFINED5 = false;  //UNDEFINED
            MD_ILLEGAL = false;
            MD_ZERODIV = false;

            _cpu.mdbits = GetMD();

            _syncWaiting = 0;

            DP_REG = 0;
            PC_REG = MemRead16(Define.VRESET);	//PC gets its reset vector

            _modules.TC1014.SetMapType(0);	//shouldn't be here
        }

        public int Exec(int cycleFor)
        {
            _cycleCounter = 0;

            while (_cycleCounter < cycleFor)
            {
                if (_pendingInterrupts != 0)
                {
                    if ((_pendingInterrupts & 4) != 0)
                    {
                        Cpu_Nmi();
                    }

                    if ((_pendingInterrupts & 2) != 0)
                    {
                        Cpu_Firq();
                    }

                    if ((_pendingInterrupts & 1) != 0)
                    {
                        if (_irqWaiter == 0)
                        {
                            // This is needed to fix a subtle timing problem
                            // It allows the CPU to see $FF03 bit 7 high before...
                            Cpu_Irq();
                        }
                        else
                        {
                            // ...The IRQ is asserted.
                            _irqWaiter -= 1;
                        }
                    }
                }

                if (_syncWaiting == 1)
                {
                    //Abort the run nothing happens asynchronously from the CPU
                    // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                    break;
                }

                _gCycleFor = cycleFor;

                byte opCode = _modules.TC1014.MemRead8(_cpu.pc.Reg++); //PC_REG

                Exec(opCode);
            }

            return cycleFor - _cycleCounter;
        }

        public void Cpu_Nmi()
        {
            _cpu.cc[(int)CCFlagMasks.E] = true;

            MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
            MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
            MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
            MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
            MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
            MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
            MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
            MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
            MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

            if (MD_NATIVE6309)
            {
                MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
            }

            MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
            MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

            MemWrite8(GetCC(), --_cpu.s.Reg);

            _cpu.cc[(int)CCFlagMasks.I] = true;
            _cpu.cc[(int)CCFlagMasks.F] = true;

            _cpu.pc.Reg = MemRead16(Define.VNMI);

            _pendingInterrupts &= 251;
        }

        public void Cpu_Firq()
        {
            if (!_cpu.cc[(int)CCFlagMasks.F])
            {
                _inInterrupt = 1; //Flag to indicate FIRQ has been asserted

                switch (MD_FIRQMODE)
                {
                    case false:
                        _cpu.cc[(int)CCFlagMasks.E] = false; // Turn E flag off

                        MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);

                        MemWrite8(GetCC(), --_cpu.s.Reg);

                        _cpu.cc[(int)CCFlagMasks.I] = true;
                        _cpu.cc[(int)CCFlagMasks.F] = true;

                        _cpu.pc.Reg = MemRead16(Define.VFIRQ);

                        break;

                    case true:		//6309
                        _cpu.cc[(int)CCFlagMasks.E] = true;

                        MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
                        MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
                        MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
                        MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
                        MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

                        if (MD_NATIVE6309)
                        {
                            MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                            MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
                        }

                        MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
                        MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

                        MemWrite8(GetCC(), --_cpu.s.Reg);

                        _cpu.cc[(int)CCFlagMasks.I] = true;
                        _cpu.cc[(int)CCFlagMasks.F] = true;

                        _cpu.pc.Reg = MemRead16(Define.VFIRQ);

                        break;
                }
            }

            _pendingInterrupts &= 253;
        }

        public void Cpu_Irq()
        {
            if (_inInterrupt == 1)
            {
                //If FIRQ is running postpone the IRQ
                return;
            }

            if (!_cpu.cc[(int)CCFlagMasks.I])
            {
                _cpu.cc[(int)CCFlagMasks.E] = true;

                MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
                MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
                MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
                MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
                MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
                MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
                MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
                MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

                if (MD_NATIVE6309)
                {
                    MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                    MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
                }

                MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
                MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

                MemWrite8(GetCC(), --_cpu.s.Reg);

                _cpu.cc[(int)CCFlagMasks.I] = true;

                _cpu.pc.Reg = MemRead16(Define.VIRQ);
            }

            _pendingInterrupts &= 254;
        }

        public byte GetCC()
        {
            int cc = 0;

            void Test(bool value, CCFlagMasks mask)
            {
                if (value) { cc |= (1 << (int)mask); }
            }

            Test(CC_E, CCFlagMasks.E);
            Test(CC_F, CCFlagMasks.F);
            Test(CC_H, CCFlagMasks.H);
            Test(CC_I, CCFlagMasks.I);
            Test(CC_N, CCFlagMasks.N);
            Test(CC_Z, CCFlagMasks.Z);
            Test(CC_V, CCFlagMasks.V);
            Test(CC_C, CCFlagMasks.C);

            return (byte)cc;
        }

        public void SetCC(byte cc)
        {
            _cpu.ccbits = cc;

            bool Test(CCFlagMasks mask)
            {
                return (cc & (1 << (int)mask)) != 0;
            }

            CC_E = Test(CCFlagMasks.E);
            CC_F = Test(CCFlagMasks.F);
            CC_H = Test(CCFlagMasks.H);
            CC_I = Test(CCFlagMasks.I);
            CC_N = Test(CCFlagMasks.N);
            CC_Z = Test(CCFlagMasks.Z);
            CC_V = Test(CCFlagMasks.V);
            CC_C = Test(CCFlagMasks.C);
        }

        public byte GetMD()
        {
            int md = 0;

            void Test(bool value, MDFlagMasks mask)
            {
                if (value) { md |= (1 << (int)mask); }
            }

            Test(MD_NATIVE6309, MDFlagMasks.NATIVE6309);
            Test(MD_FIRQMODE, MDFlagMasks.FIRQMODE);
            Test(MD_UNDEFINED2, MDFlagMasks.MD_UNDEF2);
            Test(MD_UNDEFINED3, MDFlagMasks.MD_UNDEF3);
            Test(MD_UNDEFINED4, MDFlagMasks.MD_UNDEF4);
            Test(MD_UNDEFINED5, MDFlagMasks.MD_UNDEF5);
            Test(MD_ILLEGAL, MDFlagMasks.ILLEGAL);
            Test(MD_ZERODIV, MDFlagMasks.ZERODIV);

            return (byte)md;
        }

        public void SetMD(byte md)
        {
            bool Test(MDFlagMasks mask)
            {
                return (md & (1 << (int)mask)) != 0;
            }

            MD_NATIVE6309 = Test(MDFlagMasks.NATIVE6309);
            MD_FIRQMODE = Test(MDFlagMasks.FIRQMODE);
            MD_UNDEFINED2 = Test(MDFlagMasks.MD_UNDEF2);
            MD_UNDEFINED3 = Test(MDFlagMasks.MD_UNDEF3);
            MD_UNDEFINED4 = Test(MDFlagMasks.MD_UNDEF4);
            MD_UNDEFINED5 = Test(MDFlagMasks.MD_UNDEF5);
            MD_ILLEGAL = Test(MDFlagMasks.ILLEGAL);
            MD_ZERODIV = Test(MDFlagMasks.ZERODIV);

            for (int i = 0; i < 24; i++)
            {
                var insCyclesIndex = MD_NATIVE6309 ? 1 : 0;
                _instance[i] = _instance.InsCycles[insCyclesIndex, i];
            }
        }

        public ushort CalculateEA(byte postByte)
        {
            ushort ea = 0;

            byte reg = (byte)(((postByte >> 5) & 3) + 1);

            if ((postByte & 0x80) != 0)
            {
                switch (postByte & 0x1F)
                {
                    case 0: // Post inc by 1
                        ea = PXF(reg);

                        PXF(reg, (ushort)(PXF(reg) + 1));

                        _cycleCounter += _instance._21;

                        break;

                    case 1: // post in by 2
                        ea = PXF(reg);

                        PXF(reg, (ushort)(PXF(reg) + 2));

                        _cycleCounter += _instance._32;

                        break;

                    case 2: // pre dec by 1
                        PXF(reg, (ushort)(PXF(reg) - 1));

                        ea = PXF(reg);

                        _cycleCounter += _instance._21;

                        break;

                    case 3: // pre dec by 2
                        PXF(reg, (ushort)(PXF(reg) - 2));

                        ea = PXF(reg);

                        _cycleCounter += _instance._32;

                        break;

                    case 4: // no offset
                        ea = PXF(reg);

                        break;

                    case 5: // B reg offset
                        ea = (ushort)(PXF(reg) + ((sbyte)B_REG));

                        _cycleCounter += 1;

                        break;

                    case 6: // A reg offset
                        ea = (ushort)(PXF(reg) + ((sbyte)A_REG));

                        _cycleCounter += 1;

                        break;

                    case 7: // E reg offset 
                        ea = (ushort)(PXF(reg) + ((sbyte)E_REG));

                        _cycleCounter += 1;

                        break;

                    case 8: // 8 bit offset
                        ea = (ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++));

                        _cycleCounter += 1;

                        break;

                    case 9: // 16 bit offset
                        ea = (ushort)(PXF(reg) + MemRead16(PC_REG));

                        _cycleCounter += _instance._43;

                        PC_REG += 2;

                        break;

                    case 10: // F reg offset
                        ea = (ushort)(PXF(reg) + ((sbyte)F_REG));

                        _cycleCounter += 1;

                        break;

                    case 11: // D reg offset 
                        ea = (ushort)(PXF(reg) + D_REG);

                        _cycleCounter += _instance._42;

                        break;

                    case 12: // 8 bit PC relative
                        ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);

                        _cycleCounter += 1;

                        PC_REG++;

                        break;

                    case 13: // 16 bit PC relative
                        ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);

                        _cycleCounter += _instance._53;

                        PC_REG += 2;

                        break;

                    case 14: // W reg offset
                        ea = (ushort)(PXF(reg) + W_REG);

                        _cycleCounter += 4;

                        break;

                    case 15: // W reg
                        sbyte signedByte = (sbyte)((postByte >> 5) & 3);

                        switch (signedByte)
                        {
                            case 0: // No offset from W reg
                                ea = W_REG;

                                break;

                            case 1: // 16 bit offset from W reg
                                ea = (ushort)(W_REG + MemRead16(PC_REG));

                                PC_REG += 2;

                                _cycleCounter += 2;

                                break;

                            case 2: // Post inc by 2 from W reg
                                ea = W_REG;

                                W_REG += 2;

                                _cycleCounter += 1;

                                break;

                            case 3: // Pre dec by 2 from W reg
                                W_REG -= 2;

                                ea = W_REG;

                                _cycleCounter += 1;

                                break;
                        }

                        break;

                    case 16: // W reg
                        signedByte = (sbyte)((postByte >> 5) & 3);

                        switch (signedByte)
                        {
                            case 0: // Indirect no offset from W reg
                                ea = MemRead16(W_REG);

                                _cycleCounter += 3;

                                break;

                            case 1: // Indirect 16 bit offset from W reg
                                ea = MemRead16((ushort)(W_REG + MemRead16(PC_REG)));

                                PC_REG += 2;

                                _cycleCounter += 5;

                                break;

                            case 2: // Indirect post inc by 2 from W reg
                                ea = MemRead16(W_REG);

                                W_REG += 2;

                                _cycleCounter += 4;

                                break;

                            case 3: // Indirect pre dec by 2 from W reg
                                W_REG -= 2;

                                ea = MemRead16(W_REG);

                                _cycleCounter += 4;

                                break;
                        }

                        break;

                    case 17: // Indirect post inc by 2 
                        ea = PXF(reg);

                        PXF(reg, (ushort)(PXF(reg) + 2));

                        ea = MemRead16(ea);

                        _cycleCounter += 6;

                        break;

                    case 18: // possibly illegal instruction
                        _cycleCounter += 6;

                        break;

                    case 19: // Indirect pre dec by 2
                        PXF(reg, (ushort)(PXF(reg) - 2));

                        ea = MemRead16(PXF(reg));

                        _cycleCounter += 6;

                        break;

                    case 20: // Indirect no offset 
                        ea = MemRead16(PXF(reg));

                        _cycleCounter += 3;

                        break;

                    case 21: // Indirect B reg offset
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)B_REG)));

                        _cycleCounter += 4;

                        break;

                    case 22: // indirect A reg offset
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)A_REG)));

                        _cycleCounter += 4;

                        break;

                    case 23: // indirect E reg offset
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)E_REG)));

                        _cycleCounter += 4;

                        break;

                    case 24: // indirect 8 bit offset
                        ea = MemRead16((ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++)));

                        _cycleCounter += 4;

                        break;

                    case 25: // indirect 16 bit offset
                        ea = MemRead16((ushort)(PXF(reg) + MemRead16(PC_REG)));

                        _cycleCounter += 7;

                        PC_REG += 2;

                        break;

                    case 26: // indirect F reg offset
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)F_REG)));

                        _cycleCounter += 4;

                        break;

                    case 27: // indirect D reg offset
                        ea = MemRead16((ushort)(PXF(reg) + D_REG));

                        _cycleCounter += 7;

                        break;

                    case 28: // indirect 8 bit PC relative
                        ea = MemRead16((ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1));

                        _cycleCounter += 4;

                        PC_REG++;

                        break;

                    case 29: //indirect 16 bit PC relative
                        ea = MemRead16((ushort)(PC_REG + MemRead16(PC_REG) + 2));

                        _cycleCounter += 8;

                        PC_REG += 2;

                        break;

                    case 30: // indirect W reg offset
                        ea = MemRead16((ushort)(PXF(reg) + W_REG));

                        _cycleCounter += 7;

                        break;

                    case 31: // extended indirect
                        ea = MemRead16(MemRead16(PC_REG));

                        _cycleCounter += 8;

                        PC_REG += 2;

                        break;
                }
            }
            else // 5 bit offset
            {
                sbyte signedByte = (sbyte)(postByte & 0x1F);
                signedByte <<= 3; //--Push the "sign" to the left-most bit.
                signedByte /= 8;

                ea = (ushort)(PXF(reg) + signedByte); //Was signed

                _cycleCounter += 1;
            }

            return ea;
        }
    }
}
