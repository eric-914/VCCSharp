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
