namespace VCCSharp.Models.Configuration
{
    public interface IConfiguration
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

    public class Root : IConfiguration
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
