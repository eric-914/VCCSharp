using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

internal class AccessoriesConfiguration : IAccessoriesConfiguration
{
    public string ModulePath { get; set; } = "";

    public IMultiPakConfiguration MultiPak { get; } = new MultiPakConfiguration();
    public IFloppyDiskConfiguration FloppyDisk { get; } = new FloppyDiskConfiguration();
    public IHardDiskConfiguration HardDisk { get; } = new HardDiskConfiguration();
    public ISuperIDEConfiguration SuperIDE { get; } = new SuperIDEConfiguration();
}
