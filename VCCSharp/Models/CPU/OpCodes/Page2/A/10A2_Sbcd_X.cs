﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2.A
{
    /// <summary>
    /// SBCD
    /// 🚫 6309 ONLY 🚫
    /// Subtract Memory Word and Carry from Accumulator D
    /// INDEXED
    /// ACCD’ ← ACCD - IMM16|(M:M+1) - C
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// SBCD            INDEXED             10A2        7+ / 6+     3+ 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// The SBCD instruction subtracts either a 16-bit immediate value or the contents of a double-byte value in memory, plus the value of the Carry flag from the D accumulator.
    /// The 16-bit result is placed back into Accumulator D. 
    /// Note that since subtraction is performed, the purpose of the Carry flag is to represent a Borrow.
    ///         H The Half-Carry flag is not affected by the SBCD instruction.
    ///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
    ///         Z The Zero flag is set if the new value of Accumulator D is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if a borrow into bit 15 was needed; cleared otherwise.
    /// 
    /// The SBCD instruction is most often used to perform subtraction of subsequent words of a multi-byte subtraction. 
    /// This allows the borrow from a previous SUB or SBC instruction to be included when doing subtraction for the next higher-order word.
    /// 
    /// The following instruction sequence is an example showing how 32-bit subtraction can be performed on a 6309 microprocessor:
    ///         LDQ VAL1ADR     ; Q = 32-bit minuend
    ///         SUBW VAL2ADR+2  ; Subtract lower half of subtrahend
    ///         SBCD VAL2ADR    ; Subtract upper half of subtrahend
    ///         STQ RESULT      ; Store difference
    ///         
    /// See Also: SBC (8-bit), SBCR
    /// </remarks>
    public class _10A2_Sbcd_X : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            ushort value = cpu.MemRead16(address);
            uint difference = (uint)(cpu.D_REG - value - (cpu.CC_C ? 1 : 0));

            cpu.CC_C = (difference & 0x10000) >> 16 != 0;
            cpu.CC_V = OVERFLOW16(cpu.CC_C, value, (ushort)difference, cpu.D_REG);

            cpu.D_REG = (ushort)difference;
            
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);

            return Cycles._76;
        }
    }
}
