using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ASLD
    /// --> 6309 ONLY <--
    /// Arithmetic Shift Left of Accumulator D
    /// INHERENT
    /// □   ←   □□□□□□□□□□□□□□□□ ← 0
    /// C  bit 15      ←       0
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// ASLD          INHERENT            1048         3 / 2       2
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// This instruction shifts the contents of Accumulator D to the left by one bit, clearing bit 0.
    /// Bit 15 is shifted into the Carry flag of the Condition Codes register.
    ///     N The Negative flag is set equal to the new value of bit 15; previously bit 14.
    ///     Z The Zero flag is set if the new 16-bit value is zero; cleared otherwise.
    ///     V The Overflow flag is set to the Exclusive-OR of the original values of bits 14 and 15.
    ///     C The Carry flag receives the value shifted out of bit 15.
    /// The ASL instruction can be used for simple multiplication (a single left-shift multiplies the value by 2). 
    /// Other uses include conversion of data from serial to parallel and viseversa.
    /// 
    /// The D accumulator is the only 16-bit register for which an ASL instruction has been provided. 
    /// You can however achieve the same functionality using the ADDR instruction.
    /// For example, ADDR W,W will perform the same left-shift operation on the W accumulator.
    /// 
    /// A left-shift of the 32-bit Q accumulator can be achieved as follows:
    ///     ADDR W,W ; Shift Low-word, Hi-bit into Carry
    ///     ROLD ; Shift Hi-word, Carry into Low-bit
    /// The ASLD and LSLD mnemonics are duplicates. Both produce the same object code.
    /// 
    /// See Also: ASL (8-bit), ROL (16-bit)
    /// </remarks>
    public class _1048_Asld_I : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            cpu.CC_C = cpu.D_REG >> 15 != 0;
            cpu.CC_V = ((cpu.CC_C ? 1 : 0) ^ ((cpu.D_REG & 0x4000) >> 14)) != 0;
            cpu.D_REG = (ushort)(cpu.D_REG << 1);
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return Cycles._32;
        }
    }
}
