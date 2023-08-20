using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Models.Implementation;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration
{
    /// <summary>
    /// The top level (root) configuration object.
    /// </summary>
    public class RootConfiguration : IConfiguration
    {
        public IVersionConfiguration Version { get; } = new VersionConfiguration();

        public IWindowConfiguration Window { get; } = new WindowConfiguration();
        public ICPUConfiguration CPU { get; } = new CPUConfiguration();
        public IAudioConfiguration Audio { get; } = new AudioConfiguration();
        public IVideoConfiguration Video { get; } = new VideoConfiguration();
        public IMemoryConfiguration Memory { get; } = new MemoryConfiguration();
        public IKeyboardConfiguration Keyboard { get; } = new KeyboardConfiguration();
        public IJoysticksConfiguration Joysticks { get; } = new JoysticksConfiguration();
        public IStartupConfiguration Startup { get; } = new StartupConfiguration();

        public IFilePathsConfiguration FilePaths { get; } = new FilePathsConfiguration();
        public IAccessoriesConfiguration Accessories { get; } = new AccessoriesConfiguration();
        public ISerialPortConfiguration SerialPort { get; } = new SerialPortConfiguration();
        public ICassetteRecorderConfiguration CassetteRecorder { get; } = new CassetteRecorderConfiguration();
    }
}
