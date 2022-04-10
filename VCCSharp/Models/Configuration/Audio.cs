using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public interface IAudioConfiguration
{
    string Device { get; set; }
    RangeSelect<AudioRates> Rate { get; }
}

public class Audio : IAudioConfiguration
{
    public string Device { get; set; } = "Primary Sound Driver";

    public RangeSelect<AudioRates> Rate { get; } = new(true) { Value = AudioRates._44100Hz };
}
