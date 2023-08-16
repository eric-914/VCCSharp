using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page3.A
{
    /// <summary>
    /// DIVD
    /// 🚫 6309 ONLY 🚫
    /// Signed Divide of Accumulator D by 8-bit value in Memory
    /// INDEXED
    /// ACCB’ ← ACCD ÷ (M)
    /// ACCA’ ← ACCD MOD (M)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// DIVD            INDEXED             11AD        27+*        3+ 
    /// • If a two’s complement overflow occurs, the DIVD instruction uses one fewer cycle than what is shown in the table. 
    ///   If a range overflow occurs, DIVD uses 13 fewer cycles than what is shown in the table.
    ///   
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ ↕ ↕]
    /// </summary>
    /// <remarks>
    /// This instruction divides the 16-bit value in Accumulator D (the dividend) by an 8-bit value contained in a memory byte (the divisor). 
    /// The operation is performed using two’s complement binary arithmetic. 
    /// The 16-bit result consists of the 8-bit quotient placed in Accumulator B and the 8-bit remainder placed in Accumulator A. 
    /// The sign of the remainder is always the same as the sign of the dividend unless the remainder is zero.
    ///         N The Negative flag is set equal to the new value of bit 7 in Accumulator B.
    ///         Z The Zero flag is set if the new value of Accumulator B is zero; cleared otherwise.
    ///         V The Overflow flag is set if an overflow occurred; cleared otherwise.
    ///         C The Carry flag is set if the quotient in Accumulator B is odd; cleared if even.
    ///         
    /// When the value of the specified memory byte (divisor) is zero, a Division-By-Zero exception is triggered. 
    /// This causes the CPU to set bit 7 in the MD register, stack the machine state and jump to the address taken from the Illegal Instruction vector at $FFF0.
    /// Two types of overflow may occur when the DIVD instruction is executed:
    /// 
    /// • A two’s complement overflow occurs when the sign of the resulting quotient is
    ///   incorrect. For example, when 300 is divided by 2, the result of 150 can be represented
    ///   in 8 bits only as an unsigned value. Since DIVD performs a signed operation, it
    ///   interprets the result as -106 and sets the Negative (N) and Overflow (V) flags.
    ///   
    /// • A range overflow occurs when the quotient is larger than can be represented in 8 bits.
    ///   For example, when 900 is divided by 3, the result of 300 exceeds the 8-bit range. In
    ///   this case, the CPU aborts the operation, leaving the accumulators unmodified while
    ///   setting the Overflow flag (V) and clearing the N, Z and C flags.
    ///   
    /// See Also: DIVQ
    /// </remarks>
    public class _11AD_Divd_X : OpCode, IOpCode
    {
        private static IOpCode DivByZero = new DivByZero();

        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort address = cpu.INDADDRESS(cpu.PC_REG++);
            sbyte denominator = (sbyte)cpu.MemRead8(address);

            if (denominator == 0)
            {
                DivByZero.Exec(cpu); //TODO: The cpu cycles for this should be much higher than the 3 returned

                return 3;
            }

            short numerator = (short)cpu.D_REG;
            short result = (short)(numerator / denominator);

            if (result > 255 || result < -256) //Abort
            {
                cpu.CC_V = true;
                cpu.CC_N = false;
                cpu.CC_Z = false;
                cpu.CC_C = false;

                return 19;
            }

            byte remainder = (byte)(numerator % denominator);

            cpu.A_REG = remainder;
            cpu.B_REG = (byte)result;

            if (result > 127 || result < -128)
            {
                cpu.CC_V = true;
                cpu.CC_N = true;
            }
            else
            {
                cpu.CC_Z = ZTEST(cpu.B_REG);
                cpu.CC_N = NTEST8(cpu.B_REG);
                cpu.CC_V = false;
            }

            cpu.CC_C = (cpu.B_REG & 1) != 0;

            return 27;
        }
    }
}
