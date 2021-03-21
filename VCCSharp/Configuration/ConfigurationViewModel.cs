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

        public ConfigurationViewModel()
        {
            //TODO: Left/Right won't set properly.  So hack for the short term.
            Left = new JoystickViewModel(JoystickSides.Left, this);
            Right = new JoystickViewModel(JoystickSides.Right, this);
        }

        public unsafe ConfigModel* Model => _state->Model;

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
        public bool CPUMultiplier { get; set; } = true;
        public bool FrameSkip { get; set; } = false;
        public bool SpeedThrottle { get; set; } = true;

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

        public int MaxOverclock { get; set; } = 55;

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
        public int MonitorType { get; set; } = 0;
        public int PaletteType { get; set; } = 0;
        public bool ScanLines { get; set; } = true;
        public bool ForceAspect { get; set; } = false;
        public bool RememberSize { get; set; } = true;

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
        public bool AutoStart { get; set; } = true;
        public bool CartAutoStart { get; set; } = false;

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
