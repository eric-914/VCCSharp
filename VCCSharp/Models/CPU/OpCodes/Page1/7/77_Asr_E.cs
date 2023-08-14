using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //ASR
    //Arithmetic shift of accumulator or memory right 
    //EXTENDED
    public class _77_Asr_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.MemRead16(cpu.PC_REG);
            var value = cpu.MemRead8(address);

            cpu.CC_C = (value & 1) != 0;

            value = (byte)((value & 0x80) | (value >> 1));

            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);

            cpu.MemWrite8(value, address);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
