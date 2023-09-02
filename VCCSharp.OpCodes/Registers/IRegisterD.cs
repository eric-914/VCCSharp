namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// ACCUMULATORS (A, B, D)
/// </summary>
/// <remarks>
/// The <c>A</c> and <c>B</c> registers are general purpose accumulators which are used for arithmetic calculations and manipulation of data. 
/// Certain instructions concatenate the <c>A</c> and <c>B</c> registers to form a single 16-bit accumulator. 
/// This is referred to as the <c>D</c> register, and is formed with the <c>A</c> register as the most significant byte and <c>B</c> as the least significant byte.
/// </remarks>
public interface IRegisterD : IRegisterA, IRegisterB
{
    /// <summary>
    /// 16 bit accumulator <c>D</c>. 
    /// </summary>
    ushort D { get; set; }
}

public interface IRegisterA
{
    /// <summary>
    /// 8 bit accumulator <c>A</c>. 
    /// </summary>
    byte A { get; set; }
}

public interface IRegisterB
{
    /// <summary>
    /// 8 bit accumulator <c>B</c>. 
    /// </summary>
    byte B { get; set; }
}
