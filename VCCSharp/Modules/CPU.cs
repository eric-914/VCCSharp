using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    // ReSharper disable InconsistentNaming
    public interface ICPU
    {
        void Reset();
        void Init();
        void ForcePC(ushort address);
        int Exec(int cycle);
        void AssertInterrupt(CPUInterrupts irq, byte flag);
        void DeAssertInterrupt(CPUInterrupts irq);
        void SetHD6309();
        void SetMC6809();
    }
    // ReSharper restore InconsistentNaming

    // ReSharper disable once InconsistentNaming
    public class CPU : ICPU
    {
        private readonly IModules _modules;

        private IProcessor _processor;

        public CPU(IModules modules)
        {
            _modules = modules;
        }

        public void Reset()
        {
            _processor.Reset();
        }

        public void Init()
        {
            _processor.Init();
        }

        public void ForcePC(ushort address)
        {
            _processor.ForcePc(address);
        }

        public int Exec(int cycle)
        {
            return _processor.Exec(cycle);
        }

        public void AssertInterrupt(CPUInterrupts irq, byte flag)
        {
            _processor.AssertInterrupt((byte)irq, flag);
        }

        public void DeAssertInterrupt(CPUInterrupts irq)
        {
            _processor.DeAssertInterrupt((byte)irq);
        }

        public void SetHD6309()
        {
            _processor = _modules.HD6309;
        }

        public void SetMC6809()
        {
            _processor = _modules.MC6809;
        }
    }
}
