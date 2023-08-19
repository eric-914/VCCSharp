using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

public class Window : IWindowConfiguration
{
    public int Width { get; set; }
    public int Height { get; set; }

    public bool RememberSize { get; set; }
}
