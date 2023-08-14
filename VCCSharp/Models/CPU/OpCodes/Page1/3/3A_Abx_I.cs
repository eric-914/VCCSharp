using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ABX
    /// Add B accumulator to X (unsigned)
    /// INHERENT
    /// Add Accumulator B to Index Register X
    /// X’ ← X + ACCB
    /// SOURCE FORM   ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ABX           INHERENT            3A          3 / 1       1
    ///   [E F H I N Z V C]
    ///   [               ]
    /// </summary>
    /// <remarks>
    /// The ABX instruction performs an unsigned addition of the contents of Accumulator B with the contents of Index Register X. 
    /// The 16-bit result is placed into Index Register X.
    /// None of the Condition Code flags are affected.
    /// The ABX instruction is similar in function to the LEAX B,X instruction. 
    /// A significant difference is that LEAX B,X treats B as a twos complement value (signed), whereas ABX treats B as unsigned. 
    /// For example, if X were to contain 301B16 and B were to contain FF16, then ABX would produce 311A16 in X, whereas LEAX B,X would produce 301A16 in X.
    /// Additionally, the ABX instruction does not affect any flags in the Condition Codes register, whereas the LEAX instruction does affect the Zero flag.
    /// One example of a situation where the ABX instruction may be used is when X contains the base address of a data structure or array and B contains an offset to a specific field or array element. 
    /// In this scenario, ABX will modify X to point directly to the field or array element.
    /// The ABX instruction was included in the 6x09 instruction set for compatibility with the 6801 microprocessor.
    /// </remarks>
    public class _3A_Abx_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.X_REG += cpu.B_REG;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 3);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._31);
    }
}
