using VCCSharp.Configuration.Models;

namespace VCCSharp.Configuration;

public interface IConfiguration
{
    IVersionConfiguration Version { get; }

    IWindowConfiguration Window { get; }
    ICPUConfiguration CPU { get; }
    IAudioConfiguration Audio { get; }
    IVideoConfiguration Video { get; }
    IMemoryConfiguration Memory { get; }
    IKeyboardConfiguration Keyboard { get; }
    IJoysticksConfiguration Joysticks { get; }
    IStartupConfiguration Startup { get; }

    IFilePathsConfiguration FilePaths { get; }
    IAccessoriesConfiguration Accessories { get; }
    ICassetteRecorderConfiguration CassetteRecorder { get; }
    ISerialPortConfiguration SerialPort { get; }
}
