namespace VCCSharp.Configuration.Models
{
    public interface IAccessoriesConfiguration
    {
        IFloppyDiskConfiguration FloppyDisk { get; }
        IHardDiskConfiguration HardDisk { get; }
        string ModulePath { get; set; }
        IMultiPakConfiguration MultiPak { get; }
        ISuperIDEConfiguration SuperIDE { get; }
    }
}