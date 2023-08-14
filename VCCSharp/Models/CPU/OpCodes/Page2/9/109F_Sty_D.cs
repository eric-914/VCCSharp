using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //STY
    //Store index register to memory
    public class _109F_Sty_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.MemWrite16(cpu.Y_REG, address);
            cpu.CC_Z = ZTEST(cpu.Y_REG);
            cpu.CC_N = NTEST16(cpu.Y_REG);
            cpu.CC_V = false; //0;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7); //TODO: One of these is wrong?
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
