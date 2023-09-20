using VCCSharp.OpCodes.Model.OpCodes;

namespace VCCSharp.OpCodes.Page1;

/// <summary>
/// <code>13/SYNC/IMMEDIATE</code>
/// Synchronize with Interrupt Line
/// </summary>
/// <remarks>
/// Halt Execution and Wait for Interrupt
/// </remarks>
/// 
/// The SYNC instruction allows software to synchronize itself with an external hardware event (interrupt). 
/// When executed, SYNC places the CPU’s data and address busses into a high-impedance state, stops executing instructions and waits for an interrupt. 
/// None of the Condition Code flags are directly affected by this instruction.
/// 
/// When a signal is asserted on any one of the CPU’s 3 interrupt lines (IRQ, FIRQ or NMI), the CPU clears the synchronizing state and resumes processing. 
/// If the interrupt type is not masked and the interrupt signal remains asserted for at least 3 cycles, then the CPU will stack the machine state accordingly and vector to the interrupt service routine. 
/// If the interrupt type is masked, or the interrupt signal was asserted for less than 3 cycles, then the CPU will simply resume execution at the following instruction without invoking the interrupt service routine.
/// 
/// Typically, SYNC is executed with interrupts masked so that the following instruction will be executed as quickly as possible after the synchronizing event occurs (no service routine overhead). 
/// Unlike CWAI, the SYNC instruction does not include the ability to set or clear the interrupt masks as part of its operation. 
/// A separate ORCC or ANDCC instruction would be needed to accomplish this.
/// 
/// SYNC may be useful for synchronizing with a video display or for performing fast data acquisition from an I/O device.
/// 
/// Cycles (≥ 4 / ≥ 3)
/// Byte Count (1)
/// 
/// See Also: ANDCC, CWAI, RTI, SYNC
internal class _13_Sync_I : OpCode, IOpCode
{
    public int CycleCount => SynchronizeWithInterrupt();

    public int Exec()
    {
        return CycleCount;
    }
}
