namespace VCCSharp.Models.Configuration;

public interface IConfiguration
{
    Version Version { get; }
    Window Window { get; }
    CPU CPU { get; }
    IAudioConfiguration Audio { get; }
    Video Video { get; }
    Memory Memory { get; }
    Keyboard Keyboard { get; }
    Joysticks Joysticks { get; }
    FilePaths FilePaths { get; }
    Startup Startup { get; }
    Accessories Accessories { get; }
    CassetteRecorder CassetteRecorder { get; }
    SerialPort SerialPort { get; }
}
