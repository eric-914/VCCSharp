using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// PULS
    /// Pull A, B, CC, DP, D, X, Y, U or PC from hardware stack
    /// Pull Registers from Stack
    /// IMMEDIATE
    /// SOURCE FORM         ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// PULS r0,r1,...rN    IMMEDIATE 35 5+ / 4+ 2
    /// PULS #i8            IMMEDIATE 35 5+ / 4+ 2
    /// (One additional cycle is used for each BYTE pulled.)
    /// I8 : 8-bit Immediate value
    /// </summary>
    /// <remarks>
    /// These instructions pull values for none, one or multiple registers from either the Hardware (PULS) or User (PULU) stack. 
    /// None of the Condition Code flags are affected by these instructions unless the CC register is specified as one of the registers to pull.
    /// 
    /// Only the registers present in the 6809 architecture can be pulled by these instructions.
    /// The stack pointer used by the instruction (S or U) cannot be pulled. 
    /// A value is pulled from the stack for each register specified in the operand field one at a time in the order shown below (the order you list them in the operand field is irrelevant).
    /// 
    ///                 Lower Memory Addresses
    ///                     ╷   CC
    ///                     │   A
    ///                     │   B
    ///                     │   DP
    ///                     │   X
    ///                     │   Y
    ///                     │   U or S
    ///                     ▼   PC
    ///                 Higher Memory Addresses
    /// 
    /// For each 8-bit register specified, a byte is read from the memory location pointed to by the stack pointer and then the stack pointer is incremented by one. 
    /// For each 16-bit register specified, the register’s high-order byte is read from the address pointed to by the stack pointer and then the stack pointer is incremented by one. 
    /// Next, the register’s loworder byte is read and the stack pointer is again incremented by one.
    /// 
    /// The PUL instructions use a postbyte wherein each bit position corresponds to one of the registers which may be pulled. 
    /// Bits that are set (1) specify the registers to be pulled.
    /// 
    ///             ╭───┬───┬───┬───┬───┬───┬───┬───╮
    /// POSTBYTE:   │ PC│U/S│ Y │ X │ DP│ B │ A │ CC│
    ///             ╰───┴───┴───┴───┴───┴───┴───┴───╯
    ///               7                           0
    /// 
    /// See Also: PSH, PULSW, PULUW
    /// </remarks>
    public class _35_Puls_M : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles)
        {
            byte _postByte = cpu.MemRead8(cpu.PC_REG++);

            if ((_postByte & 0x01) != 0)
            {
                cpu.CC = cpu.MemRead8(cpu.S_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x02) != 0)
            {
                cpu.A_REG = cpu.MemRead8(cpu.S_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x04) != 0)
            {
                cpu.B_REG = cpu.MemRead8(cpu.S_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x08) != 0)
            {
                cpu.DPA = cpu.MemRead8(cpu.S_REG++);

                cycles += 1;
            }

            if ((_postByte & 0x10) != 0)
            {
                cpu.X_H = cpu.MemRead8(cpu.S_REG++);
                cpu.X_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x20) != 0)
            {
                cpu.Y_H = cpu.MemRead8(cpu.S_REG++);
                cpu.Y_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x40) != 0)
            {
                cpu.U_H = cpu.MemRead8(cpu.S_REG++);
                cpu.U_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 2;
            }

            if ((_postByte & 0x80) != 0)
            {
                cpu.PC_H = cpu.MemRead8(cpu.S_REG++);
                cpu.PC_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 2;
            }

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 5);
        public int Exec(IHD6309 cpu) => Exec(cpu, Cycles._54);
    }
}
