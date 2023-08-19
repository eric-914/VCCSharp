﻿using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// TSTA
    /// Test accumulator or memory location 
    /// Test Value in Accumulator
    /// INHERENT
    /// TEMP ← r
    /// SOURCE FORM     ADDRESSING MODE     OPCODE       CYCLES     BYTE COUNT
    /// TSTA            INHERENT            4D           2 / 1      1
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The TST instructions test the value in an accumulator to setup the Condition Codes register with minimal status for that value. 
    /// The accumulator itself is not modified by these instructions.
    ///         N The Negative flag is set equal to the value of the accumulator’s high-order bit (sign bit).
    ///         Z The Zero flag is set if the accumulator’s value is zero; cleared otherwise.
    ///         V The Overflow flag is always cleared.
    ///         C The Carry flag is not affected by these instructions.
    ///         
    /// For unsigned values, the only meaningful information provided is whether or not the value is zero. 
    /// In this case, BEQ or BNE would typically follow such a test. 
    /// 
    /// For signed (twos complement) values, the information provided is sufficient to allow any of the signed conditional branches (BGE, BGT, BLE, BLT) to be used as though the accumulator’s value had been compared with zero. 
    /// You can also use BMI and BPL to branch according to the sign of the value.
    /// 
    /// To determine the sign of a 16-bit or 32-bit value, you only need to test the high order byte. 
    /// For example, TSTA is sufficient for determining the sign of a 32-bit twos complement value in accumulator Q. 
    /// A full test of accumulator Q could be accomplished by storing it to a scratchpad RAM location (or ROM address). 
    /// In a traditional stack environment, the instruction STQ -4,S may be acceptable.
    /// 
    /// See Also: CMP, STQ, TST (memory)
    /// </remarks>
    public class _4D_Tsta_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            cpu.CC_Z = ZTEST(cpu.A_REG);
            cpu.CC_N = NTEST8(cpu.A_REG);
            cpu.CC_V = false;

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 2);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._21);
    }
}
