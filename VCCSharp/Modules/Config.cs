using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using static System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules
{
    public interface IConfig
    {
        unsafe ConfigState* GetConfigState();
        unsafe void InitConfig(EmuState* emuState, ref CmdLineArguments cmdLineArgs);
        unsafe void WriteIniFile(EmuState* emuState);
        unsafe void SynchSystemWithConfig(EmuState* emuState);
        int GetPaletteType();
        string ExternalBasicImage();
        void UpdateSoundBar(ushort left, ushort right);
        unsafe void DecreaseOverclockSpeed(EmuState* emuState);
        unsafe void IncreaseOverclockSpeed(EmuState* emuState);
        void LoadIniFile();
        short GetCurrentKeyboardLayout();
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;
        private readonly IUser32 _user32;
        private readonly IKernel _kernel;

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

            string appTitle = _modules.Resource.ResourceAppTitle(emuState->Resources);
            string iniFile = GetIniFilePath(cmdLineArgs.IniFile);

            Converter.ToByteArray(appTitle, configState->Model->Release);   //--A kind of "versioning" I guess
            Converter.ToByteArray(iniFile, configState->IniFilePath);

            //--TODO: Silly way to get C# to look at the SoundCardList array correctly
            SoundCardList* soundCards = (SoundCardList*)(&configState->SoundCards);

            configState->NumberOfSoundCards = _modules.Audio.GetSoundCardList(soundCards);

            //--Synch joysticks to config instance
            JoystickState* joystickState = _modules.Joystick.GetJoystickState();

            joystickState->Left = configState->Model->Left;
            joystickState->Right = configState->Model->Right;

            ReadIniFile(emuState);

            SynchSystemWithConfig(emuState);

            ConfigureJoysticks();

            string soundCardName = Converter.ToString(configState->Model->SoundCardName);
            byte soundCardIndex = GetSoundCardIndex(soundCardName);

            var array = configState->SoundCards.ToArray();
            SoundCardList soundCard = array[soundCardIndex];
            _GUID* guid = soundCard.Guid;

            _modules.Audio.SoundInit(emuState->WindowHandle, guid, configState->Model->AudioRate);

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
            ConfigState* configState = GetConfigState();
            VccState* vccState = _modules.Vcc.GetVccState();

            ConfigModel* model = configState->Model;

            vccState->AutoStart = model->AutoStart;
            vccState->Throttle = model->SpeedThrottle;

            emuState->RamSize = model->RamSize;
            emuState->FrameSkip = model->FrameSkip;

            _modules.Graphics.SetPaletteType();
            _modules.DirectDraw.SetAspect(model->ForceAspect);
            _modules.Graphics.SetScanLines(emuState, model->ScanLines);
            _modules.Emu.SetCPUMultiplier(model->CPUMultiplier);

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

        public void UpdateSoundBar(ushort left, ushort right)
        {
            unsafe
            {
                ConfigState* configState = GetConfigState();

                if (configState->hDlgBar == null)
                {
                    return;
                }

                _modules.Callbacks.SetDialogAudioBars(configState->hDlgBar, left, right);
            }
        }

        public void ConfigureJoysticks()
        {
            int temp = 0;

            unsafe
            {
                ConfigState* configState = GetConfigState();

                JoystickModel* left = configState->Model->Left;
                JoystickModel* right = configState->Model->Right;

                configState->NumberOfJoysticks = (byte)_modules.Joystick.EnumerateJoysticks();

                for (byte index = 0; index < configState->NumberOfJoysticks; index++)
                {
                    temp = _modules.Joystick.InitJoyStick(index);
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
                return Converter.ToString(GetConfigState()->Model->ExternalBasicImage);
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
            unsafe
            {
                VccState* vccState = _modules.Vcc.GetVccState();
                EmuState* emuState = _modules.Emu.GetEmuState();

                var cpu = new Dictionary<CPUTypes, string>
                {
                    {CPUTypes.MC6809, "MC6809"},
                    {CPUTypes.HD6309, "HD6309"}
                };

                emuState->CpuType = cpuType;
                Converter.ToByteArray(cpu[(CPUTypes)cpuType], vccState->CpuName);
            }
        }

        public unsafe void ReadIniFile(EmuState* emuState)
        {
            ConfigState* configState = GetConfigState();

            string iniFilePath = Converter.ToString(configState->IniFilePath);
            string modulePath = Converter.ToString(configState->Model->ModulePath);

            LoadConfiguration(configState->Model, iniFilePath);

            ValidateModel(configState->Model);

            _modules.Keyboard.vccKeyboardBuildRuntimeTable(configState->Model->KeyMapIndex);

            _modules.PAKInterface.InsertModule(emuState, modulePath);   // Should this be here?

            if (configState->Model->RememberSize == Define.TRUE)
            {
                SetWindowSize(configState->Model->WindowSizeX, configState->Model->WindowSizeY);
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
            ConfigState* configState = GetConfigState();

            byte cpuMultiplier = (byte)(configState->Model->CPUMultiplier + change);

            if (cpuMultiplier < 2 || cpuMultiplier > configState->Model->MaxOverclock)
            {
                return;
            }

            // Send updates to the dialog if it's open.
            if (emuState->ConfigDialog != Zero)
            {
                HWND hDlg = configState->hWndConfig[1];

                _modules.Callbacks.SetDialogCpuMultiplier(hDlg, cpuMultiplier);
            }

            configState->Model->CPUMultiplier = cpuMultiplier;

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
            model->Left->UseMouse = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "UseMouse", 1, iniFilePath);
            model->Left->Left = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Left", 75, iniFilePath);
            model->Left->Right = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Right", 77, iniFilePath);
            model->Left->Up = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Up", 72, iniFilePath);
            model->Left->Down = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Down", 80, iniFilePath);
            model->Left->Fire1 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire1", 59, iniFilePath);
            model->Left->Fire2 = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "Fire2", 60, iniFilePath);
            model->Left->DiDevice = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "DiDevice", 0, iniFilePath);
            model->Left->HiRes = (byte)_kernel.GetPrivateProfileIntA("LeftJoyStick", "HiResDevice", 0, iniFilePath);

            //[RightJoyStick]
            model->Right->UseMouse = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "UseMouse", 1, iniFilePath);
            model->Right->Left = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Left", 75, iniFilePath);
            model->Right->Right = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Right", 77, iniFilePath);
            model->Right->Up = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Up", 72, iniFilePath);
            model->Right->Down = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Down", 80, iniFilePath);
            model->Right->Fire1 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire1", 59, iniFilePath);
            model->Right->Fire2 = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "Fire2", 60, iniFilePath);
            model->Right->DiDevice = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "DiDevice", 0, iniFilePath);
            model->Right->HiRes = (byte)_kernel.GetPrivateProfileIntA("RightJoyStick", "HiResDevice", 0, iniFilePath);

            //[DefaultPaths]
            _kernel.GetPrivateProfileStringA("DefaultPaths", "CassPath", "", model->CassPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "FloppyPath", "", model->FloppyPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "CoCoRomPath", "", model->CoCoRomPath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "SerialCaptureFilePath", "", model->SerialCaptureFilePath, Define.MAX_PATH, iniFilePath);
            _kernel.GetPrivateProfileStringA("DefaultPaths", "PakPath", "", model->PakPath, Define.MAX_PATH, iniFilePath);
        }

        public unsafe void ValidateModel(ConfigModel* model)
        {
            Library.Config.ValidateModel(model);
        }

        public int GetPaletteType()
        {
            return Library.Config.GetPaletteType();
        }

        public byte GetSoundCardIndex(string soundCardName)
        {
            return Library.Config.GetSoundCardIndex(soundCardName);
        }

        public unsafe void WriteIniFile(EmuState* emuState)
        {
            Library.Config.WriteIniFile(emuState);
        }

        public short GetCurrentKeyboardLayout()
        {
            return Library.Config.GetCurrentKeyboardLayout();
        }
    }
}
