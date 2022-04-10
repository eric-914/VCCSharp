namespace VCCSharp.Models.Configuration;

public class SerialPort
{
    public bool TextMode { get; set; } = true;  //--Add LF to CR
    public bool PrintMonitorWindow { get; set; }

    public string? SerialCaptureFile { get; set; }
}
