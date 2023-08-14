using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //IMMEDIATE
    public class _1085_Bitd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);
            ushort and = (ushort)(cpu.D_REG & value);

            cpu.CC_N = NTEST16(and);
            cpu.CC_Z = ZTEST(and);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
