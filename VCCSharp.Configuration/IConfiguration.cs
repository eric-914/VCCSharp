using VCCSharp.Configuration.Models;

namespace VCCSharp.Configuration;

public interface IConfiguration
{
    IVersion Version { get; }

    IWindowConfiguration Window { get; }
    ICPUConfiguration CPU { get; }
    IAudioConfiguration Audio { get; }
    IVideoConfiguration Video { get; }
    IMemoryConfiguration Memory { get; }
    IKeyboardConfiguration Keyboard { get; }
    IJoysticksConfiguration Joysticks { get; }
    IStartupConfiguration Startup { get; }

    IFilePaths FilePaths { get; }
    IAccessories Accessories { get; }
    ICassetteRecorder CassetteRecorder { get; }
    ISerialPort SerialPort { get; }
}
