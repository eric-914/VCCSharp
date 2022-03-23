namespace VCCSharp.Models.Configuration;

public interface IConfigurationRoot
{
    Version Version { get; }
    Window Window { get; }
    CPU CPU { get; }
    Audio Audio { get; }
    Video Video { get; }
    Memory Memory { get; }
    Keyboard Keyboard { get; }
    Joysticks Joysticks { get; }
    FilePaths FilePaths { get; }
    Startup Startup { get; }
    Accessories Accessories { get; }
}