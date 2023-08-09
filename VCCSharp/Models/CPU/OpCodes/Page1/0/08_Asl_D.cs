using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _08_Asl_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var value = cpu.MemRead8(address);

            cpu.CC_C = (value & 0x80) >> 7 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((value & 0x40) >> 6)) != 0;
            value <<= 1;
            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);

            cpu.MemWrite8(value, address);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
