using Ninject;
using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Cpu;

public class CpuTabViewModel : NotifyViewModel
{
    private readonly ICPUConfiguration _cpu = new CPU();
    private readonly IMemoryConfiguration _memory = ConfigurationFactory.MemoryConfiguration();

    public int MaxOverclock => _cpu.MaxOverclock;

    public CpuTabViewModel() { }

    [Inject]
    public CpuTabViewModel(ICPUConfiguration cpu, IMemoryConfiguration memory)
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
