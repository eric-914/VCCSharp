using Ninject;
using VCCSharp.Enums;
using VCCSharp.Models.Configuration;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.TabControls.Cpu;

public class CpuTabViewModel : NotifyViewModel
{
    private readonly ICPUConfiguration _model = new CPU();
    private readonly IMemoryConfiguration _memory = new Memory();

    public int MaxOverclock => _model.MaxOverclock;

    public CpuTabViewModel() { }

    [Inject]
    public CpuTabViewModel(ICPUConfiguration model, IMemoryConfiguration memory)
    {
        _model = model;
        _memory = memory;
    }

    public CPUTypes CpuType
    {
        get => _model.Type.Value;
        set => _model.Type.Value = value;
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
        get => _model.CpuMultiplier;
        set
        {
            if (value == _model.CpuMultiplier) return;

            _model.CpuMultiplier = (byte)value;
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
