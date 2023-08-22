namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// ACCUMULATORS (A, B, D)
/// </summary>
/// <remarks>
/// The A and B registers are general purpose accumulators which are used for arithmetic calculations and manipulation of data. 
/// Certain instructions concatenate the A and B registers to form a single 16-bit accumulator. 
/// This is referred to as the D register, and is formed with the A register as the most significant byte.
/// </remarks>
public interface IRegisterD : IRegisterA, IRegisterB
{
    ushort D_REG { get; set; }
}

public interface IRegisterA
{
    byte A_REG { get; set; }
}

public interface IRegisterB
{
    byte B_REG { get; set; }
}
