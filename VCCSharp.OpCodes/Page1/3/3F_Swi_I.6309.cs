using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>3F/SWI/INHERENT</code>
/// Software Interrupt
/// </summary>
/// <remarks>
/// The <c>SWI</c> instruction invokes a Software Interrupt.
/// </remarks>
/// 
/// The SWI, SWI2 and SWI3 instructions each invoke a Software Interrupt.
/// 
/// Each of these instructions first set the E flag in the CC register and then push the machine state onto the hardware stack (S).
/// 
/// After stacking the machine state, the SWI instruction sets the I and F interrupt masks in the CC register. 
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///     ╭─────────────────╮  ╭───────────────╮
///     │       SWI       │  │               ▼
///     ╰────────┬────────╯  │       ┌───────────────┐      
///              ▼           │       │ PUSH B, A, CC │
///       ┌────────────┐     │       └───────┬───────┘
///       │ SET: E = 1 │     │               ▼
///       └──────┬─────┘     │     ┌───────────────────┐
///              ▼           │     │        SWI        │  
///      ┌─────────────────┐ │     └─────────┬─────────┘
///      │      PUSH       │ │               ▼             
///      │ PC, U, Y, X, DP │ │     ┌───────────────────┐   
///      └───────┬─────────┘ │     │ SET: I = 1; F = 1 │   
///              ▼           │     └─────────┬─────────┘   
///          ╔════════╗      │               ▼
///          ║ NM=1?  ╟──NO──╯       ┌───────────────┐
///          ╚═══╤════╝      ▲       │ PC ← [FFFA:B] │
///          YES │           │       └───────┬───────┘     
///              │           │               │     
///              ▼           │               ▼     
///        ┌───────────┐     │            ╭──────╮
///        │ PUSH F, E │─────╯            │ DONE │
///        └───────────┘                  ╰──────╯
///                                 
///                                 
///     SWI Instruction Flow
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
/// Finally, control is transferred to the interrupt service routine whose address is obtained from the vector which corresponds to the particular instruction.
/// 
/// The state of the NM bit in the MD register determines whether or not the E and F accumulators are included in the stacked machine state. 
/// Service routines should be written to work properly regardless of the current state of the NM bit. 
/// This is best accomplished by avoiding modification of the NM bit and using the RTI instruction to return control to the interrupted task. 
/// If an SWI service routine needs to examine or modify the stacked machine state, it may first need to determine the current state of the NM bit. 
/// See page 144 for the listing of a subroutine that will accomplish this task. 
/// 
/// Cycles (19 / 21)
/// Byte Count (1)
/// 
/// See Also: RTI
internal class _3F_Swi_I_6309 : OpCode6309, IOpCode
{
    public int CycleCount => 12 + 7; // One cycle for each byte pushed + Overhead

    public int Exec()
    {
        Cycles = CycleCount;

        CC_E = true; //--Everything is going on stack

        Push(PC);
        Push(U);
        Push(Y);
        Push(X);
        Push(DP);

        if (MD_NATIVE6309)
        {
            Push(W);

            Cycles += 2;
        }

        Push(D);
        Push(CC);

        CC_I = true;
        CC_F = true;

        PC = M16[Define.VSWI];

        return Cycles;
    }
}
