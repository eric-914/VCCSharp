namespace VCCSharp.Configuration.Models
{
    public interface IFloppyDisk
    {
        string FilePath { get; set; }
        IMultiSlots Slots { get; }
    }
}