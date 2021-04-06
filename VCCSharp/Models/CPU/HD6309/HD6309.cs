using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;

namespace VCCSharp.Models.CPU.HD6309
{
    public interface IHD6309 : IProcessor
    {
    }

    public partial class HD6309 : IHD6309
    {
        private readonly IModules _modules;

        private readonly unsafe HD6309State* _instance;

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
                _instance->xfreg16[0] = (long)&(_instance->q.lsw); //&D_REG;
                _instance->xfreg16[1] = (long)&(_instance->x.Reg); //&X_REG;
                _instance->xfreg16[2] = (long)&(_instance->y.Reg); //&Y_REG;
                _instance->xfreg16[3] = (long)&(_instance->u.Reg); //&U_REG;
                _instance->xfreg16[4] = (long)&(_instance->s.Reg); //&S_REG;
                _instance->xfreg16[5] = (long)&(_instance->pc.Reg); //&PC_REG;
                _instance->xfreg16[6] = (long)&(_instance->q.msw); //&W_REG;
                _instance->xfreg16[7] = (long)&(_instance->v.Reg); //&V_REG;

                _instance->ureg8[0] = (long)&(_instance->q.lswmsb); //(byte*)&A_REG;
                _instance->ureg8[1] = (long)&(_instance->q.lswlsb); //(byte*)&B_REG;
                _instance->ureg8[2] = (long)&(_instance->ccbits);//(byte*)&(instance->ccbits);
                _instance->ureg8[3] = (long)&(_instance->dp.msb);//(byte*)&DPA;
                _instance->ureg8[4] = (long)&(_instance->z.msb);//(byte*)&Z_H;
                _instance->ureg8[5] = (long)&(_instance->z.lsb);//(byte*)&Z_L;
                _instance->ureg8[6] = (long)&(_instance->q.mswmsb);//(byte*)&E_REG;
                _instance->ureg8[7] = (long)&(_instance->q.mswlsb);//(byte*)&F_REG;

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

        public void Reset()
        {
            unsafe
            {
                for (byte index = 0; index <= 6; index++)
                {		//Set all register to 0 except V
                    PXF(index, 0);
                }

                for (byte index = 0; index <= 7; index++)
                {
                    PUR(index, 0);
                }

                CC_E = false; //0;
                CC_F = true; //1;
                CC_H = false; //0;
                CC_I = true; //1;
                CC_N = false; //0;
                CC_Z = false; //0;
                CC_V = false; //0;
                CC_C = false; //0;

                MD_NATIVE6309 = false; //0;
                MD_FIRQMODE = false; //0;
                MD_UNDEFINED2 = false; //0;  //UNDEFINED
                MD_UNDEFINED3 = false; //0;  //UNDEFINED
                MD_UNDEFINED4 = false; //0;  //UNDEFINED
                MD_UNDEFINED5 = false; //0;  //UNDEFINED
                MD_ILLEGAL = false; //0;
                MD_ZERODIV = false; //0;

                _instance->mdbits = HD6309_getmd();

                _instance->SyncWaiting = 0;

                DP_REG = 0;
                PC_REG = MemRead16(Define.VRESET);	//PC gets its reset vector

                _modules.TC1014.SetMapType(0);	//shouldn't be here
            }
        }

        public int Exec(int cycleFor)
        {
            unsafe
            {
                _instance->CycleCounter = 0;

                while (_instance->CycleCounter < cycleFor)
                {
                    if (_instance->PendingInterrupts != 0)
                    {
                        if ((_instance->PendingInterrupts & 4) != 0)
                        {
                            HD6309_cpu_nmi();
                        }

                        if ((_instance->PendingInterrupts & 2) != 0)
                        {
                            HD6309_cpu_firq();
                        }

                        if ((_instance->PendingInterrupts & 1) != 0)
                        {
                            if (_instance->IRQWaiter == 0)
                            {
                                // This is needed to fix a subtle timing problem
                                // It allows the CPU to see $FF03 bit 7 high before...
                                HD6309_cpu_irq();
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
                        break; // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                    }

                    _instance->gCycleFor = cycleFor;

                    byte opCode = _modules.TC1014.MemRead8(_instance->pc.Reg++); //PC_REG

                    Exec(opCode);
                }

                return cycleFor - _instance->CycleCounter;
            }
        }

        public void HD6309_cpu_nmi()
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

                if (_instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                {
                    _modules.TC1014.MemWrite8(_instance->q.mswlsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->q.mswmsb, --_instance->s.Reg);
                }

                _modules.TC1014.MemWrite8(_instance->q.lswlsb, --_instance->s.Reg);
                _modules.TC1014.MemWrite8(_instance->q.lswmsb, --_instance->s.Reg);

                _modules.TC1014.MemWrite8(HD6309_getcc(), --_instance->s.Reg);

                _instance->cc[(int)CCFlagMasks.I] = 1;
                _instance->cc[(int)CCFlagMasks.F] = 1;

                _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VNMI);

                _instance->PendingInterrupts &= 251;
            }
        }

        public void HD6309_cpu_firq()
        {
            unsafe
            {
                if (_instance->cc[(int)CCFlagMasks.F] == 0)
                {
                    _instance->InInterrupt = 1; //Flag to indicate FIRQ has been asserted

                    switch (_instance->md[(int)MDFlagMasks.FIRQMODE])
                    {
                        case 0:
                            _instance->cc[(int)CCFlagMasks.E] = 0; // Turn E flag off

                            _modules.TC1014.MemWrite8(_instance->pc.lsb, --_instance->s.Reg);
                            _modules.TC1014.MemWrite8(_instance->pc.msb, --_instance->s.Reg);

                            _modules.TC1014.MemWrite8(HD6309_getcc(), --_instance->s.Reg);

                            _instance->cc[(int)CCFlagMasks.I] = 1;
                            _instance->cc[(int)CCFlagMasks.F] = 1;

                            _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                            break;

                        case 1:		//6309
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

                            if (_instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                            {
                                _modules.TC1014.MemWrite8(_instance->q.mswlsb, --_instance->s.Reg);
                                _modules.TC1014.MemWrite8(_instance->q.mswmsb, --_instance->s.Reg);
                            }

                            _modules.TC1014.MemWrite8(_instance->q.lswlsb, --_instance->s.Reg);
                            _modules.TC1014.MemWrite8(_instance->q.lswmsb, --_instance->s.Reg);

                            _modules.TC1014.MemWrite8(HD6309_getcc(), --_instance->s.Reg);

                            _instance->cc[(int)CCFlagMasks.I] = 1;
                            _instance->cc[(int)CCFlagMasks.F] = 1;

                            _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                            break;
                    }
                }

                _instance->PendingInterrupts &= 253;
            }
        }

        public void HD6309_cpu_irq()
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

                    if (_instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                    {
                        _modules.TC1014.MemWrite8(_instance->q.mswlsb, --_instance->s.Reg);
                        _modules.TC1014.MemWrite8(_instance->q.mswmsb, --_instance->s.Reg);
                    }

                    _modules.TC1014.MemWrite8(_instance->q.lswlsb, --_instance->s.Reg);
                    _modules.TC1014.MemWrite8(_instance->q.lswmsb, --_instance->s.Reg);

                    _modules.TC1014.MemWrite8(HD6309_getcc(), --_instance->s.Reg);

                    _instance->cc[(int)CCFlagMasks.I] = 1;

                    _instance->pc.Reg = _modules.TC1014.MemRead16(Define.VIRQ);
                }

                _instance->PendingInterrupts &= 254;
            }
        }

        public byte HD6309_getcc()
        {
            int bincc = 0;

            void TST(bool _CC, CCFlagMasks _F)
            {
                if (_CC) { bincc |= (1 << (int)_F); }
            }

            TST(CC_E, CCFlagMasks.E);
            TST(CC_F, CCFlagMasks.F);
            TST(CC_H, CCFlagMasks.H);
            TST(CC_I, CCFlagMasks.I);
            TST(CC_N, CCFlagMasks.N);
            TST(CC_Z, CCFlagMasks.Z);
            TST(CC_V, CCFlagMasks.V);
            TST(CC_C, CCFlagMasks.C);

            return (byte)bincc;
        }

        public void HD6309_setcc(byte bincc)
        {
            unsafe
            {
                _instance->ccbits = bincc;
            }

            bool TST(CCFlagMasks _F)
            {
                return (bincc & (1 << (int)_F)) != 0;
            }

            CC_E = TST(CCFlagMasks.E);
            CC_F = TST(CCFlagMasks.F);
            CC_H = TST(CCFlagMasks.H);
            CC_I = TST(CCFlagMasks.I);
            CC_N = TST(CCFlagMasks.N);
            CC_Z = TST(CCFlagMasks.Z);
            CC_V = TST(CCFlagMasks.V);
            CC_C = TST(CCFlagMasks.C);
        }

        public byte HD6309_getmd()
        {
            int binmd = 0;

            void TST(bool _CC, MDFlagMasks _F)
            {
                if (_CC) { binmd |= (1 << (int)_F); }
            }

            TST(MD_NATIVE6309, MDFlagMasks.NATIVE6309);
            TST(MD_FIRQMODE, MDFlagMasks.FIRQMODE);
            TST(MD_UNDEFINED2, MDFlagMasks.MD_UNDEF2);
            TST(MD_UNDEFINED3, MDFlagMasks.MD_UNDEF3);
            TST(MD_UNDEFINED4, MDFlagMasks.MD_UNDEF4);
            TST(MD_UNDEFINED5, MDFlagMasks.MD_UNDEF5);
            TST(MD_ILLEGAL, MDFlagMasks.ILLEGAL);
            TST(MD_ZERODIV, MDFlagMasks.ZERODIV);

            return (byte)binmd;
        }

        public void HD6309_setmd(byte binmd)
        {
            bool TST(MDFlagMasks _F)
            {
                return (binmd & (1 << (int)_F)) != 0;
            }

            MD_NATIVE6309 = TST(MDFlagMasks.NATIVE6309);
            MD_FIRQMODE = TST(MDFlagMasks.FIRQMODE);
            MD_UNDEFINED2 = TST(MDFlagMasks.MD_UNDEF2);
            MD_UNDEFINED3 = TST(MDFlagMasks.MD_UNDEF3);
            MD_UNDEFINED4 = TST(MDFlagMasks.MD_UNDEF4);
            MD_UNDEFINED5 = TST(MDFlagMasks.MD_UNDEF5);
            MD_ILLEGAL = TST(MDFlagMasks.ILLEGAL);
            MD_ZERODIV = TST(MDFlagMasks.ZERODIV);

            unsafe
            {
                for (int i = 0; i < 24; i++)
                {
                    //*NatEmuCycles[i] = instance->InsCycles[MD_NATIVE6309][i];
                    uint insCyclesIndex = _instance->md[(int)MDFlagMasks.NATIVE6309];
                    byte value = _instance->InsCycles[insCyclesIndex * 25 + i];

                    byte* cycle = (byte*)(_instance->NatEmuCycles[i]);

                    *cycle = value;
                }
            }
        }

        public ushort HD6309_CalculateEA(byte postbyte)
        {
            unsafe
            {
                ushort ea = 0;

                byte reg = (byte)(((postbyte >> 5) & 3) + 1);

                if ((postbyte & 0x80) != 0)
                {
                    switch (postbyte & 0x1F)
                    {
                        case 0: // Post inc by 1
                            ea = PXF(reg);

                            PXF(reg, (ushort)(PXF(reg) + 1));

                            _instance->CycleCounter += _instance->NatEmuCycles21;

                            break;

                        case 1: // post in by 2
                            ea = PXF(reg);

                            PXF(reg, (ushort)(PXF(reg) + 2));

                            _instance->CycleCounter += _instance->NatEmuCycles32;

                            break;

                        case 2: // pre dec by 1
                            PXF(reg, (ushort)(PXF(reg) - 1));

                            ea = PXF(reg);

                            _instance->CycleCounter += _instance->NatEmuCycles21;

                            break;

                        case 3: // pre dec by 2
                            PXF(reg, (ushort)(PXF(reg) - 2));

                            ea = PXF(reg);

                            _instance->CycleCounter += _instance->NatEmuCycles32;

                            break;

                        case 4: // no offset
                            ea = PXF(reg);

                            break;

                        case 5: // B reg offset
                            ea = (ushort)(PXF(reg) + ((sbyte)B_REG));

                            _instance->CycleCounter += 1;

                            break;

                        case 6: // A reg offset
                            ea = (ushort)(PXF(reg) + ((sbyte)A_REG));

                            _instance->CycleCounter += 1;

                            break;

                        case 7: // E reg offset 
                            ea = (ushort)(PXF(reg) + ((sbyte)E_REG));

                            _instance->CycleCounter += 1;

                            break;

                        case 8: // 8 bit offset
                            ea = (ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++));

                            _instance->CycleCounter += 1;

                            break;

                        case 9: // 16 bit offset
                            ea = (ushort)(PXF(reg) + MemRead16(PC_REG));

                            _instance->CycleCounter += _instance->NatEmuCycles43;

                            PC_REG += 2;

                            break;

                        case 10: // F reg offset
                            ea = (ushort)(PXF(reg) + ((sbyte)F_REG));

                            _instance->CycleCounter += 1;

                            break;

                        case 11: // D reg offset 
                            ea = (ushort)(PXF(reg) + D_REG);

                            _instance->CycleCounter += _instance->NatEmuCycles42;

                            break;

                        case 12: // 8 bit PC relative
                            ea = (ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1);

                            _instance->CycleCounter += 1;

                            PC_REG++;

                            break;

                        case 13: // 16 bit PC relative
                            ea = (ushort)(PC_REG + MemRead16(PC_REG) + 2);

                            _instance->CycleCounter += _instance->NatEmuCycles53;

                            PC_REG += 2;

                            break;

                        case 14: // W reg offset
                            ea = (ushort)(PXF(reg) + W_REG);

                            _instance->CycleCounter += 4;

                            break;

                        case 15: // W reg
                            sbyte sbite = (sbyte)((postbyte >> 5) & 3);

                            switch (sbite)
                            {
                                case 0: // No offset from W reg
                                    ea = W_REG;

                                    break;

                                case 1: // 16 bit offset from W reg
                                    ea = (ushort)(W_REG + MemRead16(PC_REG));

                                    PC_REG += 2;

                                    _instance->CycleCounter += 2;

                                    break;

                                case 2: // Post inc by 2 from W reg
                                    ea = W_REG;

                                    W_REG += 2;

                                    _instance->CycleCounter += 1;

                                    break;

                                case 3: // Pre dec by 2 from W reg
                                    W_REG -= 2;

                                    ea = W_REG;

                                    _instance->CycleCounter += 1;

                                    break;
                            }

                            break;

                        case 16: // W reg
                            sbite = (sbyte)((postbyte >> 5) & 3);

                            switch (sbite)
                            {
                                case 0: // Indirect no offset from W reg
                                    ea = MemRead16(W_REG);

                                    _instance->CycleCounter += 3;

                                    break;

                                case 1: // Indirect 16 bit offset from W reg
                                    ea = MemRead16((ushort)(W_REG + MemRead16(PC_REG)));

                                    PC_REG += 2;

                                    _instance->CycleCounter += 5;

                                    break;

                                case 2: // Indirect post inc by 2 from W reg
                                    ea = MemRead16(W_REG);

                                    W_REG += 2;

                                    _instance->CycleCounter += 4;

                                    break;

                                case 3: // Indirect pre dec by 2 from W reg
                                    W_REG -= 2;

                                    ea = MemRead16(W_REG);

                                    _instance->CycleCounter += 4;

                                    break;
                            }

                            break;

                        case 17: // Indirect post inc by 2 
                            ea = PXF(reg);

                            PXF(reg, (ushort)(PXF(reg) + 2));

                            ea = MemRead16(ea);

                            _instance->CycleCounter += 6;

                            break;

                        case 18: // possibly illegal instruction
                            _instance->CycleCounter += 6;

                            break;

                        case 19: // Indirect pre dec by 2
                            PXF(reg, (ushort)(PXF(reg) - 2));

                            ea = MemRead16(PXF(reg));

                            _instance->CycleCounter += 6;

                            break;

                        case 20: // Indirect no offset 
                            ea = MemRead16(PXF(reg));

                            _instance->CycleCounter += 3;

                            break;

                        case 21: // Indirect B reg offset
                            ea = MemRead16((ushort)(PXF(reg) + ((sbyte)B_REG)));

                            _instance->CycleCounter += 4;

                            break;

                        case 22: // indirect A reg offset
                            ea = MemRead16((ushort)(PXF(reg) + ((sbyte)A_REG)));

                            _instance->CycleCounter += 4;

                            break;

                        case 23: // indirect E reg offset
                            ea = MemRead16((ushort)(PXF(reg) + ((sbyte)E_REG)));

                            _instance->CycleCounter += 4;

                            break;

                        case 24: // indirect 8 bit offset
                            ea = MemRead16((ushort)(PXF(reg) + (sbyte)MemRead8(PC_REG++)));

                            _instance->CycleCounter += 4;

                            break;

                        case 25: // indirect 16 bit offset
                            ea = MemRead16((ushort)(PXF(reg) + MemRead16(PC_REG)));

                            _instance->CycleCounter += 7;

                            PC_REG += 2;

                            break;

                        case 26: // indirect F reg offset
                            ea = MemRead16((ushort)(PXF(reg) + ((sbyte)F_REG)));

                            _instance->CycleCounter += 4;

                            break;

                        case 27: // indirect D reg offset
                            ea = MemRead16((ushort)(PXF(reg) + D_REG));

                            _instance->CycleCounter += 7;

                            break;

                        case 28: // indirect 8 bit PC relative
                            ea = MemRead16((ushort)((short)PC_REG + (sbyte)MemRead8(PC_REG) + 1));

                            _instance->CycleCounter += 4;

                            PC_REG++;

                            break;

                        case 29: //indirect 16 bit PC relative
                            ea = MemRead16((ushort)(PC_REG + MemRead16(PC_REG) + 2));

                            _instance->CycleCounter += 8;

                            PC_REG += 2;

                            break;

                        case 30: // indirect W reg offset
                            ea = MemRead16((ushort)(PXF(reg) + W_REG));

                            _instance->CycleCounter += 7;

                            break;

                        case 31: // extended indirect
                            ea = MemRead16(MemRead16(PC_REG));

                            _instance->CycleCounter += 8;

                            PC_REG += 2;

                            break;
                    }
                }
                else // 5 bit offset
                {
                    sbyte sbite = (sbyte)(postbyte & 0x1F);
                    sbite <<= 3; //--Push the "sign" to the left-most bit.
                    sbite /= 8;

                    ea = (ushort)(PXF(reg) + sbite); //Was signed

                    _instance->CycleCounter += 1;
                }

                return ea;
            }
        }
    }
}
