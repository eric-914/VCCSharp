using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// PULUW
    /// 🚫 6309 ONLY 🚫
    /// Pull Accumulator W from the User Stack
    /// INHERENT
    /// ACCW’ ← (U:U+1)
    ///    U’ ← U + 2
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PULUW           INHERENT            103B        6           2
    /// </summary>
    /// <remarks>
    /// This instruction pulls a value for the W accumulator (E and F) from the User Stack (U).
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// The PULUW instruction first loads the high-order byte (Accumulator E) with the value stored at the address pointed to by the user stack pointer (U) and increments the stack pointer by one. 
    /// Next, the low-order byte (Accumulator F) is loaded and the stack pointer is again incremented by one.
    /// 
    /// This instruction was included in the 6309 instruction set to supplement the PULU instruction which does not support the W accumulator.
    /// 
    /// To pull either half of the W accumulator from the user stack, you could use the instructions LDE ,U+ or LDF ,U+, however these instructions will set the Condition Code flags to reflect the pulled value.
    /// 
    /// See Also: PSHSW, PSHUW, PUL, PULSW
    /// </remarks>
    public class _103B_Puluw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.E_REG = cpu.MemRead8(cpu.U_REG++);
            cpu.F_REG = cpu.MemRead8(cpu.U_REG++);

            return 6;
        }
    }
}
