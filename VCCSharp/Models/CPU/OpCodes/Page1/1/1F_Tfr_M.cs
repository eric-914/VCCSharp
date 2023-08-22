using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// TFR
    /// Transfer R1 to R2 (R1, R2 = A, B, CC, DP)
    /// Transfer D to X, Y, S, U or PC
    /// Transfer X, Y, S, U or PC to D
    /// Transfer Register to Register
    /// IMMEDIATE
    /// r0 → r1
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES      BYTE COUNT
    /// TFR r0,r1       IMMEDIATE           1F          6 / 4       2           [6309]
    /// TFR r0,r1       IMMEDIATE           1F          6           2           [6809]
    /// </summary>
    /// <remarks>
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// ╭─────────────────────╮
    /// │ 6309 IMPLEMENTATION │
    /// ╰─────────────────────╯
    /// TFR copies the contents of a source register into a destination register. 
    /// None of the Condition Code flags are affected unless CC is specified as the destination register.
    /// 
    /// Any of the 6309 registers except Q and MD may be specified as either the source, destination or both. 
    /// Specifying the same register for both the source and destination produces an instruction which, like NOP, has no effect.
    /// 
    /// The TFR instruction can be used to alter the flow of execution by specifying PC as the destination register.
    /// 
    /// When an 8-bit source register is transferred to a 16-bit destination register, the contents of the 8-bit register are placed into both halves of the 16-bit register. 
    /// When a 16-bit source register is transferred to an 8-bit destination register, only the upper or the lower half of the 16-bit register is transferred. 
    /// As illustrated in the diagram below, which half is transferred depends on which 8-bit register is specified as the destination.
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
    /// The TFR instruction requires a postbyte in which the source and destination registers are encoded into the upper and lower nibbles respectively.
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
    /// See Also: EXG, TFR (6809 implementation)
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// ╭─────────────────────╮
    /// │ 6809 IMPLEMENTATION │
    /// ╰─────────────────────╯
    /// TFR copies the contents of a source register into a destination register. 
    /// None of the Condition Code flags are affected unless CC is specified as the destination register.
    /// The TFR instruction can be used to alter the flow of execution by specifying PC as the destination register.
    /// Any of the 6809 registers may be specified as either the source, destination or both. 
    /// Specifying the same register for both the source and destination produces an instruction which, like NOP, has no effect.
    /// The table below explains how the destination register is affected when the source and destination sizes are different. 
    /// This behavior differs from the 6309 implementation.
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///         ╭───────────┬─────────────────────┬─────────────────────────────────────────-╮
    ///         │ Operation │ 8-bit Register Used │                Results                   │
    ///         ├───────────┼─────────────────────┼──────────────────────────────────────────┤
    ///         │  16 → 8   │        Any          │ Destination = LSB from Source            │
    ///         │   8 → 16  │      A or B         │ MSB of Destination = FF16 ; LSB = Source │
    ///         │   8 → 16  │     CC or DP        │ Both MSB and LSB of Destination = Source │
    ///         ╰───────────┴─────────────────────┴─────────────────────────────────────────-╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// The TFR instruction requires a postbyte in which the source and destination registers are encoded into the upper and lower nibbles respectively.
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
    /// If an invalid register encoding is used for the source, a constant value of FF16 or FFFF16 is transferred to the destination. 
    /// If an invalid register encoding is used for the destination, then the instruction will have no effect. 
    /// The invalid register encodings have valid meanings when executed on 6309 processors, and should be avoided in code that needs to work the same way on both CPU’s. 
    /// 
    /// See Also: EXG, TFR (6309 implementation)
    /// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    /// </remarks>
    public class _1F_Tfr_M : OpCode, IOpCode
    {
        public int Exec(IMC6809 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            switch (destination)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                    cpu.PXF(destination, 0xFFFF);

                    if (source == 12 || source == 13)
                    {
                        cpu.PXF(destination, 0);
                    }
                    else if (source <= 7)
                    {
                        cpu.PXF(destination, cpu.PXF(source));
                    }

                    break;

                case 8:
                case 9:
                case 10:
                case 11:
                case 14:
                case 15:
                    cpu.PUR(destination & 7, 0xFF);

                    if ((source == 12) || (source == 13))
                    {
                        cpu.PUR(destination & 7, 0);
                    }
                    else if (source > 7)
                    {
                        cpu.PUR(destination & 7, cpu.PUR(source & 7));
                    }

                    break;
            }

            return 6;
        }

        public int Exec(IHD6309 cpu)
        {
            byte value = cpu.MemRead8(cpu.PC_REG++);

            byte source = (byte)(value >> 4);
            byte destination = (byte)(value & 15);

            if (destination < 8)
            {
                if (source < 8)
                {
                    cpu.PXF(destination, cpu.PXF(source));
                }
                else
                {
                    cpu.PXF(destination, (ushort)((cpu.PUR(source & 7) << 8) | cpu.PUR(source & 7)));
                }
            }
            else
            {
                destination &= 7;

                if (source < 8)
                    switch (destination)
                    {
                        case 0:  // A
                        case 3: // DP
                        case 6: // E
                            cpu.PUR(destination, (byte)(cpu.PXF(source) >> 8));
                            break;

                        case 1:  // B
                        case 2: // CC
                        case 7: // F
                            cpu.PUR(destination, (byte)(cpu.PXF(source) & 0xFF));
                            break;
                    }
                else
                {
                    cpu.PUR(destination, cpu.PUR(source & 7));
                }

                cpu.O_REG = 0;
            }

            return Cycles._64;
        }
    }
}
