using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// COMW
    /// 🚫 6309 ONLY 🚫
    /// Complement accumulator or memory location
    /// INHERENT
    /// r’ ← ~r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// COMW            INHERENT            1053        3 / 2       2
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
    public class _1053_Comw_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.D_REG = (ushort)(0 - cpu.D_REG);
            cpu.CC_C = cpu.D_REG > 0;
            cpu.CC_V = cpu.D_REG == 0x8000;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return Cycles._32;
        }
    }
}
