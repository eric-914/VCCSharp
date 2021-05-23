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
        //TODO: Remove STATIC once safe
        private static unsafe ConfigState* _state;
        private static unsafe ConfigModel* _model;
        private static unsafe JoystickModel* _left;
        private static unsafe JoystickModel* _right;

        public AudioSpectrum Spectrum { get; }

        public ConfigurationViewModel()
        {
            //TODO: Left/Right won't set properly.  So hack for the short term.
            Left = new JoystickViewModel(JoystickSides.Left, this);
            Right = new JoystickViewModel(JoystickSides.Right, this);

            Spectrum = new AudioSpectrum();
        }

        public unsafe ConfigModel* Model
        {
            get => _model;
            set
            {
                if (_model != null) return;
                _model = value;
            }
        }

        public unsafe JoystickModel* LeftModel
        {
            get => _left;
            set
            {
                if (_left != null) return;

                _left = value;
            }
        }

        public unsafe JoystickModel* RightModel
        {
            get => _right;
            set
            {
                if (_right != null) return;

                _right = value;
            }
        }

        public unsafe ConfigState* State
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
        public int CPUMultiplier
        {
            get
            {
                unsafe
                {
                    return Model->CPUMultiplier;
                }
            }
            set
            {
                unsafe
                {
                    if (value == Model->CPUMultiplier) return;

                    Model->CPUMultiplier = (byte)value;
                    OnPropertyChanged();
                }
            }
        }

        public int FrameSkip
        {
            get
            {
                unsafe
                {
                    return (Model->FrameSkip);
                }
            }
            set
            {
                unsafe
                {
                    if (Model->FrameSkip == (byte)value) return;

                    Model->FrameSkip = (byte)value;
                    OnPropertyChanged();
                }
            }
        }

        public bool SpeedThrottle
        {
            get
            {
                unsafe
                {
                    return Model->SpeedThrottle != 0;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (Model->SpeedThrottle != 0)) return;

                    Model->SpeedThrottle = (byte)(value ? 1 : 0);
                    OnPropertyChanged();
                }
            }
        }

        public int CpuType
        {
            get
            {
                unsafe
                {
                    return Model->CpuType;
                }
            }
            set
            {
                unsafe
                {
                    Model->CpuType = (byte)value;
                }
            }
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

        public int MaxOverclock
        {
            get
            {
                unsafe
                {
                    return Model->MaxOverclock;
                }
            }
        }

        //[Audio]
        public List<string> SoundCards
        {
            get
            {
                unsafe
                {
                    var items = new List<string>();

                    SoundCardList Lookup(int index)
                    {
                        switch (index)
                        {
                            case 0: return _state->SoundCards._0;
                            case 1: return _state->SoundCards._1;
                            case 2: return _state->SoundCards._2;
                                //TODO: Fill in the rest.  Or just figure out how to turn it into an array like it should be.
                        }

                        return default;
                    }

                    for (int index = 0; index < _state->NumberOfSoundCards; index++)
                    {
                        var card = Lookup(index);

                        items.Add(Converter.ToString(card.CardName));
                    }

                    return items;
                }
            }
        }

        public string SoundCardName
        {
            get
            {
                unsafe
                {
                    return Converter.ToString(Model->SoundCardName);
                }
            }
            set
            {
                unsafe
                {
                    Converter.ToByteArray(value, Model->SoundCardName);
                }
            }
        }

        public int AudioRate
        {
            get
            {
                unsafe
                {
                    return (int)(AudioRates)Model->AudioRate;
                }
            }
            set
            {
                unsafe
                {
                    Model->AudioRate = (ushort)value;
                }
            }
        }

        //[Video]
        public MonitorTypes? MonitorType
        {
            get
            {
                unsafe
                {
                    return (MonitorTypes)(Model->MonitorType);
                }
            }
            set
            {
                unsafe
                {
                    if (!value.HasValue || Model->MonitorType == (byte)value.Value) return;

                    Model->MonitorType = (byte)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public PaletteTypes? PaletteType
        {
            get
            {
                unsafe
                {
                    return (PaletteTypes)(Model->PaletteType);
                }
            }
            set
            {
                unsafe
                {
                    if (!value.HasValue || Model->PaletteType == (byte)value.Value) return;

                    Model->PaletteType = (byte)value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ScanLines
        {
            get
            {
                unsafe
                {
                    return Model->ScanLines != 0;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (Model->ScanLines != 0)) return;

                    Model->ScanLines = (byte)(value ? 1 : 0);
                    OnPropertyChanged();
                }
            }
        }

        public bool ForceAspect
        {
            get
            {
                unsafe
                {
                    return Model->ForceAspect != 0;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (Model->ForceAspect != 0)) return;

                    Model->ForceAspect = (byte)(value ? 1 : 0);
                    OnPropertyChanged();
                }
            }
        }

        public bool RememberSize
        {
            get
            {
                unsafe
                {
                    return Model->RememberSize != 0;
                }
            }
            set
            {
                unsafe
                {
                    if (value == (Model->RememberSize != 0)) return;

                    Model->RememberSize = (byte)(value ? 1 : 0);
                    OnPropertyChanged();
                }
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
            get
            {
                unsafe
                {
                    return Model->RamSize;
                }
            }
            set
            {
                unsafe
                {
                    Model->RamSize = (byte)value;
                }
            }
        }

        public string ExternalBasicImage { get; set; } = "External Basic Image";

        //[Misc]
        public bool AutoStart
        {
            get
            {
                unsafe
                {
                    return Model->AutoStart != 0;
                }
            }
            set
            {
                unsafe
                {
                    Model->AutoStart = (byte)(value ? 1 : 0);
                }
            }
        }

        public bool CartAutoStart
        {
            get
            {
                unsafe
                {
                    return Model->CartAutoStart != 0;
                }
            }
            set
            {
                unsafe
                {
                    Model->CartAutoStart = (byte)(value ? 1 : 0);
                }
            }
        }

        public int KeyMapIndex
        {
            get
            {
                unsafe
                {
                    return Model->KeyMapIndex;
                }
            }
            set
            {
                unsafe
                {
                    Model->KeyMapIndex = (byte)value;
                }
            }
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
