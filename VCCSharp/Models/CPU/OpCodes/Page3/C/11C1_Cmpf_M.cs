using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.C
{
    //IMMEDIATE
    public class _11C1_Cmpf_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            byte compare = (byte)(cpu.F_REG - value);

            cpu.CC_C = compare > cpu.F_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.F_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return 3;
        }
    }
}
