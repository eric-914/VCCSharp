namespace VCCSharp.Configuration.Models.Implementation;

internal class SerialPortConfiguration : ISerialPortConfiguration
{
    public bool TextMode { get; set; } = true;  //--Add LF to CR
    public bool PrintMonitorWindow { get; set; }

    public string? SerialCaptureFile { get; set; }
}
