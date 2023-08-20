using VCCSharp.Configuration.Models;
using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

//[FD-502] ### MODULE SPECIFIC ###
internal class FloppyDiskConfiguration : IFloppyDiskConfiguration
{
    public string FilePath { get; set; } = "";

    public IMultiSlotsConfiguration Slots { get; } = new MultiSlotsConfiguration();

    //DiskRom=1
    //RomPath=
    //Persist=1
    //ClkEnable=1
    //TurboDisk=1
}
