using System.Collections.Generic;
using VCCSharp.Configuration.TabControls.Audio;
using VCCSharp.Configuration.TabControls.Cpu;
using VCCSharp.Configuration.TabControls.Display;
using VCCSharp.Configuration.TabControls.Joystick;
using VCCSharp.Configuration.TabControls.Miscellaneous;
using VCCSharp.Enums;
using VCCSharp.Main.ViewModels;
using VCCSharp.Models.Configuration;

namespace VCCSharp.Configuration.ViewModel;

public class ConfigurationViewModel : NotifyViewModel, IConfigurationViewModel
{
    public AudioTabViewModel Audio { get; }
    public DisplayTabViewModel Display { get; }
    public CpuTabViewModel Cpu { get; }
    public MiscellaneousTabViewModel Miscellaneous { get; }

    public ConfigurationViewModel(IConfiguration model, AudioTabViewModel audio, CpuTabViewModel cpu, DisplayTabViewModel display, JoystickViewModel left, JoystickViewModel right, MiscellaneousTabViewModel miscellaneous)
    {
        Model = model;
        Audio = audio;
        Cpu = cpu;
        Display = display;

        Left = left;
        Right = right;

        Miscellaneous = miscellaneous;
    }

    public IConfiguration Model { get; }

    #region Constants

    public List<string> KeyboardLayouts { get; } = new()
    {
        "Color Computer 1/2",
        "Color Computer 3",
        "PC"
    };

    #endregion

    //[Version]
    public string Release { get; set; } = "Release";

    //[CPU]



    //[Video]



    //[Memory]

    public string ExternalBasicImage { get; set; } = "External Basic Image";

    public KeyboardLayouts KeyboardLayout
    {
        get => Model.Keyboard.Layout.Value;
        set => Model.Keyboard.Layout.Value = value;
    }

    //[Module]
    public string ModulePath { get; set; } = "Module Path";

    //[LeftJoyStick]
    public JoystickViewModel Left { get; }

    //[RightJoyStick]
    public JoystickViewModel Right { get; }

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