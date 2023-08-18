using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page2
{
    /// <summary>
    /// ORD
    /// 🚫 6309 ONLY 🚫
    /// Logically OR Accumulator D with Word from Memory
    /// IMMEDIATE
    /// ACCD’ ← ACCD OR (M:M+1)
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// ORD             IMMEDIATE           108A        5 / 4       4 
    ///                 DIRECT              109A        7 / 5       3 
    ///                 INDEXED             10AA        7+ / 6+     3+ 
    ///                 EXTENDED            10BA        8 / 6       4
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The ORD instruction logically ORs the contents of Accumulator D with a double-byte value from memory. 
    /// The 16-bit result is placed back into Accumulator D.
    ///         N The Negative flag is set equal to the new value of bit 15 of Accumulator D.
    ///         Z The Zero flag is set if the new value of the Accumulator D is zero; cleared otherwise.
    ///         V The Overflow flag is cleared by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    /// 
    /// The ORD instruction is commonly used for setting specific bits in the accumulator to '1' while leaving other bits unchanged.
    /// 
    /// When using an immediate operand, it is possible to optimize code by determining if the value will only affect half of the accumulator. 
    /// For example: 
    ///         ORD #$1E00
    ///         
    /// could be replaced with:
    ///         ORA #$1E
    ///         
    /// To ensure that the Negative (N) condition code is set correctly, this optimization must not be made if it would result in an ORB instruction that sets bit 7.
    /// 
    /// See Also: BIOR, BOR, OIM, OR (8-bit), ORCC, ORR
    /// </remarks>
    public class _108A_Ord_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            ushort value = cpu.MemRead16(cpu.PC_REG);

            cpu.D_REG |= value;
            cpu.CC_N = NTEST16(cpu.D_REG);
            cpu.CC_Z = ZTEST(cpu.D_REG);
            cpu.CC_V = false; 

            cpu.PC_REG += 2;

            return Cycles._54;
        }
    }
}
