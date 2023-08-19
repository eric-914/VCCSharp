namespace VCCSharp.Configuration.Models;

public interface IStartupConfiguration
{
    bool AutoStart { get; set; }
    bool CartridgeAutoStart { get; set; }
}
