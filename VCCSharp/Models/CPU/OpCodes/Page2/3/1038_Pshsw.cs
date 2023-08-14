using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    // --[HITACHI]--
    //PSHSW
    public class _1038_Pshsw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.MemWrite8(cpu.F_REG, --cpu.S_REG);
            cpu.MemWrite8(cpu.E_REG, --cpu.S_REG);

            return 6;
        }
    }
}
