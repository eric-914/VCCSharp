using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ROLA
    //Rotate accumulator or memory left
    //INHERENT
    public class _49_Rola_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var mask = cpu.CC_C ? (byte)1 : (byte)0;

            cpu.CC_C = cpu.A_REG > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((cpu.A_REG & 0x40) >> 6 != 0);

            cpu.A_REG = (byte)((cpu.A_REG << 1) | mask);

            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
