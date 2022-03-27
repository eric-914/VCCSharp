using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.ViewModel;

/// <summary>
/// Used as a model definition for WPF/Views to bind to.
/// Replaced with a valid one at run-time.
/// </summary>
public class NullConfigurationViewModel : IConfigurationViewModel
{
    public AudioTabViewModel Audio { get; } = new();
    public CpuTabViewModel Cpu { get; } = new();
    public DisplayTabViewModel Display { get; } = new();
    public KeyboardTabViewModel Keyboard { get; } = new();
    public MiscellaneousTabViewModel Miscellaneous { get; } = new();


    public IConfiguration Model => default!;
    public JoystickViewModel Left { get; } = new();
    public JoystickViewModel Right { get; } = new();
    public string CassettePath { get; set; } = string.Empty;
    public string CoCoRomPath { get; set; } = string.Empty;
    public string ExternalBasicImage { get; set; } = string.Empty;
    public string FloppyPath { get; set; } = string.Empty;
    public string ModulePath { get; set; } = string.Empty;
    public string PakPath { get; set; } = string.Empty;
    public string Release { get; set; } = string.Empty;
    public string SerialCaptureFilePath { get; set; } = string.Empty;
}
