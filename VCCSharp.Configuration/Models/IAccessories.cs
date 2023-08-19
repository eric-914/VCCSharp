using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.Models
{
    public interface IAccessories
    {
        IFloppyDisk FloppyDisk { get; }
        IHardDisk HardDisk { get; }
        string ModulePath { get; set; }
        IMultiPak MultiPak { get; }
        ISuperIDE SuperIDE { get; }
    }
}