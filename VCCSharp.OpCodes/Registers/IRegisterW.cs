namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// ACCUMULATORS (E, F, W)
/// The <c>W</c> register is like the <c>D</c> register in the 6809. 
/// It is a  concatenated register containing the values of <c>E</c> and <c>F</c> as one 16 bit  value. 
/// <c>E</c> is contained in the high 8 bits and <c>F</c> is contained in the low 8 bits. 
/// <code>🚫 6309 ONLY 🚫</code>
/// </summary>
public interface IRegisterW //: IRegisterE, IRegisterF
{
    /// <summary>
    /// 16 bit accumulator <c>W</c>. 
    /// <code>🚫 6309 ONLY 🚫</code>
    /// </summary>
    ushort W { get; set; }
}

public interface IRegisterE
{
    /// <summary>
    /// 8 bit accumulator <c>E</c>. 
    /// <code>🚫 6309 ONLY 🚫</code>
    /// </summary>
    byte E { get; set; }
}

public interface IRegisterF
{
    /// <summary>
    /// 8 bit accumulator <c>F</c>. 
    /// <code>🚫 6309 ONLY 🚫</code>
    /// </summary>
    byte F { get; set; }
}
