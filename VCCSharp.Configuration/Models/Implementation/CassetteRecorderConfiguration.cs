using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Models.Implementation;

internal class CassetteRecorderConfiguration : ICassetteRecorderConfiguration
{
    public int TapeCounter { get; set; }
    public TapeModes TapeMode { get; set; } = TapeModes.Stop;

    public string? TapeFileName { get; set; }
}
