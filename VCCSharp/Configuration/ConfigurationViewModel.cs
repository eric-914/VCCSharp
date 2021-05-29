using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VCCSharp.Annotations;
using VCCSharp.Enums;
using VCCSharp.Models;
using VCCSharp.Modules;

namespace VCCSharp.Configuration
{
    public class ConfigurationViewModel : INotifyPropertyChanged
    {
        //TODO: Remove STATIC once safe
        private static IConfig _state;
        private static ConfigModel _model;
        private static JoystickModel _left;
        private static JoystickModel _right;
        private static IConfig _config;

        public AudioSpectrum Spectrum { get; }

        public ConfigurationViewModel()
        {
            //TODO: Left/Right won't set properly.  So hack for the short term.
            Left = new JoystickViewModel(JoystickSides.Left, this);
            Right = new JoystickViewModel(JoystickSides.Right, this);

            Spectrum = new AudioSpectrum();
        }

        public ConfigModel Model
        {
            get => _model;
            set
            {
                if (_model != null) return;
                _model = value;
            }
        }

        public JoystickModel LeftModel
        {
            get => _left;
            set
            {
                if (_left != null) return;

                _left = value;
            }
        }

        public JoystickModel RightModel
        {
            get => _right;
            set
            {
                if (_right != null) return;

                _right = value;
            }
        }

        public IConfig Config
        {
            get => _config;
            set
            {
                if (_config != null) return;

                _config = value;
            }
        }

        public IConfig State
        {
            get => _state;
            set
            {
                if (_state != null) return;

                _state = value;
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
        public int CpuMultiplier
        {
            get => Model.CPUMultiplier;
            set
            {
                if (value == Model.CPUMultiplier) return;

                Model.CPUMultiplier = (byte)value;
                OnPropertyChanged();
            }
        }

        public int FrameSkip
        {
            get => Model.FrameSkip;
            set
            {
                if (Model.FrameSkip == (byte)value) return;

                Model.FrameSkip = (byte)value;
                OnPropertyChanged();
            }
        }

        public bool SpeedThrottle
        {
            get => Model.SpeedThrottle != 0;
            set
            {
                if (value == (Model.SpeedThrottle != 0)) return;

                Model.SpeedThrottle = (byte)(value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        public int CpuType
        {
            get => Model.CpuType;
            set => Model.CpuType = (byte)value;
        }

        public CPUTypes? Cpu
        {
            get => (CPUTypes)CpuType;
            set
            {
                if (value.HasValue)
                {
                    CpuType = (int)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxOverclock => Model.MaxOverclock;

        //[Audio]
        public List<string> SoundCards
        {
            get
            {
                unsafe
                {
                    var items = new List<string>();

                    for (int index = 0; index < Config.NumberOfSoundCards; index++)
                    {
                        var card = Config.SoundCards[index];

                        items.Add(Converter.ToString(card.CardName));
                    }

                    return items;
                }
            }
        }

        public string SoundCardName
        {
            get => Model.SoundCardName;
            set => Model.SoundCardName = value;
        }

        public int AudioRate
        {
            get => (int)(AudioRates)Model.AudioRate;
            set => Model.AudioRate = (ushort)value;
        }

        //[Video]
        public MonitorTypes? MonitorType
        {
            get => (MonitorTypes)(Model.MonitorType);
            set
            {
                if (!value.HasValue || Model.MonitorType == (byte)value.Value) return;

                Model.MonitorType = (byte)value.Value;
                OnPropertyChanged();
            }
        }

        public PaletteTypes? PaletteType
        {
            get => (PaletteTypes)(Model.PaletteType);
            set
            {
                if (!value.HasValue || Model.PaletteType == (byte)value.Value) return;

                Model.PaletteType = (byte)value.Value;
                OnPropertyChanged();
            }
        }

        public bool ScanLines
        {
            get => Model.ScanLines != 0;
            set
            {
                if (value == (Model.ScanLines != 0)) return;

                Model.ScanLines = (byte)(value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        public bool ForceAspect
        {
            get => Model.ForceAspect != 0;
            set
            {
                if (value == (Model.ForceAspect != 0)) return;

                Model.ForceAspect = (byte)(value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        public bool RememberSize
        {
            get => Model.RememberSize != 0;
            set
            {
                if (value == (Model.RememberSize != 0)) return;

                Model.RememberSize = (byte)(value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        public int WindowSizeX { get; set; } = 1111;
        public int WindowSizeY { get; set; } = 2222;

        public MemorySizes? Memory
        {
            get => (MemorySizes)RamSize;
            set
            {
                if (value.HasValue)
                {
                    RamSize = (int)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        //[Memory]
        public int RamSize
        {
            get => Model.RamSize;
            set => Model.RamSize = (byte)value;
        }

        public string ExternalBasicImage { get; set; } = "External Basic Image";

        //[Misc]
        public bool AutoStart
        {
            get => Model.AutoStart != 0;
            set => Model.AutoStart = (byte)(value ? 1 : 0);
        }

        public bool CartAutoStart
        {
            get => Model.CartAutoStart != 0;
            set => Model.CartAutoStart = (byte)(value ? 1 : 0);
        }

        public int KeyMapIndex
        {
            get => Model.KeyMapIndex;
            set => Model.KeyMapIndex = (byte)value;
        }

        //[Module]
        public string ModulePath { get; set; } = "Module Path";

        //[LeftJoyStick]
        public JoystickViewModel Left { get; }

        //[RightJoyStick]
        public JoystickViewModel Right { get; }

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
