using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //SYNC
    //Synchronize with interrupt line
    public class _13_Sync_I : OpCode, IOpCode
    {
        public static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.IsSyncWaiting = true;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, cpu.SyncCycle);
        public int Exec(IHD6309 cpu) => Exec(cpu, cpu.SyncCycle);
    }
}
