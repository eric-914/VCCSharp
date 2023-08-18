﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// RORA
    /// Rotate accumulator or memory right
    /// Rotate 8-Bit Accumulator or Memory Byte Right through Carry
    /// INHERENT
    ///     ╭────────────────────────────────────────╮
    ///     │    ╭──┬──┬──┬──┬──┬──┬──┬──╮     ╭──╮  |
    ///     ╰──▶ │  │  │  │  │  │  │  │  | ──▶ |  | -╯
    ///          ╰──┴──┴──┴──┴──┴──┴──┴──╯     ╰──╯
    ///           b7 ────────────────▶ b0       C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// RORA            INHERENT            46          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕   ↕]
    /// </summary>
    /// <remarks>
    /// These instructions rotate the contents of the A or B accumulator or a specified byte in memory to the right by one bit, through the Carry bit of the CC register (effectively a 9- bit rotation). 
    /// Bit 7 receives the original value of the Carry flag, while the Carry flag receives the original value of bit 0.
    ///         N The Negative flag is set equal to the new value of bit 7 (original value of Carry).
    ///         Z The Zero flag is set if the new 8-bit value is zero; cleared otherwise.
    ///         V The Overflow flag is not affected by these instructions.
    ///         C The Carry flag receives the value shifted out of bit 0.
    /// 
    /// The ROR instructions can be used for subsequent bytes of a multi-byte shift to bring in the carry bit from previous shift or rotate instructions. 
    /// Other uses include conversion of data from serial to parallel and vise-versa. 
    /// 
    /// The 6309 does not provide variants of ROR to operate on the E and F accumulators.
    /// 
    /// See Also: ROR (16-bit)
    /// </remarks>
    public class _46_Rora_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var mask = cpu.CC_C ? (byte)0x80 : (byte)0x00; //CC_C << 7;

            cpu.CC_C = (cpu.A_REG & 1) != 0;

            cpu.A_REG = (byte)((cpu.A_REG >> 1) | mask);

            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
