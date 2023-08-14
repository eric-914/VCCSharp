using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    //CWAI
    //AND condition code register, then wait for interrupt
    public class _3C_Cwai_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);

            cpu.CC &= _postByte;

            cpu.IsSyncWaiting = true;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, cpu.SyncCycle);
        public int Exec(IHD6309 cpu)=> Exec(cpu, cpu.SyncCycle);
    }
}
