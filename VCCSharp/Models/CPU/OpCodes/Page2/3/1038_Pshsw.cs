using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// PSHSW
    /// 🚫 6309 ONLY 🚫
    /// Push Accumulator W onto the Hardware Stack
    /// INHERENT
    ///       S’ ← S - 2
    /// (S:S+1)’ ← ACCW
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PSHSW           INHERENT            1038        6           2
    /// </summary>
    /// <remarks>
    /// This instruction pushes the contents of the W accumulator (E and F) onto the Hardware Stack (S). 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// The PSHSW instruction first decrements hardware stack pointer (S) by one and stores the low-order byte (Accumulator F) at the address pointed to by S. 
    /// The stack pointer is then decremented by one again, and the high-order byte (Accumulator E) is stored.
    /// 
    /// This instruction was included in the 6309 instruction set to supplement the PSHS instruction which does not support the W accumulator.
    /// 
    /// To push either half of the W accumulator onto the hardware stack, you could use the instructions STE ,-S or STF ,-S, however these instructions will set the Condition Code flags to reflect the pushed value.
    /// 
    /// See Also: PSH, PSHUW, PULSW, PULUW
    /// </remarks>
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
