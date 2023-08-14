using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //DIRECT
    public class _1092_Sbcd_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            uint difference = (uint)(cpu.D_REG - value - (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, difference, cpu.D_REG, value);
            cpu.D_REG = (ushort)difference;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return Cycles._75;
        }
    }
}
