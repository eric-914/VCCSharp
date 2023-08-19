using VCCSharp.Configuration.Support;

namespace VCCSharp.Configuration.Models
{
    public interface IMultiPak
    {
        string FilePath { get; set; }
        IMultiSlots Slots { get; }
        RangeSelect SwitchPosition { get; }
    }
}