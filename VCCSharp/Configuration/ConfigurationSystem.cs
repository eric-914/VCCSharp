using VCCSharp.IoC;

namespace VCCSharp.Configuration;

public class ConfigurationSystem : IConfigurationSystem
{
    private readonly IModules _modules;

    public ConfigurationSystem(IModules modules)
    {
        _modules = modules;
    }

    public string GetExecPath() => _modules.Vcc.GetExecPath();
}
