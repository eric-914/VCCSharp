using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;

namespace VCCSharp.Configuration.ViewModel;

public interface IConfigurationViewModel
{
    Models.Configuration.IConfiguration Model { get; }

    AudioTabViewModel Audio { get; }
    CpuTabViewModel Cpu { get; }
    DisplayTabViewModel Display { get; }
    KeyboardTabViewModel Keyboard { get; }
    MiscellaneousTabViewModel Miscellaneous { get; }

    string Release { get; set; }
    string ExternalBasicImage { get; set; }
    string ModulePath { get; set; }
    JoystickViewModel Left { get; }
    JoystickViewModel Right { get; }
    string CassettePath { get; set; }
    string PakPath { get; set; }
    string FloppyPath { get; set; }
    string CoCoRomPath { get; set; }
    string SerialCaptureFilePath { get; set; }
}
