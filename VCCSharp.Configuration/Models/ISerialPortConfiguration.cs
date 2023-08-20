namespace VCCSharp.Configuration.Models
{
    public interface ISerialPortConfiguration
    {
        bool PrintMonitorWindow { get; set; }
        string? SerialCaptureFile { get; set; }
        bool TextMode { get; set; }
    }
}