using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //INHERENT
    public class _1059_Rolw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var _postWord = cpu.CC_C ? (ushort)1 : (ushort)0;

            cpu.CC_C = cpu.W_REG >> 15 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((cpu.W_REG & 0x4000) >> 14)) != 0;
            cpu.W_REG = (ushort)((cpu.W_REG << 1) | _postWord);
            cpu.CC_Z = ZTEST(cpu.W_REG);
            cpu.CC_N = NTEST16(cpu.W_REG);

            return Cycles._32;
        }
    }
}
