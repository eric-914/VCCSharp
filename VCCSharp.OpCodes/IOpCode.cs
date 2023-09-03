namespace VCCSharp.OpCodes;

/// <summary>
/// OpCodes are a particular instruction to be executed.
/// </summary>
public interface IOpCode
{
    /// <summary>
    /// Invokes the opcode
    /// </summary>
    /// <returns>The execution cycle count</returns>
    int Exec();
}
