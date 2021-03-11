using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;

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
    }

    public class Config : IConfig
    {
        private readonly IModules _modules;

        public Config(IModules modules)
        {
            _modules = modules;
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

        //TODO: Still being used by LoadIniFile(...)
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

        public int GetPaletteType()
        {
            return Library.Config.GetPaletteType();
        }

        public byte GetSoundCardIndex(string soundCardName)
        {
            return Library.Config.GetSoundCardIndex(soundCardName);
        }

        public unsafe void DecreaseOverclockSpeed(EmuState* emuState)
        {
            Library.Config.DecreaseOverclockSpeed(emuState);
        }

        public unsafe void IncreaseOverclockSpeed(EmuState* emuState)
        {
            Library.Config.IncreaseOverclockSpeed(emuState);
        }

        public unsafe void WriteIniFile(EmuState* emuState)
        {
            Library.Config.WriteIniFile(emuState);
        }

        public unsafe void ReadIniFile(EmuState* emuState)
        {
            Library.Config.ReadIniFile(emuState);
        }

        public void SetCpuType(byte cpuType)
        {
            Library.Config.SetCpuType(cpuType);
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
    }
}
