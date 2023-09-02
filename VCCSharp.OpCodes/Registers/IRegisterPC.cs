namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// PROGRAM COUNTER (PC)
/// </summary>
/// <remarks>
/// The program counter is used by the processor to point to the address of the next instruction to be executed by the processor. 
/// Relative addressing is provided allowing the program counter to be used like an index register in some situations.
/// </remarks>
public interface IRegisterPC
{
    ushort PC { get; set; }
}
