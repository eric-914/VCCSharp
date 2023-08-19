using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

public class Startup : IStartupConfiguration
{
    public bool AutoStart { get; set; } = true;
    public bool CartridgeAutoStart { get; set; } = true;
}
