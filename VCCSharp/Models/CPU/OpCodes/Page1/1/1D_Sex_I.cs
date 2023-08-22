using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// SEX
    /// Sign Extend B accumulator into A accumulator
    /// Sign Extend the 8-bit Value in B to a 16-bit Value in D
    /// INHERENT
    ///          ╭──────────────────Accumulator D──────────────────╮ 
    ///      N   │     Accumulator A             Accumulator B     │
    ///     ╭──╮ ╭──┬──┬──┬──┬──┬──┬──┬──╮ ╭──┬──┬──┬──┬──┬──┬──┬──╮ 
    ///     │  │ │  │  │  │  │  │  │  │  │ │b7│  │  │  │  │  │  │  │ 
    ///     ╰──╯ ╰──┴──┴──┴──┴──┴──┴──┴──╯ ╰──┴──┴──┴──┴──┴──┴──┴──╯ 
    ///      ▲    ▲  ▲  ▲  ▲  ▲  ▲  ▲  ▲    │
    ///      ╰────┴──┴──┴──┴──┴──┴──┴──┴────╯
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SEX             INHERENT            1D          2 / 1       1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕    ]
    /// </summary>
    /// <remarks>
    /// This instruction extends the 8-bit twos complement value in Accumulator B into a 16-bit twos complement value in Accumulator D. 
    /// This is accomplished by copying the value of bit 7 (the sign bit) from Accumulator B into all 8 bits of Accumulator A.
    ///     N The Negative flag is also set equal the value of bit 7 in Accumulator B
    ///     Z The Zero flag is set if the new value of Accumulator D is zero (B was zero); cleared otherwise.
    ///     V The Overflow flag is not affected by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    /// 
    /// The SEX instruction is used when a signed (twos complement) 8-bit value needs to be promoted to a full 16-bit value. 
    /// For unsigned arithmetic, promoting an 8-bit value in Accumulator A to a 16-bit value in Accumulator D requires zero-extending the value by executing a CLRA instruction instead.
    /// 
    /// On a 6309, you can sign extend an 8-bit value in Accumulator A to a 32-bit value in Accumulator Q by executing the following sequence of instructions:
    ///         SEX     ; Sign extend A into D
    ///         TFR D,W ; Move D to W
    ///         SEXW    ; Sign extend W into Q
    /// 
    /// See Also: SEXW
    /// </remarks>
    public class _1D_Sex_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.A_REG = (byte)(0 - (cpu.B_REG >> 7));
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
