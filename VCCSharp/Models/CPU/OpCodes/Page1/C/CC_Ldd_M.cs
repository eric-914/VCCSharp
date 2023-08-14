using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.C
{
    //LDD
    //Load D accumulator from memory
    //IMMEDIATE
    public class CC_Ldd_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.D_REG = cpu.MemRead16(cpu.PC_REG);

            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return cycles;
        }

        //TODO: One of these is wrong?
        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, 3);
    }
}
