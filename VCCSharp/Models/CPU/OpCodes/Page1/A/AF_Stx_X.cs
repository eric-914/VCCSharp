using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.A
{
    //STX
    //Store index register to memory
    public class AF_Stx_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.MemWrite16(cpu.X_REG, address);

            cpu.CC_Z = ZTEST(cpu.X_REG);
            cpu.CC_N = NTEST16(cpu.X_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, 5);
    }
}
