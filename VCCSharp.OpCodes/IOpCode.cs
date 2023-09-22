namespace VCCSharp.OpCodes;

/// <summary>
/// OpCodes are a particular instruction to be executed.
/// </summary>
public interface IOpCode
{
    /// <summary>
    /// Default cycle count
    /// </summary>
    int CycleCount { get; }

    /// <summary>
    /// Final cycle count
    /// </summary>
    int Cycles { get; set; }

    /// <summary>
    /// Invokes the opcode
    /// </summary>
    /// <returns>The execution cycle count</returns>
    void Exec();
}
