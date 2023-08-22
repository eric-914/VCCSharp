using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// LSR
    /// Logical shift right accumulator or memory location
    /// Logical Shift Right of 8-Bit Accumulator or Memory Byte
    /// DIRECT
    ///          ╭──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮
    ///    0 ──▶ │  │  │  │  │  │  │  │  │ ──▶ │  │
    ///          ╰──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
    ///           b7 ────────────────▶ b0       C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// LSR             DIRECT              04          6 / 5       2 
    ///   [E F H I N Z V C]
    ///   [        0 ↕   ↕]
    /// </summary>
    /// <remarks>
    /// These instructions logically shift the contents of the A or B accumulator or a specified byte in memory to the right by one bit, clearing bit 7. 
    /// Bit 0 is shifted into the Carry flag of the Condition Codes register.
    ///         N The Negative flag is cleared by these instructions.
    ///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is not affected by these instructions.
    ///         C The Carry flag receives the value shifted out of bit 0.
    ///        
    /// The LSR instruction can be used in simple division routines on unsigned values (a single right-shift divides the value by 2).
    /// The 6309 does not provide variants of LSR to operate on the E and F accumulators.
    /// 
    /// See Also: LSR (16-bit)
    /// </remarks>
    public class _04_Lsr_D : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.DPADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);

            cpu.CC_C = (value & 1) != 0;
            value >>= 1;
            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = false;

            cpu.MemWrite8(value, address);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._65);
    }
}
