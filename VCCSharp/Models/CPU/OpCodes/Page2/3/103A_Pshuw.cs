using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    public class _103A_Pshuw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.MemWrite8(cpu.F_REG, --cpu.U_REG);
            cpu.MemWrite8(cpu.E_REG, --cpu.U_REG);

            return 6;
        }
    }
}
