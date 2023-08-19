using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// COM
    /// Complement accumulator or memory location
    /// Complement a Byte in Memory
    /// EXTENDED
    /// (M)’ ← ~(M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// COM             EXTENDED            73          7 / 6       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0 1]
    /// </summary>
    /// <remarks>
    /// This instruction changes the value of a byte in memory to that of it’s logical complement; that is each 1 bit is changed to a 0, and each 0 bit is changed to a 1. 
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is set equal to the new value of bit 7.
    ///         Z The Zero flag is set if the new value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is always set.
    /// 
    /// This instruction performs a ones-complement operation. 
    /// A twos-complement can be achieved with the NEG instruction.
    /// 
    /// See Also: COM (accumulator), NEG
    /// </remarks>
    public class _73_Com_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);

            value = (byte)(0xFF - value);

            cpu.CC_Z = ZTEST(value);
            cpu.CC_N = NTEST8(value);
            cpu.CC_C = true;
            cpu.CC_V = false;

            cpu.MemWrite8(value, address);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
