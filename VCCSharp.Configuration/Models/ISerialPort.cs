namespace VCCSharp.Configuration.Models
{
    public interface ISerialPort
    {
        bool PrintMonitorWindow { get; set; }
        string? SerialCaptureFile { get; set; }
        bool TextMode { get; set; }
    }
}