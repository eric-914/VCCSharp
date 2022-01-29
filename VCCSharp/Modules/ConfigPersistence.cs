using System;
using VCCSharp.Enums;
using VCCSharp.Libraries;
using VCCSharp.Models;

namespace VCCSharp.Modules
{
    public interface IConfigPersistence
    {
        void Load(ConfigModel model, JoystickModel left, JoystickModel right, string iniFilePath);
        void Save(ConfigModel model, JoystickModel left, JoystickModel right, string iniFilePath);
    }

    public class ConfigPersistence : IConfigPersistence
    {
        private readonly IKernel _kernel;

        public ConfigPersistence(IKernel kernel)
        {
            _kernel = kernel;
        }

        public void Load(ConfigModel model, JoystickModel left, JoystickModel right, string iniFilePath)
        {
            byte[] buffer = new byte[Define.MAX_PATH];

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
            _kernel.GetPrivateProfileStringA("Audio", "SoundCardName", "", buffer, Define.MAX_LOADSTRING, iniFilePath);

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
            _kernel.GetPrivateProfileStringA("Memory", "ExternalBasicImage", "", buffer, Define.MAX_PATH, iniFilePath);

            model.SetExternalBasicImage(Convert.ToString(buffer));

            //[Misc]
            model.AutoStart = _kernel.GetPrivateProfileIntA("Misc", "AutoStart", 1, iniFilePath) != 0;
            model.CartAutoStart = (byte)_kernel.GetPrivateProfileIntA("Misc", "CartAutoStart", 1, iniFilePath);
            model.KeyboardLayout = (KeyboardLayouts)_kernel.GetPrivateProfileIntA("Misc", "KeyboardLayout", 0, iniFilePath);

            //[Module]
            _kernel.GetPrivateProfileStringA("Module", "ModulePath", "", buffer, Define.MAX_PATH, iniFilePath);

            model.ModulePath = Converter.ToString(buffer);

            //[LeftJoyStick]
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
            _kernel.GetPrivateProfileStringA("DefaultPaths", "CassPath", "", buffer, Define.MAX_PATH, iniFilePath);
            model.CassPath = Converter.ToString(buffer);

            _kernel.GetPrivateProfileStringA("DefaultPaths", "FloppyPath", "", buffer, Define.MAX_PATH, iniFilePath);
            model.FloppyPath = Converter.ToString(buffer);

            _kernel.GetPrivateProfileStringA("DefaultPaths", "CoCoRomPath", "", buffer, Define.MAX_PATH, iniFilePath);
            model.SetCoCoRomPath(Converter.ToString(buffer));

            _kernel.GetPrivateProfileStringA("DefaultPaths", "SerialCaptureFilePath", "", buffer, Define.MAX_PATH, iniFilePath);
            model.SerialCaptureFilePath = Converter.ToString(buffer);

            _kernel.GetPrivateProfileStringA("DefaultPaths", "PakPath", "", buffer, Define.MAX_PATH, iniFilePath);
            model.PakPath = Converter.ToString(buffer);
        }

        public void Save(ConfigModel model, JoystickModel left, JoystickModel right, string iniFilePath)
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
            SaveText("Version", "Release", model.GetRelease()); //## Write-only ##//

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
            SaveInt("Misc", "KeyboardLayout", (int)model.KeyboardLayout);

            //[Module]
            SaveText("Module", "ModulePath", model.ModulePath);

            //[LeftJoyStick]
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

    }
}