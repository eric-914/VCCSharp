using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _0B_Tim_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            throw new NotImplementedException();
        }

        public int Exec(IHD6309 cpu)
        {
            byte mask = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);

            mask &= value;

            cpu.CC_N = NTEST8(mask);
            cpu.CC_Z = ZTEST(mask);
            cpu.CC_V = false;

            return 6;
        }
    }
}
