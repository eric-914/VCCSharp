using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _1081_Cmpw_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort compare = (ushort)(cpu.W_REG - value);

            cpu.CC_C = compare > cpu.W_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, compare, cpu.W_REG, value);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
