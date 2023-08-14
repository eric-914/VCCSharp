using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //DIRECT
    public class _92_Scba_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            ushort compare = (ushort)(cpu.A_REG - value - (cpu.CC_C ? (byte)1 : (byte)0));

            cpu.CC_C = (compare & 0x100) >> 8 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.A_REG);

            cpu.A_REG = (byte)compare;

            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_Z = ZTEST(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._43);
    }
}
