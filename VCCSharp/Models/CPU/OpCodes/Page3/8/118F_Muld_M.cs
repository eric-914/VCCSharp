using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// MULD
    /// 🚫 6309 ONLY 🚫
    /// Signed Multiply of Accumulator D and Memory Word
    /// IMMEDIATE
    /// ACCQ’ ← ACCD x IMM16|(M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// MULD            IMMEDIATE           118F        28          4 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕    ]
    /// </summary>
    /// <remarks>
    /// This instruction multiplies the signed 16-bit value in Accumulator D by either a 16-bit immediate value or the contents of a double-byte value from memory. 
    /// The signed 32-bit product is placed into Accumulator Q. 
    /// Only two Condition Code flags are affected:
    ///         N The Negative flag is set if the twos complement result is negative; cleared otherwise.
    ///         Z The Zero flag is set if the 32-bit result is zero; cleared otherwise.
    /// 
    /// See Also: MUL
    /// </remarks>
    public class _118F_Muld_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            short value = (short)cpu.MemRead16(cpu.PC_REG);

            cpu.Q_REG = (uint)((short)cpu.D_REG * value);

            cpu.CC_C = false;
            cpu.CC_Z = ZTEST(cpu.Q_REG);
            cpu.CC_V = false;
            cpu.CC_N = NTEST32(cpu.Q_REG);

            cpu.PC_REG += 2;

            return 28;
        }
    }
}
