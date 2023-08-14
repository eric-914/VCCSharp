using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //RTI
    //Return from interrupt 
    //INHERENT
    public class _3B_Rti_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles, Func<int> addendum)
        {
            cpu.CC = cpu.MemRead8(cpu.S_REG++);

            cpu.IsInInterrupt = false;

            if (cpu.CC_E)
            {
                cpu.A_REG = cpu.MemRead8(cpu.S_REG++);
                cpu.B_REG = cpu.MemRead8(cpu.S_REG++);

                cycles += addendum();

                cpu.DPA = cpu.MemRead8(cpu.S_REG++);
                cpu.X_H = cpu.MemRead8(cpu.S_REG++);
                cpu.X_L = cpu.MemRead8(cpu.S_REG++);
                cpu.Y_H = cpu.MemRead8(cpu.S_REG++);
                cpu.Y_L = cpu.MemRead8(cpu.S_REG++);
                cpu.U_H = cpu.MemRead8(cpu.S_REG++);
                cpu.U_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 9;
            }

            cpu.PC_H = cpu.MemRead8(cpu.S_REG++);
            cpu.PC_L = cpu.MemRead8(cpu.S_REG++);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6, () => 0);

        public int Exec(IHD6309 cpu)
        {
            int Addendum()
            {
                if (cpu.MD_NATIVE6309)
                {
                    cpu.E_REG = cpu.MemRead8(cpu.S_REG++);
                    cpu.F_REG = cpu.MemRead8(cpu.S_REG++);

                    return 2;
                }

                return 0;
            }

            return Exec(cpu, 6, Addendum);
        }
    }
}
