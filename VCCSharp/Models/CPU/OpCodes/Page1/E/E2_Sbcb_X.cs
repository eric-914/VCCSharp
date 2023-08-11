using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.E
{
    public class E2_Sbcb_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            ushort difference = (ushort)(cpu.B_REG - value - (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (difference & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, difference, cpu.B_REG);

            cpu.B_REG = (byte)difference;

            cpu.CC_N = NTEST8(cpu.B_REG);
            cpu.CC_Z = ZTEST(cpu.B_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
