using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //BRN
    //Branch never
    //RELATIVE
    public class _21_Brn_R : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            cpu.PC_REG++;

            return 3;
        }

        public int Exec(IHD6309 cpu)
        {
            cpu.PC_REG++;

            return 3;
        }
    }
}
