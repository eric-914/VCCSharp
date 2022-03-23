using System.Collections.Generic;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Enums;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.ViewModel;

/// <summary>
/// Used as a model definition for WPF/Views to bind to.
/// Replaced with a valid one at run-time.
/// </summary>
public class NullConfigurationViewModel : IConfigurationViewModel
{
    public AudioRates AudioRate { get; set; }
    public AudioSpectrum Spectrum { get; set; } = new();
    public CPUTypes CpuType { get; set; }
    public CPUTypes? Cpu { get; set; }
    public IConfigurationRoot Model => default!;
    public JoystickViewModel Left { get; } = new();
    public JoystickViewModel Right { get; } = new();
    public KeyboardLayouts KeyboardLayout { get; set; }
    public List<string> KeyboardLayouts { get; } = new();
    public List<string> SoundDevices => new();
    public List<string> SoundRates { get; } = new();
    public MemorySizes RamSize { get; set; }
    public MemorySizes? Memory { get; set; }
    public MonitorTypes? MonitorType { get; set; }
    public PaletteTypes? PaletteType { get; set; }
    public bool AutoStart { get; set; }
    public bool CartAutoStart { get; set; }
    public bool ForceAspect { get; set; }
    public bool RememberSize { get; set; }
    public bool ScanLines { get; set; }
    public bool SpeedThrottle { get; set; }
    public int CpuMultiplier { get; set; }
    public int FrameSkip { get; set; }
    public int MaxOverclock => 0;
    public string CassettePath { get; set; } = string.Empty;
    public string CoCoRomPath { get; set; } = string.Empty;
    public string ExternalBasicImage { get; set; } = string.Empty;
    public string FloppyPath { get; set; } = string.Empty;
    public string ModulePath { get; set; } = string.Empty;
    public string PakPath { get; set; } = string.Empty;
    public string Release { get; set; } = string.Empty;
    public string SerialCaptureFilePath { get; set; } = string.Empty;
    public string SoundDevice { get; set; } = string.Empty;
}
