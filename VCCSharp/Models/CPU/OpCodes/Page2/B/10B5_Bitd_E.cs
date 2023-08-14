using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.B
{
    //EXTENDED
    public class _10B5_Bitd_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            ushort value = cpu.MemRead16(address);
            ushort _temp16 = (ushort)(cpu.D_REG & value);

            cpu.CC_N = NTEST16(_temp16);
            cpu.CC_Z = ZTEST(_temp16);
            cpu.CC_V = false;
            cpu.PC_REG += 2;

            return Cycles._86;
        }
    }
}
