namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// INDEX REGISTER (X)
/// </summary>
/// <remarks>
/// The index registers are used in indexed mode of addressing. 
/// The 16-bit address in this register takes part in the calculation of effective addresses. 
/// This address may be used to point to data directly or may be modified by an optional constant or register offset. 
/// During some indexed modes, the contents of the index register are incremented and decremented to point to the next item of tabular type data. 
/// All four pointer registers (X, Y, U, S) may be used as index registers.
/// </remarks>
public interface IRegisterX
{
    ushort X_REG { get; set; }

    byte X_L { get; set; }
    byte X_H { get; set; }
}
