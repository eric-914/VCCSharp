using VCCSharp.Models.CPU.OpCodes;

namespace VCCSharp.OpCodes.HD6309;

public interface IState : IMode, IInterrupt, IRegisters, IMemory
{
}
