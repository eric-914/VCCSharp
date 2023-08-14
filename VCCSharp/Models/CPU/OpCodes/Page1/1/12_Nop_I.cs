using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //NOP
    //No operation
    //INHERENT
    public class _12_Nop_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            return 2;
        }

        public int Exec(IHD6309 cpu)
        {
            return Cycles._21;
        }
    }
}
