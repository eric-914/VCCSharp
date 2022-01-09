using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Properties;
using static System.IntPtr;
using HWND = System.IntPtr;
using Point = System.Drawing.Point;

namespace VCCSharp.Modules
{
    public interface IConfig
    {
        ConfigModel ConfigModel { get; }
        JoystickModel GetLeftJoystick();
        JoystickModel GetRightJoystick();

        void InitConfig(ref CmdLineArguments cmdLineArgs);
        void WriteIniFile();
        void SynchSystemWithConfig();
        int GetPaletteType();
        void DecreaseOverclockSpeed();
        void IncreaseOverclockSpeed();
        void LoadIniFile();
        short GetCurrentKeyboardLayout();
        void SaveConfig();
        byte GetSoundCardIndex(string soundCardName);
        bool GetRememberSize();

        Point GetIniWindowSize();
        string AppTitle { get; }
        byte TextMode { get; set; }
        bool PrintMonitorWindow { get; set; }
        ushort TapeCounter { get; set; }
        byte TapeMode { get; set; }
        short NumberOfSoundCards { get; set; }
        string TapeFileName { get; set; }
        string SerialCaptureFile { get; set; }
        string IniFilePath { get; set; }

        SoundCardList[] SoundCards { get; }
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IKernel _kernel;

        public string AppTitle { get; } = Resources.ResourceManager.GetString("AppTitle");

        public byte TextMode { get; set; } = 1;  //--Add LF to CR
        public bool PrintMonitorWindow { get; set; }

        public ushort TapeCounter { get; set; }
        public byte TapeMode { get; set; } = Define.STOP;

        public short NumberOfSoundCards { get; set; }

        public string TapeFileName { get; set; }
        public string SerialCaptureFile { get; set; }
        //public string OutBuffer;

        private byte _numberOfJoysticks;

        private readonly JoystickModel _left = new JoystickModel();
        private readonly JoystickModel _right = new JoystickModel();

        public SoundCardList[] SoundCards { get; } = new SoundCardList[Define.MAXCARDS];

        public ConfigModel ConfigModel { get; set; } = new ConfigModel();

        public string IniFilePath { get; set; }

        public Config(IModules modules, IUser32 user32, IKernel kernel)
        {
            _modules = modules;
            _user32 = user32;
            _kernel = kernel;
        }

        public JoystickModel GetLeftJoystick()
        {
            return _left;
        }

        public JoystickModel GetRightJoystick()
        {
            return _right;
        }

        public unsafe void InitConfig(ref CmdLineArguments cmdLineArgs)
        {
            string iniFile = GetIniFilePath(cmdLineArgs.IniFile);

            ConfigModel.Release = AppTitle; //--A kind of "version" I guess
            IniFilePath = iniFile;

            //--TODO: Silly way to get C# to look at the SoundCardList array correctly
            //SoundCardList* soundCards = (SoundCardList*)(&configState->SoundCards);

            NumberOfSoundCards = 0;
            _modules.Sound.DirectSoundEnumerateSoundCards();

            //--Synch joysticks to config instance
            _modules.Joystick.SetLeftJoystick(_left);
            _modules.Joystick.SetRightJoystick(_right);

            ReadIniFile();

            SynchSystemWithConfig();

            ConfigureJoysticks();

            string soundCardName = ConfigModel.SoundCardName;
            byte soundCardIndex = GetSoundCardIndex(soundCardName);

            SoundCardList soundCard = SoundCards[soundCardIndex];

            _modules.Audio.SoundInit(_modules.Emu.WindowHandle, soundCard.Guid, ConfigModel.AudioRate);

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

            WriteIniFile();
        }

        public void SynchSystemWithConfig()
        {
            _modules.Vcc.AutoStart = ConfigModel.AutoStart;
            _modules.Vcc.Throttle = ConfigModel.SpeedThrottle != Define.FALSE;

            _modules.Emu.RamSize = ConfigModel.RamSize;
            _modules.Emu.FrameSkip = ConfigModel.FrameSkip;

            _modules.Graphics.SetPaletteType();
            _modules.Draw.SetAspect(ConfigModel.ForceAspect);
            _modules.Graphics.SetScanLines(ConfigModel.ScanLines);
            _modules.Emu.SetCpuMultiplier(ConfigModel.CPUMultiplier);

            SetCpuType(ConfigModel.CpuType);

            _modules.Graphics.SetMonitorType(ConfigModel.MonitorType);
            _modules.MC6821.SetCartAutoStart(ConfigModel.CartAutoStart);
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
                WriteIniFile();

                IniFilePath = openFileDlg.FileName;

                // Load it
                ReadIniFile();

                SynchSystemWithConfig();

                _modules.Emu.ResetPending = (byte)ResetPendingStates.Hard;
            }
        }

        public void ConfigureJoysticks()
        {
            JoystickModel left = GetLeftJoystick();
            JoystickModel right = GetRightJoystick();

            _numberOfJoysticks = (byte)_modules.Joystick.EnumerateJoysticks();

            for (byte index = 0; index < _numberOfJoysticks; index++)
            {
                _modules.Joystick.InitJoyStick(index);
            }

            if (right.DiDevice >= _numberOfJoysticks)
            {
                right.DiDevice = 0;
            }

            if (left.DiDevice >= _numberOfJoysticks)
            {
                left.DiDevice = 0;
            }

            _modules.Joystick.SetStickNumbers(left.DiDevice, right.DiDevice);

            if (_numberOfJoysticks == 0)	//Use Mouse input if no Joysticks present
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
            string iniFilePath = IniFilePath;

            LoadConfiguration(ConfigModel, iniFilePath);

            ValidateModel(ConfigModel);

            _modules.Keyboard.KeyboardBuildRuntimeTable(ConfigModel.KeyMapIndex);

            ConfigModel.PakPath = _modules.Emu.PakPath;
            _modules.PAKInterface.InsertModule(_modules.Emu.EmulationRunning, ConfigModel.ModulePath);   // Should this be here?

            if (ConfigModel.RememberSize)
            {
                SetWindowSize(ConfigModel.WindowSizeX, ConfigModel.WindowSizeY);
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
            byte cpuMultiplier = (byte)(ConfigModel.CPUMultiplier + change);

            if (cpuMultiplier < 2 || cpuMultiplier > ConfigModel.MaxOverclock)
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

            ConfigModel.CPUMultiplier = cpuMultiplier;

            _modules.Emu.ResetPending = (byte)ResetPendingStates.ClsSynch; // Without this, changing the config does nothing.
        }

        public void SetWindowSize(short width, short height)
        {
            HWND handle = _user32.GetActiveWindow();

            SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

            _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
        }

        public unsafe void LoadConfiguration(ConfigModel model, string iniFilePath)
        {
            byte[] buffer = new byte[Define.MAX_PATH];

            fixed (byte* p = buffer)
            {
                //[Version]
                //_kernel.GetPrivateProfileStringA("Version", "Release", "", model.Release, Define.MAX_LOADSTRING, iniFilePath);  //## Write-only ##//

                //[CPU]
                model.CPUMultiplier = (byte)_kernel.GetPrivateProfileIntA("CPU", "CPUMultiplier", 2, iniFilePath);
                model.FrameSkip = (byte)_kernel.GetPrivateProfileIntA("CPU", "FrameSkip", 1, iniFilePath);
                model.SpeedThrottle = (byte)_kernel.GetPrivateProfileIntA("CPU", "SpeedThrottle", 1, iniFilePath);
                model.CpuType = (byte)_kernel.GetPrivateProfileIntA("CPU", "CpuType", 0, iniFilePath);
                model.MaxOverclock = _kernel.GetPrivateProfileIntA("CPU", "MaxOverClock", 227, iniFilePath);

                //[Audio]
                model.AudioRate = _kernel.GetPrivateProfileIntA("Audio", "AudioRate", 3, iniFilePath);
                _kernel.GetPrivateProfileStringA("Audio", "SoundCardName", "", p, Define.MAX_LOADSTRING, iniFilePath);

                model.SoundCardName = Converter.ToString(buffer);

                //[Video]
                model.MonitorType = (MonitorTypes)_kernel.GetPrivateProfileIntA("Video", "MonitorType", 1, iniFilePath);
                model.PaletteType = (byte)_kernel.GetPrivateProfileIntA("Video", "PaletteType", 1, iniFilePath);
                model.ScanLines = (byte)_kernel.GetPrivateProfileIntA("Video", "ScanLines", 0, iniFilePath);
                model.ForceAspect = (byte)_kernel.GetPrivateProfileIntA("Video", "ForceAspect", 0, iniFilePath) != 0;
                model.RememberSize = _kernel.GetPrivateProfileIntA("Video", "RememberSize", 0, iniFilePath) != 0;
                model.WindowSizeX = (short)_kernel.GetPrivateProfileIntA("Video", "WindowSizeX", Define.DEFAULT_WIDTH, iniFilePath);
                model.WindowSizeY = (short)_kernel.GetPrivateProfileIntA("Video", "WindowSizeY", Define.DEFAULT_HEIGHT, iniFilePath);

                //[Memory]
                model.RamSize = (byte)_kernel.GetPrivateProfileIntA("Memory", "RamSize", 1, iniFilePath);
                _kernel.GetPrivateProfileStringA("Memory", "ExternalBasicImage", "", p, Define.MAX_PATH, iniFilePath);

                model.ExternalBasicImage = Convert.ToString(buffer);

                //[Misc]
                model.AutoStart = _kernel.GetPrivateProfileIntA("Misc", "AutoStart", 1, iniFilePath) != 0;
                model.CartAutoStart = (byte)_kernel.GetPrivateProfileIntA("Misc", "CartAutoStart", 1, iniFilePath);
                model.KeyMapIndex = (byte)_kernel.GetPrivateProfileIntA("Misc", "KeyMapIndex", 0, iniFilePath);

                //[Module]
                _kernel.GetPrivateProfileStringA("Module", "ModulePath", "", p, Define.MAX_PATH, iniFilePath);

                model.ModulePath = Converter.ToString(buffer);

                //[LeftJoyStick]
                var left = GetLeftJoystick();
                left.UseMouse = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "UseMouse", 1, iniFilePath);
                left.Left = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Left", 75, iniFilePath);
                left.Right = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Right", 77, iniFilePath);
                left.Up = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Up", 72, iniFilePath);
                left.Down = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Down", 80, iniFilePath);
                left.Fire1 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire1", 59, iniFilePath);
                left.Fire2 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire2", 60, iniFilePath);
                left.DiDevice = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "DiDevice", 0, iniFilePath);
                left.HiRes = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "HiResDevice", 0, iniFilePath);

                //[RightJoyStick]
                var right = GetRightJoystick();
                right.UseMouse = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "UseMouse", 1, iniFilePath);
                right.Left = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Left", 75, iniFilePath);
                right.Right = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Right", 77, iniFilePath);
                right.Up = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Up", 72, iniFilePath);
                right.Down = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Down", 80, iniFilePath);
                right.Fire1 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire1", 59, iniFilePath);
                right.Fire2 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire2", 60, iniFilePath);
                right.DiDevice = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "DiDevice", 0, iniFilePath);
                right.HiRes = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "HiResDevice", 0, iniFilePath);

                //[DefaultPaths]
                _kernel.GetPrivateProfileStringA("DefaultPaths", "CassPath", "", p, Define.MAX_PATH, iniFilePath);
                model.CassPath = Converter.ToString(buffer);

                _kernel.GetPrivateProfileStringA("DefaultPaths", "FloppyPath", "", p, Define.MAX_PATH, iniFilePath);
                model.FloppyPath = Converter.ToString(buffer);

                _kernel.GetPrivateProfileStringA("DefaultPaths", "CoCoRomPath", "", p, Define.MAX_PATH, iniFilePath);
                model.CoCoRomPath = Converter.ToString(buffer);

                _kernel.GetPrivateProfileStringA("DefaultPaths", "SerialCaptureFilePath", "", p, Define.MAX_PATH, iniFilePath);
                model.SerialCaptureFilePath = Converter.ToString(buffer);

                _kernel.GetPrivateProfileStringA("DefaultPaths", "PakPath", "", p, Define.MAX_PATH, iniFilePath);
                model.PakPath = Converter.ToString(buffer);

            }
        }

        public void SaveConfig()
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
                WriteIniFile(); // Flush current config

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

        public short GetCurrentKeyboardLayout()
        {
            return ConfigModel.KeyMapIndex;
        }

        public void WriteIniFile()
        {
            ConfigModel.WindowSizeX = (short)_modules.Emu.WindowSize.X;
            ConfigModel.WindowSizeY = (short)_modules.Emu.WindowSize.Y;

            string modulePath = ConfigModel.ModulePath;

            if (string.IsNullOrEmpty(modulePath))
            {
                modulePath = _modules.PAKInterface.GetCurrentModule();
            }

            ConfigModel.ModulePath = modulePath;

            ValidateModel(ConfigModel);

            string iniFilePath = IniFilePath;

            SaveConfiguration(ConfigModel, iniFilePath);
        }

        public void ValidateModel(ConfigModel model)
        {
            if (model.KeyMapIndex > 3)
            {
                model.KeyMapIndex = 0;	//Default to DECB Mapping
            }

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
            model.ExternalBasicImage = externalBasicImage;
        }

        public int GetPaletteType()
        {
            return ConfigModel.PaletteType;
        }

        public void SaveConfiguration(ConfigModel model, string iniFilePath)
        {
            void SaveText(string group, string key, string value)
            {
                _kernel.WritePrivateProfileStringA(group, key, value, iniFilePath); //## Write-only ##//
            }

            void SaveInt(string group, string key, int value)
            {
                _kernel.WritePrivateProfileStringA(group, key, value.ToString(), iniFilePath); //## Write-only ##//
            }

            //[Version]
            SaveText("Version", "Release", model.Release); //## Write-only ##//

            //[CPU]
            SaveInt("CPU", "CPUMultiplier", model.CPUMultiplier);
            SaveInt("CPU", "FrameSkip", model.FrameSkip);
            SaveInt("CPU", "SpeedThrottle", model.SpeedThrottle);
            SaveInt("CPU", "CpuType", model.CpuType);
            SaveInt("CPU", "MaxOverClock", model.MaxOverclock);

            //[Audio]
            SaveText("Audio", "SoundCardName", model.SoundCardName);
            SaveInt("Audio", "AudioRate", model.AudioRate);

            //[Video]
            SaveInt("Video", "MonitorType", (int)model.MonitorType);
            SaveInt("Video", "PaletteType", model.PaletteType);
            SaveInt("Video", "ScanLines", model.ScanLines);
            SaveInt("Video", "ForceAspect", model.ForceAspect ? 1 : 0);
            SaveInt("Video", "RememberSize", model.RememberSize ? 1 : 0);
            SaveInt("Video", "WindowSizeX", model.WindowSizeX);
            SaveInt("Video", "WindowSizeY", model.WindowSizeY);

            //[Memory]
            SaveInt("Memory", "RamSize", model.RamSize);
            //_kernel.WritePrivateProfileStringA("Memory", "ExternalBasicImage", model.ExternalBasicImage, iniFilePath); //## READ-ONLY ##//

            //[Misc]
            SaveInt("Misc", "AutoStart", model.AutoStart ? 1 : 0);
            SaveInt("Misc", "CartAutoStart", model.CartAutoStart);
            SaveInt("Misc", "KeyMapIndex", model.KeyMapIndex);

            //[Module]
            SaveText("Module", "ModulePath", model.ModulePath);

            //[LeftJoyStick]
            var left = GetLeftJoystick();
            SaveInt("LeftJoyStick", "UseMouse", left.UseMouse);
            SaveInt("LeftJoyStick", "Left", left.Left);
            SaveInt("LeftJoyStick", "Right", left.Right);
            SaveInt("LeftJoyStick", "Up", left.Up);
            SaveInt("LeftJoyStick", "Down", left.Down);
            SaveInt("LeftJoyStick", "Fire1", left.Fire1);
            SaveInt("LeftJoyStick", "Fire2", left.Fire2);
            SaveInt("LeftJoyStick", "DiDevice", left.DiDevice);
            SaveInt("LeftJoyStick", "HiResDevice", left.HiRes);

            //[RightJoyStick]
            var right = GetRightJoystick();
            SaveInt("RightJoyStick", "UseMouse", right.UseMouse);
            SaveInt("RightJoyStick", "Left", right.Left);
            SaveInt("RightJoyStick", "Right", right.Right);
            SaveInt("RightJoyStick", "Up", right.Up);
            SaveInt("RightJoyStick", "Down", right.Down);
            SaveInt("RightJoyStick", "Fire1", right.Fire1);
            SaveInt("RightJoyStick", "Fire2", right.Fire2);
            SaveInt("RightJoyStick", "DiDevice", right.DiDevice);
            SaveInt("RightJoyStick", "HiResDevice", right.HiRes);

            //[DefaultPaths]
            SaveText("DefaultPaths", "CassPath", model.CassPath);
            SaveText("DefaultPaths", "PakPath", model.PakPath);
            SaveText("DefaultPaths", "FloppyPath", model.FloppyPath);
            //SaveText("DefaultPaths", "CoCoRomPath", model.CoCoRomPath); //## READ-ONLY ##//
            SaveText("DefaultPaths", "SerialCaptureFilePath", model.SerialCaptureFilePath);

            //--Flush .ini file
            _kernel.WritePrivateProfileStringA(null, null, null, iniFilePath);
        }

        public byte GetSoundCardIndex(string soundCardName)
        {
            for (byte index = 0; index < NumberOfSoundCards; index++)
            {
                var item = GetSoundCardNameAtIndex(index);

                if (soundCardName == item)
                {
                    return index;
                }
            }

            return 0;
        }

        public unsafe string GetSoundCardNameAtIndex(byte index)
        {
            var card = SoundCards[index];

            return Converter.ToString(card.CardName);
            //var cards = GetConfigState()->SoundCards.ToArray();
            //var card = cards[index]; 

            //return Converter.ToString(card.CardName);
        }

        public bool GetRememberSize()
        {
            return ConfigModel.RememberSize;
        }

        public Point GetIniWindowSize()
        {
            Point p = new Point { X = ConfigModel.WindowSizeX, Y = ConfigModel.WindowSizeY };

            return p;
        }

    }
}
