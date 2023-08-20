using VCCSharp.Configuration.Models;
using VCCSharp.Configuration.Support;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

/// <summary>
/// The TRS-80(R) Color Computer Multi-Pak Interface is a welcome addition
/// to the Color Computer family.  With it, you can connect up to four Color
/// Computer Program Paks(tm) or Interface Controllers to your Computer at
/// once.  Then, when you're ready to change from one Program Pak to another,
/// simply select another "slot."
/// </summary>
/// <see href="https://colorcomputerarchive.com/repo/Documents/Manuals/Hardware/Multi-Pak%20Interface%20Owners%20Manual%20%28Tandy%29.pdf"/>
public class MultiPakConfiguration : IMultiPakConfiguration
{
    public RangeSelect SwitchPosition { get; } = new(1, 2, 3, 4);

    public string FilePath { get; set; } = ""; //C:\CoCo

    public IMultiSlotsConfiguration Slots { get; } = new MultiSlotsConfiguration();
}
