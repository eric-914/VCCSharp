using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

/// <summary>
/// All registers required for MC6809 CPU OpCodes
/// </summary>
public interface IRegisters : IRegisterPC, IRegisterDP, IRegisterD, IRegisterX, IRegisterY, IRegisterS, IRegisterU, IRegisterCC
{
}
