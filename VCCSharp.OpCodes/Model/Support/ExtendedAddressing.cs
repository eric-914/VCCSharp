using VCCSharp.OpCodes.Memory;
using VCCSharp.OpCodes.Model.Memory;
using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.Model.Support;

internal interface IExtendedAddress : IRegisterPC, IRegisterD
{
    Memory8Bit M8 { get; }
    Memory16Bit M16 { get; }
    IRegisters16Bit R16 { get; }

    int Cycles { get; set; }
}

/// <summary>
/// INDEXED:
/// In these addressing modes, one of the pointer registers (X, Y, U, or S), and sometimes the program counter (PC) is used in the calculation of the effective address of the instruction operand. 
/// The basic types (and their variations) of indexed addressing available are shown in the table below along with the postbyte configuration used.
/// 
/// Constant Offset from Register:
/// The contents of the register designated in the postbyte are added to a twos complement offset value to form the effective address of the instruction operand. 
/// The contents of the designated register are not affected by this addition. 
/// 
/// The offset sizes available are:
/// 
///     No offset	= designated register contains the effective address
///     5-bit	    = -16 to +15
///     8-bit	    = -128 to +127
///     16-bit	    = -32768 to + 32767
///     
/// Postbyte Usage for Indexed Addressing Modes
/// ╭──────────────────────────────────┬──────────────────────┬──────────┬───────────────────╮
/// │           Mode Type              │ Variation            │ Direct   │ Indirect          │
/// ├──────────────────────────────────┼──────────────────────┼──────────┼───────────────────┤
/// │ Constant Offset from Register    │ No Offset            │ 1RR00100 │ 1RR10100          │
/// │ (twos Complement Offset)         │ 5-Bit Offset         │ 0RRnnnnn │ Defaults to 8-bit │
/// │                                  │ 8-Bit Offset         │ 1RR01000 │ 1RR11000          │
/// │                                  │ 16-Bit Offset        │ 1RR01001 │ 1RR11001          │
/// ├──────────────────────────────────┼──────────────────────┼──────────┼───────────────────┤
/// │ Accumulator Offset from Register │ A Accumulator Offset │ 1RR00110 │ 1RR10110          │
/// │ (twos Complement Offset)         │ B Accumulator Offset │ 1RR00101 │ 1RR10101          │
/// │                                  │ D Accumulator Offset │ 1RR01011 │ 1RR11011          │
/// ├──────────────────────────────────┼──────────────────────┼──────────┼───────────────────┤
/// │ Auto Increment/Decrement from    │ Increment by 1       │ 1RR00000 │ Not Allowed       │
/// │ Register                         │ Increment by 2       │ 1RR00001 │ 1RR10001          │
/// │                                  │ Decrement by 1       │ 1RR00010 │ Not Allowed       │
/// │                                  │ Decrement by 2       │ 1RR00011 │ 1RR10011          │
/// ├──────────────────────────────────┼──────────────────────┼──────────┼───────────────────┤
/// │ Constant Offset from Program     │ 8-Bit Offset         │ 1XX01100 │ 1XX11100          │
/// │ Counter                          │ 16-Bit Offset        │ 1XX01101 │ 1XX11101          │
/// ├──────────────────────────────────┼──────────────────────┼──────────┼───────────────────┤
/// │ Extended Indirect                │ 16-Bit Address       │ -------- │ 10011111          │
/// ╰──────────────────────────────────┴──────────────────────┴──────────┴───────────────────╯
///     			
/// The 5-bit offset value is contained in the postbyte. 
/// The 8- and 16-bit offset values are contained in the byte or bytes immediately following the postbyte. 
/// If the Motorola assembler is used, it will automatically determine the most efficient offset; thus, the programmer need not be concerned about the offset size.
///  
/// Examples:   LDA ,X              LDY -64000,U
///             LDB 0,Y             LDA 17,PC
///             LDX 64000,S         LDA There,PCR
///             
/// Accumulator Offset from Register:
///     The contents of the index or pointer register designed in the postbyte are temporarily added to the twos complement offset value contained in an accumulator (A, B, or D) also designated in the postbyte. Neither the designated register nor the accumulator contents are affected by this addition.
///     
/// Example:    LDA A,X             LDA D,U
///             LDA B,Y		
///             
/// Autoincrement/Decrement from Register.
/// This addressing mode works in a postincrementing or predecrementing manner. 
/// The amount of increment or decrement, one or two positions, is designated in the postbyte.
///     
/// In the autoincrement mode, the contents of the effective address contained in the pointer register, designated in the postbyte, and then the pointer register is automatically incremented; thus, the pointer register is postincremented.
///     
/// In the autodecrement mode, the pointer register, designated in the postbyte, is automatically decremented first and then the contents of the new address are used; thus, the pointer register is predecremented.
/// 
/// Example:	Autoincrement       Autodecrement
///             LDA	,X+	LDY	,X++	LDA	,-X LDY ,--X
///             LDA ,Y+	LDX	,Y++	LDA	,-Y LDX ,--Y
///             LDA ,S+	LDX	,U++	LDA	,-S LDX ,--U
///             LDA ,U+	LDX	,S++	LDA	,-U LDX ,--S
///             
/// Indirection:
/// When using indirection, the effective address of the base indexed addressing mode is used to fetch two bytes which contain the final effective address of the operand. 
/// It can be used with all the indexed addressing modes and the program counter relative addressing mode.
///     
/// Extended Indirect:
/// The effective address of the argument is located at the address specified by the two bytes following the postbyte. 
/// The postbyte is used to indicate indirection.
///     
/// Example:	LDA [$F000]
/// 
/// Program Counter Relative:
/// The program counter can also be used as a pointer with either an 8-bit or 16-bit signed constant offset. 
/// The offset value is added to the program counter to develop an effective address. 
/// Part of the postbyte is used to indicate whether the offset is 8 or 16 bits.
/// </summary>
/// <see cref="https://www.maddes.net/m6809pm/sections.htm"/>
internal class ExtendedAddressing : IExtendedAddressing
{
    private readonly IExtendedAddress _ea;

    private IRegisters16Bit R16 => _ea.R16;

    private Memory8Bit M8 => _ea.M8;
    private Memory16Bit M16 => _ea.M16;

    private ushort PC { get => _ea.PC; set => _ea.PC = value;}
    private ushort D { get => _ea.D; set => _ea.D = value;}
    private byte A { get => _ea.A(); set => _ea.A(value); }
    private byte B { get => _ea.B(); set => _ea.B(value); }

    private int Cycles { get => _ea.Cycles; set => _ea.Cycles = value; }

    public ExtendedAddressing(IExtendedAddress ea)
    {
        _ea = ea;
    }

    public ushort CalculateEA(byte postByte)
    {
        ushort address = 0;
        byte reg = (byte)(((postByte >> 5) & 3) + 1); //_RR_____ + 1

        if (postByte.Bit7())
        {
            switch (postByte & 0x1F)
            {
                #region Auto Increment/Decrement from Register

                case 0: //1RR00000
                    //Increment by 1
                    address = R16[reg];
                    R16[reg] = (ushort)(address + 1);
                    Cycles += 2;
                    break;

                case 1: //1RR00001
                    //Increment by 2
                    address = R16[reg];
                    R16[reg] = (ushort)(R16[reg] + 2);
                    Cycles += 3;
                    break;

                case 2: //1RR00010
                    //Decrement by 1
                    R16[reg] = (ushort)(R16[reg] - 1);
                    address = R16[reg];
                    Cycles += 2;
                    break;

                case 3: //1RR00011
                    //Decrement by 2
                    R16[reg] = (ushort)(R16[reg] - 2);
                    address = R16[reg];
                    Cycles += 3;
                    break;

                #endregion

                case 4: //1RR00100
                    //No Offset
                    address = R16[reg];
                    break;

                case 5: //1RR00101
                    //B Accumulator Offset
                    address = (ushort)(R16[reg] + ((sbyte)B));
                    Cycles += 1;
                    break;

                case 6: //1RR00110
                    //A Accumulator Offset
                    address = (ushort)(R16[reg] + ((sbyte)A));
                    Cycles += 1;
                    break;

                case 7: //1RR00111
                    //--UNDEFINED--
                    Cycles += 1;
                    break;

                case 8: //1RR01000
                    //8-Bit Offset
                    address = (ushort)(R16[reg] + (sbyte)M8[PC++]);
                    Cycles += 1;
                    break;

                case 9: //1RR01001
                    //16-Bit Offset
                    address = (ushort)(R16[reg] + M16[PC]);
                    Cycles += 4;
                    PC += 2;
                    break;

                case 10: //1RR01010
                    //--UNDEFINED--
                    Cycles += 1;
                    break;

                case 11: //1RR01011
                    //D Accumulator Offset
                    address = (ushort)(R16[reg] + D);
                    Cycles += 4;
                    break;

                case 12: //1XX01100
                    //8-Bit Offset
                    address = (ushort)((short)PC + (sbyte)M8[PC] + 1);
                    Cycles += 1;
                    PC++;
                    break;

                case 13: //1XX01101
                    //16-Bit Offset
                    address = (ushort)(PC + M16[PC] + 2);
                    Cycles += 5;
                    PC += 2;
                    break;

                case 14: //1XX01110
                    //--UNDEFINED--
                    Cycles += 4;
                    break;

                case 15: //01111
                    sbyte signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            break;

                        case 1:
                            PC += 2;
                            break;

                        case 2:
                            break;

                        case 3:
                            break;
                    }
                    break;

                case 16: //10000
                    signedByte = (sbyte)((postByte >> 5) & 3);

                    switch (signedByte)
                    {
                        case 0:
                            break;

                        case 1:
                            PC += 2;
                            break;

                        case 2:
                            break;

                        case 3:
                            break;
                    }
                    break;

                case 17: //1RR10001
                    //Increment by 2
                    address = R16[reg];
                    R16[reg] = (ushort)(R16[reg] + 2);
                    address = M16[address];
                    Cycles += 6;
                    break;

                case 18: //1RR10010
                    //Not Allowed
                    Cycles += 6;
                    break;

                case 19: //1RR10011
                    //Decrement by 2
                    R16[reg] = (ushort)(R16[reg] - 2);
                    address = M16[R16[reg]];
                    Cycles += 6;
                    break;

                case 20: //1RR10100
                    //No Offset
                    address = M16[R16[reg]];
                    Cycles += 3;
                    break;

                case 21: //1RR10101
                    //B Accumulator Offset
                    address = M16[(ushort)(R16[reg] + ((sbyte)B))];
                    Cycles += 4;
                    break;

                case 22: //1RR10110
                    //A Accumulator Offset
                    address = M16[(ushort)(R16[reg] + ((sbyte)A))];
                    Cycles += 4;
                    break;

                case 23: //10111
                    address = M16[address];
                    Cycles += 4;
                    break;

                case 24: //1RR11000
                    //8-Bit Offset
                    address = M16[(ushort)(R16[reg] + (sbyte)M8[PC++])];
                    Cycles += 4;
                    break;

                case 25: //1RR11001
                    //16-Bit Offset
                    address = M16[(ushort)(R16[reg] + M16[PC])];
                    Cycles += 7;
                    PC += 2;
                    break;

                case 26: //11010
                    address = M16[address];
                    Cycles += 4;
                    break;

                case 27: //1RR11011
                    //D Accumulator Offset
                    address = M16[(ushort)(R16[reg] + D)];
                    Cycles += 7;
                    break;

                case 28: //1XX11100
                    //8-Bit Offset
                    address = M16[(ushort)((short)PC + (sbyte)M8[PC] + 1)];
                    Cycles += 4;
                    PC++;
                    break;

                case 29: //1XX11101
                    //16-Bit Offset
                    address = M16[(ushort)(PC + M16[PC] + 2)];
                    Cycles += 8;
                    PC += 2;
                    break;

                case 30: //11110
                    address = M16[address];
                    Cycles += 7;
                    break;

                case 31: //10011111
                    //16-Bit Address
                    address = M16[M16[PC]];
                    Cycles += 8;
                    PC += 2;
                    break;
            }
        }
        else
        {
            //0RRnnnnn
            //5-Bit Offset

            sbyte signedByte = (sbyte)(postByte & 0x1F); //--Right most 5-bits
            signedByte <<= 3; //--Push the "sign" to the left-most bit.
            signedByte /= 8;

            address = (ushort)(R16[reg] + signedByte); //Was signed

            Cycles += 1;
        }

        return address;
    }
}
