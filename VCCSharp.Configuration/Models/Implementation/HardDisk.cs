using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

public class HardDisk : IHardDisk
{
    public string FilePath { get; set; } = "";
}
