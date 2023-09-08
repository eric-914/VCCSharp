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
    void AssertInterrupt(byte irq, byte flag);
    void DeAssertInterrupt(byte irq);
}
