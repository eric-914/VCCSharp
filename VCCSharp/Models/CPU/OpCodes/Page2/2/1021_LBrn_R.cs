using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// LBRN
    /// Branch never
    /// Long Branch Never
    /// RELATIVE
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LBRN address        RELATIVE            1021        5           4
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// This instruction is essentially a no-operation; that is, the CPU never branches but merely advances the Program Counter to the next instruction in sequence. 
    /// None of the Condition Code flags are affected.
    /// 
    /// The LBRN instruction provides a 4-byte no-op that consumes 5 bus cycles, whereas NOP is a single-byte instruction that consumes either 1 or 2 bus cycles. 
    /// In addition, there is the BRN instruction which provides a 2-byte no-op that consumes 3 bus cycles.
    /// 
    /// Since the branch is never taken, the third and fourth bytes of the instruction do not serve any purpose and may contain any value. 
    /// These bytes could contain program code or data that is accessed by some other instruction(s).
    /// 
    /// See Also: BRN, LBRA, NOP
    /// </remarks>
    public class _1021_LBrn_R : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, 5);
    }
}
