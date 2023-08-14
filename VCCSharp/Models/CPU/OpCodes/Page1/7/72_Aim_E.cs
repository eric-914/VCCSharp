using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// AIM
    /// --> 6309 ONLY <--
    /// Logical AND of Immediate Value with Memory Byte
    /// EXTENDED
    /// M’ ← (M) AND IMM
    /// SOURCE FORM   ADDRESSING MODE     OPCODE       CYCLES      BYTE COUNT
    /// AIM #i8;EA    EXTENDED            72           7           4
    /// 
    /// I8 : 8-bit Immediate value
    /// EA : Effective Address
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The AIM instruction logically ANDs the contents of a byte in memory with an 8-bit immediate value. 
    /// The resulting value is placed back into the designated memory location.
    ///     N The Negative flag is set equal to the new value of bit 7 of the memory byte.
    ///     Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
    ///     V The Overflow flag is cleared by this instruction.
    ///     C The Carry flag is not affected by this instruction.
    /// AIM is one of the more useful additions to the 6309 instruction set. 
    /// It takes three separate instructions to perform the same operation on a 6809:
    /// 
    ///     6809 (6 instruction bytes; 12 cycles):
    ///         LDA #$3F
    ///         ANDA 4,U
    ///         STA 4,U
    ///     6309 (3 instruction bytes; 8 cycles):
    ///         AIM #$3F;4,U
    ///     
    /// Note that the assembler syntax used for the AIM operand is non-typical. 
    /// Some assemblers may require a comma (,) rather than a semicolon (;) between the immediate operand and the address operand.
    /// 
    /// The object code format for the AIM instruction is:
    ///     +--------+-------------+------------------------+
    ///     | OPCODE | IMMED VALUE |ADDRESS / INDEX BYTE(S) |
    ///     +--------+-------------+------------------------+
    ///     
    /// See Also: AND, EIM, OIM, TIM
    /// </remarks>
    public class _72_Aim_E : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);
            ushort address = cpu.MemRead16(cpu.PC_REG);
            byte mask = cpu.MemRead8(address);

            value &= mask;
            
            cpu.MemWrite8(value, address);
            
            cpu.CC_N = NTEST8(value);
            cpu.CC_Z = ZTEST(value);
            cpu.CC_V = false;
            
            cpu.PC_REG += 2;

            return 7;
        }
    }
}
