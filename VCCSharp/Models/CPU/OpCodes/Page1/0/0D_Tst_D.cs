using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _0D_Tst_D : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.DPADDRESS(cpu.PC_REG++);
            var _temp8 = cpu.MemRead8(address);

            cpu.CC_Z = ZTEST(_temp8);
            cpu.CC_N = NTEST8(_temp8);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._64);
    }
}
