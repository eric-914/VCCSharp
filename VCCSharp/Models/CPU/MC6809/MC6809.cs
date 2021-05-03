using VCCSharp.Enums;
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
            Library.MC6809.MC6809AssertInterrupt(irq, flag);
        }

        public void Reset()
        {
            Library.MC6809.MC6809Reset();
        }

        public int Exec(int cycleFor)
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

                instance->CycleCounter = 0;

                while (instance->CycleCounter < cycleFor)
                {
                    if (instance->PendingInterrupts != 0)
                    {
                        if ((instance->PendingInterrupts & 4) != 0)
                        {
                            MC6809_cpu_nmi();
                        }

                        if ((instance->PendingInterrupts & 2) != 0)
                        {
                            MC6809_cpu_firq();
                        }

                        if ((instance->PendingInterrupts & 1) != 0)
                        {
                            if (instance->IRQWaiter == 0)
                            {
                                // This is needed to fix a subtle timing problem
                                // It allows the CPU to see $FF03 bit 7 high before...
                                MC6809_cpu_irq();
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
                        // WDZ - Experimental SyncWaiting should still return used cycles (and not zero) by breaking from loop
                        break;
                    }

                    _gCycleFor = cycleFor;

                    byte opCode = _modules.TC1014.MemRead8(instance->pc.Reg++);   //PC_REG

                    Exec(opCode);
                }

                return cycleFor - instance->CycleCounter;
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
                MC6809State* instance = GetMC6809State();

                if (instance->cc[(int)CCFlagMasks.F] == 0)
                {
                    instance->InInterrupt = 1; //Flag to indicate FIRQ has been asserted

                    instance->cc[(int)CCFlagMasks.E] = 0; // Turn E flag off

                    _modules.TC1014.MemWrite8(instance->pc.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->pc.msb, --instance->s.Reg);

                    _modules.TC1014.MemWrite8(MC6809_getcc(), --instance->s.Reg);

                    instance->cc[(int)CCFlagMasks.I] = 1;
                    instance->cc[(int)CCFlagMasks.F] = 1;

                    instance->pc.Reg = _modules.TC1014.MemRead16(Define.VFIRQ);
                }

                instance->PendingInterrupts &= 253;
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
            //int cc = 0;

            //void Test(bool value, CCFlagMasks mask)
            //{
            //    if (value) { cc |= (1 << (int)mask); }
            //}

            //Test(CC_E, CCFlagMasks.E);
            //Test(CC_F, CCFlagMasks.F);
            //Test(CC_H, CCFlagMasks.H);
            //Test(CC_I, CCFlagMasks.I);
            //Test(CC_N, CCFlagMasks.N);
            //Test(CC_Z, CCFlagMasks.Z);
            //Test(CC_V, CCFlagMasks.V);
            //Test(CC_C, CCFlagMasks.C);

            //return (byte)cc;

            return Library.MC6809.MC6809_getcc();
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
            return Library.MC6809.MC6809_CalculateEA(postByte);
        }
    }
}
