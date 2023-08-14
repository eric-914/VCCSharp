using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.B
{
    //SUBD
    //Subtract memory from D accumulator
    //EXTENDED
    public class B3_Subd_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            ushort _temp16 = cpu.MemRead16(address);
            uint difference = (uint)(cpu.D_REG - _temp16);

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, _temp16, cpu.D_REG);

            cpu.D_REG = (ushort)difference;

            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._75);
    }
}
