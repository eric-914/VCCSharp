using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// OIM
    /// 🚫 6309 ONLY 🚫
    /// Logical OR of Immediate Value with Memory Byte
    /// DIRECT
    /// (M)’ ← (M) OR IMM
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// OIM #i8;EA      DIRECT              01          6           3 
    ///                 
    /// I8 : 8-bit Immediate value
    ///   [E F H I N Z V C]
    ///   [        ↕ ↕ 0  ]
    /// </summary>
    /// <remarks>
    /// The OIM instruction logically ORs the contents of a byte in memory with an 8-bit immediate value. 
    /// The resulting value is placed back into the designated memory location.
    ///         N The Negative flag is set equal to the new value of bit 7 of the memory byte.
    ///         Z The Zero flag is set if the new value of the memory byte is zero; cleared otherwise.
    ///         V The Overflow flag is cleared by this instruction.
    ///         C The Carry flag is not affected by this instruction.
    /// 
    /// OIM is one of the instructions added to the 6309 which allow logical operations to be performed directly in memory instead of having to use an accumulator. 
    /// It takes three separate instructions to perform the same operation on a 6809:
    ///     6809 (6 instruction bytes; 12 cycles):
    ///         LDA #$C0
    ///         ORA 4,U
    ///         STA 4,U
    ///     6309 (3 instruction bytes; 8 cycles):
    ///         OIM #$C0;4,U
    ///         
    /// Note that the assembler syntax used for the OIM operand is non-typical. 
    /// Some assemblers may require a comma (,) rather than a semicolon (;) between the immediate operand and the address operand.
    /// 
    /// The object code format for the OIM instruction is:
    ///     ╭────────┬─────────────┬─────────────────────────╮
    ///     │ OPCODE │ IMMED VALUE │ ADDRESS / INDEX BYTE(S) │
    ///     ╰────────┴─────────────┴─────────────────────────╯
    ///     
    /// See Also: AIM, EIM, TIM
    /// </remarks>
    public class _01_Oim_D : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu) => throw new NotImplementedException();

        public int Exec(IHD6309 cpu)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);
            ushort _temp16 = cpu.DPADDRESS(cpu.PC_REG++);

            _postByte |= cpu.MemRead8(_temp16);

            cpu.MemWrite8(_postByte, _temp16);

            cpu.CC_N = NTEST8(_postByte);
            cpu.CC_Z = ZTEST(_postByte);
            cpu.CC_V = false;

            return 6;
        }
    }
}
