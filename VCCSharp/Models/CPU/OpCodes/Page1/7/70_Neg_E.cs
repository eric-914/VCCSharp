using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// NEG
    /// Negate accumulator or memory
    /// Negate (Twos Complement) a Byte in Memory
    /// EXTENDED
    /// (M)’ ← 0 - (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// NEG             EXTENDED            70          7 / 6       3
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// This instruction changes the value of a byte in memory to that of it’s twos-complement; that is the value which when added to the original value produces a sum of zero. 
    /// The Condition Code flags are also modified as follows:
    ///         N The Negative flag is set equal to the new value of bit 7.
    ///         Z The Zero flag is set if the new value is zero; cleared otherwise.
    ///         V The Overflow flag is set if the original value was 8016; cleared otherwise.
    ///         C The Carry flag is cleared if the original value was 0; set otherwise.
    /// 
    /// The operation performed by the NEG instruction can be expressed as:
    ///         result = 0 - value
    ///         
    /// The Carry flag represents a Borrow for this operation and is therefore always set unless the memory byte’s original value was zero.
    /// 
    /// If the original value of the memory byte is 8016 then the Overflow flag (V) is set and the byte’s value is not modified.
    /// 
    /// This instruction performs a twos-complement operation. 
    /// A ones-complement can be achieved with the COM instruction.
    /// 
    /// See Also: COM, NEG (accumulator)
    /// </remarks>
    public class _70_Neg_E : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte value = cpu.MemRead8(address);
            byte negative = (byte)(0 - value);

            cpu.CC_C = negative > 0;
            cpu.CC_V = value == 0x80;
            cpu.CC_N = NTEST8(negative);
            cpu.CC_Z = ZTEST(negative);

            cpu.MemWrite8(negative, address);

            cpu.PC_REG += 2;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
