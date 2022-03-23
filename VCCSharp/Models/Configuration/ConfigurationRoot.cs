namespace VCCSharp.Models.Configuration
{
    public class ConfigurationRoot : IConfigurationRoot
    {
        public Version Version { get; } = new();
        public Window Window { get; } = new();

        public CPU CPU { get; } = new();
        public Audio Audio { get; } = new();
        public Video Video { get; } = new();
        public Memory Memory { get; } = new();
        public Keyboard Keyboard { get; } = new();
        public Joysticks Joysticks { get; } = new();

        public FilePaths FilePaths { get; } = new();
        public Startup Startup { get; } = new();

        public Accessories Accessories { get; } = new();
    }
}
