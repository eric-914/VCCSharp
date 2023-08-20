using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

internal class HardDiskConfiguration : IHardDiskConfiguration
{
    public string FilePath { get; set; } = "";
}
