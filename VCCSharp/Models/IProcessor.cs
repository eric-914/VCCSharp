using VCCSharp.Models.CPU.OpCodes;

namespace VCCSharp.Models;

public interface IProcessor
{
    void Init();
    int Exec(int cycleFor);
    void ForcePc(ushort address);
    void Reset();
    void AssertInterrupt(byte irq, byte flag);
    void DeAssertInterrupt(byte irq);
}

public interface ICpuProcessor : IProcessor, IRegisters, IBus, IInterrupt { }