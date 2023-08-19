using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Options;
using VCCSharp.Configuration.Support;

namespace VCCSharp.Models.Configuration;

public class Memory : IMemoryConfiguration
{
    public RangeSelect<MemorySizes> Ram { get; } = new(true) { Value = MemorySizes._512K };

    public string ExternalBasicImage { get; private set; } = ""; //## READ-ONLY ##//

    public void SetExternalBasicImage(string value) => ExternalBasicImage = value;
}
