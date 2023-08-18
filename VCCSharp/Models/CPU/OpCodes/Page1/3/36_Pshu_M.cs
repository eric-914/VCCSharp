using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// PSHU
    /// Push A, B, CC, DP, D, X, Y, U, or PC onto hardware stack
    /// Push Registers onto a Stack
    /// IMMEDIATE
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PSHU r0,r1,...rN    IMMEDIATE           36          5+ / 4+     2
    /// PSHU #i8            IMMEDIATE           36          5+ / 4+     2
    /// (One additional cycle is used for each BYTE pushed)
    /// I8 : 8-bit Immediate value
    /// </summary>
    /// <remarks>
    /// These instructions push the current values of none, one or multiple registers onto either the Hardware (PSHS) or User (PSHU) stack. 
    /// None of the Condition Code flags are affected by these instructions.
    /// 
    /// Only the registers present in the 6809 architecture can be pushed by these instructions.
    /// Additionally, the stack pointer used by the instruction (S or U) cannot be pushed. 
    /// Each register specified in the operand field is pushed onto the stack one at a time in the order shown in the figure below (the order you list them in the operand field is irrelevant).
    /// 
    ///                 Lower Memory Addresses
    ///                     ▲   CC
    ///                     │   A
    ///                     │   B
    ///                     │   DP
    ///                     │   X
    ///                     │   Y
    ///                     │   U or S
    ///                     ╵   PC
    ///                 Higher Memory Addresses
    /// 
    /// For each 8-bit register specified, the stack pointer is decremented by one and the register’s value is stored in the memory location pointed to by the stack pointer. 
    /// For each 16-bit register specified, the stack pointer is decremented by one, the register’s low-order byte is stored, the stack pointer is again decremented by one and the register’s high-order byte is then stored.
    /// 
    /// The PSH instructions use a postbyte wherein each bit position corresponds to one of the registers which may be pushed. Bits that are set (1) specify the registers to be pushed.
    /// 
    ///             ╭───┬───┬───┬───┬───┬───┬───┬───╮
    /// POSTBYTE:   | PC|U/S| Y | X | DP| B | A | CC|
    ///             ╰───┴───┴───┴───┴───┴───┴───┴───╯
    ///               7                           0
    ///               
    /// See Also: PSHSW, PSHUW, PUL
    /// </remarks>
    public class _36_Pshu_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            var _postByte = cpu.MemRead8(cpu.PC_REG++);

            if ((_postByte & 0x80) != 0)
            {
                cpu.MemWrite8(cpu.PC_L, --cpu.U_REG);
                cpu.MemWrite8(cpu.PC_H, --cpu.U_REG);

                cycles += 2;
            }

            if ((_postByte & 0x40) != 0)
            {
                cpu.MemWrite8(cpu.S_L, --cpu.U_REG);
                cpu.MemWrite8(cpu.S_H, --cpu.U_REG);

                cycles += 2;
            }

            if ((_postByte & 0x20) != 0)
            {
                cpu.MemWrite8(cpu.Y_L, --cpu.U_REG);
                cpu.MemWrite8(cpu.Y_H, --cpu.U_REG);

                cycles += 2;
            }

            if ((_postByte & 0x10) != 0)
            {
                cpu.MemWrite8(cpu.X_L, --cpu.U_REG);
                cpu.MemWrite8(cpu.X_H, --cpu.U_REG);

                cycles += 2;
            }

            if ((_postByte & 0x08) != 0)
            {
                cpu.MemWrite8(cpu.DPA, --cpu.U_REG);

                cycles += 1;
            }

            if ((_postByte & 0x04) != 0)
            {
                cpu.MemWrite8(cpu.B_REG, --cpu.U_REG);

                cycles += 1;
            }

            if ((_postByte & 0x02) != 0)
            {
                cpu.MemWrite8(cpu.A_REG, --cpu.U_REG);

                cycles += 1;
            }

            if ((_postByte & 0x01) != 0)
            {
                cpu.MemWrite8(cpu.CC, --cpu.U_REG);

                cycles += 1;
            }

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
