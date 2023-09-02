namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// 32 bit concatenated register (ACCA, ACCB, ACCE and ACCF combined).
/// </summary>
/// <remarks>
/// The <c>Q</c> register is a 32 bit concatenated register. 
/// This register is the same as the <c>D</c> and <c>W</c> register except for one respect. 
/// It contains the values of <c>A</c>, <c>B</c>, <c>E</c> and <c>F</c> respectively. 
/// This register is used mostly with the additional math instructions supplied with the 6309.
/// <code>🚫 6309 ONLY 🚫</code>
/// </remarks>
public interface IRegisterQ : IRegisterA, IRegisterB, IRegisterE, IRegisterF, IRegisterD, IRegisterW
{
    uint Q { get; set; }
}
