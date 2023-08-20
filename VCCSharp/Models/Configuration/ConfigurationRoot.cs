using VCCSharp.Configuration;
using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

public class ConfigurationRoot : IConfiguration
{
    public IVersionConfiguration Version { get; } = ConfigurationFactory.VersionConfiguration();

    public IWindowConfiguration Window { get; } = ConfigurationFactory.WindowConfiguration();
    public ICPUConfiguration CPU { get; } = ConfigurationFactory.CPUConfiguration();
    public IAudioConfiguration Audio { get; } = ConfigurationFactory.AudioConfiguration();
    public IVideoConfiguration Video { get; } = ConfigurationFactory.VideoConfiguration();
    public IMemoryConfiguration Memory { get; } = ConfigurationFactory.MemoryConfiguration();
    public IKeyboardConfiguration Keyboard { get; } = ConfigurationFactory.KeyboardConfiguration();
    public IJoysticksConfiguration Joysticks { get; } = new Joysticks();
    public IStartupConfiguration Startup { get; } = ConfigurationFactory.StartupConfiguration();

    public IFilePathsConfiguration FilePaths { get; } = ConfigurationFactory.FilePathsConfiguration();
    public IAccessoriesConfiguration Accessories { get; } = ConfigurationFactory.AccessoriesConfiguration();
    public ISerialPortConfiguration SerialPort { get; } = ConfigurationFactory.SerialPortConfiguration();
    public ICassetteRecorderConfiguration CassetteRecorder { get; } = ConfigurationFactory.CassetteRecorderConfiguration();
}
