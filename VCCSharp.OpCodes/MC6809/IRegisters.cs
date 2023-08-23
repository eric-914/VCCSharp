using VCCSharp.OpCodes.Registers;

namespace VCCSharp.OpCodes.MC6809;

/// <summary>
/// All registers required for MC6809 CPU OpCodes
/// </summary>
public interface IRegisters : IRegisterPC, IRegisterDP, IRegisterD, IRegisterX, IRegisterY, IRegisterS, IRegisterU, IRegisterCC
{
    //TOOD: Refactor these away from this interface
    void AUR(int i, byte value);
    void OUR(int i, byte value);
    void XUR(int i, byte value);
}
