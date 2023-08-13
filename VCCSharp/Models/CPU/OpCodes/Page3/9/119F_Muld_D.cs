using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    public class _119F_Muld_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            short value = (short)cpu.MemRead16(address);

            cpu.Q_REG = (uint)((short)cpu.D_REG * value);

            cpu.CC_C = false;
            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_V = false;
            cpu.CC_N = NTEST32(cpu.Q_REG);

            return Cycles._3029;  //--TODO: This implies a 6809 implementation?
        }
    }
}
