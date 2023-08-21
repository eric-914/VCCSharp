using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.ViewModel;

public interface IConfigurationViewModel
{
    IAudioTabViewModel Audio { get; }
    ICpuTabViewModel Cpu { get; }
    IDisplayTabViewModel Display { get; }
    IJoystickPairViewModel Joystick { get; }
    IKeyboardTabViewModel Keyboard { get; }
    IMiscellaneousTabViewModel Miscellaneous { get; }
    string CassettePath { get; set; }
    string CoCoRomPath { get; set; }
    string ExternalBasicImage { get; set; }
    string FloppyPath { get; set; }
    string ModulePath { get; set; }
    string PakPath { get; set; }
    string Release { get; set; }
    string SerialCaptureFilePath { get; set; }
}

public abstract class ConfigurationViewModelBase : NotifyViewModel, IConfigurationViewModel
{
    public IAudioTabViewModel Audio { get; } = new AudioTabViewModelStub();
    public IDisplayTabViewModel Display { get; } = new DisplayTabViewModelStub();
    public ICpuTabViewModel Cpu { get; } = new CpuTabViewModelStub();
    public IKeyboardTabViewModel Keyboard { get; } = new KeyboardTabViewModelStub();
    public IJoystickPairViewModel Joystick { get; } = new JoystickPairViewModelStub();
    public IMiscellaneousTabViewModel Miscellaneous { get; } = new MiscellaneousTabViewModelStub();

    protected ConfigurationViewModelBase(IAudioTabViewModel audio, ICpuTabViewModel cpu, IDisplayTabViewModel display, IKeyboardTabViewModel keyboard, IJoystickPairViewModel joystick, IMiscellaneousTabViewModel miscellaneous)
    {
        Audio = audio;
        Cpu = cpu;
        Display = display;
        Keyboard = keyboard;
        Joystick = joystick;
        Miscellaneous = miscellaneous;
    }

    #region Constants


    #endregion

    //[Version]
    public string Release { get; set; } = "Release";

    public string ExternalBasicImage { get; set; } = "External Basic Image";

    //[Module]
    public string ModulePath { get; set; } = "Module Path";

    //[DefaultPaths]
    public string CassettePath { get; set; } = "Cassette Path";
    public string PakPath { get; set; } = "Pak Path";
    public string FloppyPath { get; set; } = "Floppy Path";
    public string CoCoRomPath { get; set; } = "CoCo ROM Path";
    public string SerialCaptureFilePath { get; set; } = "Serial Capture File Path";

    #region MODULE SPECIFIC

    //[FD-502]  //### MODULE SPECIFIC ###//
    //DiskRom=1
    //RomPath=
    //Persist=1
    //Disk#0=
    //Disk#1=
    //Disk#2=
    //Disk#3=
    //ClkEnable=1
    //TurboDisk=1

    //[MPI]     //### MODULE SPECIFIC ###//
    //SWPOSITION=3
    //PersistPaks=1
    //SLOT1=
    //SLOT2=
    //SLOT3=
    //SLOT4=C:\CoCo\Mega-Bug (1982) (26-3076) (Tandy).ccc
    //"MPIPath"   //TODO: Originally in [DefaultPaths]

    //[HardDisk]  //### MODULE SPECIFIC ###// 
    //"HardDiskPath"  //TODO: Originally in [DefaultPaths]

    //[SuperIDE]  //### MODULE SPECIFIC ###//
    //"SuperIDEPath"  //TODO: Originally in [DefaultPaths]

    #endregion
}

public class ConfigurationViewModelStub : ConfigurationViewModelBase
{
    public ConfigurationViewModelStub() : base(new AudioTabViewModelStub(), new CpuTabViewModelStub(), new DisplayTabViewModelStub(), new KeyboardTabViewModelStub(), new JoystickPairViewModelStub(), new MiscellaneousTabViewModelStub()) { }
}

public class ConfigurationViewModel : ConfigurationViewModelBase
{
    public ConfigurationViewModel(AudioTabViewModel audio, CpuTabViewModel cpu, DisplayTabViewModel display, KeyboardTabViewModel keyboard, JoystickPairViewModel joystick, MiscellaneousTabViewModel miscellaneous)
        : base(audio, cpu, display, keyboard, joystick, miscellaneous) { }
}
