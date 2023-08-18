using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// LEAY
    /// Load effective address into index register 
    /// Load Effective Address
    /// INDEXED
    /// r’ ← EA
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LEAY            INDEXED             31          4+          2+
    ///   [E F H I N Z V C]
    ///   [          *    ]
    ///   * The Z flag is updated by LEAX and LEAY only.
    /// </summary>
    /// <remarks>
    /// These instructions compute the effective address from an Indexed Addressing Mode operand and place that address into one of the Stack Pointers (S or U) or one of the Index Registers (X or Y).
    /// 
    /// The LEAS and LEAU instructions do not affect any of the Condition Code flags. 
    /// The LEAX and LEAY instructions set the Z flag when the effective address is 0 and clear it otherwise. 
    /// This permits X and Y to be used as 16-bit loop counters as well as providing compatibility with the INX and DEX instructions of the 6800 microprocessor.
    /// 
    /// LEA instructions differ from LD instructions in that the value loaded into the register is the address specified by the operand rather than the data pointed to by the address. 
    /// LEA instructions might be used when you need to pass a parameter by-reference as opposed to by-value.
    /// 
    /// The LEA instructions can be quite versatile. 
    /// For example, adding the contents of Accumulator B to Index Register Y and depositing the result in the User Stack pointer (U) can be accomplished with the single instruction:
    ///         LEAU B,Y
    ///         
    /// NOTE: The effective address of an auto-increment operand is the value prior to incrementing. 
    /// Therefore, an instruction such as LEAX ,X+ will leave X unmodified. 
    /// To achieve the expected results, you can use LEAX 1,X instead.
    /// 
    /// See Also: ADDR, LD (16-bit), SUBR
    /// </remarks>
    public class _31_Leay_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.Y_REG = cpu.INDADDRESS(cpu.PC_REG++);

            cpu.CC_Z = ZTEST(cpu.Y_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
