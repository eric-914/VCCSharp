using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.CPU.HD6309;

namespace VCCSharp.Modules
{
    public interface IHD6309 : IProcessor
    {
        unsafe HD6309State* GetHD6309State();
        byte HD6309_getcc();
        void HD6309_setcc(byte bincc);
        byte HD6309_getmd();
        void HD6309_setmd(byte binmd);
        ushort HD6309_CalculateEA(byte postbyte);
    }

    public class HD6309 : IHD6309
    {
        private readonly IModules _modules;
        private readonly IHD6309OpCodes _opCodes;

        public HD6309(IModules modules, IHD6309OpCodes opCodes)
        {
            _modules = modules;
            _opCodes = opCodes;
        }

        public unsafe HD6309State* GetHD6309State()
        {
            return Library.HD6309.GetHD6309State();
        }

        public void Init()
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                //Call this first or RESET will core!
                // reg pointers for TFR and EXG and LEA ops
                instance->xfreg16[0] = (long)&(instance->q.lsw); //&D_REG;
                instance->xfreg16[1] = (long)&(instance->x.Reg); //&X_REG;
                instance->xfreg16[2] = (long)&(instance->y.Reg); //&Y_REG;
                instance->xfreg16[3] = (long)&(instance->u.Reg); //&U_REG;
                instance->xfreg16[4] = (long)&(instance->s.Reg); //&S_REG;
                instance->xfreg16[5] = (long)&(instance->pc.Reg); //&PC_REG;
                instance->xfreg16[6] = (long)&(instance->q.msw); //&W_REG;
                instance->xfreg16[7] = (long)&(instance->v.Reg); //&V_REG;

                instance->ureg8[0] = (long)&(instance->q.lswmsb); //(byte*)&A_REG;
                instance->ureg8[1] = (long)&(instance->q.lswlsb); //(byte*)&B_REG;
                instance->ureg8[2] = (long)&(instance->ccbits);//(byte*)&(instance->ccbits);
                instance->ureg8[3] = (long)&(instance->dp.msb);//(byte*)&DPA;
                instance->ureg8[4] = (long)&(instance->z.msb);//(byte*)&Z_H;
                instance->ureg8[5] = (long)&(instance->z.lsb);//(byte*)&Z_L;
                instance->ureg8[6] = (long)&(instance->q.mswmsb);//(byte*)&E_REG;
                instance->ureg8[7] = (long)&(instance->q.mswlsb);//(byte*)&F_REG;

                instance->NatEmuCycles[0] = (long)&(instance->NatEmuCycles65);
                instance->NatEmuCycles[1] = (long)&(instance->NatEmuCycles64);
                instance->NatEmuCycles[2] = (long)&(instance->NatEmuCycles32);
                instance->NatEmuCycles[3] = (long)&(instance->NatEmuCycles21);
                instance->NatEmuCycles[4] = (long)&(instance->NatEmuCycles54);
                instance->NatEmuCycles[5] = (long)&(instance->NatEmuCycles97);
                instance->NatEmuCycles[6] = (long)&(instance->NatEmuCycles85);
                instance->NatEmuCycles[7] = (long)&(instance->NatEmuCycles51);
                instance->NatEmuCycles[8] = (long)&(instance->NatEmuCycles31);
                instance->NatEmuCycles[9] = (long)&(instance->NatEmuCycles1110);
                instance->NatEmuCycles[10] = (long)&(instance->NatEmuCycles76);
                instance->NatEmuCycles[11] = (long)&(instance->NatEmuCycles75);
                instance->NatEmuCycles[12] = (long)&(instance->NatEmuCycles43);
                instance->NatEmuCycles[13] = (long)&(instance->NatEmuCycles87);
                instance->NatEmuCycles[14] = (long)&(instance->NatEmuCycles86);
                instance->NatEmuCycles[15] = (long)&(instance->NatEmuCycles98);
                instance->NatEmuCycles[16] = (long)&(instance->NatEmuCycles2726);
                instance->NatEmuCycles[17] = (long)&(instance->NatEmuCycles3635);
                instance->NatEmuCycles[18] = (long)&(instance->NatEmuCycles3029);
                instance->NatEmuCycles[19] = (long)&(instance->NatEmuCycles2827);
                instance->NatEmuCycles[20] = (long)&(instance->NatEmuCycles3726);
                instance->NatEmuCycles[21] = (long)&(instance->NatEmuCycles3130);
                instance->NatEmuCycles[22] = (long)&(instance->NatEmuCycles42);
                instance->NatEmuCycles[23] = (long)&(instance->NatEmuCycles53);

                //This handles the disparity between 6309 and 6809 Instruction timing
                instance->InsCycles[0 * 25 + Define.M65] = 6;    //6-5
                instance->InsCycles[1 * 25 + Define.M65] = 5;
                instance->InsCycles[0 * 25 + Define.M64] = 6;    //6-4
                instance->InsCycles[1 * 25 + Define.M64] = 4;
                instance->InsCycles[0 * 25 + Define.M32] = 3;    //3-2
                instance->InsCycles[1 * 25 + Define.M32] = 2;
                instance->InsCycles[0 * 25 + Define.M21] = 2;    //2-1
                instance->InsCycles[1 * 25 + Define.M21] = 1;
                instance->InsCycles[0 * 25 + Define.M54] = 5;    //5-4
                instance->InsCycles[1 * 25 + Define.M54] = 4;
                instance->InsCycles[0 * 25 + Define.M97] = 9;    //9-7
                instance->InsCycles[1 * 25 + Define.M97] = 7;
                instance->InsCycles[0 * 25 + Define.M85] = 8;    //8-5
                instance->InsCycles[1 * 25 + Define.M85] = 5;
                instance->InsCycles[0 * 25 + Define.M51] = 5;    //5-1
                instance->InsCycles[1 * 25 + Define.M51] = 1;
                instance->InsCycles[0 * 25 + Define.M31] = 3;    //3-1
                instance->InsCycles[1 * 25 + Define.M31] = 1;
                instance->InsCycles[0 * 25 + Define.M1110] = 11; //11-10
                instance->InsCycles[1 * 25 + Define.M1110] = 10;
                instance->InsCycles[0 * 25 + Define.M76] = 7;    //7-6
                instance->InsCycles[1 * 25 + Define.M76] = 6;
                instance->InsCycles[0 * 25 + Define.M75] = 7;    //7-5
                instance->InsCycles[1 * 25 + Define.M75] = 5;
                instance->InsCycles[0 * 25 + Define.M43] = 4;    //4-3
                instance->InsCycles[1 * 25 + Define.M43] = 3;
                instance->InsCycles[0 * 25 + Define.M87] = 8;    //8-7
                instance->InsCycles[1 * 25 + Define.M87] = 7;
                instance->InsCycles[0 * 25 + Define.M86] = 8;    //8-6
                instance->InsCycles[1 * 25 + Define.M86] = 6;
                instance->InsCycles[0 * 25 + Define.M98] = 9;    //9-8
                instance->InsCycles[1 * 25 + Define.M98] = 8;
                instance->InsCycles[0 * 25 + Define.M2726] = 27; //27-26
                instance->InsCycles[1 * 25 + Define.M2726] = 26;
                instance->InsCycles[0 * 25 + Define.M3635] = 36; //36-25
                instance->InsCycles[1 * 25 + Define.M3635] = 35;
                instance->InsCycles[0 * 25 + Define.M3029] = 30; //30-29
                instance->InsCycles[1 * 25 + Define.M3029] = 29;
                instance->InsCycles[0 * 25 + Define.M2827] = 28; //28-27
                instance->InsCycles[1 * 25 + Define.M2827] = 27;
                instance->InsCycles[0 * 25 + Define.M3726] = 37; //37-26
                instance->InsCycles[1 * 25 + Define.M3726] = 26;
                instance->InsCycles[0 * 25 + Define.M3130] = 31; //31-30
                instance->InsCycles[1 * 25 + Define.M3130] = 30;
                instance->InsCycles[0 * 25 + Define.M42] = 4;    //4-2
                instance->InsCycles[1 * 25 + Define.M42] = 2;
                instance->InsCycles[0 * 25 + Define.M53] = 5;    //5-3
                instance->InsCycles[1 * 25 + Define.M53] = 3;
            }
        }

        public void ForcePC(ushort address)
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->pc.Reg = address;

                instance->PendingInterrupts = 0;
                instance->SyncWaiting = 0;
            }
        }

        public void DeAssertInterrupt(byte irq)
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->PendingInterrupts &= (byte)~(1 << (irq - 1));
                instance->InInterrupt = 0;
            }
        }

        public void AssertInterrupt(byte irq, byte flag)
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->SyncWaiting = 0;
                instance->PendingInterrupts |= (byte)(1 << (irq - 1));
                instance->IRQWaiter = flag;
            }
        }

        public void Reset()
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                for (byte index = 0; index <= 6; index++)
                {		//Set all register to 0 except V
                    _opCodes.PXF(index, 0);
                }

                for (byte index = 0; index <= 7; index++)
                {
                    _opCodes.PUR(index, 0);
                }

                _opCodes.CC_E = false; //0;
                _opCodes.CC_F = true; //1;
                _opCodes.CC_H = false; //0;
                _opCodes.CC_I = true; //1;
                _opCodes.CC_N = false; //0;
                _opCodes.CC_Z = false; //0;
                _opCodes.CC_V = false; //0;
                _opCodes.CC_C = false; //0;

                _opCodes.MD_NATIVE6309 = false; //0;
                _opCodes.MD_FIRQMODE = false; //0;
                _opCodes.MD_UNDEFINED2 = false; //0;  //UNDEFINED
                _opCodes.MD_UNDEFINED3 = false; //0;  //UNDEFINED
                _opCodes.MD_UNDEFINED4 = false; //0;  //UNDEFINED
                _opCodes.MD_UNDEFINED5 = false; //0;  //UNDEFINED
                _opCodes.MD_ILLEGAL = false; //0;
                _opCodes.MD_ZERODIV = false; //0;

                instance->mdbits = HD6309_getmd();

                instance->SyncWaiting = 0;

                _opCodes.DP_REG = 0;
                _opCodes.PC_REG = _opCodes.MemRead16(Define.VRESET);	//PC gets its reset vector

                _modules.TC1014.SetMapType(0);	//shouldn't be here
            }
        }

        public int Exec(int cycleFor)
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->CycleCounter = 0;

                while (instance->CycleCounter < cycleFor)
                {
                    if (instance->PendingInterrupts != 0)
                    {
                        if ((instance->PendingInterrupts & 4) != 0)
                        {
                            HD6309_cpu_nmi();
                        }

                        if ((instance->PendingInterrupts & 2) != 0)
                        {
                            HD6309_cpu_firq();
                        }

                        if ((instance->PendingInterrupts & 1) != 0)
                        {
                            if (instance->IRQWaiter == 0)
                            {
                                // This is needed to fix a subtle timing problem
                                // It allows the CPU to see $FF03 bit 7 high before...
                                HD6309_cpu_irq();
                            }
                            else
                            {
                                // ...The IRQ is asserted.
                                instance->IRQWaiter -= 1;
                            }
                        }
                    }

                    if (instance->SyncWaiting == 1)
                    {
                        //Abort the run nothing happens asynchronously from the CPU
                        break; // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                    }

                    instance->gCycleFor = cycleFor;

                    byte opCode = _modules.TC1014.MemRead8(instance->pc.Reg++); //PC_REG

                    _opCodes.Exec(opCode);
                }

                return cycleFor - instance->CycleCounter;
            }
        }

        public void HD6309_cpu_nmi()
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->cc[(int)CCFlagMasks.E] = 1;

                _modules.TC1014.MemWrite8(instance->pc.lsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->pc.msb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->u.lsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->u.msb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->y.lsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->y.msb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->x.lsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->x.msb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->dp.msb, --instance->s.Reg);

                if (instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                {
                    _modules.TC1014.MemWrite8(instance->q.mswlsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->q.mswmsb, --instance->s.Reg);
                }

                _modules.TC1014.MemWrite8(instance->q.lswlsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->q.lswmsb, --instance->s.Reg);

                _modules.TC1014.MemWrite8(HD6309_getcc(), --instance->s.Reg);

                instance->cc[(int)CCFlagMasks.I] = 1;
                instance->cc[(int)CCFlagMasks.F] = 1;

                instance->pc.Reg = _modules.TC1014.MemRead16(Define.VNMI);

                instance->PendingInterrupts &= 251;
            }
        }

        public void HD6309_cpu_firq()
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                if (instance->cc[(int)CCFlagMasks.F] == 0)
                {
                    instance->InInterrupt = 1; //Flag to indicate FIRQ has been asserted

                    switch (instance->md[(int)MDFlagMasks.FIRQMODE])
                    {
                        case 0:
                            instance->cc[(int)CCFlagMasks.E] = 0; // Turn E flag off

                            _modules.TC1014.MemWrite8(instance->pc.lsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->pc.msb, --instance->s.Reg);

                            _modules.TC1014.MemWrite8(HD6309_getcc(), --instance->s.Reg);

                            instance->cc[(int)CCFlagMasks.I] = 1;
                            instance->cc[(int)CCFlagMasks.F] = 1;

                            instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                            break;

                        case 1:		//6309
                            instance->cc[(int)CCFlagMasks.E] = 1;

                            _modules.TC1014.MemWrite8(instance->pc.lsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->pc.msb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->u.lsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->u.msb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->y.lsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->y.msb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->x.lsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->x.msb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->dp.msb, --instance->s.Reg);

                            if (instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                            {
                                _modules.TC1014.MemWrite8(instance->q.mswlsb, --instance->s.Reg);
                                _modules.TC1014.MemWrite8(instance->q.mswmsb, --instance->s.Reg);
                            }

                            _modules.TC1014.MemWrite8(instance->q.lswlsb, --instance->s.Reg);
                            _modules.TC1014.MemWrite8(instance->q.lswmsb, --instance->s.Reg);

                            _modules.TC1014.MemWrite8(HD6309_getcc(), --instance->s.Reg);

                            instance->cc[(int)CCFlagMasks.I] = 1;
                            instance->cc[(int)CCFlagMasks.F] = 1;

                            instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);

                            break;
                    }
                }

                instance->PendingInterrupts &= 253;
            }
        }

        public void HD6309_cpu_irq()
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                if (instance->InInterrupt == 1)
                {
                    //If FIRQ is running postpone the IRQ
                    return;
                }

                if (instance->cc[(int)CCFlagMasks.I] == 0)
                {
                    instance->cc[(int)CCFlagMasks.E] = 1;

                    _modules.TC1014.MemWrite8(instance->pc.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->pc.msb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->u.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->u.msb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->y.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->y.msb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->x.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->x.msb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->dp.msb, --instance->s.Reg);

                    if (instance->md[(int)MDFlagMasks.NATIVE6309] != 0)
                    {
                        _modules.TC1014.MemWrite8(instance->q.mswlsb, --instance->s.Reg);
                        _modules.TC1014.MemWrite8(instance->q.mswmsb, --instance->s.Reg);
                    }

                    _modules.TC1014.MemWrite8(instance->q.lswlsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->q.lswmsb, --instance->s.Reg);

                    _modules.TC1014.MemWrite8(HD6309_getcc(), --instance->s.Reg);

                    instance->cc[(int)CCFlagMasks.I] = 1;

                    instance->pc.Reg = _modules.TC1014.MemRead16(Define.VIRQ);
                }

                instance->PendingInterrupts &= 254;
            }
        }

        public byte HD6309_getcc()
        {
            int bincc = 0;

            void TST(bool _CC, CCFlagMasks _F)
            {
                if (_CC) { bincc |= (1 << (int)_F); }
            }

            TST(_opCodes.CC_E, CCFlagMasks.E);
            TST(_opCodes.CC_F, CCFlagMasks.F);
            TST(_opCodes.CC_H, CCFlagMasks.H);
            TST(_opCodes.CC_I, CCFlagMasks.I);
            TST(_opCodes.CC_N, CCFlagMasks.N);
            TST(_opCodes.CC_Z, CCFlagMasks.Z);
            TST(_opCodes.CC_V, CCFlagMasks.V);
            TST(_opCodes.CC_C, CCFlagMasks.C);

            return (byte)bincc;
        }

        public void HD6309_setcc(byte bincc)
        {
            unsafe
            {
                HD6309State* instance = GetHD6309State();

                instance->ccbits = bincc;
            }

            bool TST(CCFlagMasks _F)
            {
                return (bincc & (1 << (int)_F)) != 0;
            }

            _opCodes.CC_E = TST(CCFlagMasks.E);
            _opCodes.CC_F = TST(CCFlagMasks.F);
            _opCodes.CC_H = TST(CCFlagMasks.H);
            _opCodes.CC_I = TST(CCFlagMasks.I);
            _opCodes.CC_N = TST(CCFlagMasks.N);
            _opCodes.CC_Z = TST(CCFlagMasks.Z);
            _opCodes.CC_V = TST(CCFlagMasks.V);
            _opCodes.CC_C = TST(CCFlagMasks.C);
        }

        public byte HD6309_getmd()
        {
            int binmd = 0;

            void TST(bool _CC, MDFlagMasks _F)
            {
                if (_CC) { binmd |= (1 << (int)_F); }
            }

            TST(_opCodes.MD_NATIVE6309, MDFlagMasks.NATIVE6309);
            TST(_opCodes.MD_FIRQMODE, MDFlagMasks.FIRQMODE);
            TST(_opCodes.MD_UNDEFINED2, MDFlagMasks.MD_UNDEF2);
            TST(_opCodes.MD_UNDEFINED3, MDFlagMasks.MD_UNDEF3);
            TST(_opCodes.MD_UNDEFINED4, MDFlagMasks.MD_UNDEF4);
            TST(_opCodes.MD_UNDEFINED5, MDFlagMasks.MD_UNDEF5);
            TST(_opCodes.MD_ILLEGAL, MDFlagMasks.ILLEGAL);
            TST(_opCodes.MD_ZERODIV, MDFlagMasks.ZERODIV);

            return (byte)binmd;
        }

        public void HD6309_setmd(byte binmd)
        {
            bool TST(MDFlagMasks _F)
            {
                return (binmd & (1 << (int)_F)) != 0;
            }

            _opCodes.MD_NATIVE6309 = TST(MDFlagMasks.NATIVE6309);
            _opCodes.MD_FIRQMODE = TST(MDFlagMasks.FIRQMODE);
            _opCodes.MD_UNDEFINED2 = TST(MDFlagMasks.MD_UNDEF2);
            _opCodes.MD_UNDEFINED3 = TST(MDFlagMasks.MD_UNDEF3);
            _opCodes.MD_UNDEFINED4 = TST(MDFlagMasks.MD_UNDEF4);
            _opCodes.MD_UNDEFINED5 = TST(MDFlagMasks.MD_UNDEF5);
            _opCodes.MD_ILLEGAL = TST(MDFlagMasks.ILLEGAL);
            _opCodes.MD_ZERODIV = TST(MDFlagMasks.ZERODIV);

            unsafe
            {
                HD6309State* instance = GetHD6309State();
                for (int i = 0; i < 24; i++)
                {
                    //*NatEmuCycles[i] = instance->InsCycles[MD_NATIVE6309][i];
                    uint insCyclesIndex = instance->md[(int)MDFlagMasks.NATIVE6309];
                    byte value = instance->InsCycles[insCyclesIndex * 25 + i];

                    byte* cycle = (byte*)(instance->NatEmuCycles[i]);

                    *cycle = value;
                }
            }
        }

        public ushort HD6309_CalculateEA(byte postbyte)
        {
            unsafe
            {
                ushort ea = 0;
                sbyte sbite = 0;

                HD6309State* instance = GetHD6309State();

                byte reg = (byte)(((postbyte >> 5) & 3) + 1);

                if ((postbyte & 0x80) != 0)
                {
                    switch (postbyte & 0x1F)
                    {
                        case 0: // Post inc by 1
                            ea = _opCodes.PXF(reg);

                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) + 1));

                            instance->CycleCounter += instance->NatEmuCycles21;

                            break;

                        case 1: // post in by 2
                            ea = _opCodes.PXF(reg);

                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) + 2));

                            instance->CycleCounter += instance->NatEmuCycles32;

                            break;

                        case 2: // pre dec by 1
                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) - 1));

                            ea = _opCodes.PXF(reg);

                            instance->CycleCounter += instance->NatEmuCycles21;

                            break;

                        case 3: // pre dec by 2
                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) - 2));

                            ea = _opCodes.PXF(reg);

                            instance->CycleCounter += instance->NatEmuCycles32;

                            break;

                        case 4: // no offset
                            ea = _opCodes.PXF(reg);

                            break;

                        case 5: // B reg offset
                            ea = (ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.B_REG));

                            instance->CycleCounter += 1;

                            break;

                        case 6: // A reg offset
                            ea = (ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.A_REG));

                            instance->CycleCounter += 1;

                            break;

                        case 7: // E reg offset 
                            ea = (ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.E_REG));

                            instance->CycleCounter += 1;

                            break;

                        case 8: // 8 bit offset
                            ea = (ushort)(_opCodes.PXF(reg) + (sbyte)_opCodes.MemRead8(_opCodes.PC_REG++));

                            instance->CycleCounter += 1;

                            break;

                        case 9: // 16 bit offset
                            ea = (ushort)(_opCodes.PXF(reg) + _opCodes.MemRead16(_opCodes.PC_REG));

                            instance->CycleCounter += instance->NatEmuCycles43;

                            _opCodes.PC_REG += 2;

                            break;

                        case 10: // F reg offset
                            ea = (ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.F_REG));

                            instance->CycleCounter += 1;

                            break;

                        case 11: // D reg offset 
                            ea = (ushort)(_opCodes.PXF(reg) + _opCodes.D_REG);

                            instance->CycleCounter += instance->NatEmuCycles42;

                            break;

                        case 12: // 8 bit PC relative
                            ea = (ushort)((short)_opCodes.PC_REG + (sbyte)_opCodes.MemRead8(_opCodes.PC_REG) + 1);

                            instance->CycleCounter += 1;

                            _opCodes.PC_REG++;

                            break;

                        case 13: // 16 bit PC relative
                            ea = (ushort)(_opCodes.PC_REG + _opCodes.MemRead16(_opCodes.PC_REG) + 2);

                            instance->CycleCounter += instance->NatEmuCycles53;

                            _opCodes.PC_REG += 2;

                            break;

                        case 14: // W reg offset
                            ea = (ushort)(_opCodes.PXF(reg) + _opCodes.W_REG);

                            instance->CycleCounter += 4;

                            break;

                        case 15: // W reg
                            sbite = (sbyte)((postbyte >> 5) & 3);

                            switch (sbite)
                            {
                                case 0: // No offset from W reg
                                    ea = _opCodes.W_REG;

                                    break;

                                case 1: // 16 bit offset from W reg
                                    ea = (ushort)(_opCodes.W_REG + _opCodes.MemRead16(_opCodes.PC_REG));

                                    _opCodes.PC_REG += 2;

                                    instance->CycleCounter += 2;

                                    break;

                                case 2: // Post inc by 2 from W reg
                                    ea = _opCodes.W_REG;

                                    _opCodes.W_REG += 2;

                                    instance->CycleCounter += 1;

                                    break;

                                case 3: // Pre dec by 2 from W reg
                                    _opCodes.W_REG -= 2;

                                    ea = _opCodes.W_REG;

                                    instance->CycleCounter += 1;

                                    break;
                            }

                            break;

                        case 16: // W reg
                            sbite = (sbyte)((postbyte >> 5) & 3);

                            switch (sbite)
                            {
                                case 0: // Indirect no offset from W reg
                                    ea = _opCodes.MemRead16(_opCodes.W_REG);

                                    instance->CycleCounter += 3;

                                    break;

                                case 1: // Indirect 16 bit offset from W reg
                                    ea = _opCodes.MemRead16((ushort)(_opCodes.W_REG + _opCodes.MemRead16(_opCodes.PC_REG)));

                                    _opCodes.PC_REG += 2;

                                    instance->CycleCounter += 5;

                                    break;

                                case 2: // Indirect post inc by 2 from W reg
                                    ea = _opCodes.MemRead16(_opCodes.W_REG);

                                    _opCodes.W_REG += 2;

                                    instance->CycleCounter += 4;

                                    break;

                                case 3: // Indirect pre dec by 2 from W reg
                                    _opCodes.W_REG -= 2;

                                    ea = _opCodes.MemRead16(_opCodes.W_REG);

                                    instance->CycleCounter += 4;

                                    break;
                            }

                            break;

                        case 17: // Indirect post inc by 2 
                            ea = _opCodes.PXF(reg);

                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) + 2));

                            ea = _opCodes.MemRead16(ea);

                            instance->CycleCounter += 6;

                            break;

                        case 18: // possibly illegal instruction
                            instance->CycleCounter += 6;

                            break;

                        case 19: // Indirect pre dec by 2
                            _opCodes.PXF(reg, (ushort)(_opCodes.PXF(reg) - 2));

                            ea = _opCodes.MemRead16(_opCodes.PXF(reg));

                            instance->CycleCounter += 6;

                            break;

                        case 20: // Indirect no offset 
                            ea = _opCodes.MemRead16(_opCodes.PXF(reg));

                            instance->CycleCounter += 3;

                            break;

                        case 21: // Indirect B reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.B_REG)));

                            instance->CycleCounter += 4;

                            break;

                        case 22: // indirect A reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.A_REG)));

                            instance->CycleCounter += 4;

                            break;

                        case 23: // indirect E reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.E_REG)));

                            instance->CycleCounter += 4;

                            break;

                        case 24: // indirect 8 bit offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + (sbyte)_opCodes.MemRead8(_opCodes.PC_REG++)));

                            instance->CycleCounter += 4;

                            break;

                        case 25: // indirect 16 bit offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + _opCodes.MemRead16(_opCodes.PC_REG)));

                            instance->CycleCounter += 7;

                            _opCodes.PC_REG += 2;

                            break;

                        case 26: // indirect F reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + ((sbyte)_opCodes.F_REG)));

                            instance->CycleCounter += 4;

                            break;

                        case 27: // indirect D reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + _opCodes.D_REG));

                            instance->CycleCounter += 7;

                            break;

                        case 28: // indirect 8 bit PC relative
                            ea = _opCodes.MemRead16((ushort)((short)_opCodes.PC_REG + (sbyte)_opCodes.MemRead8(_opCodes.PC_REG) + 1));

                            instance->CycleCounter += 4;

                            _opCodes.PC_REG++;

                            break;

                        case 29: //indirect 16 bit PC relative
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PC_REG + _opCodes.MemRead16(_opCodes.PC_REG) + 2));

                            instance->CycleCounter += 8;

                            _opCodes.PC_REG += 2;

                            break;

                        case 30: // indirect W reg offset
                            ea = _opCodes.MemRead16((ushort)(_opCodes.PXF(reg) + _opCodes.W_REG));

                            instance->CycleCounter += 7;

                            break;

                        case 31: // extended indirect
                            ea = _opCodes.MemRead16(_opCodes.MemRead16(_opCodes.PC_REG));

                            instance->CycleCounter += 8;

                            _opCodes.PC_REG += 2;

                            break;
                    }
                }
                else // 5 bit offset
                {
                    sbite = (sbyte)(postbyte & 0x1F);
                    sbite <<= 3; //--Push the "sign" to the left-most bit.
                    sbite /= 8;

                    ea = (ushort)(_opCodes.PXF(reg) + sbite); //Was signed

                    instance->CycleCounter += 1;
                }

                return ea;
            }
        }
    }
}
