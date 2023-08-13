using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.F
{
    public class _11FB_Addf_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            ushort sum = (ushort)(cpu.F_REG + value);

            cpu.CC_C = (sum & 0x100) >> 8 != 0;
            cpu.CC_H = ((cpu.F_REG ^ value ^ sum) & 0x10) >> 4 != 0;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, sum, cpu.F_REG);

            cpu.F_REG = (byte)sum;
            
            cpu.CC_N = NTEST8(cpu.F_REG);
            cpu.CC_Z = ZTEST(cpu.F_REG);
            
            cpu.PC_REG += 2;

            return Cycles._65;
        }
    }
}
