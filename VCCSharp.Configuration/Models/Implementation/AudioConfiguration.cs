using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

internal class AudioConfiguration : IAudioConfiguration
{
    public string Device { get; set; } = "Primary Sound Driver";

    public RangeSelect<AudioRates> Rate { get; } = new(true) { Value = AudioRates._44100Hz };
}
