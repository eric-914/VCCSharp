using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface ICPU
    {
        void CPUReset();
        void CPUInit();
        void CPUForcePC(ushort xferAddress);
        int CPUExec(int cycle);
        void CPUAssertInterrupt(CPUInterrupts irq, byte flag);
        void CPUDeAssertInterrupt(CPUInterrupts irq);
        void SetCPUToHD6309();
        void SetCPUToMC6809();
    }

    public class CPU : ICPU
    {
        private readonly IModules _modules;

        private IProcessor _processor;

        public CPU(IModules modules)
        {
            _modules = modules;
        }

        public void CPUReset()
        {
            _processor.Reset();
        }

        public void CPUInit()
        {
            _processor.Init();
        }

        public void CPUForcePC(ushort xferAddress)
        {
            _processor.ForcePC(xferAddress);
        }

        public int CPUExec(int cycle)
        {
            return _processor.Exec(cycle);
        }

        public void CPUAssertInterrupt(CPUInterrupts irq, byte flag)
        {
            Library.CPU.CPUAssertInterrupt((byte)irq, flag);
        }

        public void CPUDeAssertInterrupt(CPUInterrupts irq)
        {
            Library.CPU.CPUDeAssertInterrupt((byte)irq);
        }

        public void SetCPUToHD6309()
        {
            _processor = _modules.HD6309;

            Library.CPU.SetCPUToHD6309();
        }

        public void SetCPUToMC6809()
        {
            _processor = _modules.MC6809;

            Library.CPU.SetCPUToMC6809();
        }

    }
}
