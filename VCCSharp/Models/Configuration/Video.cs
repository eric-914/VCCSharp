using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public interface IVideoConfiguration
{
    RangeSelect<MonitorTypes> Monitor { get; }
    RangeSelect<PaletteTypes> Palette { get; }
    bool ScanLines { get; set; }
    bool ForceAspect { get; set; }
}

public class Video : IVideoConfiguration
{
    public RangeSelect<MonitorTypes> Monitor { get; } = new();

    public RangeSelect<PaletteTypes> Palette { get; } = new() { Value = PaletteTypes.Updated };

    public bool ScanLines { get; set; } = false;
    public bool ForceAspect { get; set; } = true;
}
