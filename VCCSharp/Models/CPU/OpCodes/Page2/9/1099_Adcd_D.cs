using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _1099_Adcd_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var value = cpu.MemRead16(address);
            var sum = (uint)(cpu.D_REG + value + (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (sum & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, (ushort)sum, cpu.D_REG);
            cpu.CC_H = ((cpu.D_REG ^ sum ^ value) & 0x100) >> 8 != 0;
            cpu.D_REG = (ushort)sum;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return Cycles._75;
        }
    }
}
