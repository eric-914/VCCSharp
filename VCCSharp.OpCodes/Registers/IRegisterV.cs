namespace VCCSharp.OpCodes.Registers;

/// <summary>
/// Transfer Value Register
/// The Transfer Value register V is never stacked upon interrupts. No instructions are provided to directly push or pull the V register.
/// </summary>
public interface IRegisterV
{
    ushort V_REG { get; set; }
}