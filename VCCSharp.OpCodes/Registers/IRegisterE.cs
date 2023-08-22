namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// ACCUMULATORS (E, F, W)
/// 🚫 6309 ONLY 🚫
/// The W register is like the D register in the 6809. 
/// It is a  concatenated register containing the values of ACCE and ACCF as one 16 bit  value. 
/// ACCE is contained in the high 8 bits and ACCF is contained in the low 8 bits. 
/// </summary>
public interface IRegisterW : IRegisterE, IRegisterF
{
    ushort W_REG { get; set; }
}

public interface IRegisterE
{
    /// <summary>
    /// 🚫 6309 ONLY 🚫
    /// ACCE - 8 bit accumulator. 
    /// </summary>
    byte E_REG { get; set; }
}

public interface IRegisterF
{
    /// <summary>
    /// 🚫 6309 ONLY 🚫
    /// ACCF - 8 bit accumulator. 
    /// </summary>
    byte F_REG { get; set; }
}
