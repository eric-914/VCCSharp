namespace VCCSharp.Configuration.Models.Implementation;

public class SerialPort : ISerialPort
{
    public bool TextMode { get; set; } = true;  //--Add LF to CR
    public bool PrintMonitorWindow { get; set; }

    public string? SerialCaptureFile { get; set; }
}
