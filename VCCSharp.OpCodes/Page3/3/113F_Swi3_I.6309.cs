using VCCSharp.OpCodes.Definitions;
using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page3;

/// <summary>
/// <code>113F/SWI/INHERENT</code>
/// Software Interrupt
/// </summary>
/// <remarks>
/// The <c>SWI3</c> instruction invokes a Software Interrupt.
/// </remarks>
/// 
/// The SWI, SWI2 and SWI3 instructions each invoke a Software Interrupt.
/// 
/// Each of these instructions first set the E flag in the CC register and then push the machine state onto the hardware stack (S).
/// 
/// After stacking the machine state, the SWI instruction sets the I and F interrupt masks in the CC register. 
/// SWI3 does not modify the mask.
/// ────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────────
///     ╭─────────────────╮  ╭───────────────╮
///     │      SWI3       │  │               ▼
///     ╰────────┬────────╯  │       ┌───────────────┐      
///              ▼           │       │ PUSH B, A, CC │
///       ┌────────────┐     │       └───────┬───────┘
///       │ SET: E = 1 │     │               ▼
///       └──────┬─────┘     │     ┌───────────────────┐
///              ▼           │     │        SWI3       │  
///      ┌─────────────────┐ │     └─────────┬─────────┘
///      │      PUSH       │ │               ▼
///      │ PC, U, Y, X, DP │ │        ┌───────────────┐
///      └───────┬─────────┘ │        │ PC ← [FFF2:3] │
///              ▼           │        └──────┬────────┘
///          ╔════════╗      │               │
///          ║ NM=1?  ╟──NO──╯               │
///          ╚═══╤════╝      ▲               │
///          YES │           │               │
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
/// Cycles (20 / 22)
/// Byte Count (2)
/// 
/// See Also: RTI
internal class _113F_Swi3_I_6309 : OpCode6309, IOpCode
{
    public int Exec()
    {
        int cycles = 20;

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

        if (MD_NATIVE6309)
        {
            M8[--S] = F;
            M8[--S] = E;

            cycles += 2;
        }

        M8[--S] = B;
        M8[--S] = A;
        M8[--S] = CC;

        PC = M16[Define.VSWI3];

        return cycles;
    }
}
