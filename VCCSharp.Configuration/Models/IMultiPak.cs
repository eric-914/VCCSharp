using VCCSharp.Configuration.Support;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Configuration.Models
{
    public interface IMultiPak
    {
        string FilePath { get; set; }
        MultiSlots Slots { get; }
        RangeSelect SwitchPosition { get; }
    }
}