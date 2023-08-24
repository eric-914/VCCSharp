using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>3C/CWAI/INHERENT</code>
/// AND Condition Code register, then wait for interrupt
/// <code>CC’ ← CC AND IMM</code>
/// </summary>
/// <remarks>
/// This instruction logically ANDs the contents of the Condition Codes register with the 8-bit value specified by the immediate operand. 
/// The CPU then halts execution and waits for an unmasked interrupt to occur. 
/// </remarks>
/// 
/// This instruction logically ANDs the contents of the Condition Codes register with the 8-bit value specified by the immediate operand. 
/// The result is placed back into the Condition Codes register. 
/// 
/// TODO: Is this [IsSyncWaiting = true]?
/// The E flag in the CC register is then set and the entire machine state is pushed onto the hardware stack (S). 
///     (Push Onto S Stack: PC, U, Y, X, DP, [W (If NM = 1)], D, CC)
/// The CPU then halts execution and waits for an unmasked interrupt to occur. 
/// When such an interrupt occurs, the CPU resumes execution at the address obtained from the corresponding interrupt vector.
/// 
/// You can specify a value in the immediate operand to clear either or both the I and F interrupt masks to ensure that the desired interrupt types are enabled. 
/// One of the following values is typically used for the immediate operand:
///         $FF = Leave CC unmodified
///         $EF = Enable IRQ
///         $BF = Enable FIRQ
///         $AF = Enable both IRQ and FIRQ
/// 
/// Some assemblers will accept a comma-delimited list of the Condition Code bits to be cleared as an alternative to the immediate value. 
/// For example:
///         CWAI I,F ; Clear I and F, wait for interrupt
///         
/// It is important to note that because the entire machine state is stacked prior to the actual occurrence of an interrupt, any FIRQ service routine that may be invoked must not assume that PC and CC are the only registers that have been stacked. 
/// The RTI instruction will operate correctly in this situation because CWAI sets the E flag prior to stacking the CC register.
/// 
/// Unlike SYNC, the CWAI instruction does not place the data and address busses in a high-impedance state while waiting for an interrupt.
/// 
/// CWAI #i8
/// I8 : 8-bit Immediate value
/// Cycles (22 / 20)
/// Byte Count (2)
/// 
/// See Also: ANDCC, RTI, SYNC
internal class _3C_Cwai_I : OpCode, IOpCode
{
    internal _3C_Cwai_I(MC6809.IState cpu) : base(cpu) { }

    public int Exec()
    {
        byte value = M8[PC++];

        CC &= value;

        IsSyncWaiting = true;

        return SyncCycle;
    }
}
