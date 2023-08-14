using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    //INDEXED
    public class _11AF_Muld_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            short value = (short)cpu.MemRead16(address);

            cpu.Q_REG = (ushort)((short)cpu.D_REG * value);

            cpu.CC_C = false;
            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_V = false;
            cpu.CC_N = NTEST32(cpu.Q_REG);

            return 30;
        }
    }
}
