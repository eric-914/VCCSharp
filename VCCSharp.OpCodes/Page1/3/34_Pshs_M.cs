using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>34/PSHS/IMMEDIATE</code>
/// Push <c>{ A, B, CC, DP, D, X, Y, U, PC }</c> onto hardware stack
/// </summary>
/// <remarks>
/// The <c>PSHS</c> instruction pushes values for none, one or multiple registers from either the Hardware <c>(S)</c> stack. 
/// </remarks>
/// 
/// None of the Condition Code flags are affected by these instructions.
/// 
/// Only the registers present in the 6809 architecture can be pushed by these instructions.
/// The stack pointer used by the instruction cannot be pushed. 
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
/// The PSHS instructions use a postbyte wherein each bit position corresponds to one of the registers which may be pushed. 
/// Bits that are set (1) specify the registers to be pushed.
/// 
///             ╭───┬───┬───┬───┬───┬───┬───┬───╮
/// POSTBYTE:   │ PC│ U │ Y │ X │ DP│ B │ A │ CC│
///             ╰───┴───┴───┴───┴───┴───┴───┴───╯
///               7                           0
///               
/// PSHS r0,r1,...rN
/// PSHS #i8
/// I8 : 8-bit Immediate value
/// Cycles (5+ / 4+) → (One additional cycle is used for each BYTE pushed.)
/// Byte Count (2)
/// 
/// See Also: PSHSW, PSHUW, PUL
internal class _34_Pshs_M : OpCode, IOpCode
{
    internal _34_Pshs_M(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        int cycles = Cycles._54;

        void Write(byte value)
        {
            cycles++;

            M8[--S] = value;
        }

        byte value = M8[PC++];

        if (value.Bit7()) { Write(PC_L); Write(PC_H); }
        if (value.Bit6()) { Write(U_L); Write(U_H); }
        if (value.Bit5()) { Write(Y_L); Write(Y_H); }
        if (value.Bit4()) { Write(X_L); Write(X_H); }
        if (value.Bit3()) { Write(DP); }
        if (value.Bit2()) { Write(B); }
        if (value.Bit1()) { Write(A); }
        if (value.Bit0()) { Write(CC); }

        return cycles;
    }
}
