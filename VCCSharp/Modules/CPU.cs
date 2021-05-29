﻿using System.Runtime.InteropServices;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate void CPUASSERTINTERRUPT(byte irq, byte flag);

    // ReSharper disable once InconsistentNaming
    public interface ICPU
    {
        void CPUReset();
        void CPUInit();
        void CPUForcePC(ushort address);
        int CPUExec(int cycle);
        void CPUAssertInterrupt(CPUInterrupts irq, byte flag);
        void CPUDeAssertInterrupt(CPUInterrupts irq);
        void SetCPUToHD6309();
        void SetCPUToMC6809();
    }

    // ReSharper disable once InconsistentNaming
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
            _processor.AssertInterrupt((byte)irq, flag);
        }

        public void CPUDeAssertInterrupt(CPUInterrupts irq)
        {
            _processor.DeAssertInterrupt((byte)irq);
        }

        public void SetCPUToHD6309()
        {
            _processor = _modules.HD6309;
        }

        public void SetCPUToMC6809()
        {
            _processor = _modules.MC6809;
        }
    }
}
