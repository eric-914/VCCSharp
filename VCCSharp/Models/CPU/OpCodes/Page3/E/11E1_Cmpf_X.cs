using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.E
{
    /// <summary>
    /// CMPF
    /// 🚫 6309 ONLY 🚫
    /// Compare memory from accumulator 
    /// Compare Memory Byte from 8-Bit Accumulator
    /// INDEXED
    /// TEMP ← r - (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CMPF            INDEXED             11E1        5+          3+ 
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
    public class _11E1_Cmpf_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            byte value = cpu.MemRead8(address);
            byte compare = (byte)(cpu.F_REG - value);

            cpu.CC_C = compare > cpu.F_REG;
            cpu.CC_V = OVERFLOW8(cpu.CC_C, value, compare, cpu.F_REG);
            cpu.CC_N = NTEST8(compare);
            cpu.CC_Z = ZTEST(compare);

            return 5;
        }
    }
}
