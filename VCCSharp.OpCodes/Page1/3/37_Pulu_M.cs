using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>37/PULU/IMMEDIATE</code>
/// Pull <c>{ A, B, CC, DP, D, X, Y, S, PC }</c> from hardware stack
/// </summary>
/// <remarks>
/// The <c>PULU</c> instruction pulls values for none, one or multiple registers from either the User <c>(U)</c> stack. 
/// </remarks>
/// 
/// None of the Condition Code flags are affected by these instructions unless the CC register is specified as one of the registers to pull.
/// 
/// Only the registers present in the 6809 architecture can be pulled by these instructions.
/// The user pointer used by the instruction cannot be pulled. 
/// A value is pulled from the stack for each register specified in the operand field one at a time in the order shown below (the order you list them in the operand field is irrelevant).
/// 
///                 Lower Memory Addresses
///                     ╷   CC
///                     │   A
///                     │   B
///                     │   DP
///                     │   X
///                     │   Y
///                     │   S
///                     ▼   PC
///                 Higher Memory Addresses
/// 
/// For each 8-bit register specified, a byte is read from the memory location pointed to by the stack pointer and then the stack pointer is incremented by one. 
/// For each 16-bit register specified, the register’s high-order byte is read from the address pointed to by the stack pointer and then the stack pointer is incremented by one. 
/// Next, the register’s loworder byte is read and the stack pointer is again incremented by one.
/// 
/// The PULU instructions use a postbyte wherein each bit position corresponds to one of the registers which may be pulled. 
/// Bits that are set (1) specify the registers to be pulled.
/// 
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮
/// POSTBYTE:   │ PC│ S │ Y │ X │ DP│ B │ A │ CC│
///             ╰───┴───┴───┴───┴───┴───┴───┴───╯
///               7                           0
/// 
/// PULU r0,r1,...rN
/// PULU #i8
/// I8 : 8-bit Immediate value
/// Cycles (5+ / 4+) → (One additional cycle is used for each BYTE pulled.)
/// Byte Count (2)
/// 
/// See Also: PSH, PULSW, PULUW
internal class _37_Pulu_M : OpCode, IOpCode
{
    public int CycleCount => DynamicCycles._54;

    public int Exec()
    {
        Cycles = CycleCount;

        byte _8() { Cycles++; return M8[U++]; }
        ushort _16() { Cycles += 2; return (ushort)(M8[U++] << 8 | M8[U++]); }

        byte value = M8[PC++];

        if (value.Bit0()) { CC = _8(); }
        if (value.Bit1()) { A = _8(); }
        if (value.Bit2()) { B = _8(); }
        if (value.Bit3()) { DP = _8(); }
        if (value.Bit4()) { X = _16(); }
        if (value.Bit5()) { Y = _16(); }
        if (value.Bit6()) { S = _16(); }
        if (value.Bit7()) { PC = _16(); }

        return Cycles;
    }
}
