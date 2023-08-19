using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// COMA
    /// Complement accumulator or memory location
    /// INHERENT
    /// r’ ← ~r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// COMA            INHERENT            43          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0 1]
    /// </summary>
    /// <remarks>
    /// Each of these instructions change the value of the specified accumulator to that of it’s logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1.
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is set equal to the new value of the accumulators high-order bit.
    ///         Z The Zero flag is set if the new value of the accumulator is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is always set.
    /// 
    /// This instruction performs a ones-complement operation. 
    /// A twos-complement can be achieved with the NEG instruction.
    /// 
    /// Complementing the Q accumulator requires executing both COMW and COMD.
    /// 
    /// The COMA and COMB instructions provide the smallest, fastest way to set the Carry flag in the CC register.
    /// 
    /// See Also: COM (memory), NEG
    /// </remarks>
    public class _43_Coma_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.A_REG = (byte)(0xFF - cpu.A_REG);

            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_C = true;
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
