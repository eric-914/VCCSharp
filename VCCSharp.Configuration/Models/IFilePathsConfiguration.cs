namespace VCCSharp.Configuration.Models
{
    public interface IFilePathsConfiguration
    {
        string? Cassette { get; set; }
        string Rom { get; }
        string? SerialCapture { get; set; }

        //TODO: Probably not configuration related
        string SetRom(string value);
    }
}
