namespace VCCSharp.Configuration.Models;

public interface IWindowConfiguration
{
    int Width { get; set; }
    int Height { get; set; }
    bool RememberSize { get; set; }
}
