using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models.Implementation;

internal class MemoryConfiguration : IMemoryConfiguration
{
    public RangeSelect<MemorySizes> Ram { get; } = new(true) { Value = MemorySizes._512K };

    public string ExternalBasicImage { get; private set; } = ""; //## READ-ONLY ##//

    public void SetExternalBasicImage(string value) => ExternalBasicImage = value;
}
