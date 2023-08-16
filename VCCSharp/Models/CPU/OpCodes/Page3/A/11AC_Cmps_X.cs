using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    /// <summary>
    /// CMPS
    /// Compare memory from stack pointer
    /// Compare Memory Word from 16-Bit Register
    /// INDEXED
    /// TEMP ← r - (M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// CMPS            INDEXED             11AC        7+ / 6+     3+ 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// These instructions subtract the contents of a double-byte value in memory from the value contained in one of the 16-bit accumulators (D,W) or one of the Index/Stack registers (X,Y,U,S) and set the Condition Codes accordingly. 
    /// Neither the memory bytes nor the register are modified unless an auto-increment / auto-decrement addressing mode is used with the same register.
    ///         H The Half-Carry flag is not affected by these instructions.
    ///         N The Negative flag is set equal to the value of bit 15 of the result.
    ///         Z The Zero flag is set if the resulting value is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into bit 15 was needed; cleared otherwise
    /// 
    /// The Compare instructions are usually used to set the Condition Code flags prior to executing a conditional branch instruction.
    /// 
    /// The 16-bit CMP instructions for accumulators perform exactly the same operation as the 16-bit SUB instructions, with the exception that the value in the accumulator is not changed. 
    /// Note that since a subtraction is performed, the Carry flag actually represents a Borrow.
    /// 
    /// See Also: CMP (8-bit), CMPR
    /// </remarks>
    public class _11AC_Cmps_X : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            ushort compare = (ushort)(cpu.S_REG - value);

            cpu.CC_C = compare > cpu.S_REG;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, compare, cpu.S_REG);
            cpu.CC_N = NTEST16(compare);
            cpu.CC_Z = ZTEST(compare);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 7);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._76);
    }
}
