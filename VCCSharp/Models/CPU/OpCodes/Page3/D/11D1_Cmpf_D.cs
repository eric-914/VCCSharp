using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.D
{
    public class _11D1_Cmpf_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.F_REG - value);

            cpu.CC_C = compare > cpu.F_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.F_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return Cycles._54;
        }
    }
}
