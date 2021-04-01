namespace VCCSharp.Models
{
    public interface IProcessor
    {
        void Init();
        int Exec(int cycleFor);
        void ForcePC(ushort xferAddress);
        void Reset();
        void AssertInterrupt(byte irq, byte flag);
        void DeAssertInterrupt(byte irq);
    }
}
