using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// PSHUW
    /// 🚫 6309 ONLY 🚫
    /// Push Accumulator W onto the User Stack
    /// INHERENT
    ///       U’ ← U - 2
    /// (U:U+1)’ ← ACCW
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PSHSW           INHERENT            1038        6           2
    /// </summary>   
    /// <remarks>
    /// This instruction pushes the contents of the W accumulator (E and F) onto the User Stack (U). 
    /// None of the Condition Code flags are affected by this instruction.
    /// 
    /// The PSHUW instruction first decrements user stack pointer (U) by one and stores the low-order byte (accumulator F) at the address pointed to by U. 
    /// The stack pointer is then decremented by one again, and the high-order byte (accumulator E) is stored.
    /// 
    /// This instruction was included in the 6309 instruction set to supplement the PSHU instruction which does not support the W accumulator.
    /// 
    /// To push either half of the W accumulator onto the user stack, you could use the instructions STE ,-U or STF ,-U, however these instructions will set the Condition Code flags to reflect the pushed value.
    ///  
    /// See Also: PSH, PSHSW, PULSW, PULUW
    /// </remarks>
    public class _103A_Pshuw : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.MemWrite8(cpu.F_REG, --cpu.U_REG);
            cpu.MemWrite8(cpu.E_REG, --cpu.U_REG);

            return 6;
        }
    }
}
