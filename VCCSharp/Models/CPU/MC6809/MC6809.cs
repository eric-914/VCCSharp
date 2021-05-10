﻿using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.MC6809
{
    // ReSharper disable once InconsistentNaming
    public interface IMC6809 : IProcessor
    {
        unsafe MC6809State* GetMC6809State();
    }

    // ReSharper disable once InconsistentNaming
    public partial class MC6809 : IMC6809
    {
        private readonly IModules _modules;

        //private readonly MC6809CpuRegisters _cpu = new MC6809CpuRegisters();

        private readonly unsafe MC6809State* _instance;

        private int _cycleCounter;

        private int _gCycleFor;

        public MC6809(IModules modules)
        {
            _modules = modules;

            InitializeJmpVectors();

            unsafe
            {
                _instance = GetMC6809State();
            }
        }

        public unsafe MC6809State* GetMC6809State()
        {
            return Library.MC6809.GetMC6809State();
        }

        public void Init()
        {
            unsafe
            {
                //Call this first or RESET will core!
                // reg pointers for TFR and EXG and LEA ops
                _instance->xfreg16[0] = (long)&(_instance->d.Reg); // &D_REG;
                _instance->xfreg16[1] = (long)&(_instance->x.Reg); // &X_REG;
                _instance->xfreg16[2] = (long)&(_instance->y.Reg); // &Y_REG;
                _instance->xfreg16[3] = (long)&(_instance->u.Reg); // &U_REG;
                _instance->xfreg16[4] = (long)&(_instance->s.Reg); // &S_REG;
                _instance->xfreg16[5] = (long)&(_instance->pc.Reg); // &PC_REG;

                _instance->ureg8[0] = (long)&(_instance->d.msb); //(byte*)(&A_REG);
                _instance->ureg8[1] = (long)&(_instance->d.lsb); //(byte*)(&B_REG);
                _instance->ureg8[2] = (long)&(_instance->ccbits); //(byte*)(&(instance->ccbits));
                _instance->ureg8[3] = (long)&(_instance->dp.msb); //(byte*)(&DPA);
                _instance->ureg8[4] = (long)&(_instance->dp.msb); //(byte*)(&DPA);
                _instance->ureg8[5] = (long)&(_instance->dp.msb); //(byte*)(&DPA);
                _instance->ureg8[6] = (long)&(_instance->dp.msb); //(byte*)(&DPA);
                _instance->ureg8[7] = (long)&(_instance->dp.msb); //(byte*)(&DPA);
            }
        }

        public void ForcePC(ushort address)
        {
            unsafe
            {
                _instance->pc.Reg = address;

                _instance->PendingInterrupts = 0;
                _instance->SyncWaiting = 0;
            }
        }

        public void DeAssertInterrupt(byte irq)
        {
            unsafe
            {
                _instance->PendingInterrupts &= (byte)~(1 << (irq - 1));
                _instance->InInterrupt = 0;
            }
        }

        public void AssertInterrupt(byte irq, byte flag)
        {
            unsafe
            {
                _instance->SyncWaiting = 0;
                _instance->PendingInterrupts |= (byte)(1 << (irq - 1));
                _instance->IRQWaiter = flag;
            }
        }

        public unsafe void Reset()
        {
            byte index;

            for (index = 0; index <= 5; index++)
            {
                PXF(index, 0);
            }

            for (index = 0; index <= 7; index++)
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

            DP_REG = 0;

            _instance->SyncWaiting = 0;

            PC_REG = MemRead16(Define.VRESET);	//PC gets its reset vector

            _modules.TC1014.SetMapType(0);	//shouldn't be here
        }

        public int Exec(int cycleFor)
        {
            unsafe
            {
                _cycleCounter = 0;

                while (_cycleCounter < cycleFor)
                {
                    if (_instance->PendingInterrupts != 0)
                    {
                        if ((_instance->PendingInterrupts & 4) != 0)
                        {
                            MC6809_cpu_nmi();
                        }

                        if ((_instance->PendingInterrupts & 2) != 0)
                        {
                            MC6809_cpu_firq();
                        }

                        if ((_instance->PendingInterrupts & 1) != 0)
                        {
                            if (_instance->IRQWaiter == 0)
                            {
                                // This is needed to fix a subtle timing problem
                                // It allows the CPU to see $FF03 bit 7 high before...
                                MC6809_cpu_irq();
                            }
                            else
                            {
                                // ...The IRQ is asserted.
                                _instance->IRQWaiter -= 1;
                            }
                        }
                    }

                    if (_instance->SyncWaiting == 1)
                    {
                        //Abort the run nothing happens asynchronously from the CPU
                        // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                        break;
                    }

                    _gCycleFor = cycleFor;

                    byte opCode = _modules.TC1014.MemRead8(_instance->pc.Reg++);   //PC_REG

                    Exec(opCode);
                }

                return cycleFor - _cycleCounter;
            }
        }

        public void MC6809_cpu_nmi()
        {
            unsafe
            {
                _instance->cc[(int)CCFlagMasks.E] = 1;

                _modules.TC1014.MemWrite8(_instance->pc.lsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->pc.msb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->u.lsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->u.msb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->y.lsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->y.msb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->x.lsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->x.msb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->dp.msb, --_instance->s.Reg);

                _modules.TC1014.MemWrite8(_instance->d.lsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->d.msb, --_instance->s.Reg);

                _modules.TC1014.MemWrite8(MC6809_getcc(), --_instance->s.Reg);

                _instance->cc[(int)CCFlagMasks.I] = 1;
                _instance->cc[(int)CCFlagMasks.F] = 1;

                _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VNMI);

                _instance->PendingInterrupts &= 251;
            }
        }

        public void MC6809_cpu_firq()
        {
            unsafe
            {
                if (_instance->cc[(int)CCFlagMasks.F] == 0)
                {
                    _instance->InInterrupt = 1; //Flag to indicate FIRQ has been asserted

                    _instance->cc[(int)CCFlagMasks.E] = 0; // Turn E flag off

                    _modules.TC1014.MemWrite8(_instance->pc.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->pc.msb, --_instance->s.Reg);

                    _modules.TC1014.MemWrite8(MC6809_getcc(), --_instance->s.Reg);

                    _instance->cc[(int)CCFlagMasks.I] = 1;
                    _instance->cc[(int)CCFlagMasks.F] = 1;

                    _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);
                }

                _instance->PendingInterrupts &= 253;
            }
        }

        public void MC6809_cpu_irq()
        {
            unsafe
            {
                if (_instance->InInterrupt == 1)
                {
                    //If FIRQ is running postpone the IRQ
                    return;
                }

                if (_instance->cc[(int)CCFlagMasks.I] == 0)
                {
                    _instance->cc[(int)CCFlagMasks.E] = 1;

                    _modules.TC1014.MemWrite8(_instance->pc.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->pc.msb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->u.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->u.msb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->y.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->y.msb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->x.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->x.msb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->dp.msb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->d.lsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->d.msb, --_instance->s.Reg);

                    _modules.TC1014.MemWrite8(MC6809_getcc(), --_instance->s.Reg);

                    _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VIRQ);
                    _instance->cc[(int)CCFlagMasks.I] = 1;
                }

                _instance->PendingInterrupts &= 254;
            }
        }

        public byte MC6809_getcc()
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

        public void MC6809_setcc(byte cc)
        {
            unsafe { _instance->ccbits = cc; }

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

        public ushort MC6809_CalculateEA(byte postByte)
        {
            ushort ea = 0;

            byte reg = (byte)(((postByte >> 5) & 3) + 1);

            if ((postByte & 0x80) != 0)
            {
                switch (postByte & 0x1F)
                {
                    case 0:
                        ea = PXF(reg);
                        PXF(reg, (ushort)(PXF(reg) + 1));
                        _cycleCounter += 2;
                        break;

                    case 1:
                        ea = PXF(reg);
                        PXF(reg, (ushort)(PXF(reg) + 2));
                        _cycleCounter += 3;
                        break;

                    case 2:
                        PXF(reg, (ushort)(PXF(reg) - 1));
                        ea = PXF(reg);
                        _cycleCounter += 2;
                        break;

                    case 3:
                        PXF(reg, (ushort)(PXF(reg) - 2));
                        ea = PXF(reg);
                        _cycleCounter += 3;
                        break;

                    case 4:
                        ea = PXF(reg);
                        break;

                    case 5:
                        ea = (ushort)(PXF(reg) + ((sbyte)B_REG));
                        _cycleCounter += 1;
                        break;

                    case 6:
                        ea = (ushort)(PXF(reg) + ((sbyte)A_REG));
                        _cycleCounter += 1;
                        break;

                    case 7:
                        //ea = (ushort)(PXF(reg) + ((sbyte)E_REG));
                        _cycleCounter += 1;
                        break;

                    case 8:
                        ea = (ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++));
                        _cycleCounter += 1;
                        break;

                    case 9:
                        ea = (ushort)(PXF(reg) + MemRead16(PC_REG));
                        _cycleCounter += 4;
                        PC_REG += 2;
                        break;

                    case 10:
                        //ea = (ushort)(PXF(reg) + ((sbyte)F_REG));
                        _cycleCounter += 1;
                        break;

                    case 11:
                        ea = (ushort)(PXF(reg) + D_REG);
                        _cycleCounter += 4;
                        break;

                    case 12:
                        ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);
                        _cycleCounter += 1;
                        PC_REG++;
                        break;

                    case 13: //MM
                        ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);
                        _cycleCounter += 5;
                        PC_REG += 2;
                        break;

                    case 14:
                        //ea = (ushort)(PXF(reg) + W_REG);
                        _cycleCounter += 4;
                        break;

                    case 15: //01111
                        sbyte signedByte = (sbyte)((postByte >> 5) & 3);

                        switch (signedByte)
                        {
                            case 0:
                                //ea = W_REG;
                                break;

                            case 1:
                                //ea = (ushort)(W_REG + MemRead16(PC_REG));
                                PC_REG += 2;
                                //_cycleCounter += 2;
                                break;

                            case 2:
                                //ea = W_REG;
                                break;

                            case 3:
                                //W_REG -= 2;
                                break;
                        }
                        break;

                    case 16: //10000
                        signedByte = (sbyte)((postByte >> 5) & 3);

                        switch (signedByte)
                        {
                            case 0:
                                //ea = MemRead16(W_REG);
                                break;

                            case 1:
                                //ea = MemRead16((ushort)(W_REG + MemRead16(PC_REG)));
                                PC_REG += 2;
                                //_cycleCounter += 5;
                                break;

                            case 2:
                                //ea = MemRead16(W_REG);
                                break;

                            case 3:
                                //W_REG -= 2;
                                break;
                        }
                        break;

                    case 17: //10001
                        ea = PXF(reg);
                        PXF(reg, (ushort)(PXF(reg) + 2));
                        ea = MemRead16(ea);
                        _cycleCounter += 6;
                        break;

                    case 18: //10010
                        _cycleCounter += 6;
                        break;

                    case 19: //10011
                        PXF(reg, (ushort)(PXF(reg) - 2));
                        ea = MemRead16(PXF(reg));
                        _cycleCounter += 6;
                        break;

                    case 20: //10100
                        ea = MemRead16(PXF(reg));
                        _cycleCounter += 3;
                        break;

                    case 21: //10101
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)B_REG)));
                        _cycleCounter += 4;
                        break;

                    case 22: //10110
                        ea = MemRead16((ushort)(PXF(reg) + ((sbyte)A_REG)));
                        _cycleCounter += 4;
                        break;

                    case 23: //10111
                        //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)E_REG)));
                        ea = MemRead16(ea);
                        _cycleCounter += 4;
                        break;

                    case 24: //11000
                        ea = MemRead16((ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++)));
                        _cycleCounter += 4;
                        break;

                    case 25: //11001
                        ea = MemRead16((ushort)(PXF(reg) + MemRead16(PC_REG)));
                        _cycleCounter += 7;
                        PC_REG += 2;
                        break;

                    case 26: //11010
                        //ea = MemRead16((ushort)(PXF(reg) + ((sbyte)F_REG)));
                        ea = MemRead16(ea);
                        _cycleCounter += 4;
                        break;

                    case 27: //11011
                        ea = MemRead16((ushort)(PXF(reg) + D_REG));
                        _cycleCounter += 7;
                        break;

                    case 28: //11100
                        ea = MemRead16((ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1));
                        _cycleCounter += 4;
                        PC_REG++;
                        break;

                    case 29: //11101
                        ea = MemRead16((ushort)(PC_REG + MemRead16(PC_REG) + 2));
                        _cycleCounter += 8;
                        PC_REG += 2;
                        break;

                    case 30: //11110
                        //ea = MemRead16((ushort)(PXF(reg) + W_REG));
                        ea = MemRead16(ea);
                        _cycleCounter += 7;
                        break;

                    case 31: //11111
                        ea = MemRead16(MemRead16(PC_REG));
                        _cycleCounter += 8;
                        PC_REG += 2;
                        break;
                }
            }
            else
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