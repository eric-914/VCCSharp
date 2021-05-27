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
        unsafe ConfigState* GetConfigState();
        unsafe ConfigModel* GetConfigModel();
        unsafe JoystickModel* GetLeftJoystick();
        unsafe JoystickModel* GetRightJoystick();

        unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);
        unsafe void WriteIniFile(EmuState* emuState);
        unsafe void SynchSystemWithConfig(EmuState* emuState);
        int GetPaletteType();
        string ExternalBasicImage();
        unsafe void DecreaseOverclockSpeed(EmuState* emuState);
        unsafe void IncreaseOverclockSpeed(EmuState* emuState);
        void LoadIniFile();
        short GetCurrentKeyboardLayout();
        void SaveConfig();
        byte GetSoundCardIndex(string soundCardName);
        bool GetRememberSize();

        Point GetIniWindowSize();
        string AppTitle { get; }
        byte TextMode { get; set; }
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IKernel _kernel;

        public string AppTitle { get; } = Resources.ResourceManager.GetString("AppTitle");

        public byte TextMode { get; set; } = 1;  //--Add LF to CR

        public Config(IModules modules, IUser32 user32, IKernel kernel)
        {
            _modules = modules;
            _user32 = user32;
            _kernel = kernel;
        }

        public unsafe ConfigState* GetConfigState()
        {
            return Library.Config.GetConfigState();
        }

        public unsafe ConfigModel* GetConfigModel()
        {
            return Library.Config.GetConfigModel();
        }

        public unsafe JoystickModel* GetLeftJoystick()
        {
            return Library.Config.GetLeftJoystick();
        }

        public unsafe JoystickModel* GetRightJoystick()
        {
            return Library.Config.GetRightJoystick();
        }

        public unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs)
        {
            ConfigState* configState = GetConfigState();
            ConfigModel* configModel = GetConfigModel();

            string iniFile = GetIniFilePath(cmdLineArgs.IniFile);

            Converter.ToByteArray(AppTitle, configModel->Release);   //--A kind of "version" I guess
            Converter.ToByteArray(iniFile, configState->IniFilePath);

            //--TODO: Silly way to get C# to look at the SoundCardList array correctly
            SoundCardList* soundCards = (SoundCardList*)(&configState->SoundCards);

            configState->NumberOfSoundCards = 0;
            _modules.DirectSound.DirectSoundEnumerateSoundCards();

            //--Synch joysticks to config instance
            JoystickState* joystickState = _modules.Joystick.GetJoystickState();

            joystickState->Left = GetLeftJoystick();
            joystickState->Right = GetRightJoystick();

            ReadIniFile(emuState);

            SynchSystemWithConfig(emuState);

            ConfigureJoysticks();

            string soundCardName = Converter.ToString(configModel->SoundCardName);
            byte soundCardIndex = GetSoundCardIndex(soundCardName);

            var array = configState->SoundCards.ToArray();
            SoundCardList soundCard = array[soundCardIndex];
            _GUID* guid = soundCard.Guid;

            _modules.Audio.SoundInit(emuState->WindowHandle, guid, configModel->AudioRate);

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

            WriteIniFile(emuState);
        }

        public unsafe void SynchSystemWithConfig(EmuState* emuState)
        {
            ConfigModel* model = GetConfigModel();

            _modules.Vcc.AutoStart = model->AutoStart;
            _modules.Vcc.Throttle = model->SpeedThrottle;

            _modules.Emu.RamSize = model->RamSize;
            _modules.Emu.FrameSkip = model->FrameSkip;

            _modules.Graphics.SetPaletteType();
            _modules.DirectDraw.SetAspect(model->ForceAspect);
            _modules.Graphics.SetScanLines(emuState, model->ScanLines);
            _modules.Emu.SetCpuMultiplier(model->CPUMultiplier);

            SetCpuType(model->CpuType);

            _modules.Graphics.SetMonitorType(model->MonitorType);
            _modules.MC6821.MC6821_SetCartAutoStart(model->CartAutoStart);
        }

        // LoadIniFile allows user to browse for an ini file and reloads the config from it.
        public void LoadIniFile()
        {
            unsafe
            {
                ConfigState* configState = GetConfigState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                string szFileName = Converter.ToString(configState->IniFilePath);
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
                    WriteIniFile(emuState);

                    Converter.ToByteArray(openFileDlg.FileName, configState->IniFilePath);

                    // Load it
                    ReadIniFile(emuState);

                    SynchSystemWithConfig(emuState);

                    emuState->ResetPending = (byte)ResetPendingStates.Hard;
                }
            }
        }

        public void ConfigureJoysticks()
        {
            unsafe
            {
                ConfigState* configState = GetConfigState();

                JoystickModel* left = GetLeftJoystick();
                JoystickModel* right = GetRightJoystick();

                configState->NumberOfJoysticks = (byte)_modules.Joystick.EnumerateJoysticks();

                for (byte index = 0; index < configState->NumberOfJoysticks; index++)
                {
                    _modules.Joystick.InitJoyStick(index);
                }

                if (right->DiDevice >= configState->NumberOfJoysticks)
                {
                    right->DiDevice = 0;
                }

                if (left->DiDevice >= configState->NumberOfJoysticks)
                {
                    left->DiDevice = 0;
                }

                _modules.Joystick.SetStickNumbers(left->DiDevice, right->DiDevice);

                if (configState->NumberOfJoysticks == 0)	//Use Mouse input if no Joysticks present
                {
                    if (left->UseMouse == 3)
                    {
                        left->UseMouse = 1;
                    }

                    if (right->UseMouse == 3)
                    {
                        right->UseMouse = 1;
                    }
                }

            }
        }

        public string ExternalBasicImage()
        {
            unsafe
            {
                return Converter.ToString(GetConfigModel()->ExternalBasicImage);
            }
        }

        public unsafe string GetIniFilePath(string argIniFile)
        {
            ConfigState* configState = GetConfigState();

            if (!string.IsNullOrEmpty(argIniFile))
            {
                Converter.ToByteArray(argIniFile, configState->IniFilePath);

                return argIniFile;
            }

            const string vccFolder = "VCC";
            const string iniFileName = "Vcc.ini";

            string appDataPath = Path.Combine(Converter.ToString(configState->AppDataPath), vccFolder);

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

        public unsafe void ReadIniFile(EmuState* emuState)
        {
            ConfigState* configState = GetConfigState();
            ConfigModel* configModel = GetConfigModel();

            string iniFilePath = Converter.ToString(configState->IniFilePath);
            string modulePath = Converter.ToString(configModel->ModulePath);

            LoadConfiguration(configModel, iniFilePath);

            ValidateModel(configModel);

            _modules.Keyboard.KeyboardBuildRuntimeTable(configModel->KeyMapIndex);

            _modules.PAKInterface.InsertModule(emuState, modulePath);   // Should this be here?

            if (configModel->RememberSize == Define.TRUE)
            {
                SetWindowSize(configModel->WindowSizeX, configModel->WindowSizeY);
            }
            else
            {
                SetWindowSize(640, 480);
            }
        }

        /**
         * Decrease the overclock speed, as seen after a POKE 65497,0.
         * Setting this value to 0 will make the emulator pause.  Hence the minimum of 2.
         */
        public unsafe void DecreaseOverclockSpeed(EmuState* emuState)
        {
            AdjustOverclockSpeed(emuState, 0xFF); //--Stupid compiler can't figure out (byte)(-1) = 0xFF
        }

        /**
         * Increase the overclock speed, as seen after a POKE 65497,0.
         * Valid values are [2,100].
         */
        public unsafe void IncreaseOverclockSpeed(EmuState* emuState)
        {
            AdjustOverclockSpeed(emuState, 1);
        }

        public unsafe void AdjustOverclockSpeed(EmuState* emuState, byte change)
        {
            ConfigModel* configModel = GetConfigModel();

            byte cpuMultiplier = (byte)(configModel->CPUMultiplier + change);

            if (cpuMultiplier < 2 || cpuMultiplier > configModel->MaxOverclock)
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

            configModel->CPUMultiplier = cpuMultiplier;

            emuState->ResetPending = (byte)ResetPendingStates.ClsSynch; // Without this, changing the config does nothing.
        }

        public void SetWindowSize(short width, short height)
        {
            HWND handle = _user32.GetActiveWindow();

            SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

            _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
        }

        public unsafe void LoadConfiguration(ConfigModel* model, string iniFilePath)
        {
            //[Version]
            //_kernel.GetPrivateProfileStringA("Version", "Release", "", model.Release, Define.MAX_LOADSTRING, iniFilePath);  //## Write-only ##//

            //[CPU]
            model->CPUMultiplier = (byte)_kernel.GetPrivateProfileIntA("CPU", "CPUMultiplier", 2, iniFilePath);
            model->FrameSkip = (byte)_kernel.GetPrivateProfileIntA("CPU", "FrameSkip", 1, iniFilePath);
            model->SpeedThrottle = (byte)_kernel.GetPrivateProfileIntA("CPU", "SpeedThrottle", 1, iniFilePath);
            model->CpuType = (byte)_kernel.GetPrivateProfileIntA("CPU", "CpuType", 0, iniFilePath);
            model->MaxOverclock = _kernel.GetPrivateProfileIntA("CPU", "MaxOverClock", 227, iniFilePath);

            //[Audio]
            model->AudioRate = _kernel.GetPrivateProfileIntA("Audio", "AudioRate", 3, iniFilePath);
            _kernel.GetPrivateProfileStringA("Audio", "SoundCardName", "", model->SoundCardName, Define.MAX_LOADSTRING, iniFilePath);

            //[Video]
            model->MonitorType = (byte)_kernel.GetPrivateProfileIntA("Video", "MonitorType", 1, iniFilePath);
            model->PaletteType = (byte)_kernel.GetPrivateProfileIntA("Video", "PaletteType", 1, iniFilePath);
            model->ScanLines = (byte)_kernel.GetPrivateProfileIntA("Video", "ScanLines", 0, iniFilePath);
            model->ForceAspect = (byte)_kernel.GetPrivateProfileIntA("Video", "ForceAspect", 0, iniFilePath);
            model->RememberSize = _kernel.GetPrivateProfileIntA("Video", "RememberSize", 0, iniFilePath);
            model->WindowSizeX = (short)_kernel.GetPrivateProfileIntA("Video", "WindowSizeX", 640, iniFilePath);
            model->WindowSizeY = (short)_kernel.GetPrivateProfileIntA("Video", "WindowSizeY", 480, iniFilePath);

            //[Memory]
            model->RamSize = (byte)_kernel.GetPrivateProfileIntA("Memory", "RamSize", 1, iniFilePath);
            _kernel.GetPrivateProfileStringA("Memory", "ExternalBasicImage", "", model->ExternalBasicImage, Define.MAX_PATH, iniFilePath);

            //[Misc]
            model->AutoStart = (byte)_kernel.GetPrivateProfileIntA("Misc", "AutoStart", 1, iniFilePath);
            model->CartAutoStart = (byte)_kernel.GetPrivateProfileIntA("Misc", "CartAutoStart", 1, iniFilePath);
            model->KeyMapIndex = (byte)_kernel.GetPrivateProfileIntA("Misc", "KeyMapIndex", 0, iniFilePath);

            //[Module]
            _kernel.GetPrivateProfileStringA("Module", "ModulePath", "", model->ModulePath, Define.MAX_PATH, iniFilePath);

            //[LeftJoyStick]
            var left = GetLeftJoystick();
            left->UseMouse = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "UseMouse", 1, iniFilePath);
            left->Left = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Left", 75, iniFilePath);
            left->Right = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Right", 77, iniFilePath);
            left->Up = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Up", 72, iniFilePath);
            left->Down = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Down", 80, iniFilePath);
            left->Fire1 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire1", 59, iniFilePath);
            left->Fire2 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire2", 60, iniFilePath);
            left->DiDevice = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "DiDevice", 0, iniFilePath);
            left->HiRes = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "HiResDevice", 0, iniFilePath);

            //[RightJoyStick]
            var right = GetRightJoystick();
            right->UseMouse = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "UseMouse", 1, iniFilePath);
            right->Left = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Left", 75, iniFilePath);
            right->Right = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Right", 77, iniFilePath);
            right->Up = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Up", 72, iniFilePath);
            right->Down = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Down", 80, iniFilePath);
            right->Fire1 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire1", 59, iniFilePath);
            right->Fire2 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire2", 60, iniFilePath);
            right->DiDevice = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "DiDevice", 0, iniFilePath);
            right->HiRes = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "HiResDevice", 0, iniFilePath);

            //[DefaultPaths]
            _kernel.GetPrivateProfileStringA("DefaultPaths", "CassPath", "", model->CassPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "FloppyPath", "", model->FloppyPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "CoCoRomPath", "", model->CoCoRomPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "SerialCaptureFilePath", "", model->SerialCaptureFilePath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "PakPath", "", model->PakPath, Define.MAX_PATH, iniFilePath);
        }

        public void SaveConfig()
        {
            unsafe
            {
                ConfigState* configState = GetConfigState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                // EJJ get current ini file path
                string curIni = Converter.ToString(configState->IniFilePath);

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
                    WriteIniFile(emuState); // Flush current config

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
        }

        public short GetCurrentKeyboardLayout()
        {
            unsafe
            {
                return GetConfigModel()->KeyMapIndex;
            }
        }

        public unsafe void WriteIniFile(EmuState* emuState)
        {
            ConfigState* configState = GetConfigState();
            ConfigModel* configModel = GetConfigModel();

            configModel->WindowSizeX = (short)_modules.Emu.WindowSize.X;
            configModel->WindowSizeY = (short)_modules.Emu.WindowSize.Y;

            string modulePath = Converter.ToString(configModel->ModulePath);

            if (string.IsNullOrEmpty(modulePath))
            {
                modulePath = _modules.PAKInterface.GetCurrentModule();
            }

            Converter.ToByteArray(modulePath, configModel->ModulePath);

            ValidateModel(configModel);

            string iniFilePath = Converter.ToString(configState->IniFilePath);

            SaveConfiguration(configModel, iniFilePath);
        }

        public unsafe void ValidateModel(ConfigModel* model)
        {
            if (model->KeyMapIndex > 3)
            {
                model->KeyMapIndex = 0;	//Default to DECB Mapping
            }

            string exePath = Path.GetDirectoryName(_modules.Vcc.GetExecPath());

            string modulePath = Converter.ToString(model->ModulePath);
            string externalBasicImage = Converter.ToString(model->ExternalBasicImage);

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

            Converter.ToByteArray(modulePath, model->ModulePath);
            Converter.ToByteArray(externalBasicImage, model->ExternalBasicImage);
        }

        public int GetPaletteType()
        {
            unsafe
            {
                return GetConfigModel()->PaletteType;
            }
        }

        public unsafe void SaveConfiguration(ConfigModel* model, string iniFilePath)
        {
            void SaveText(string group, string key, byte* value)
            {
                _kernel.WritePrivateProfileStringA(group, key, Converter.ToString(value), iniFilePath); //## Write-only ##//
            }

            void SaveInt(string group, string key, int value)
            {
                _kernel.WritePrivateProfileStringA(group, key, value.ToString(), iniFilePath); //## Write-only ##//
            }

            //[Version]
            SaveText("Version", "Release", model->Release); //## Write-only ##//

            //[CPU]
            SaveInt("CPU", "CPUMultiplier", model->CPUMultiplier);
            SaveInt("CPU", "FrameSkip", model->FrameSkip);
            SaveInt("CPU", "SpeedThrottle", model->SpeedThrottle);
            SaveInt("CPU", "CpuType", model->CpuType);
            SaveInt("CPU", "MaxOverClock", model->MaxOverclock);

            //[Audio]
            SaveText("Audio", "SoundCardName", model->SoundCardName);
            SaveInt("Audio", "AudioRate", model->AudioRate);

            //[Video]
            SaveInt("Video", "MonitorType", model->MonitorType);
            SaveInt("Video", "PaletteType", model->PaletteType);
            SaveInt("Video", "ScanLines", model->ScanLines);
            SaveInt("Video", "ForceAspect", model->ForceAspect);
            SaveInt("Video", "RememberSize", model->RememberSize);
            SaveInt("Video", "WindowSizeX", model->WindowSizeX);
            SaveInt("Video", "WindowSizeY", model->WindowSizeY);

            //[Memory]
            SaveInt("Memory", "RamSize", model->RamSize);
            //_kernel.WritePrivateProfileStringA("Memory", "ExternalBasicImage", model->ExternalBasicImage, iniFilePath); //## READ-ONLY ##//

            //[Misc]
            SaveInt("Misc", "AutoStart", model->AutoStart);
            SaveInt("Misc", "CartAutoStart", model->CartAutoStart);
            SaveInt("Misc", "KeyMapIndex", model->KeyMapIndex);

            //[Module]
            SaveText("Module", "ModulePath", model->ModulePath);

            //[LeftJoyStick]
            var left = GetLeftJoystick();
            SaveInt("LeftJoyStick", "UseMouse", left->UseMouse);
            SaveInt("LeftJoyStick", "Left", left->Left);
            SaveInt("LeftJoyStick", "Right", left->Right);
            SaveInt("LeftJoyStick", "Up", left->Up);
            SaveInt("LeftJoyStick", "Down", left->Down);
            SaveInt("LeftJoyStick", "Fire1", left->Fire1);
            SaveInt("LeftJoyStick", "Fire2", left->Fire2);
            SaveInt("LeftJoyStick", "DiDevice", left->DiDevice);
            SaveInt("LeftJoyStick", "HiResDevice", left->HiRes);

            //[RightJoyStick]
            var right = GetRightJoystick();
            SaveInt("RightJoyStick", "UseMouse", right->UseMouse);
            SaveInt("RightJoyStick", "Left", right->Left);
            SaveInt("RightJoyStick", "Right", right->Right);
            SaveInt("RightJoyStick", "Up", right->Up);
            SaveInt("RightJoyStick", "Down", right->Down);
            SaveInt("RightJoyStick", "Fire1", right->Fire1);
            SaveInt("RightJoyStick", "Fire2", right->Fire2);
            SaveInt("RightJoyStick", "DiDevice", right->DiDevice);
            SaveInt("RightJoyStick", "HiResDevice", right->HiRes);

            //[DefaultPaths]
            SaveText("DefaultPaths", "CassPath", model->CassPath);
            SaveText("DefaultPaths", "PakPath", model->PakPath);
            SaveText("DefaultPaths", "FloppyPath", model->FloppyPath);
            //SaveText("DefaultPaths", "CoCoRomPath", model->CoCoRomPath); //## READ-ONLY ##//
            SaveText("DefaultPaths", "SerialCaptureFilePath", model->SerialCaptureFilePath);

            //--Flush .ini file
            _kernel.WritePrivateProfileStringA(null, null, null, iniFilePath);
        }

        public byte GetSoundCardIndex(string soundCardName)
        {
            unsafe
            {
                ConfigState* instance = GetConfigState();

                for (byte index = 0; index < instance->NumberOfSoundCards; index++)
                {
                    byte* item = GetSoundCardNameAtIndex(index);

                    var t = Converter.ToString(item);

                    if (soundCardName == t)
                    {
                        return index;
                    }
                }

                return 0;
            }
        }

        public unsafe byte* GetSoundCardNameAtIndex(byte index)
        {
            return Library.Config.GetSoundCardNameAtIndex(index);
        }

        public bool GetRememberSize()
        {
            unsafe
            {
                return (GetConfigModel()->RememberSize != 0);
            }
        }

        public Point GetIniWindowSize()
        {
            Point p = new Point();

            unsafe
            {
                ConfigModel* model = GetConfigModel();

                p.X = model->WindowSizeX;
                p.Y = model->WindowSizeY;
            }

            return p;
        }

    }
}
