﻿using System.Collections.Generic;
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
        private static Models.Configuration.IConfiguration _model;
        private static Models.Configuration.Joystick _left;
        private static Models.Configuration.Joystick _right;
        private static IConfig _config;

        public AudioSpectrum Spectrum { get; }

        public ConfigurationViewModel()
        {
            //TODO: Left/Right won't set properly.  So hack for the short term.
            Left = new JoystickViewModel(JoystickSides.Left, this);
            Right = new JoystickViewModel(JoystickSides.Right, this);

            Spectrum = new AudioSpectrum();
        }

        public Models.Configuration.IConfiguration Model
        {
            get => _model;
            set
            {
                if (_model != null) return;
                _model = value;
            }
        }

        public Models.Configuration.Joystick LeftModel
        {
            get => _left;
            set
            {
                if (_left != null) return;

                _left = value;
            }
        }

        public Models.Configuration.Joystick RightModel
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

        public List<string> SoundRates { get; } = new List<string> { "Disabled", "11025 Hz", "22050 Hz", "44100 Hz" };

        #endregion

        //[Version]
        public string Release { get; set; } = "Release";

        //[CPU]
        public int CpuMultiplier
        {
            get => Model.CPU.CpuMultiplier;
            set
            {
                if (value == Model.CPU.CpuMultiplier) return;

                Model.CPU.CpuMultiplier = (byte)value;
                OnPropertyChanged();
            }
        }

        public int FrameSkip
        {
            get => Model.CPU.FrameSkip;
            set
            {
                if (Model.CPU.FrameSkip == (byte)value) return;

                Model.CPU.FrameSkip = (byte)value;
                OnPropertyChanged();
            }
        }

        public bool SpeedThrottle
        {
            get => Model.CPU.ThrottleSpeed;
            set
            {
                if (value == (Model.CPU.ThrottleSpeed)) return;

                Model.CPU.ThrottleSpeed = value;
                OnPropertyChanged();
            }
        }

        public CPUTypes CpuType
        {
            get => Model.CPU.Type.Value;
            set => Model.CPU.Type.Value = value;
        }

        public CPUTypes? Cpu
        {
            get => CpuType;
            set
            {
                if (value.HasValue)
                {
                    CpuType = value.Value;
                    OnPropertyChanged();
                }
            }
        }

        public int MaxOverclock => Model.CPU.MaxOverclock;

        //[Audio]
        public List<string> SoundDevices => Config.SoundDevices;

        public string SoundDevice
        {
            get => Model.Audio.Device;
            set => Model.Audio.Device = value;
        }

        public AudioRates AudioRate
        {
            get => Model.Audio.Rate.Value;
            set => Model.Audio.Rate.Value = value;
        }

        //[Video]
        public MonitorTypes? MonitorType
        {
            get => Model.Video.Monitor.Value;
            set
            {
                if (!value.HasValue || Model.Video.Monitor.Value == value.Value) return;

                Model.Video.Monitor.Value = value.Value;
                OnPropertyChanged();
            }
        }

        public PaletteTypes? PaletteType
        {
            get => Model.Video.Palette.Value;
            set
            {
                if (!value.HasValue || Model.Video.Palette.Value == value.Value) return;

                Model.Video.Palette.Value = value.Value;
                OnPropertyChanged();
            }
        }

        public bool ScanLines
        {
            get => Model.Video.ScanLines;
            set
            {
                if (value == Model.Video.ScanLines) return;

                Model.Video.ScanLines = value;
                OnPropertyChanged();
            }
        }

        public bool ForceAspect
        {
            get => Model.Video.ForceAspect;
            set
            {
                if (value == Model.Video.ForceAspect) return;

                Model.Video.ForceAspect = value;
                OnPropertyChanged();
            }
        }

        public bool RememberSize
        {
            get => Model.Window.RememberSize;
            set
            {
                if (value == Model.Window.RememberSize) return;

                Model.Window.RememberSize = value;
                OnPropertyChanged();
            }
        }

        public MemorySizes? Memory
        {
            get => RamSize;
            set
            {
                if (value.HasValue)
                {
                    RamSize = value.Value;
                    OnPropertyChanged();
                }
            }
        }

        //[Memory]
        public MemorySizes RamSize
        {
            get => Model.Memory.Ram.Value;
            set => Model.Memory.Ram.Value = value;
        }

        public string ExternalBasicImage { get; set; } = "External Basic Image";

        //[Misc]
        public bool AutoStart
        {
            get => Model.Startup.AutoStart;
            set => Model.Startup.AutoStart = value;
        }

        public bool CartAutoStart
        {
            get => Model.Startup.CartridgeAutoStart;
            set => Model.Startup.CartridgeAutoStart = value;
        }

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
}
