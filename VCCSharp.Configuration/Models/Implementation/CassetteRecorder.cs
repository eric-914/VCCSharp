using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Models.Implementation;

public class CassetteRecorder : ICassetteRecorder
{
    public int TapeCounter { get; set; }
    public TapeModes TapeMode { get; set; } = TapeModes.Stop;

    public string? TapeFileName { get; set; }
}
