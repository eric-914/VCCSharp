namespace VCCSharp.Configuration.Models
{
    public interface IMultiSlotsConfiguration
    {
        string this[int index] { get; set; }

        string _1 { get; set; }
        string _2 { get; set; }
        string _3 { get; set; }
        string _4 { get; set; }
    }
}