using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    //DIRECT
    public class _119B_Adde_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            ushort sum = (ushort)(cpu.E_REG + value);

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.E_REG ^ value ^ sum) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.E_REG);

            cpu.E_REG = (byte)sum;

            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_Z = ZTEST(cpu.E_REG);

            return Cycles._54;
        }
    }
}
