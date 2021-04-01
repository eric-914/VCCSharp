using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IMC6809 : IProcessor
    {

    }

    public class MC6809 : IMC6809
    {
        public void Init()
        {
            Library.MC6809.MC6809Init();
        }

        public int Exec(int cycleFor)
        {
            return Library.MC6809.MC6809Exec(cycleFor);
        }

        public void ForcePC(ushort xferAddress)
        {
            Library.MC6809.MC6809ForcePC(xferAddress);
        }

        public void Reset()
        {
            Library.MC6809.MC6809Reset();
        }

        public void AssertInterrupt(byte irq, byte flag)
        {
            Library.MC6809.MC6809AssertInterrupt(irq, flag);
        }

        public void DeAssertInterrupt(byte irq)
        {
            Library.MC6809.MC6809DeAssertInterrupt(irq);
        }
    }
}
