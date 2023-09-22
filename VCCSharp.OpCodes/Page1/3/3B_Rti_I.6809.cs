using VCCSharp.OpCodes.Model.OpCodes;

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
/// │ 6809 IMPLEMENTATION │
/// ╰─────────────────────╯
/// 
/// The RTI instruction restores the machine state which was stacked upon the invocation of an interrupt service routine.
/// 
/// The exact behavior of the RTI instruction depends on the state of the E flag in the stacked CC register and the state of the NM bit in the MD register.
/// 
/// The E flag will have been set or cleared at the time of the interrupt, based on the type of interrupt that occurred and the state of the FM bit in the MD register at that time.
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
///          │             │            │
///          ▼             │            │
///      ╔═══════╗         │            │
///      ║ E=1?  ╟───YES───╯            │
///      ╚═══╤═══╝                      │
///       NO │                          │
///          │◀────────────╮            │
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
/// The RTI instruction restores the machine state which was stacked upon the invocation of an interrupt service routine.
/// 
/// The exact behavior of the RTI instruction depends on the state of the E flag in the stacked CC register and the state of the NM bit in the MD register.
/// 
/// The E flag will have been set or cleared at the time of the interrupt.
/// 
/// Interrupt service routines should strive to use the RTI instruction for returning control to the interrupted task. 
/// All the logic for proper restoration of the machine state, based on the CPU’s current execution mode, is built-in.
/// 
/// Service routines which must examine or modify the stacked machine state can require a considerable amount of additional code to determine which registers have been preserved. 
/// 
/// Cycles → [CC.E=0: 6]
///          [CC.E=1: 15]
/// Byte Count (1)
/// 
/// See Also: CWAI, RTS, SWI, SWI2, SWI3
internal class _3B_Rti_I_6809 : OpCode, IOpCode
{
    public int CycleCount => 6;

    public void Exec()
    {
        CC = Pop8();

        ClearInterrupt(); //TODO: Does the interrupt technically end before or after the stack pull?

        if (CC_E)
        {
            D = Pop16();
            DP = Pop8();
            X = Pop16();
            Y = Pop16();
            U = Pop16();

            Cycles += 9; //--9 bytes pushed
        }

        PC = Pop16();
    }
}
