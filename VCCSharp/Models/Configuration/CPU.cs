using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class CPU
    {
        public RangeSelect<CPUTypes> Type { get; } = new RangeSelect<CPUTypes>();

        public bool ThrottleSpeed { get; set; } = true;
        public int CpuMultiplier { get; set; } = 227;
        public int FrameSkip { get; set; } = 1;
        public int MaxOverclock { get; set; } = 227;
    }
}
