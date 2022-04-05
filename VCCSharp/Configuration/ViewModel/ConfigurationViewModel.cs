using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Keyboard;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Shared.ViewModels;

namespace VCCSharp.Configuration.ViewModel;

public class ConfigurationViewModel : NotifyViewModel
{
    public AudioTabViewModel Audio { get; } = new();
    public DisplayTabViewModel Display { get; }= new();
    public CpuTabViewModel Cpu { get; }= new();
    public KeyboardTabViewModel Keyboard { get; }= new();
    public JoystickPairViewModel Joystick { get; }= new();
    public MiscellaneousTabViewModel Miscellaneous { get; }= new();

    public ConfigurationViewModel() { }

    public ConfigurationViewModel(AudioTabViewModel audio, CpuTabViewModel cpu, DisplayTabViewModel display, KeyboardTabViewModel keyboard, JoystickPairViewModel joystick, MiscellaneousTabViewModel miscellaneous)
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