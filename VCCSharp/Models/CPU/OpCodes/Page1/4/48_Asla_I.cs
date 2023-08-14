using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ASLA
    //Arithmetic shift of accumulator or memory 
    //LSLA
    //Logical shift left accumulator or memory location
    //INHERENT
    public class _48_Asla_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.CC_C = cpu.A_REG > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((cpu.A_REG & 0x40) >> 6 != 0);

            cpu.A_REG = (byte)(cpu.A_REG << 1);

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
