using VCCSharp.Models.Configuration.Support;

namespace VCCSharp.Models.Configuration;

//[FD-502] ### MODULE SPECIFIC ###
public class FloppyDisk
{
    public string FilePath { get; set; } = "";

    public MultiSlots Slots { get; } = new();

    //DiskRom=1
    //RomPath=
    //Persist=1
    //ClkEnable=1
    //TurboDisk=1
}
