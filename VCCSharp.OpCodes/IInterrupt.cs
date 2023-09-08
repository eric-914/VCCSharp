namespace VCCSharp.Models.CPU.OpCodes;

/// <summary>
/// Some opcodes interact with the interrupt process.
/// </summary>
public interface IInterrupt
{
    /// <summary>
    /// Halt Execution and Wait for Interrupt (SYNC/CWAI)
    /// </summary>
    /// <returns>Cycle count while waiting for interrupt</returns>
    int SynchronizeWithInterrupt();

    /// <summary>
    /// Called once interrupt has been handled (RTI)
    /// </summary>
    void ClearInterrupt();
}
