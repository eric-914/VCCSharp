using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //INHERENT
    public class _1046_Rord_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var bit = cpu.CC_C ? (ushort)0x8000 : (ushort)0x0000; //CC_C<< 15;

            cpu.CC_C = (cpu.D_REG & 1) != 0;
            cpu.D_REG = (ushort)((cpu.D_REG >> 1) | bit);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}
