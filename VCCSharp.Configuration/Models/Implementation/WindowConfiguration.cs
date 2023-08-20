namespace VCCSharp.Configuration.Models.Implementation;

internal class WindowConfiguration : IWindowConfiguration
{
    public int Width { get; set; }
    public int Height { get; set; }

    public bool RememberSize { get; set; }
}
