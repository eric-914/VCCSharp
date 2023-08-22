using VCCSharp.Models.CPU.OpCodes;

namespace VCCSharp.OpCodes.MC6809;

public interface IState : IMode, IInterrupt, IRegisters, IMemory
{
}
