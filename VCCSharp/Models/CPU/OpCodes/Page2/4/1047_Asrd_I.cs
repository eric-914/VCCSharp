using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ASRD
    /// --> 6309 ONLY <--
    /// Arithmetic Shift Right of Accumulator D
    /// INHERENT
    ///      ⤿□□□□□□□□□□□□□□□□ → □
    ///  bit 15      →       0   C
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ASRD          INHERENT            1047         3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕   ↕]
    /// </summary>
    /// <remarks>
    /// This instruction shifts the contents of Accumulator D to the right by one bit. 
    /// Bit 0 is     shifted into the Carry flag of the Condition Codes register. 
    /// The value of bit 15 is not changed.
    /// N The Negative flag is set equal to the value of bit 15.
    /// Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
    /// V The Overflow flag is not affected by this instruction.
    /// C The Carry flag receives the value shifted out of bit 0.
    /// The ASRD instruction can be used in simple division routines (a single right-shift divides the value by 2). 
    /// Be careful here, as a right-shift is not the same as a division when the value is negative; it rounds in the wrong direction. 
    /// For example, -5 (FFFB16) divided by 2 should be -2 but, when arithmetically shifted right, is -3 (FFFD16).
    /// 
    /// The 6309 does not provide a variant of ASR to operate on the W accumulator, although it does provide the LSRW instruction for performing a logical shift.
    /// An arithmetic right-shift of the 32-bit Q accumulator can be achieved as follows:
    ///     ASRD ; Shift Hi-word, Low-bit into Carry
    ///     RORW ; Shift Low-word, Carry into Hi-bit
    ///     
    /// See Also: ASR (8-bit), ROR (16-bit)
    /// </remarks>
    public class _1047_Asrd_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.CC_C = (cpu.D_REG & 1) != 0;
            cpu.D_REG = (ushort)((cpu.D_REG & 0x8000) | (cpu.D_REG >> 1));
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_N = NTEST16(cpu.D_REG);

            return Cycles._32;
        }
    }
}
