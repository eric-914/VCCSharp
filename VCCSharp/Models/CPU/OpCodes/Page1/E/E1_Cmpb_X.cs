using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1.E
{
    /// <summary>
    /// CMPB
    /// Compare memory from accumulator 
    /// Compare Memory Byte from 8-Bit Accumulator
    /// INDEXED
    /// TEMP ← r - (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CMPB            INDEXED             E1          4+          2+ 
    ///   [E F H I N Z V C]
    ///   [    ~   ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions subtract the contents of a byte in memory from the value contained in one of the 8-bit accumulators (A,B,E,F) and set the Condition Codes accordingly.
    /// Neither the memory byte nor the accumulator are modified.
    ///         H The affect on the Half-Carry flag is undefined for these instructions.
    ///         N The Negative flag is set equal to the value of bit 7 of the result.
    ///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into bit-7 was needed; cleared otherwise.
    /// 
    /// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
    /// 
    /// The 8-bit CMP instructions perform exactly the same operation as the 8-bit SUB instructions, with the exception that the value in the accumulator is not changed. 
    /// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
    /// 
    /// See Also: CMP (16-bit), CMPR
    /// </remarks>
    public class E1_Cmpb_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.B_REG - value);

            cpu.CC_C = compare > cpu.B_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.B_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 4);
        public int Exec(IHD6309 cpu) => Exec(cpu, 4);
    }
}
