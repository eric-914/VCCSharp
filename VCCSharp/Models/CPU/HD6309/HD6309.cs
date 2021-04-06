using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.HD6309
{
    // ReSharper disable once InconsistentNaming
    public interface IHD6309 : IProcessor { }

    // ReSharper disable once InconsistentNaming
    public partial class HD6309 : IHD6309
    {
        private readonly IModules _modules;

        private readonly unsafe HD6309State* _instance;
        private readonly HD6309CpuRegisters _cpu = new HD6309CpuRegisters();

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

            unsafe
            {
                _instance = Library.HD6309.GetHD6309State();
            }
        }

        public void Init()
        {
            unsafe
            {
                //Call this first or RESET will core!
                // reg pointers for TFR and EXG and LEA ops
                //_cpu.xfreg16[0] = (long)&(_cpu.q.lsw); //&D_REG;
                //_cpu.xfreg16[1] = (long)&(_cpu.x.Reg); //&X_REG;
                //_cpu.xfreg16[2] = (long)&(_cpu.y.Reg); //&Y_REG;
                //_cpu.xfreg16[3] = (long)&(_cpu.u.Reg); //&U_REG;
                //_cpu.xfreg16[4] = (long)&(_cpu.s.Reg); //&S_REG;
                //_cpu.xfreg16[5] = (long)&(_cpu.pc.Reg); //&PC_REG;
                //_cpu.xfreg16[6] = (long)&(_cpu.q.msw); //&W_REG;
                //_cpu.xfreg16[7] = (long)&(_cpu->v.Reg); //&V_REG;

                //_cpu.ureg8[0] = (long)&(_cpu.q.lswmsb); //(byte*)&A_REG;
                //_cpu.ureg8[1] = (long)&(_cpu.q.lswlsb); //(byte*)&B_REG;
                //_cpu.ureg8[2] = (long)&(_cpu.ccbits);//(byte*)&(instance->ccbits);
                //_cpu.ureg8[3] = (long)&(_cpu.dp.msb);//(byte*)&DPA;
                //_cpu.ureg8[4] = (long)&(_cpu->z.msb);//(byte*)&Z_H;
                //_cpu.ureg8[5] = (long)&(_cpu->z.lsb);//(byte*)&Z_L;
                //_cpu.ureg8[6] = (long)&(_cpu.q.mswmsb);//(byte*)&E_REG;
                //_cpu.ureg8[7] = (long)&(_cpu.q.mswlsb);//(byte*)&F_REG;

                _instance->NatEmuCycles[0] = (long)&(_instance->NatEmuCycles65);
                _instance->NatEmuCycles[1] = (long)&(_instance->NatEmuCycles64);
                _instance->NatEmuCycles[2] = (long)&(_instance->NatEmuCycles32);
                _instance->NatEmuCycles[3] = (long)&(_instance->NatEmuCycles21);
                _instance->NatEmuCycles[4] = (long)&(_instance->NatEmuCycles54);
                _instance->NatEmuCycles[5] = (long)&(_instance->NatEmuCycles97);
                _instance->NatEmuCycles[6] = (long)&(_instance->NatEmuCycles85);
                _instance->NatEmuCycles[7] = (long)&(_instance->NatEmuCycles51);
                _instance->NatEmuCycles[8] = (long)&(_instance->NatEmuCycles31);
                _instance->NatEmuCycles[9] = (long)&(_instance->NatEmuCycles1110);
                _instance->NatEmuCycles[10] = (long)&(_instance->NatEmuCycles76);
                _instance->NatEmuCycles[11] = (long)&(_instance->NatEmuCycles75);
                _instance->NatEmuCycles[12] = (long)&(_instance->NatEmuCycles43);
                _instance->NatEmuCycles[13] = (long)&(_instance->NatEmuCycles87);
                _instance->NatEmuCycles[14] = (long)&(_instance->NatEmuCycles86);
                _instance->NatEmuCycles[15] = (long)&(_instance->NatEmuCycles98);
                _instance->NatEmuCycles[16] = (long)&(_instance->NatEmuCycles2726);
                _instance->NatEmuCycles[17] = (long)&(_instance->NatEmuCycles3635);
                _instance->NatEmuCycles[18] = (long)&(_instance->NatEmuCycles3029);
                _instance->NatEmuCycles[19] = (long)&(_instance->NatEmuCycles2827);
                _instance->NatEmuCycles[20] = (long)&(_instance->NatEmuCycles3726);
                _instance->NatEmuCycles[21] = (long)&(_instance->NatEmuCycles3130);
                _instance->NatEmuCycles[22] = (long)&(_instance->NatEmuCycles42);
                _instance->NatEmuCycles[23] = (long)&(_instance->NatEmuCycles53);

                //This handles the disparity between 6309 and 6809 Instruction timing
                _instance->InsCycles[0 * 25 + Define.M65] = 6;    //6-5
                _instance->InsCycles[1 * 25 + Define.M65] = 5;
                _instance->InsCycles[0 * 25 + Define.M64] = 6;    //6-4
                _instance->InsCycles[1 * 25 + Define.M64] = 4;
                _instance->InsCycles[0 * 25 + Define.M32] = 3;    //3-2
                _instance->InsCycles[1 * 25 + Define.M32] = 2;
                _instance->InsCycles[0 * 25 + Define.M21] = 2;    //2-1
                _instance->InsCycles[1 * 25 + Define.M21] = 1;
                _instance->InsCycles[0 * 25 + Define.M54] = 5;    //5-4
                _instance->InsCycles[1 * 25 + Define.M54] = 4;
                _instance->InsCycles[0 * 25 + Define.M97] = 9;    //9-7
                _instance->InsCycles[1 * 25 + Define.M97] = 7;
                _instance->InsCycles[0 * 25 + Define.M85] = 8;    //8-5
                _instance->InsCycles[1 * 25 + Define.M85] = 5;
                _instance->InsCycles[0 * 25 + Define.M51] = 5;    //5-1
                _instance->InsCycles[1 * 25 + Define.M51] = 1;
                _instance->InsCycles[0 * 25 + Define.M31] = 3;    //3-1
                _instance->InsCycles[1 * 25 + Define.M31] = 1;
                _instance->InsCycles[0 * 25 + Define.M1110] = 11; //11-10
                _instance->InsCycles[1 * 25 + Define.M1110] = 10;
                _instance->InsCycles[0 * 25 + Define.M76] = 7;    //7-6
                _instance->InsCycles[1 * 25 + Define.M76] = 6;
                _instance->InsCycles[0 * 25 + Define.M75] = 7;    //7-5
                _instance->InsCycles[1 * 25 + Define.M75] = 5;
                _instance->InsCycles[0 * 25 + Define.M43] = 4;    //4-3
                _instance->InsCycles[1 * 25 + Define.M43] = 3;
                _instance->InsCycles[0 * 25 + Define.M87] = 8;    //8-7
                _instance->InsCycles[1 * 25 + Define.M87] = 7;
                _instance->InsCycles[0 * 25 + Define.M86] = 8;    //8-6
                _instance->InsCycles[1 * 25 + Define.M86] = 6;
                _instance->InsCycles[0 * 25 + Define.M98] = 9;    //9-8
                _instance->InsCycles[1 * 25 + Define.M98] = 8;
                _instance->InsCycles[0 * 25 + Define.M2726] = 27; //27-26
                _instance->InsCycles[1 * 25 + Define.M2726] = 26;
                _instance->InsCycles[0 * 25 + Define.M3635] = 36; //36-25
                _instance->InsCycles[1 * 25 + Define.M3635] = 35;
                _instance->InsCycles[0 * 25 + Define.M3029] = 30; //30-29
                _instance->InsCycles[1 * 25 + Define.M3029] = 29;
                _instance->InsCycles[0 * 25 + Define.M2827] = 28; //28-27
                _instance->InsCycles[1 * 25 + Define.M2827] = 27;
                _instance->InsCycles[0 * 25 + Define.M3726] = 37; //37-26
                _instance->InsCycles[1 * 25 + Define.M3726] = 26;
                _instance->InsCycles[0 * 25 + Define.M3130] = 31; //31-30
                _instance->InsCycles[1 * 25 + Define.M3130] = 30;
                _instance->InsCycles[0 * 25 + Define.M42] = 4;    //4-2
                _instance->InsCycles[1 * 25 + Define.M42] = 2;
                _instance->InsCycles[0 * 25 + Define.M53] = 5;    //5-3
                _instance->InsCycles[1 * 25 + Define.M53] = 3;
            }
        }

        public void ForcePC(ushort address)
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
            {		//Set all register to 0 except V
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

            _cpu.mdbits = getmd();

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
                        cpu_nmi();
                    }

                    if ((_pendingInterrupts & 2) != 0)
                    {
                        cpu_firq();
                    }

                    if ((_pendingInterrupts & 1) != 0)
                    {
                        if (_irqWaiter == 0)
                        {
                            // This is needed to fix a subtle timing problem
                            // It allows the CPU to see $FF03 bit 7 high before...
                            cpu_irq();
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
                    break; // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                }

                _gCycleFor = cycleFor;

                byte opCode = _modules.TC1014.MemRead8(_cpu.pc.Reg++); //PC_REG

                Exec(opCode);
            }

            return cycleFor - _cycleCounter;
        }

        public void cpu_nmi()
        {
            _cpu.cc[(int)CCFlagMasks.E] = 1;

            _modules.TC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

            if (_cpu.md[(int)MDFlagMasks.NATIVE6309] != 0)
            {
                _modules.TC1014.MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
            }

            _modules.TC1014.MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
            _modules.TC1014.MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

            _modules.TC1014.MemWrite8(getcc(), --_cpu.s.Reg);

            _cpu.cc[(int)CCFlagMasks.I] = 1;
            _cpu.cc[(int)CCFlagMasks.F] = 1;

            _cpu.pc.Reg = _modules.TC1014.MemRead16(Define.VNMI);

            _pendingInterrupts &= 251;
        }

        public void cpu_firq()
        {
            if (_cpu.cc[(int)CCFlagMasks.F] == 0)
            {
                _inInterrupt = 1; //Flag to indicate FIRQ has been asserted

                switch (_cpu.md[(int)MDFlagMasks.FIRQMODE])
                {
                    case 0:
                        _cpu.cc[(int)CCFlagMasks.E] = 0; // Turn E flag off

                        _modules.TC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);

                        _modules.TC1014.MemWrite8(getcc(), --_cpu.s.Reg);

                        _cpu.cc[(int)CCFlagMasks.I] = 1;
                        _cpu.cc[(int)CCFlagMasks.F] = 1;

                        _cpu.pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                        break;

                    case 1:		//6309
                        _cpu.cc[(int)CCFlagMasks.E] = 1;

                        _modules.TC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

                        if (_cpu.md[(int)MDFlagMasks.NATIVE6309] != 0)
                        {
                            _modules.TC1014.MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                            _modules.TC1014.MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
                        }

                        _modules.TC1014.MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
                        _modules.TC1014.MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

                        _modules.TC1014.MemWrite8(getcc(), --_cpu.s.Reg);

                        _cpu.cc[(int)CCFlagMasks.I] = 1;
                        _cpu.cc[(int)CCFlagMasks.F] = 1;

                        _cpu.pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                        break;
                }
            }

            _pendingInterrupts &= 253;
        }

        public void cpu_irq()
        {
            if (_inInterrupt == 1)
            {
                //If FIRQ is running postpone the IRQ
                return;
            }

            if (_cpu.cc[(int)CCFlagMasks.I] == 0)
            {
                _cpu.cc[(int)CCFlagMasks.E] = 1;

                _modules.TC1014.MemWrite8(_cpu.pc.lsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.pc.msb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.u.lsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.u.msb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.y.lsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.y.msb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.x.lsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.x.msb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.dp.msb, --_cpu.s.Reg);

                if (_cpu.md[(int)MDFlagMasks.NATIVE6309] != 0)
                {
                    _modules.TC1014.MemWrite8(_cpu.q.mswlsb, --_cpu.s.Reg);
                    _modules.TC1014.MemWrite8(_cpu.q.mswmsb, --_cpu.s.Reg);
                }

                _modules.TC1014.MemWrite8(_cpu.q.lswlsb, --_cpu.s.Reg);
                _modules.TC1014.MemWrite8(_cpu.q.lswmsb, --_cpu.s.Reg);

                _modules.TC1014.MemWrite8(getcc(), --_cpu.s.Reg);

                _cpu.cc[(int)CCFlagMasks.I] = 1;

                _cpu.pc.Reg = _modules.TC1014.MemRead16(Define.VIRQ);
            }

            _pendingInterrupts &= 254;
        }

        public byte getcc()
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

        public void setcc(byte cc)
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

        public byte getmd()
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

        public void setmd(byte md)
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

            unsafe
            {
                for (int i = 0; i < 24; i++)
                {
                    uint insCyclesIndex = _cpu.md[(int)MDFlagMasks.NATIVE6309];
                    byte value = _instance->InsCycles[insCyclesIndex * 25 + i];

                    byte* cycle = (byte*)(_instance->NatEmuCycles[i]);

                    *cycle = value;
                }
            }
        }

        public ushort CalculateEA(byte postByte)
        {
            unsafe
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

                            _cycleCounter += _instance->NatEmuCycles21;

                            break;

                        case 1: // post in by 2
                            ea = PXF(reg);

                            PXF(reg, (ushort)(PXF(reg) + 2));

                            _cycleCounter += _instance->NatEmuCycles32;

                            break;

                        case 2: // pre dec by 1
                            PXF(reg, (ushort)(PXF(reg) - 1));

                            ea = PXF(reg);

                            _cycleCounter += _instance->NatEmuCycles21;

                            break;

                        case 3: // pre dec by 2
                            PXF(reg, (ushort)(PXF(reg) - 2));

                            ea = PXF(reg);

                            _cycleCounter += _instance->NatEmuCycles32;

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

                            _cycleCounter += _instance->NatEmuCycles43;

                            PC_REG += 2;

                            break;

                        case 10: // F reg offset
                            ea = (ushort)(PXF(reg) + ((sbyte)F_REG));

                            _cycleCounter += 1;

                            break;

                        case 11: // D reg offset 
                            ea = (ushort)(PXF(reg) + D_REG);

                            _cycleCounter += _instance->NatEmuCycles42;

                            break;

                        case 12: // 8 bit PC relative
                            ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);

                            _cycleCounter += 1;

                            PC_REG++;

                            break;

                        case 13: // 16 bit PC relative
                            ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);

                            _cycleCounter += _instance->NatEmuCycles53;

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
}
