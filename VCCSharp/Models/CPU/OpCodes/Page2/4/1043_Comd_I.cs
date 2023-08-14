using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //COMD
    //INHERENT
    public class _1043_Comd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG = (ushort)(0xFFFF - cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_C = true;
            cpu.CC_V = false;

            return Cycles._32;
        }
    }
}
