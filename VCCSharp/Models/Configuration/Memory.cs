using VCCSharp.Enums;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration
{
    public class Memory
    {
        public RangeSelect<MemorySizes> Ram { get; } = new RangeSelect<MemorySizes>(true) { Value = MemorySizes._512K };

        public string ExternalBasicImage { get; private set; } = ""; //## READ-ONLY ##//

        public void SetExternalBasicImage(string value) => ExternalBasicImage = value;
    }
}