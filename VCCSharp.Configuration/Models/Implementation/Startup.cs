namespace VCCSharp.Configuration.Models.Implementation;

public class Startup : IStartupConfiguration
{
    public bool AutoStart { get; set; } = true;
    public bool CartridgeAutoStart { get; set; } = true;
}
