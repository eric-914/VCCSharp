using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //RORW
    //INHERENT
    public class _1056_Rorw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var bit = (ushort)((cpu.CC_C ? 1 : 0) << 15);

            cpu.CC_C = (cpu.W_REG & 1) != 0;
            cpu.W_REG = (ushort)((cpu.W_REG >> 1) | bit);
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);

            return Cycles._32;
        }
    }
}
