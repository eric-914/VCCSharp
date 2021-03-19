using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.Models;

namespace VCCSharp.Configuration
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        private ConfigModel _model;

        public ConfigModel Model
        {
            get => _model;
            set
            {
                _model = value;
                unsafe
                {
                    Left.Model = *_model.Left;
                    Right.Model = *_model.Right;
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Constants

        public List<string> KeyboardLayouts { get; } = new List<string>
        {
            "CoCo (DECB)",
            "Natural (OS-9)",
            "Compact (OS-9)",
            "Custom"
        };

        public List<string> SoundRates { get; } = new List<string> { "Mute", "11025 Hz", "22050 Hz", "44100 Hz" };

        #endregion

        //[Version]
        public string Release { get; set; } = "Release";

        //[CPU]
        public bool CPUMultiplier { get; set; } = true;
        public bool FrameSkip { get; set; } = false;
        public bool SpeedThrottle { get; set; } = true;
        public bool CpuType { get; set; } = false;

        public int MaxOverclock { get; set; } = 55;

        //[Audio]
        public string SoundCardName { get; set; } = "Sound Card Name";
        public int AudioRate { get; set; } = 3;

        //[Video]
        public bool MonitorType { get; set; } = true;
        public bool PaletteType { get; set; } = false;
        public bool ScanLines { get; set; } = true;
        public bool ForceAspect { get; set; } = false;
        public bool RememberSize { get; set; } = true;

        public int WindowSizeX { get; set; } = 1111;
        public int WindowSizeY { get; set; } = 2222;

        //[Memory]
        public int RamSize { get; set; } = 9999;
        public string ExternalBasicImage { get; set; } = "External Basic Image";

        //[Misc]
        public bool AutoStart { get; set; } = true;
        public bool CartAutoStart { get; set; } = false;
        public bool KeyMapIndex { get; set; } = true;

        //[Module]
        public string ModulePath { get; set; } = "Module Path";

        //[LeftJoyStick]
        public JoystickViewModel Left { get; } = new JoystickViewModel { Side = JoystickSides.Left };

        //[RightJoyStick]
        public JoystickViewModel Right { get; } = new JoystickViewModel { Side = JoystickSides.Right };

        //[DefaultPaths]
        public string CassPath { get; set; } = "Cassette Path";
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
        //PesistPaks=1
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
}
