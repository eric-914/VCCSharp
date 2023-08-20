namespace VCCSharp.Configuration.Models.Implementation;

internal class StartupConfiguration : IStartupConfiguration
{
    public bool AutoStart { get; set; } = true;
    public bool CartridgeAutoStart { get; set; } = true;
}
