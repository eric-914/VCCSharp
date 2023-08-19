using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models;

public interface IMemoryConfiguration
{
    RangeSelect<MemorySizes> Ram { get; }
    string ExternalBasicImage { get; } //## READ-ONLY ##//
    void SetExternalBasicImage(string value);
}
