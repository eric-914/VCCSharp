using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ANDD
    /// 🚫 6309 ONLY 🚫
    /// Add Source Register plus Carry to Destination Register
    /// Logically AND Memory Word with Accumulator D
    /// IMMEDIATE
    /// ACCD’ ← ACCD AND (M:M+1)
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ANDD          IMMEDIATE           1084         5 / 4       4 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The ANDD instruction logically ANDs the contents of a double-byte value in memory with the contents of Accumulator D. 
    /// The 16-bit result is placed back into Accumulator D.
    ///     N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
    ///     Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    ///     One use for the ANDD instruction is to truncate bits of an address value. 
    /// For example:
    ///     ANDD #$E000 ;Convert address to that of its 8K page
    ///     
    /// For testing bits, it is often preferable to use the BITD instruction instead, since it performs the same logical AND operation without modifying the contents of Accumulator D.
    /// 
    /// See Also: AND (8-bit), ANDCC, ANDR, BITD
    /// </remarks>
    public class _1084_Andd_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);

            cpu.D_REG &= value;

            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false;

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
