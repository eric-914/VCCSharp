﻿using System.Collections.Generic;
using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Enums;

namespace VCCSharp.Configuration.ViewModel;

public interface IConfigurationViewModel
{
    Models.Configuration.IConfiguration Model { get; }

    AudioTabViewModel Audio { get; }
    CpuTabViewModel Cpu { get; }
    MiscellaneousTabViewModel Miscellaneous { get; }

    List<string> KeyboardLayouts { get; }
    string Release { get; set; }
    int FrameSkip { get; set; }
    bool SpeedThrottle { get; set; }
    MonitorTypes? MonitorType { get; set; }
    PaletteTypes? PaletteType { get; set; }
    bool ScanLines { get; set; }
    bool ForceAspect { get; set; }
    bool RememberSize { get; set; }
    string ExternalBasicImage { get; set; }
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
