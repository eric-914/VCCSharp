using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //CMPW
    //DIRECT
    public class _1091_Cmpw_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            ushort compare = (ushort)(cpu.W_REG - value);

            cpu.CC_C = compare > cpu.W_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.W_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            return Cycles._75;
        }
    }
}
