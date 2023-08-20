using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models
{
    public interface IMultiPakConfiguration
    {
        string FilePath { get; set; }
        IMultiSlotsConfiguration Slots { get; }
        RangeSelect SwitchPosition { get; }
    }
}