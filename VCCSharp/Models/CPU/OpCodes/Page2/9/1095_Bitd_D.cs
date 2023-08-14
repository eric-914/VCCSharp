using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    //DIRECT
    public class _1095_Bitd_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            ushort _temp16 = (ushort)(cpu.D_REG & value);

            cpu.CC_N = NTEST16(_temp16);
            cpu.CC_Z = ZTEST(_temp16);
            cpu.CC_V = false;

            return Cycles._75;
        }
    }
}
