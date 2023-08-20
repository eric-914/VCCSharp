using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Models.Implementation;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration
{
    public static class ConfigurationFactory
    {
        public static IAccessoriesConfiguration AccessoriesConfiguration() => new AccessoriesConfiguration();
        public static IAudioConfiguration AudioConfiguration() => new AudioConfiguration();
        public static ICassetteRecorderConfiguration CassetteRecorderConfiguration() => new CassetteRecorderConfiguration();
        public static ICPUConfiguration CPUConfiguration() => new CPUConfiguration();
        public static IFilePathsConfiguration FilePathsConfiguration() => new FilePathsConfiguration();
        public static IKeyboardConfiguration KeyboardConfiguration() => new KeyboardConfiguration();
        public static IMemoryConfiguration MemoryConfiguration() => new MemoryConfiguration();
        public static ISerialPortConfiguration SerialPortConfiguration() => new SerialPortConfiguration();
        public static IStartupConfiguration StartupConfiguration() => new StartupConfiguration();
        public static IVersionConfiguration VersionConfiguration() => new VersionConfiguration();
        public static IVideoConfiguration VideoConfiguration() => new VideoConfiguration();
        public static IWindowConfiguration WindowConfiguration() => new WindowConfiguration();
    }
}
