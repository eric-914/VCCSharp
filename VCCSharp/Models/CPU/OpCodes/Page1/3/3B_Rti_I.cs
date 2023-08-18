using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;

namespace VCCSharp.Models.CPU.OpCodes.Page1
{
    /// <summary>
    /// RTI
    /// Return from interrupt 
    /// INHERENT
    /// SOURCE FORM     ADDRESSING MODE     OPCODE      CYCLES              BYTE COUNT
    /// RTI             INHERENT            3B          [CC.E=0: 6]         1
    /// RTI             INHERENT            3B          [CC.E=1: 15 / 17]   1
    /// </summary>
    /// <remarks>
    /// The RTI instruction restores the machine state which was stacked upon the invocation of an interrupt service routine.
    /// 
    /// The exact behavior of the RTI instruction depends on the state of the E flag in the stacked CC register and the state of the NM bit in the MD register.
    /// 
    /// The E flag will have been set or cleared at the time of the interrupt, based on the type of interrupt that occurred and the state of the FM bit in the MD register at that time.
    /// 
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    ///     ╭─────────╮
    ///     |   RTI   |
    ///     ╰────┬────╯        ╭────────────╮
    ///          ▼             │            ▼
    ///     ┌─────────┐        │      ┌───────────┐
    ///     | PULL CC |        │      │ PULL A, B │             ╭┄┄┄┄┄┄┄┄┄┄┄╮
    ///     └────┬────┘        │      └─────┬─────┘             ┆ 6309 Only ┊
    ///          │             │  ╭┄┄┄┄┄┄┄┄ │ ┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄╯           ┊
    ///          │             │  ┊         │             ╭────────╮        ┊
    ///          ▼             │  ┊         ▼             │        ▼        ┊
    ///      ╔═══════╗         │  ┊    ╔════════╗         │  ┌───────────┐  ┊
    ///      ║ E=1?  ╟───YES───╯  ┊    ║ NM=1?  ╟───YES───╯  │ PULL E, F │  ┊
    ///      ╚═══╤═══╝            ┊    ╚════╤═══╝            └─────┬─────┘  ┊
    ///       NO |                ┊      NO |                      │        ┊
    ///          │◀────────────╮  ┊         │◀─────────────────────╯        ┊
    ///          ▼             │  ╰┄┄┄┄┄┄┄┄ ▼ ┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄┄╯
    ///     ┌─────────┐        │   ┌──────────────────┐
    ///     | PULL PC |        │   | PULL DP, X, Y, U |
    ///     └────┬────┘        │   └────────┬─────────┘
    ///          ▼             ╰────────────╯
    ///     ╭─────────╮
    ///     |  DONE   |
    ///     ╰─────────╯
    /// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
    /// 
    /// Interrupt service routines should strive to use the RTI instruction for returning control to the interrupted task. 
    /// All the logic for proper restoration of the machine state, based on the CPU’s current execution mode, is built-in.
    /// 
    /// When an RTI instruction is executed, the state of the NM bit in the MD register must match the state it was in when the interrupt occurred, otherwise if the E flag was set, the wrong values will be restored to the DP, X, Y, U and PC registers. 
    /// For this reason, interrupt service routines should avoid changing the NM bit unless they are prepared to deal with this situation.
    /// 
    /// Service routines which must examine or modify the stacked machine state can require a considerable amount of additional code to determine which registers have been preserved. 
    /// In particular, the 6309 provides no instruction for testing the state of the NM bit in the MD register (see page 144 for the listing of a subroutine which can accomplish this).
    /// 
    /// See Also: CWAI, RTS, SWI, SWI2, SWI3
    /// </remarks>
    public class _3B_Rti_I : OpCode, IOpCode
    {
        private static int Exec(ICpuProcessor cpu, int cycles, Func<int> addendum)
        {
            cpu.CC = cpu.MemRead8(cpu.S_REG++);

            cpu.IsInInterrupt = false;

            if (cpu.CC_E)
            {
                cpu.A_REG = cpu.MemRead8(cpu.S_REG++);
                cpu.B_REG = cpu.MemRead8(cpu.S_REG++);

                cycles += addendum();

                cpu.DPA = cpu.MemRead8(cpu.S_REG++);
                cpu.X_H = cpu.MemRead8(cpu.S_REG++);
                cpu.X_L = cpu.MemRead8(cpu.S_REG++);
                cpu.Y_H = cpu.MemRead8(cpu.S_REG++);
                cpu.Y_L = cpu.MemRead8(cpu.S_REG++);
                cpu.U_H = cpu.MemRead8(cpu.S_REG++);
                cpu.U_L = cpu.MemRead8(cpu.S_REG++);

                cycles += 9;
            }

            cpu.PC_H = cpu.MemRead8(cpu.S_REG++);
            cpu.PC_L = cpu.MemRead8(cpu.S_REG++);

            return cycles;
        }

        public int Exec(IMC6809 cpu) => Exec(cpu, 6, () => 0);

        public int Exec(IHD6309 cpu)
        {
            int Addendum()
            {
                if (cpu.MD_NATIVE6309)
                {
                    cpu.E_REG = cpu.MemRead8(cpu.S_REG++);
                    cpu.F_REG = cpu.MemRead8(cpu.S_REG++);

                    return 2;
                }

                return 0;
            }

            return Exec(cpu, 6, Addendum);
        }
    }
}
