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
                instance->xfreg16[0] = (long) &(instance->d.Reg); // &D_REG;
                instance->xfreg16[1] = (long) &(instance->x.Reg); // &X_REG;
                instance->xfreg16[2] = (long) &(instance->y.Reg); // &Y_REG;
                instance->xfreg16[3] = (long) &(instance->u.Reg); // &U_REG;
                instance->xfreg16[4] = (long) &(instance->s.Reg); // &S_REG;
                instance->xfreg16[5] = (long) &(instance->pc.Reg); // &PC_REG;

                instance->ureg8[0] = (long) &(instance->d.msb); //(byte*)(&A_REG);
                instance->ureg8[1] = (long) &(instance->d.lsb); //(byte*)(&B_REG);
                instance->ureg8[2] = (long) &(instance->ccbits); //(byte*)(&(instance->ccbits));
                instance->ureg8[3] = (long) &(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[4] = (long) &(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[5] = (long) &(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[6] = (long) &(instance->dp.msb); //(byte*)(&DPA);
                instance->ureg8[7] = (long) &(instance->dp.msb); //(byte*)(&DPA);
            }
        }

        public int Exec(int cycleFor)
        {
            return Library.MC6809.MC6809Exec(cycleFor);
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

        public void Reset()
        {
            Library.MC6809.MC6809Reset();
        }

        public void AssertInterrupt(byte irq, byte flag)
        {
            Library.MC6809.MC6809AssertInterrupt(irq, flag);
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
    }
}
