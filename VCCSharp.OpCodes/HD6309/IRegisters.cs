using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.HD6309;

/// <summary>
/// All registers required for HD6309 CPU OpCodes, which are all MC6809 registered, extended.
/// </summary>
public interface IRegisters : MC6809.IRegisters, IRegisterE, IRegisterF, IRegisterW, IRegisterMD, IRegisterZ, IRegisterQ
{
}
