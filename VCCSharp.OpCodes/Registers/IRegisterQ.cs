namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// 32 bit concatenated register (ACCA, ACCB, ACCE and ACCF combined).
/// </summary>
/// <remarks>
/// The Q register is a 32 bit concatenated register. 
/// This register is the same as the D and W register except for one respect. 
/// It contains the values of ACCA, ACCB, ACCE and ACCF respectively. 
/// This register is used mostly with the additional math instructions supplied with the 6309.
/// </remarks>
public interface IRegisterQ : IRegisterA, IRegisterB, IRegisterE, IRegisterF, IRegisterD, IRegisterW
{
    uint Q_REG { get; set; }
}
