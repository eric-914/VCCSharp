using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// PULSW
    /// 🚫 6309 ONLY 🚫
    /// Pull Accumulator W from the Hardware Stack
    /// INHERENT
    /// ACCW’ ← (S:S+1)
    ///    S’ ← S + 2
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PULSW           INHERENT            1039        6           2
    /// </summary>
    /// <remarks>
    /// This instruction pulls a value for the W accumulator (E and F) from the Hardware Stack (S). 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// The PULSW instruction first loads the high-order byte (Accumulator E) with the value stored at the address pointed to by the hardware stack pointer (S) and increments the stack pointer by one. 
    /// Next, the low-order byte (Accumulator F) is loaded and the stack pointer is again incremented by one.
    /// 
    /// This instruction was included in the 6309 instruction set to supplement the PULS instruction which does not support the W accumulator.
    /// 
    /// To pull either half of the W accumulator from the hardware stack, you could use the instructions LDE ,S+ or LDF ,S+, however these instructions will set the Condition Code flags to reflect the pulled value.
    /// 
    /// See Also: PSHSW, PSHUW, PUL, PULUW
    /// </remarks>
    public class _1039_Pulsw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.E_REG = cpu.MemRead8(cpu.S_REG++);
            cpu.F_REG = cpu.MemRead8(cpu.S_REG++);

            return 6;
        }
    }
}
