using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //SWI
    //Software interrupt (absolute indirect) 
    public class _103F_Swi2_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles, Func<int> addendum)
        {
            cpu.CC_E = true;

            cpu.MemWrite8(cpu.PC_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.PC_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.U_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.Y_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_L, --cpu.S_REG);
            cpu.MemWrite8(cpu.X_H, --cpu.S_REG);
            cpu.MemWrite8(cpu.DPA, --cpu.S_REG);

            cycles += addendum();

            cpu.MemWrite8(cpu.B_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.A_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.CC, --cpu.S_REG);

            cpu.PC_REG = cpu.MemRead16(Define.VSWI2);

            return cycles;
        }

        public int Exec(IMC6809 cpu)
        {
            return Exec(cpu, 20, () => 0);
        }

        public int Exec(IHD6309 cpu)
        {
            int Addendum()
            {
                if (cpu.MD_NATIVE6309)
                {
                    cpu.MemWrite8(cpu.F_REG, --cpu.S_REG);
                    cpu.MemWrite8(cpu.E_REG, --cpu.S_REG);

                    return 2;
                }

                return 0;
            }

            return Exec(cpu, 20, Addendum);
        }
    }
}
