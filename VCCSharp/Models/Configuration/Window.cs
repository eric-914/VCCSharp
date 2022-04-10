namespace VCCSharp.Models.Configuration;

public interface IWindowConfiguration
{
    int Width { get; set; }
    int Height { get; set; }
    bool RememberSize { get; set; }
}

public class Window : IWindowConfiguration
{
    public int Width { get; set; }
    public int Height { get; set; }

    public bool RememberSize { get; set; }
}
