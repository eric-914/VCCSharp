using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.D
{
    //DIRECT
    public class _11D6_Ldf_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.F_REG = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(cpu.F_REG);
            cpu.CC_N = NTEST8(cpu.F_REG);
            cpu.CC_V = false;

            return Cycles._54;
        }
    }
}
