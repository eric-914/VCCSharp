using VCCSharp.Models.CPU.OpCodes;

namespace VCCSharp.OpCodes.Model.OpCodes;

internal abstract class OpCodeBase<T>
    where T : IMode, IInterrupt
{
    private readonly T _cpu;

    protected bool IsInInterrupt
    {
        get => _cpu.IsInInterrupt;
        set => _cpu.IsInInterrupt = value;
    }

    protected bool IsSyncWaiting
    {
        get => _cpu.IsSyncWaiting;
        set => _cpu.IsSyncWaiting = value;
    }

    protected int SyncCycle
    {
        get => _cpu.SyncCycle;
        set => _cpu.SyncCycle = value;
    }

    protected Cycles Cycles { get; }

    public OpCodeBase(T cpu)
    {
        _cpu = cpu;

        Cycles = new Cycles(cpu);
    }
}
