using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.D
{
    // --[HITACHI]--
    //LDQ
    //DIRECT
    public class _10DC_Ldq_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);

            cpu.Q_REG = cpu.MemRead32(address);

            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_N = NTEST32(cpu.Q_REG);
            cpu.CC_V = false;

            return Cycles._87;
        }
    }
}
