using VCCSharp.Configuration.Options;

namespace VCCSharp.Configuration.Models
{
    public interface ICassetteRecorder
    {
        int TapeCounter { get; set; }
        string? TapeFileName { get; set; }
        TapeModes TapeMode { get; set; }
    }
}