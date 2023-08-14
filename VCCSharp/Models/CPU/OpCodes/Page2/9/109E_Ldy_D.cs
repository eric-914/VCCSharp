using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //LDY
    //Load index register from memory
    public class _109E_Ldy_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.Y_REG = cpu.MemRead16(address);
            cpu.CC_Z = ZTEST(cpu.Y_REG);
            cpu.CC_N = NTEST16(cpu.Y_REG);
            cpu.CC_V = false; //0;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
