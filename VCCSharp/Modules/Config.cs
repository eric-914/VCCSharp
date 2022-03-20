﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using VCCSharp.Properties;
using static System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IConfig
    {
        IConfiguration Model { get; }

        void Load(string filePath);
        void LoadFrom();
        void Save();
        void SaveAs();

        void SynchSystemWithConfig();

        void DecreaseOverclockSpeed();
        void IncreaseOverclockSpeed();

        string AppTitle { get; }
        bool TextMode { get; set; }
        bool PrintMonitorWindow { get; set; }
        int TapeCounter { get; set; }
        TapeModes TapeMode { get; set; }
        string? TapeFileName { get; set; }
        string? SerialCaptureFile { get; set; }
        string? FilePath { get; }

        List<string> SoundDevices { get; }
        List<string> JoystickDevices { get; }
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IConfigPersistence _persistence;

        public string AppTitle { get; } = Resources.ResourceManager.GetString("AppTitle") ?? "<Unable to read AppTitle>";

        public bool TextMode { get; set; } = true;  //--Add LF to CR
        public bool PrintMonitorWindow { get; set; }

        public int TapeCounter { get; set; }
        public TapeModes TapeMode { get; set; } = TapeModes.Stop;

        public string? TapeFileName { get; set; }
        public string? SerialCaptureFile { get; set; }

        private readonly JoystickModel _left = new();
        private readonly JoystickModel _right = new();

        public List<string> SoundDevices => _modules.Audio.FindSoundDevices();
        public List<string> JoystickDevices => _modules.Joystick.FindJoysticks();

        public IConfiguration Model { get; private set; } = default!;

        public string? FilePath { get; set; }

        public Config(IModules modules, IUser32 user32, IConfigPersistence persistence)
        {
            _modules = modules;
            _user32 = user32;
            _persistence = persistence;
        }

        public void Load(string filePath)
        {
            FilePath = GetConfigurationFilePath(filePath);  //--Use default if needed

            Load();

            Model.Version.Release = AppTitle; //--A kind of "version" I guess

            //--Synch joysticks to config instance
            _modules.Joystick.SetLeftJoystick(_left);
            _modules.Joystick.SetRightJoystick(_right);

            SynchSystemWithConfig();

            ConfigureJoysticks();

            string device = Model.Audio.Device;
            int deviceIndex = SoundDevices.IndexOf(device);

            _modules.Audio.SoundInit(_modules.Emu.WindowHandle, deviceIndex, Model.Audio.Rate.Value);

            if (_persistence.IsNew(FilePath))
            {
                Save();
            }
        }

        public void SynchSystemWithConfig()
        {
            _modules.Vcc.AutoStart = Model.Startup.AutoStart;
            _modules.Vcc.Throttle = Model.CPU.ThrottleSpeed;

            _modules.Emu.RamSize = Model.Memory.Ram.Value;
            _modules.Emu.FrameSkip = Model.CPU.FrameSkip;

            _modules.Graphics.SetPaletteType();
            _modules.Draw.SetAspect(Model.Video.ForceAspect);
            _modules.Graphics.SetScanLines(Model.Video.ScanLines);
            _modules.Emu.SetCpuMultiplier(Model.CPU.CpuMultiplier);

            SetCpuType(Model.CPU.Type.Value);

            _modules.Graphics.SetMonitorType(Model.Video.Monitor.Value);
            _modules.MC6821.SetCartAutoStart(Model.Startup.CartridgeAutoStart);
        }

        // LoadFrom allows user to browse for an ini file and reloads the config from it.
        public void LoadFrom() => _persistence.LoadFrom(FilePath, LoadFrom);

        private void LoadFrom(string filePath)
        {
            Save();

            FilePath = filePath;

            Load();

            SynchSystemWithConfig();

            _modules.Emu.ResetPending = ResetPendingStates.Hard;
        }

        public void Load()
        {
            Model = _persistence.Load(FilePath);

            _modules.Keyboard.KeyboardBuildRuntimeTable(Model.Keyboard.Layout.Value);

            if (!string.IsNullOrEmpty(_modules.Emu.PakPath))
            {
                Model.Accessories.MultiPak.FilePath = _modules.Emu.PakPath;
            }

            _modules.PAKInterface.InsertModule(_modules.Emu.EmulationRunning, Model.Accessories.ModulePath);   // Should this be here?

            if (Model.Window.RememberSize)
            {
                SetWindowSize((short)Model.Window.Width, (short)Model.Window.Height);
            }
            else
            {
                SetWindowSize(Define.DEFAULT_WIDTH, Define.DEFAULT_HEIGHT);
            }
        }

        public void SaveAs()
        {
            _persistence.SaveAs(FilePath, SaveAs);
        }

        private void SaveAs(string filePath)
        {
            FilePath = filePath;
            Save();
        }

        public void Save()
        {
            Model.Window.Width = (short)_modules.Emu.WindowSize.X;
            Model.Window.Height = (short)_modules.Emu.WindowSize.Y;

            string? modulePath = Model.Accessories.ModulePath;

            if (string.IsNullOrEmpty(modulePath))
            {
                modulePath = _modules.PAKInterface.GetCurrentModule();
            }

            if (!string.IsNullOrEmpty(modulePath))
            {
                Model.Accessories.ModulePath = modulePath;
            }

            _persistence.Save(FilePath, Model);
        }

        public void ConfigureJoysticks()
        {
            var joystick = _modules.Joystick;

            var joysticks = joystick.FindJoysticks();

            if (_right.DeviceIndex >= joysticks.Count)
            {
                _right.DeviceIndex = 0;
            }

            if (_left.DeviceIndex >= joysticks.Count)
            {
                _left.DeviceIndex = 0;
            }

            joystick.SetStickNumbers(_left.DeviceIndex, _right.DeviceIndex);

            if (joysticks.Count == 0)	//Use Mouse input if no Joysticks present
            {
                if (Model.Joysticks.Left.InputSource.Value == Enums.JoystickDevices.Joystick)
                {
                    _left.InputSource = Enums.JoystickDevices.Mouse;
                }

                if (Model.Joysticks.Right.InputSource.Value == Enums.JoystickDevices.Joystick)
                {
                    _right.InputSource = Enums.JoystickDevices.Mouse;
                }
            }
        }

        private static string GetConfigurationFilePath(string argIniFile)
        {
            if (!string.IsNullOrEmpty(argIniFile))
            {
                return argIniFile;
            }

            const string iniFileName = "coco.json";

            string appDataPath = GetApplicationFolder();

            return Path.Combine(appDataPath, iniFileName);
        }

        private static string GetApplicationFolder()
        {
            const string vccFolder = "VCCSharp";

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            //==> C:\Users\erich\AppData\Roaming\VCC
            appDataPath = Path.Combine(appDataPath, vccFolder);

            if (!Directory.Exists(appDataPath))
            {
                try
                {
                    Directory.CreateDirectory(appDataPath);
                }
                catch (Exception)
                {
                    Debug.WriteLine($"Unable to create application data folder: {appDataPath}");
                    //TODO: And still use appDataPath?
                }
            }

            return appDataPath;
        }

        public void SetCpuType(CPUTypes cpuType)
        {
            var cpu = new Dictionary<CPUTypes, string>
            {
                {CPUTypes.MC6809, "MC6809"},
                {CPUTypes.HD6309, "HD6309"}
            };

            _modules.Emu.CpuType = cpuType;
            _modules.Vcc.CpuName = cpu[cpuType];
        }

        /**
         * Decrease the overclock speed, as seen after a POKE 65497,0.
         * Setting this value to 0 will make the emulator pause.  Hence the minimum of 2.
         */
        public void DecreaseOverclockSpeed()
        {
            AdjustOverclockSpeed(0xFF); //--Stupid compiler can't figure out (byte)(-1) = 0xFF
        }

        /**
         * Increase the overclock speed, as seen after a POKE 65497,0.
         * Valid values are [2,100].
         */
        public void IncreaseOverclockSpeed()
        {
            AdjustOverclockSpeed(1);
        }

        private void AdjustOverclockSpeed(byte change)
        {
            byte cpuMultiplier = (byte)(Model.CPU.CpuMultiplier + change);

            if (cpuMultiplier < 2 || cpuMultiplier > Model.CPU.MaxOverclock)
            {
                return;
            }

            // Send updates to the dialog if it's open.
            //TODO: Apply this to new configuration dialog
            //if (emuState->ConfigDialog != Zero)
            //{
            //    HWND hDlg = configState->hWndConfig[1];

            //    _modules.Callbacks.SetDialogCpuMultiplier(hDlg, cpuMultiplier);
            //}

            Model.CPU.CpuMultiplier = cpuMultiplier;

            _modules.Emu.ResetPending = ResetPendingStates.ClsSynch; // Without this, changing the config does nothing.
        }

        public void SetWindowSize(short width, short height)
        {
            HWND handle = _user32.GetActiveWindow();

            SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

            _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
        }

    }
}
