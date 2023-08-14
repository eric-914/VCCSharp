using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.D
{
    //LDS
    //Load stack pointer from memory
    //DIRECT
    public class _10DE_Lds_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.S_REG = cpu.MemRead16(address);

            cpu.CC_Z = ZTEST(cpu.S_REG);
            cpu.CC_N = NTEST16(cpu.S_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
