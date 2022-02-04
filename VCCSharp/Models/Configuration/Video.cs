using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Video
    {
        public RangeSelect<MonitorTypes> Monitor { get; } = new();

        public RangeSelect<PaletteTypes> Palette { get; } = new() { Value = PaletteTypes.Updated };

        public bool ScanLines { get; set; } = false;
        public bool ForceAspect { get; set; } = true;
    }
}