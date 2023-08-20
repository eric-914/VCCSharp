using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IVideoConfiguration
{
    RangeSelect<MonitorTypes> Monitor { get; }
    RangeSelect<PaletteTypes> Palette { get; }
    bool ScanLines { get; set; }
    bool ForceAspect { get; set; }
}
