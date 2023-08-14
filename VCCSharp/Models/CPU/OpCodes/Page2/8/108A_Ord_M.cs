using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //ORD
    //IMMEDIATE
    public class _108A_Ord_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);

            cpu.D_REG |= value;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false; 

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
