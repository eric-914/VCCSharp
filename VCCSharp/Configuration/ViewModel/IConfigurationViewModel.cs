using System.Collections.Generic;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Enums;

namespace VCCSharp.Configuration.ViewModel;

public interface IConfigurationViewModel
{
    Models.Configuration.IConfiguration Model { get; }
    List<string> KeyboardLayouts { get; }
    string Release { get; set; }
    int CpuMultiplier { get; set; }
    int FrameSkip { get; set; }
    bool SpeedThrottle { get; set; }
    CPUTypes CpuType { get; set; }
    CPUTypes? Cpu { get; set; }
    int MaxOverclock { get; }
    MonitorTypes? MonitorType { get; set; }
    PaletteTypes? PaletteType { get; set; }
    bool ScanLines { get; set; }
    bool ForceAspect { get; set; }
    bool RememberSize { get; set; }
    MemorySizes? Memory { get; set; }
    MemorySizes RamSize { get; set; }
    string ExternalBasicImage { get; set; }
    bool AutoStart { get; set; }
    bool CartAutoStart { get; set; }
    KeyboardLayouts KeyboardLayout { get; set; }
    string ModulePath { get; set; }
    JoystickViewModel Left { get; }
    JoystickViewModel Right { get; }
    string CassettePath { get; set; }
    string PakPath { get; set; }
    string FloppyPath { get; set; }
    string CoCoRomPath { get; set; }
    string SerialCaptureFilePath { get; set; }
}
