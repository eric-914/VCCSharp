using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    public class _13_Sync_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            //_cycleCounter = _gCycleFor;
            //_syncWaiting = 1;

            throw new NotImplementedException();
        }

        public int Exec(IHD6309 cpu)
        {
            //_cycleCounter = _gCycleFor;
            //_syncWaiting = 1;

            throw new NotImplementedException();
        }
    }
}
