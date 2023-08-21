using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Cpu;

public interface ICpuTabViewModel
{
    CPUTypes? Cpu { get; set; }
    int CpuMultiplier { get; set; }
    CPUTypes CpuType { get; set; }
    int MaxOverclock { get; }
    MemorySizes? Memory { get; set; }
    MemorySizes RamSize { get; set; }
}

public abstract class CpuTabViewModelBase : NotifyViewModel, ICpuTabViewModel
{
    private readonly ICPUConfiguration _cpu;
    private readonly IMemoryConfiguration _memory;

    public int MaxOverclock => _cpu.MaxOverclock;

    protected CpuTabViewModelBase(ICPUConfiguration cpu, IMemoryConfiguration memory)
    {
        _cpu = cpu;
        _memory = memory;
    }

    public CPUTypes CpuType
    {
        get => _cpu.Type.Value;
        set => _cpu.Type.Value = value;
    }

    public CPUTypes? Cpu
    {
        get => CpuType;
        set
        {
            if (value.HasValue)
            {
                CpuType = value.Value;
                OnPropertyChanged();
            }
        }
    }

    public int CpuMultiplier
    {
        get => _cpu.CpuMultiplier;
        set
        {
            if (value == _cpu.CpuMultiplier) return;

            _cpu.CpuMultiplier = (byte)value;
            OnPropertyChanged();
        }
    }

    public MemorySizes? Memory
    {
        get => RamSize;
        set
        {
            if (value.HasValue)
            {
                RamSize = value.Value;
                OnPropertyChanged();
            }
        }
    }

    public MemorySizes RamSize
    {
        get => _memory.Ram.Value;
        set => _memory.Ram.Value = value;
    }

}

public class CpuTabViewModelStub : CpuTabViewModelBase
{
    public CpuTabViewModelStub() : base(ConfigurationFactory.CPUConfiguration(), ConfigurationFactory.MemoryConfiguration()) { }
}

public class CpuTabViewModel : CpuTabViewModelBase
{
    public CpuTabViewModel(ICPUConfiguration cpu, IMemoryConfiguration memory) : base(cpu, memory) { }
}
