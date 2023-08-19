using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface ICPUConfiguration
{
    RangeSelect<CPUTypes> Type { get; }
    bool ThrottleSpeed { get; set; }
    int CpuMultiplier { get; set; }
    int FrameSkip { get; set; }
    int MaxOverclock { get; set; }

    //TODO: Probably not configuration related
    void AdjustOverclockSpeed(int change);
}
