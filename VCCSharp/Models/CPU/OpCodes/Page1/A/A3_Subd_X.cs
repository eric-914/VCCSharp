using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.A
{
    //SUBD
    //Subtract memory from D accumulator
    //INDEXED
    public class A3_Subd_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            uint difference = (uint)(cpu.D_REG - value);

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, value, cpu.D_REG);

            cpu.D_REG = (ushort)difference;

            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
