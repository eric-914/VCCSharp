using Newtonsoft.Json.Linq;
using System;
using System.Numerics;
using System.Security.Cryptography;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules.TCC1014;

namespace VCCSharp.Models.CPU.OpCodes.Page3
{
    /// <summary>
    /// DIVQ
    /// 🚫 6309 ONLY 🚫
    /// Signed Divide of Accumulator Q by 16-bit value in Memory
    /// IMMEDIATE
    /// ACCW’ ← ACCQ ÷ (M:M+1)
    /// ACCD’ ← ACCQ MOD (M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// DIVQ            IMMEDIATE           118E        34*         4 
    /// • When a range overflow occurs, the DIVQ instruction uses 21 fewer cycles than what is shown in the table.
    /// 
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// This instruction divides the 32-bit value in Accumulator Q (the dividend) by a 16-bit value contained in memory (the divisor). 
    /// The operation is performed using two’s complement binary arithmetic. 
    /// The 32-bit result consists of the 16-bit quotient placed in Accumulator W and the 16-bit remainder placed in Accumulator D. 
    /// The sign of the remainder is always the same as the sign of the dividend unless the remainder is zero.
    ///         N The Negative flag is set equal to the new value of bit 15 in Accumulator W.
    ///         Z The Zero flag is set if the new value of Accumulator W is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if the quotient in Accumulator W is odd; cleared if even.    
    /// 
    /// When the value of the specified memory byte (divisor) is zero, a Division-By-Zero exception is triggered. 
    /// This causes the CPU to set bit 7 in the MD register, stack the machine state and jump to the address taken from the Illegal Instruction vector at $FFF0.
    /// 
    /// Two types of overflow are possible when the DIVQ instruction is executed:
    /// 
    /// • A two’s complement overflow occurs when the sign of the resulting quotient is
    ///   incorrect. For example, when 80,000 is divided by 2, the result of 40,000 can be
    ///   represented in 16 bits only as an unsigned value. Since DIVQ is a signed operation, it
    ///   interprets the result as -25,536 and sets the Negative (N) and Overflow (V) flags.
    /// 
    /// • A range overflow occurs when the quotient is larger than can be represented in 16
    ///   bits. For example, when 210,000 is divided by 3, the result of 70,000 exceeds the 16-
    ///   bit range. In this case, the CPU aborts the operation, leaving the accumulators
    ///   unmodified while setting the Overflow flag (V) and clearing the N, Z and C flags.
    ///   
    /// See Also: DIVD
    /// </remarks>
    public class _118E_Divq_M : OpCode, IOpCode
    {
        private static IOpCode DivByZero = new DivByZero();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            short denominator = (short)cpu.MemRead16(cpu.PC_REG);

            cpu.PC_REG += 2;

            if (denominator == 0)
            {
                DivByZero.Exec(cpu); //TODO: The cpu cycles for this should be much higher than the 4 returned

                return 4;
            }

            int numerator = (int)cpu.Q_REG;
            int result = numerator / denominator;

            if (result > 65535 || result < -65536) //Abort
            {
                cpu.CC_V = true;
                cpu.CC_N = false;
                cpu.CC_Z = false;
                cpu.CC_C = false;

                return 13; //34 - 21
            }

            int remainder = numerator % denominator;

            cpu.D_REG = (ushort)remainder;
            cpu.W_REG = (ushort)result;

            //TODO: Originally "result" was "_signedShort" which doesn't make sense.
            if (result > 32767 || result < -32768)
            {
                cpu.CC_V = true;
                cpu.CC_N = true;
            }
            else
            {
                cpu.CC_Z = ZTEST(cpu.W_REG);
                cpu.CC_N = NTEST16(cpu.W_REG);
                cpu.CC_V = false;
            }

            cpu.CC_C = (cpu.B_REG & 1) != 0;

            return 34;
        }
    }
}
