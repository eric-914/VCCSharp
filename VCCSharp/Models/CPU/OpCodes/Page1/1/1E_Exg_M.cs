using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// EXG
    /// Exchange R1 with R2 (R1, R2 = A, B, CC, DP)
    /// IMMEDIATE
    /// r0 ↔ r1
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// EXG r0,r1       IMMEDIATE           1E          8 / 5       2
    /// </summary>
    /// <remarks>
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// ╭─────────────────────╮
    /// │ 6309 IMPLEMENTATION │
    /// ╰─────────────────────╯
    /// This instruction exchanges the contents of two registers. 
    /// None of the Condition Code flags are affected unless CC is one of the registers involved in the exchange.
    /// 
    /// Program flow can be altered by specifying PC as one of the registers. 
    /// When this occurs, the other register is set to the address of the instruction that follows EXG.
    /// 
    /// Any of the 6309 registers except Q and MD may be used in the exchange. 
    /// The order in which the two registers are specified is irrelevant. 
    /// For example, EXG A,B will operate exactly the same as EXG B,A although the object code will be different.
    /// 
    /// When an 8-bit register is exchanged with a 16-bit register, the contents of the 8-bit register are placed into both halves of the 16-bit register. 
    /// Conversely, only the upper or the lower half of the 16-bit register is placed into the 8-bit register. 
    /// As illustrated in the diagram below, which half is transferred depends on which 8-bit register is involved.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///                                                     b15  b8  b7   b0
    ///                                                    ╭───────╮╭───────╮
    ///     16-bit register ( D, X, Y, U, S, PC, W, V ):   │  MSB  ││  LSB  │
    ///                                                    ╰─┬─┬─┬─╯╰─┬─┬─┬─╯
    ///                                      ╭───────────────╯ │ │    │ │ ╰───────────────╮
    ///                                      │        ╭───────────────╯ │                 │
    ///                                      │        │        │ ╰──────│────────╮        │
    ///                                   ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮  ╭──┴──╮
    ///                 8-bit register:   │  A  │  │  B  │  │  E  │  │  F  │  │ DP  │  │ CC  │
    ///                                   ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯  ╰─────╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// The EXG instruction requires a postbyte in which the two registers that are involved are encoded into the upper and lower nibbles.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
    ///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
    /// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
    ///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││▒▒1100▒▒│▒▒▒▒0▒▒▒▒▒│
    ///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││▒▒1101▒▒│▒▒▒▒0▒▒▒▒▒│
    ///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││▒▒0110▒▒│▒▒▒▒W▒▒▒▒▒││▒▒1110▒▒│▒▒▒▒E▒▒▒▒▒│
    ///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││▒▒0111▒▒│▒▒▒▒V▒▒▒▒▒││▒▒1111▒▒│▒▒▒▒F▒▒▒▒▒│
    ///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
    ///                                                      ▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒Shaded encodings are invalid on 6809 microprocessors▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// See Also: EXG (6809 implementation), TFR
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// ╭─────────────────────╮
    /// │ 6809 IMPLEMENTATION │
    /// ╰─────────────────────╯
    /// This instruction exchanges the contents of two registers. 
    /// None of the Condition Code flags are affected unless CC is one of the registers involved in the exchange.
    /// 
    /// Program flow can be altered by specifying PC as one of the registers. 
    /// When this occurs, the other register is set to the address of the instruction that follows EXG.
    /// 
    /// Any of the 6809 registers may be used in the exchange. 
    /// When exchanging registers of the same size, the order in which they are specified is irrelevant. 
    /// For example, EXG A,B will operate exactly the same as EXG B,A although the object code will be different.
    /// 
    /// When exchanging registers of different sizes, a 6809 operates differently than a 6309.
    /// The 8-bit register is always exchanged with the lower half of the 16-bit register, and the the upper half of the 16-bit register is then set to the value shown in the table below.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///         ╭───────────────┬─────────────────────┬────────────────────────────────-╮
    ///         │ Operand Order │ 8-bit Register Used │ 16-bit Register’s MSB after EXG │
    ///         ├───────────────┼─────────────────────┼─────────────────────────────────┤
    ///         │    16 , 8     │         Any         │            FF(16) *             │
    ///         │     8 , 16    │        A or B       │            FF(16) *             │
    ///         │     8 , 16    │       CC or DP      │           Same as LSB           │
    ///         ╰───────────────┴─────────────────────┴────────────────────────────────-╯
    ///         *The one exception is for EXG A,D which produces exactly the same result as EXG A,B
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// The EXG instruction requires a postbyte in which the two registers are encoded into the upper and lower nibbles.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///                                                      ╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮╭────────┬─────────-╮
    ///             ╭───┬───┬───┬───┬───┬───┬───┬───╮        │  Code  │ Register ││  Code  │ Register ││  Code  │ Register ││  Code  │ Register │
    /// POSTBYTE:   │ b7│   │   │ b4│ b3│   │   │ b0│        ├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤├────────┼──────────┤
    ///             ╰─┬─┴───┴───┴─┬─┴─┬─┴───┴───┴─┬─╯        │  0000  │    D     ││  1000  │    A     ││  0100  │    S     ││  1100  │ invalid  │
    ///               ╰─────┬─────╯   ╰─────┬─────╯          │  0001  │    X     ││  1001  │    B     ││  0101  │    PC    ││  1101  │ invalid  │
    ///        r0  ─────────╯               │                │  0010  │    Y     ││  1010  │    CC    ││  0110  │ invalid  ││  1110  │ invalid  │
    ///        r1  ─────────────────────────╯                │  0011  │    U     ││  1011  │    DP    ││  0111  │ invalid  ││  1111  │ invalid  │
    ///                                                      ╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯╰────────┴─────────-╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// If an invalid register encoding is specified for either register, a constant value of FF(16) or FFFF(16) is used for the exchange. 
    /// The invalid register encodings have valid meanings on 6309 processors, and should be avoided in code intended to run on both CPU’s.
    /// 
    /// See Also: EXG (6309 implementation), TFR
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// </remarks>
    public class _1E_Exg_M : OpCode, IOpCode
    {
        //TODO: Need to verify all this
        public int Exec(IMC6809 cpu)
        {
            var value = cpu.MemRead8(cpu.PC_REG++);
            byte temp8;
            ushort temp16;

            if (((value & 0x80) >> 4) == (value & 0x08)) //Verify like size registers
            {
                if ((value & 0x08) != 0) //8 bit EXG
                {
                    temp8 = cpu.PUR(((value & 0x70) >> 4));
                    cpu.PUR(((value & 0x70) >> 4), cpu.PUR(value & 0x07));
                    cpu.PUR(value & 0x07, temp8);
                }
                else // 16 bit EXG
                {
                    temp16 = cpu.PXF((value & 0x70) >> 4);
                    cpu.PXF((value & 0x70) >> 4, cpu.PXF(value & 0x07));
                    cpu.PXF(value & 0x07, temp16);
                }
            }

            return 8;
        }

        public int Exec(IHD6309 cpu)
        {
            var value = cpu.MemRead8(cpu.PC_REG++);
            var source = (byte)(value >> 4);
            var destination = (byte)(value & 15);
            byte temp8;
            ushort temp16;

            if ((source & 0x08) == (destination & 0x08)) //Verify like size registers
            {
                if ((destination & 0x08) != 0) //8 bit EXG
                {
                    source &= 0x07;
                    destination &= 0x07;

                    temp8 = cpu.PUR(source);
                    cpu.PUR(source, cpu.PUR(destination));
                    cpu.PUR(destination, temp8);

                    cpu.O_REG = 0;
                }
                else // 16 bit EXG
                {
                    source &= 0x07;
                    destination &= 0x07;

                    temp16 = cpu.PXF(source);
                    cpu.PXF(source, destination);
                    cpu.PXF(destination, temp16);
                }
            }
            else
            {
                if ((destination & 0x08) != 0) // Swap 16 to 8 bit exchange to be 8 to 16 bit exchange (for convenience)
                {
                    temp8 = destination; destination = source; source = temp8;
                }

                source &= 0x07;
                destination &= 0x07;

                byte tmp;

                switch (source)
                {
                    case 0x04: // Z
                    case 0x05: // Z
                        cpu.PXF(destination, 0); // Source is Zero reg. Just zero the Destination.
                        break;

                    case 0x00: // A
                    case 0x03: // DP
                    case 0x06: // E
                        temp8 = cpu.PUR(source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(cpu.PXF(destination) >> 8);
                        cpu.PUR(source, tmp); // A, DP, E get high byte of 16 bit Dest
                        cpu.PXF(destination, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;

                    case 0x01: // B
                    case 0x02: // CC
                    case 0x07: // F
                        temp8 = cpu.PUR(source);
                        temp16 = (ushort)((temp8 << 8) | temp8);
                        tmp = (byte)(cpu.PXF(destination) & 0xFF);
                        cpu.PUR(source, tmp); // B, CC, F get low byte of 16 bit Dest
                        cpu.PXF(destination, temp16); // Place 8 bit source in both halves of 16 bit Dest
                        break;
                }
            }

            return Cycles._85;
        }
    }
}