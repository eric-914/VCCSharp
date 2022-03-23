using VCCSharp.Enums;

namespace VCCSharp.Models.Configuration;

public class CassetteRecorder
{
    public int TapeCounter { get; set; }
    public TapeModes TapeMode { get; set; } = TapeModes.Stop;

    public string? TapeFileName { get; set; }
}
