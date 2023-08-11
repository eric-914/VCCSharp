using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.E
{
    public class E7_Stb_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            throw new NotImplementedException();

            //cpu.MemWrite8(cpu.B_REG, CalculateEA(cpu.MemRead8(cpu.PC_REG++)));

            cpu.CC_Z = ZTEST(cpu.B_REG);
            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
