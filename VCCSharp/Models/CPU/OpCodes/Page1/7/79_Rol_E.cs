using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// ROL
    /// Rotate accumulator or memory left
    /// Rotate 8-Bit Accumulator or Memory Byte Left through Carry
    /// EXTENDED
    ///   ╭────────────────────────────────────────╮
    ///   │  ╭──╮     ╭──┬──┬──┬──┬──┬──┬──┬──╮    │
    ///   ╰─ │  │ ◀── │  │  │  │  │  │  │  │  │ ◀──╯
    ///      ╰──╯     ╰──┴──┴──┴──┴──┴──┴──┴──╯
    ///       C        b7 ────────────────▶ b0       
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ROL             EXTENDED            79          7 / 6       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions rotate the contents of the A or B accumulator or a specified byte in memory to the left by one bit, through the Carry bit of the CC register (effectively a 9-bit rotation). 
    /// Bit 0 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 7.
    ///         N The Negative flag is set equal to the new value of bit 7.
    ///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is set equal to the exclusive-OR of the original values of bits 6 and 7.
    ///         C The Carry flag receives the value shifted out of bit 7.
    ///         
    /// The ROL instructions can be used for subsequent bytes of a multi-byte shift to bring in the carry bit from previous shift or rotate instructions. 
    /// Other uses include conversion of data from serial to parallel and vise-versa.
    /// 
    /// The 6309 does not provide variants of ROL to operate on the E and F accumulators.
    /// However, you can achieve the same functionality using the ADCR instruction. 
    /// The instructions ADCR E,E and ADCR F,F will perform a left-rotate operation on the E and F accumulators respectively.
    /// 
    /// See Also: ADCR, ROL (16-bit)
    /// </remarks>
    public class _79_Rol_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            byte mask = cpu.CC_C ? (byte)1 : (byte)0;

            cpu.CC_C = value > 0x7F;
            cpu.CC_V = cpu.CC_C ^ ((value & 0x40) >> 6 != 0);

            value = (byte)((value << 1) | mask);

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
