using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public interface ICPUConfiguration
{
    RangeSelect<CPUTypes> Type { get; }
    bool ThrottleSpeed { get; set; }
    int CpuMultiplier { get; set; }
    int FrameSkip { get; set; }
    int MaxOverclock { get; set; }
    void AdjustOverclockSpeed(int change);
}

public class CPU : ICPUConfiguration
{
    public RangeSelect<CPUTypes> Type { get; } = new();

    public bool ThrottleSpeed { get; set; } = true;
    public int CpuMultiplier { get; set; } = 227;
    public int FrameSkip { get; set; } = 1;
    public int MaxOverclock { get; set; } = 227;

    /**
     * Increase/Decrease the overclock speed, as seen after a POKE 65497,0.
     * Valid values are [2,100].
     * Setting this value to 0 will make the emulator pause.  Hence the minimum of 2.
     */
    public void AdjustOverclockSpeed(int change)
    {
        int cpuMultiplier = CpuMultiplier + change;

        if (cpuMultiplier < 2 || cpuMultiplier > MaxOverclock)
        {
            return;
        }

        CpuMultiplier = cpuMultiplier;
    }
}
