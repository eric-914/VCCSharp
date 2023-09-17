using VCCSharp.Enums;

namespace VCCSharp.Models;

/// <summary>
/// This is how the emulation system sees the processing unit.
/// </summary>
public interface IProcessor
{
    void Init();
    int Exec(int cycleFor);
    void ForcePc(ushort address);
    void Reset();
    void AssertInterrupt(CPUInterrupts irq, byte flag);
    void DeAssertInterrupt(CPUInterrupts irq);
}
