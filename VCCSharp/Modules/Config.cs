using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Properties;
using static System.IntPtr;
using HWND = System.IntPtr;
using Point = System.Drawing.Point;

namespace VCCSharp.Modules
{
    public interface IConfig
    {
        ConfigModel Model { get; }
        JoystickModel GetLeftJoystick();
        JoystickModel GetRightJoystick();

        void InitConfig(string iniFile);
        void Save();
        void SynchSystemWithConfig();
        int GetPaletteType();
        void DecreaseOverclockSpeed();
        void IncreaseOverclockSpeed();
        void LoadIniFile();
        KeyboardLayouts GetCurrentKeyboardLayout();
        void SaveAs();
        bool GetRememberSize();

        Point GetWindowSize();
        string AppTitle { get; }
        byte TextMode { get; set; }
        bool PrintMonitorWindow { get; set; }
        ushort TapeCounter { get; set; }
        byte TapeMode { get; set; }
        string TapeFileName { get; set; }
        string SerialCaptureFile { get; set; }
        string IniFilePath { get; set; }

        List<string> SoundDevices { get; set; }
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IConfigPersistence _persistence;
        private readonly IPersistence _io;

        public string AppTitle { get; } = Resources.ResourceManager.GetString("AppTitle");

        public byte TextMode { get; set; } = 1;  //--Add LF to CR
        public bool PrintMonitorWindow { get; set; }

        public ushort TapeCounter { get; set; }
        public byte TapeMode { get; set; } = Define.STOP;

        public string TapeFileName { get; set; }
        public string SerialCaptureFile { get; set; }

        private readonly JoystickModel _left = new JoystickModel();
        private readonly JoystickModel _right = new JoystickModel();

        public List<string> SoundDevices { get; set; } = new List<string>();

        public ConfigModel Model { get; set; } = new ConfigModel();

        public string IniFilePath { get; set; }

        public Config(IModules modules, IUser32 user32, IConfigPersistence persistence, IPersistence io)
        {
            _modules = modules;
            _user32 = user32;
            _persistence = persistence;
            _io = io;
        }

        public JoystickModel GetLeftJoystick()
        {
            return _left;
        }

        public JoystickModel GetRightJoystick()
        {
            return _right;
        }

        public void InitConfig(string iniFile)
        {
            iniFile = GetIniFilePath(iniFile);  //--Use default if needed

            Model.Release = AppTitle; //--A kind of "version" I guess
            IniFilePath = iniFile;

            SoundDevices = _modules.Audio.FindSoundDevices();

            //--Synch joysticks to config instance
            _modules.Joystick.SetLeftJoystick(_left);
            _modules.Joystick.SetRightJoystick(_right);

            ReadIniFile();

            SynchSystemWithConfig();

            ConfigureJoysticks();

            string soundCardName = Model.SoundCardName;
            int soundCardIndex = SoundDevices.IndexOf(soundCardName);

            _modules.Audio.SoundInit(_modules.Emu.WindowHandle, soundCardIndex, Model.AudioRate);

            //  Try to open the config file.  Create it if necessary.  Abort if failure.
            if (File.Exists(iniFile))
            {
                return;
            }

            try
            {
                File.WriteAllText(iniFile, "");
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open ini file", "Error");

                Environment.Exit(0);
            }

            Save();
        }

        public void SynchSystemWithConfig()
        {
            _modules.Vcc.AutoStart = Model.AutoStart;
            _modules.Vcc.Throttle = Model.SpeedThrottle != Define.FALSE;

            _modules.Emu.RamSize = Model.RamSize;
            _modules.Emu.FrameSkip = Model.FrameSkip;

            _modules.Graphics.SetPaletteType();
            _modules.Draw.SetAspect(Model.ForceAspect);
            _modules.Graphics.SetScanLines(Model.ScanLines);
            _modules.Emu.SetCpuMultiplier(Model.CPUMultiplier);

            SetCpuType(Model.CpuType);

            _modules.Graphics.SetMonitorType(Model.MonitorType);
            _modules.MC6821.SetCartAutoStart(Model.CartAutoStart);
        }

        // LoadIniFile allows user to browse for an ini file and reloads the config from it.
        public void LoadIniFile()
        {
            string szFileName = IniFilePath;
            string appPath = Path.GetDirectoryName(szFileName) ?? "C:\\";

            var openFileDlg = new Microsoft.Win32.OpenFileDialog
            {
                FileName = szFileName,
                DefaultExt = ".ini",
                Filter = "INI files (.ini)|*.ini",
                InitialDirectory = appPath,
                CheckFileExists = true,
                ShowReadOnly = false,
                Title = "Load Vcc Config File"
            };

            if (openFileDlg.ShowDialog() == true)
            {
                // Flush current profile
                Save();

                IniFilePath = openFileDlg.FileName;

                // Load it
                ReadIniFile();

                SynchSystemWithConfig();

                _modules.Emu.ResetPending = (byte)ResetPendingStates.Hard;
            }
        }

        public void ConfigureJoysticks()
        {
            var joystick = _modules.Joystick;

            var joysticks = joystick.FindJoysticks();

            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            if (right.DiDevice >= joysticks.Count)
            {
                right.DiDevice = 0;
            }

            if (left.DiDevice >= joysticks.Count)
            {
                left.DiDevice = 0;
            }

            joystick.SetStickNumbers(left.DiDevice, right.DiDevice);

            if (joysticks.Count == 0)	//Use Mouse input if no Joysticks present
            {
                if (left.UseMouse == 3)
                {
                    left.UseMouse = 1;
                }

                if (right.UseMouse == 3)
                {
                    right.UseMouse = 1;
                }
            }
        }

        public string GetIniFilePath(string argIniFile)
        {
            if (!string.IsNullOrEmpty(argIniFile))
            {
                IniFilePath = argIniFile;

                return argIniFile;
            }

            const string vccFolder = "VCC";
            const string iniFileName = "Vcc.ini";

            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            appDataPath = Path.Combine(appDataPath, vccFolder);

            if (!Directory.Exists(appDataPath))
            {
                try
                {
                    Directory.CreateDirectory(appDataPath);
                }
                catch (Exception)
                {
                    Debug.WriteLine("Unable to create VCC config folder.");
                }
            }

            return Path.Combine(appDataPath, iniFileName);
        }

        public void SetCpuType(byte cpuType)
        {
            var cpu = new Dictionary<CPUTypes, string>
            {
                {CPUTypes.MC6809, "MC6809"},
                {CPUTypes.HD6309, "HD6309"}
            };

            _modules.Emu.CpuType = cpuType;
            _modules.Vcc.CpuName = cpu[(CPUTypes)cpuType];
        }

        public void ReadIniFile()
        {
            var root = _io.Load(@"C:\CoCo\coco.json");

            string iniFilePath = IniFilePath;

            _persistence.Load(Model, GetLeftJoystick(), GetRightJoystick(), iniFilePath);

            ValidateModel(Model);

            _modules.Keyboard.KeyboardBuildRuntimeTable(Model.KeyboardLayout);

            Model.PakPath = _modules.Emu.PakPath;
            _modules.PAKInterface.InsertModule(_modules.Emu.EmulationRunning, Model.ModulePath);   // Should this be here?

            if (Model.RememberSize)
            {
                SetWindowSize(Model.WindowSizeX, Model.WindowSizeY);
            }
            else
            {
                SetWindowSize(Define.DEFAULT_WIDTH, Define.DEFAULT_HEIGHT);
            }
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
            byte cpuMultiplier = (byte)(Model.CPUMultiplier + change);

            if (cpuMultiplier < 2 || cpuMultiplier > Model.MaxOverclock)
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

            Model.CPUMultiplier = cpuMultiplier;

            _modules.Emu.ResetPending = (byte)ResetPendingStates.ClsSynch; // Without this, changing the config does nothing.
        }

        public void SetWindowSize(short width, short height)
        {
            HWND handle = _user32.GetActiveWindow();

            SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

            _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
        }


        public void SaveAs()
        {
            // EJJ get current ini file path
            string curIni = IniFilePath;

            // Let SaveFileDialog suggest it
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = curIni,
                DefaultExt = ".ini",
                Filter = "INI files (.ini)|*.ini",
                FilterIndex = 1,
                InitialDirectory = curIni,
                CheckPathExists = true,
                Title = "Save Vcc Config File",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                Save(); // Flush current config

                string newIni = saveFileDialog.FileName;

                if (newIni != curIni)
                {
                    try
                    {
                        File.Copy(curIni, newIni, true);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Copy config failed", "error");
                    }
                }
            }
        }

        public void Save()
        {
            Model.WindowSizeX = (short)_modules.Emu.WindowSize.X;
            Model.WindowSizeY = (short)_modules.Emu.WindowSize.Y;

            string modulePath = Model.ModulePath;

            if (string.IsNullOrEmpty(modulePath))
            {
                modulePath = _modules.PAKInterface.GetCurrentModule();
            }

            Model.ModulePath = modulePath;

            ValidateModel(Model);

            string iniFilePath = IniFilePath;

            _persistence.Save(Model, GetLeftJoystick(), GetRightJoystick(), iniFilePath);

            _io.Save(@"C:\CoCo\coco.json", new Root());
        }

        public KeyboardLayouts GetCurrentKeyboardLayout()
        {
            return Model.KeyboardLayout;
        }

        public void ValidateModel(ConfigModel model)
        {
            string exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());

            if (string.IsNullOrEmpty(exePath))
            {
                throw new Exception("Invalid exePath");
            }

            string modulePath = model.ModulePath;
            string externalBasicImage = model.ExternalBasicImage;

            //--If module is in same location as .exe, strip off path portion, leaving only module name

            //--If relative to EXE path, simplify
            if (!string.IsNullOrEmpty(modulePath) && modulePath.StartsWith(exePath))
            {
                modulePath = modulePath[exePath.Length..];
            }

            //--If relative to EXE path, simplify
            if (!string.IsNullOrEmpty(externalBasicImage) && externalBasicImage.StartsWith(exePath))
            {
                externalBasicImage = externalBasicImage[exePath.Length..];
            }

            model.ModulePath = modulePath;
            model.SetExternalBasicImage(externalBasicImage);
        }

        public int GetPaletteType()
        {
            return Model.PaletteType;
        }


        public bool GetRememberSize()
        {
            return Model.RememberSize;
        }

        public Point GetWindowSize()
        {
            return new Point { X = Model.WindowSizeX, Y = Model.WindowSizeY };
        }
    }

}
