using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>35/PULS/IMMEDIATE</code>
/// Pull <c>{ A, B, CC, DP, D, X, Y, U, PC }</c> from hardware stack
/// </summary>
/// <remarks>
/// The <c>PULS</c> instruction pulls values for none, one or multiple registers from either the Hardware <c>(S)</c> stack. 
/// </remarks>
/// 
/// None of the Condition Code flags are affected by these instructions unless the CC register is specified as one of the registers to pull.
/// 
/// Only the registers present in the 6809 architecture can be pulled by these instructions.
/// The stack pointer used by the instruction cannot be pulled. 
/// A value is pulled from the stack for each register specified in the operand field one at a time in the order shown below (the order you list them in the operand field is irrelevant).
/// 
///                 Lower Memory Addresses
///                     ╷   CC
///                     │   A
///                     │   B
///                     │   DP
///                     │   X
///                     │   Y
///                     │   U
///                     ▼   PC
///                 Higher Memory Addresses
/// 
/// For each 8-bit register specified, a byte is read from the memory location pointed to by the stack pointer and then the stack pointer is incremented by one. 
/// For each 16-bit register specified, the register’s high-order byte is read from the address pointed to by the stack pointer and then the stack pointer is incremented by one. 
/// Next, the register’s loworder byte is read and the stack pointer is again incremented by one.
/// 
/// The PULS instructions use a postbyte wherein each bit position corresponds to one of the registers which may be pulled. 
/// Bits that are set (1) specify the registers to be pulled.
/// 
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮
/// POSTBYTE:   │ PC│ U │ Y │ X │ DP│ B │ A │ CC│
///             ╰───┴───┴───┴───┴───┴───┴───┴───╯
///               7                           0
/// 
/// PULS r0,r1,...rN
/// PULS #i8
/// I8 : 8-bit Immediate value
/// Cycles (5+ / 4+) → (One additional cycle is used for each BYTE pulled.)
/// Byte Count (2)
/// 
/// See Also: PSH, PULSW, PULUW
internal class _35_Puls_M : OpCode, IOpCode
{
    public int Exec()
    {
        int cycles = DynamicCycles._54;

        byte _8() { cycles++; return Pop8(); }
        ushort _16() { cycles += 2; return Pop16(); }

        byte value = M8[PC++];

        if (value.Bit0()) { CC = _8(); }
        if (value.Bit1()) { A = _8(); }
        if (value.Bit2()) { B = _8(); }
        if (value.Bit3()) { DP = _8(); }
        if (value.Bit4()) { X = _16(); }
        if (value.Bit5()) { Y = _16(); }
        if (value.Bit6()) { U = _16(); }
        if (value.Bit7()) { PC = _16(); }

        return cycles;
    }
}
