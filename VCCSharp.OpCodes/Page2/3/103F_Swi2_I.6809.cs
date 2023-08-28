using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page2;

/// <summary>
/// <code>103F/SWI2/INHERENT</code>
/// Software Interrupt
/// </summary>
/// <remarks>
/// The <c>SWI2</c> instruction invokes a Software Interrupt.
/// </remarks>
/// ━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
/// ╭─────────────────────╮
/// │ 6809 IMPLEMENTATION │
/// ╰─────────────────────╯
/// 
/// Each of these instructions first set the E flag in the CC register and then push the machine state onto the hardware stack (S).
/// 
/// After stacking the machine state, the SWI instruction sets the I and F interrupt masks in the CC register. 
/// SWI2 and SWI3 do not modify the mask.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///     ╭─────────────────╮  ╭───────────────╮
///     │      SWI2       │  │               ▼
///     ╰────────┬────────╯  │       ┌───────────────┐      
///              ▼           │       │ PUSH B, A, CC │
///       ┌────────────┐     │       └───────┬───────┘
///       │ SET: E = 1 │     │               ▼
///       └──────┬─────┘     │     ┌───────────────────┐
///              ▼           │     │       SWI2        │  
///      ┌─────────────────┐ │     └─────────┬─────────┘
///      │      PUSH       │ │               │     
///      │ PC, U, Y, X, DP │ │               │     
///      └───────┬─────────┘ │               │     
///              │           │               │     
///              │           │               ▼
///              ╰───────────╯       ┌───────────────┐     
///                                  │ PC ← [FFF4:5] │
///                                  └───────┬───────┘        
///                                          │
///                                          ▼
///                                       ╭──────╮
///                                       │ DONE │
///                                       ╰──────╯
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
/// NOTE: When Motorola introduced the 6809, they designated SWI2 as an instruction reserved for the end user, and not to be used in packaged software. 
/// Under the OS9 operating system, SWI2 is used to invoke Service Requests.
/// 
/// Cycles (20 / 22)
/// Byte Count (2)
/// 
/// See Also: RTI
internal class _103F_Swi2_I_6809 : OpCode, IOpCode
{
    internal _103F_Swi2_I_6809(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        CC_E = true;

        M8[--S] = PC_L;
        M8[--S] = PC_H;
        M8[--S] = U_L;
        M8[--S] = U_H;
        M8[--S] = Y_L;
        M8[--S] = Y_H;
        M8[--S] = X_L;
        M8[--S] = X_H;
        M8[--S] = DP;
        M8[--S] = B;
        M8[--S] = A;
        M8[--S] = CC;

        PC = M16[Define.VSWI2];

        return 20;
    }
}
