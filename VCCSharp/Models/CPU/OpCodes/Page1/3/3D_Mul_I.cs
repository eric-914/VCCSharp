using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// MUL
    /// Unsigned multiply (A x B —> D)
    /// Unsigned Multiply of Accumulator A and Accumulator B
    /// INHERENT
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// MUL             INHERENT            3D          11 / 10     1
    /// ACCD’ ← ACCA * ACCB
    ///   [E F H I N Z V C]
    ///   [          ↕   ↕]
    /// </summary>
    /// <remarks>
    /// This instruction multiplies the unsigned 8-bit value in Accumulator A by the unsigned 8-bit value in Accumulator B. 
    /// The 16-bit unsigned product is placed into Accumulator D.
    /// Only two Condition Code flags are affected:
    ///         Z The Zero flag is set if the 16-bit result is zero; cleared otherwise.
    ///         C The Carry flag is set equal to the new value of bit 7 in Accumulator B.
    ///         
    /// The Carry flag is set equal to bit 7 of the least-significant byte so that rounding of the most-significant byte can be accomplished by executing:
    ///         ADCA #0
    ///         
    /// See Also: ADCA, MULD
    /// </remarks>
    public class _3D_Mul_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.D_REG = (ushort)(cpu.A_REG * cpu.B_REG);
            cpu.CC_C = cpu.B_REG > 0x7F;
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 11);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._1110);
    }
}
