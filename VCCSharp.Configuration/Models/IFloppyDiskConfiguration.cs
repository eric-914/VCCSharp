namespace VCCSharp.Configuration.Models
{
    public interface IFloppyDiskConfiguration
    {
        string FilePath { get; set; }
        IMultiSlotsConfiguration Slots { get; }
    }
}