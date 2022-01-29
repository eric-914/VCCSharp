namespace VCCSharp.Models.Configuration
{
    public class ConfigurationModel
    {
        public Version Version { get; } = new Version();
        public Window Window { get; } = new Window();

        public CPU CPU { get; } = new CPU();
        public Audio Audio { get; } = new Audio();
        public Video Video { get; } = new Video();
        public Memory Memory { get; } = new Memory();
        public Keyboard Keyboard { get; } = new Keyboard();
        public Joysticks Joysticks { get; } = new Joysticks();

        public FilePaths FilePaths { get; } = new FilePaths();
        public Startup Startup { get; } = new Startup();

        public Accessories Accessories { get; } = new Accessories();
    }
}
