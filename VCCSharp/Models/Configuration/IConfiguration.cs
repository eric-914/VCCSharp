using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Models.Implementation;

namespace VCCSharp.Models.Configuration;

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
    Accessories Accessories { get; }
    CassetteRecorder CassetteRecorder { get; }
    SerialPort SerialPort { get; }
}
