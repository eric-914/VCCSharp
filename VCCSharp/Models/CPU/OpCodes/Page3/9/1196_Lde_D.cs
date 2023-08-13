using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _1196_Lde_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.E_REG = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(cpu.E_REG);
            cpu.CC_N = NTEST8(cpu.E_REG);
            cpu.CC_V = false;

            return Cycles._54;
        }
    }
}
