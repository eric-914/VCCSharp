using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _1080_Subw_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort difference = (ushort)(cpu.W_REG - value);

            cpu.CC_C = difference > cpu.W_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, cpu.W_REG, value);
            cpu.CC_N = NTEST16(difference);
            cpu.CC_Z = ZTEST(difference);
            cpu.W_REG = difference;

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
