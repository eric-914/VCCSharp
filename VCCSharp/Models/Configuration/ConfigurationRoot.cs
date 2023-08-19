using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Models.Implementation;
using Implementation = VCCSharp.Configuration.Models.Implementation;

namespace VCCSharp.Models.Configuration;

public class ConfigurationRoot : IConfiguration
{
    public IVersion Version { get; } = new Implementation.Version();

    public IWindowConfiguration Window { get; } = new Window();
    public ICPUConfiguration CPU { get; } = new CPU();
    public IAudioConfiguration Audio { get; } = new Audio();
    public IVideoConfiguration Video { get; } = new Video();
    public IMemoryConfiguration Memory { get; } = new Memory();
    public IKeyboardConfiguration Keyboard { get; } = new Keyboard();
    public IJoysticksConfiguration Joysticks { get; } = new Joysticks();
    public IStartupConfiguration Startup { get; } = new Startup();

    public IFilePaths FilePaths { get; } = new FilePaths();
    public Accessories Accessories { get; } = new();
    public SerialPort SerialPort { get; } = new();
    public CassetteRecorder CassetteRecorder { get; } = new();
}
