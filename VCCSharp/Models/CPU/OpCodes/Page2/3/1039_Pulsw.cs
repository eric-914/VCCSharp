using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _1039_Pulsw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.E_REG = cpu.MemRead8(cpu.S_REG++);
            cpu.F_REG = cpu.MemRead8(cpu.S_REG++);

            return 6;
        }
    }
}
