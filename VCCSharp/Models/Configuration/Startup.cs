namespace VCCSharp.Models.Configuration;

public interface IStartupConfiguration
{
    bool AutoStart { get; set; }
    bool CartridgeAutoStart { get; set; }
}

public class Startup : IStartupConfiguration
{
    public bool AutoStart { get; set; } = true;
    public bool CartridgeAutoStart { get; set; } = true;
}
