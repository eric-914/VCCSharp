using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMC6809 : IProcessor
    {
        unsafe MC6809State* GetMC6809State();
    }

    public class MC6809 : IMC6809
    {
        private readonly IModules _modules;

        public MC6809(IModules modules)
        {
            _modules = modules;
        }

        public unsafe MC6809State* GetMC6809State()
        {
            return Library.MC6809.GetMC6809State();
        }

        public void Init()
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

                //Call this first or RESET will core!
                // reg pointers for TFR and EXG and LEA ops
                instance->xfreg16[0] = (long)&(instance->d.Reg); // &D_REG;
                instance->xfreg16[1] = (long)&(instance->x.Reg); // &X_REG;
                instance->xfreg16[2] = (long)&(instance->y.Reg); // &Y_REG;
                instance->xfreg16[3] = (long)&(instance->u.Reg); // &U_REG;
                instance->xfreg16[4] = (long)&(instance->s.Reg); // &S_REG;
                instance->xfreg16[5] = (long)&(instance->pc.Reg); // &PC_REG;

                instance->ureg8[0] = (long)&(instance->d.msb); //(byte*)(&A_REG);
                instance->ureg8[1] = (long)&(instance->d.lsb); //(byte*)(&B_REG);
                instance->ureg8[2] = (long)&(instance->ccbits); //(byte*)(&(instance->ccbits));
                instance->ureg8[3] = (long)&(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[4] = (long)&(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[5] = (long)&(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[6] = (long)&(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[7] = (long)&(instance->dp.msb); //(byte*)(&DPA);
            }
        }

        public void ForcePC(ushort address)
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

                instance->pc.Reg = address;

                instance->PendingInterrupts = 0;
                instance->SyncWaiting = 0;
            }
        }

        public void DeAssertInterrupt(byte irq)
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

                instance->PendingInterrupts &= (byte)~(1 << (irq - 1));
                instance->InInterrupt = 0;
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
                            { // This is needed to fix a subtle timming problem
                                MC6809_cpu_irq();		// It allows the CPU to see $FF03 bit 7 high before
                            }
                            else
                            {				// The IRQ is asserted.
                                instance->IRQWaiter -= 1;
                            }
                        }
                    }

                    if (instance->SyncWaiting == 1)
                    {
                        break;
                    }

                    byte opCode = _modules.TC1014.MemRead8(instance->pc.Reg++);   //PC_REG

                    MC6809ExecOpCode(cycleFor, opCode);
                }

                return cycleFor - instance->CycleCounter;
            }
        }

        public void MC6809ExecOpCode(int cycleFor, byte opCode)
        {
            Library.MC6809.MC6809ExecOpCode(cycleFor, opCode);
        }

        public void MC6809_cpu_nmi()
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

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

                _modules.TC1014.MemWrite8(instance->d.lsb, --instance->s.Reg);
                _modules.TC1014.MemWrite8(instance->d.msb, --instance->s.Reg);

                _modules.TC1014.MemWrite8(MC6809_getcc(), --instance->s.Reg);

                instance->cc[(int)CCFlagMasks.I] = 1;
                instance->cc[(int)CCFlagMasks.F] = 1;

                instance->pc.Reg = _modules.TC1014.MemRead16(Define.VNMI);

                instance->PendingInterrupts &= 251;
            }
        }

        public void MC6809_cpu_irq()
        {
            unsafe
            {
                MC6809State* instance = GetMC6809State();

                if (instance->InInterrupt == 1)
                { //If FIRQ is running postpone the IRQ
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
                    _modules.TC1014.MemWrite8(instance->d.lsb, --instance->s.Reg);
                    _modules.TC1014.MemWrite8(instance->d.msb, --instance->s.Reg);

                    _modules.TC1014.MemWrite8(MC6809_getcc(), --instance->s.Reg);

                    instance->pc.Reg = _modules.TC1014.MemRead16(Define.VIRQ);
                    instance->cc[(int)CCFlagMasks.I] = 1;
                }

                instance->PendingInterrupts &= 254;
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

        public byte MC6809_getcc()
        {
            return Library.MC6809.MC6809_getcc();
        }
    }
}
