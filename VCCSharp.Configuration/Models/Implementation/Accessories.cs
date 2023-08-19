using VCCSharp.Configuration.Models;

namespace VCCSharp.Models.Configuration;

public class Accessories : IAccessories
{
    public string ModulePath { get; set; } = "";

    public IMultiPak MultiPak { get; } = new MultiPak();
    public IFloppyDisk FloppyDisk { get; } = new FloppyDisk();
    public IHardDisk HardDisk { get; } = new HardDisk();
    public ISuperIDE SuperIDE { get; } = new SuperIDE();
}
