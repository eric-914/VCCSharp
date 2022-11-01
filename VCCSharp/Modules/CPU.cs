using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models;

namespace VCCSharp.Modules;

// ReSharper disable InconsistentNaming
public interface ICPU : IModule, IChip
{
    void Init(CPUTypes cpu);
    void ForcePC(ushort address);
    int Exec(int cycle);
    void AssertInterrupt(CPUInterrupts irq, byte flag);
    void DeAssertInterrupt(CPUInterrupts irq);
}
// ReSharper restore InconsistentNaming

/// <summary>
/// A wrapper around the true CPU-processor instance.
/// HD6309 vs. MC6809
/// </summary>
// ReSharper disable once InconsistentNaming
public class CPU : ICPU
{
    private readonly IModules _modules;

    private IProcessor _processor;

    public CPU(IModules modules)
    {
        _modules = modules;
        _processor = _modules.MC6809; //--Default to 6809 until specified otherwise.
    }

    public void Init(CPUTypes cpu)
    {
        _processor = cpu == CPUTypes.HD6309 ? _modules.HD6309 : _modules.MC6809;

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

    public void ModuleReset()
    {
        ChipReset();
    }

    public void ChipReset()
    {
        _processor.Reset();
    }
}