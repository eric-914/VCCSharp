using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.A
{
    //INDEXED
    public class _10A8_Eord_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);

            cpu.D_REG ^= value;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false;

            return Cycles._76;
        }
    }
}
