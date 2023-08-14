using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.E
{
    //STS
    //Store stack pointer to memory
    //INDEXED
    public class _10EF_Sts_X : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.MemWrite16(cpu.S_REG, address);

            cpu.CC_Z = ZTEST(cpu.S_REG);
            cpu.CC_N = NTEST16(cpu.S_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, 6);
    }
}
