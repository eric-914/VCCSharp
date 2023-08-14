using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ASR
    /// Arithmetic shift of accumulator or memory right 
    /// EXTENDED
    ///     ⤿□□□□□□□□ → □
    ///  bit 7      0   C
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ASR           EXTENDED            77           7 / 6       3
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕   ↕]
    /// </summary>
    /// <remarks>
    /// These instructions arithmetically shift the contents of the A or B accumulator or a specified byte in memory to the right by one bit. 
    /// Bit 0 is shifted into the Carry flag of the Condition Codes register. 
    /// The value of bit 7 is not changed.
    ///     H The affect on the Half-Carry flag is undefined for these instructions.
    ///     N The Negative flag is set equal to the value of bit 7.
    ///     Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
    ///     V The Overflow flag is not affected by these instructions.
    ///     C The Carry flag receives the value shifted out of bit 0.
    /// The ASR instruction can be used in simple division routines (a single right-shift divides the value by 2). 
    /// Be careful here, as a right-shift is not the same as a division when the value is negative; it rounds in the wrong direction. 
    /// For example, -5 (FB16) divided by 2 should be -2 but, when arithmetically shifted right, is -3 (FD16).
    /// 
    /// The 6309 does not provide variants of ASR to operate on the E and F accumulators.
    /// </remarks>
    public class _77_Asr_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var address = cpu.MemRead16(cpu.PC_REG);
            var value = cpu.MemRead8(address);

            cpu.CC_C = (value & 1) != 0;

            value = (byte)((value & 0x80) | (value >> 1));

            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);

            cpu.MemWrite8(value, address);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
