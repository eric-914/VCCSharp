﻿using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>3B/RTI/INHERENT</code>
/// Return from interrupt 
/// </summary>
/// <remarks>
/// The <c>RTI</c> instruction restores the machine state which was stacked upon the invocation of an interrupt service routine.
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6309 IMPLEMENTATION │
/// ╰─────────────────────╯
/// 
/// The RTI instruction restores the machine state which was stacked upon the invocation of an interrupt service routine.
/// 
/// The exact behavior of the RTI instruction depends on the state of the E flag in the stacked CC register and the state of the NM bit in the MD register.
/// 
/// The E flag will have been set or cleared at the time of the interrupt, based on the type of interrupt that occurred and the state of the FM bit in the MD register at that time.
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
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///     ╭─────────╮
///     │   RTI   │
///     ╰────┬────╯        ╭────────────╮
///          ▼             │            ▼
///     ┌─────────┐        │      ┌───────────┐
///     │ PULL CC │        │      │ PULL A, B │
///     └────┬────┘        │      └─────┬─────┘
///          │             │            │
///          │             │            │             ╭────────╮
///          ▼             │            ▼             │        ▼
///      ╔═══════╗         │       ╔════════╗         │  ┌───────────┐
///      ║ E=1?  ╟───YES───╯       ║ NM=1?  ╟───YES───╯  │ PULL E, F │
///      ╚═══╤═══╝                 ╚════╤═══╝            └─────┬─────┘
///       NO │                       NO │                      │
///          │◀────────────╮            │◀─────────────────────╯
///          ▼             │            ▼
///     ┌─────────┐        │   ┌──────────────────┐
///     │ PULL PC │        │   │ PULL DP, X, Y, U │
///     └────┬────┘        │   └────────┬─────────┘
///          ▼             ╰────────────╯
///     ╭─────────╮
///     │  DONE   │
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
/// Cycles → [CC.E=0: 6]
///          [CC.E=1: 15 / 17]
/// Byte Count (1)
/// 
/// See Also: CWAI, RTS, SWI, SWI2, SWI3
internal class _3B_Rti_I_6309 : OpCode6309, IOpCode
{
    internal _3B_Rti_I_6309(HD6309.IState cpu) : base(cpu) { }

    public int Exec()
    {
        int cycles = 6;

        CC = M8[S++];

        IsInInterrupt = false;

        if (CC_E)
        {
            A = M8[S++];
            B = M8[S++];

            if (MD_NATIVE6309)
            {
                E = M8[S++];
                F = M8[S++];

                cycles += 2;
            }

            DP = M8[S++];
            X_H = M8[S++];
            X_L = M8[S++];
            Y_H = M8[S++];
            Y_L = M8[S++];
            U_H = M8[S++];
            U_L = M8[S++];

            cycles += 9;
        }

        PC_H = M8[S++];
        PC_L = M8[S++];

        return cycles;
    }
}