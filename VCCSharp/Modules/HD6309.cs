using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IHD6309 : IProcessor
    {
        unsafe HD6309State* GetHD6309State();
    }

    public class HD6309 : IHD6309
    {
        private readonly IModules _modules;

        public HD6309(IModules modules)
        {
            _modules = modules;
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
            Library.HD6309.HD6309AssertInterrupt(irq, flag);
        }

        public void Reset()
        {
            Library.HD6309.HD6309Reset();
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

                    HD6309ExecOpCode(cycleFor, _modules.TC1014.MemRead8(instance->pc.Reg++)); //PC_REG
                }

                return cycleFor - instance->CycleCounter;
            }
        }

        public void HD6309ExecOpCode(int cycleFor, byte opcode)
        {
            Library.HD6309.HD6309ExecOpCode(cycleFor, opcode);
        }

        public void HD6309_cpu_nmi()
        {
            Library.HD6309.HD6309_cpu_nmi();
        }

        public void HD6309_cpu_firq()
        {
            Library.HD6309.HD6309_cpu_firq();
        }

        public void HD6309_cpu_irq()
        {
            Library.HD6309.HD6309_cpu_irq();
        }
    }
}
