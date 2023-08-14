using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.A
{
    //INDEXED
    public class _10A5_Bitd_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            var address = cpu.INDADDRESS(cpu.PC_REG++);
            var value = cpu.MemRead16(address);
            var and = (ushort)(cpu.D_REG & value);

            cpu.CC_N = NTEST16(and);
            cpu.CC_Z = ZTEST(and);
            cpu.CC_V = false;

            return Cycles._76;
        }
    }
}
