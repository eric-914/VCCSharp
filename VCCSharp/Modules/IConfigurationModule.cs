using System.Collections.Generic;
using VCCSharp.Enums;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Modules;

public interface IConfigurationModule
{
    IConfigurationRoot Model { get; }

    void Load(string filePath);
    void LoadFrom();
    void Save();
    void SaveAs();

    void SynchSystemWithConfig();

    void DecreaseOverclockSpeed();
    void IncreaseOverclockSpeed();

    string AppTitle { get; }
    bool TextMode { get; set; }
    bool PrintMonitorWindow { get; set; }
    int TapeCounter { get; set; }
    TapeModes TapeMode { get; set; }
    string? TapeFileName { get; set; }
    string? SerialCaptureFile { get; set; }
    string? FilePath { get; }

    List<string> SoundDevices { get; }
    List<string> JoystickDevices { get; }
}