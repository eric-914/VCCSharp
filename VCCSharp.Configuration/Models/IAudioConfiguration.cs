using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IAudioConfiguration
{
    string Device { get; set; }
    RangeSelect<AudioRates> Rate { get; }
}
