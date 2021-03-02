#include "ConfigModel.h"

#include "fileoperations.h"

extern "C" {
  __declspec(dllexport) void __cdecl SaveConfiguration(ConfigModel model, char* iniFilePath) {
    //[Version]
    WritePrivateProfileString("Version", "Release", model.Release, iniFilePath); //## Write-only ##//

    //[CPU]
    FileWritePrivateProfileInt("CPU", "CPUMultiplier", model.CPUMultiplier, iniFilePath);
    FileWritePrivateProfileInt("CPU", "FrameSkip", model.FrameSkip, iniFilePath);
    FileWritePrivateProfileInt("CPU", "SpeedThrottle", model.SpeedThrottle, iniFilePath);
    FileWritePrivateProfileInt("CPU", "CpuType", model.CpuType, iniFilePath);
    FileWritePrivateProfileInt("CPU", "MaxOverClock", model.MaxOverclock, iniFilePath);

    //[Audio]
    WritePrivateProfileString("Audio", "SoundCardName", model.SoundCardName, iniFilePath);
    FileWritePrivateProfileInt("Audio", "AudioRate", model.AudioRate, iniFilePath);

    //[Video]
    FileWritePrivateProfileInt("Video", "MonitorType", model.MonitorType, iniFilePath);
    FileWritePrivateProfileInt("Video", "PaletteType", model.PaletteType, iniFilePath);
    FileWritePrivateProfileInt("Video", "ScanLines", model.ScanLines, iniFilePath);
    FileWritePrivateProfileInt("Video", "ForceAspect", model.ForceAspect, iniFilePath);
    FileWritePrivateProfileInt("Video", "RememberSize", model.RememberSize, iniFilePath);
    FileWritePrivateProfileInt("Video", "WindowSizeX", model.WindowSizeX, iniFilePath);
    FileWritePrivateProfileInt("Video", "WindowSizeY", model.WindowSizeY, iniFilePath);

    //[Memory]
    FileWritePrivateProfileInt("Memory", "RamSize", model.RamSize, iniFilePath);
    //WritePrivateProfileString("Memory", "ExternalBasicImage", model.ExternalBasicImage, iniFilePath); //## READ-ONLY ##//

    //[Misc]
    FileWritePrivateProfileInt("Misc", "AutoStart", model.AutoStart, iniFilePath);
    FileWritePrivateProfileInt("Misc", "CartAutoStart", model.CartAutoStart, iniFilePath);
    FileWritePrivateProfileInt("Misc", "KeyMapIndex", model.KeyMapIndex, iniFilePath);

    //[Module]
    WritePrivateProfileString("Module", "ModulePath", model.ModulePath, iniFilePath);

    //[LeftJoyStick]
    FileWritePrivateProfileInt("LeftJoyStick", "UseMouse", model.Left.UseMouse, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Left", model.Left.Left, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Right", model.Left.Right, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Up", model.Left.Up, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Down", model.Left.Down, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Fire1", model.Left.Fire1, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Fire2", model.Left.Fire2, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "DiDevice", model.Left.DiDevice, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "HiResDevice", model.Left.HiRes, iniFilePath);

    //[RightJoyStick]
    FileWritePrivateProfileInt("RightJoyStick", "UseMouse", model.Right.UseMouse, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Left", model.Right.Left, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Right", model.Right.Right, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Up", model.Right.Up, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Down", model.Right.Down, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Fire1", model.Right.Fire1, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Fire2", model.Right.Fire2, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "DiDevice", model.Right.DiDevice, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "HiResDevice", model.Right.HiRes, iniFilePath);

    //[DefaultPaths]
    WritePrivateProfileString("DefaultPaths", "CassPath", model.CassPath, iniFilePath);
    WritePrivateProfileString("DefaultPaths", "PakPath", model.PakPath, iniFilePath);
    WritePrivateProfileString("DefaultPaths", "FloppyPath", model.FloppyPath, iniFilePath);
    //WritePrivateProfileString("DefaultPaths", "CoCoRomPath", model.CoCoRomPath, iniFilePath); //## READ-ONLY ##//
    WritePrivateProfileString("DefaultPaths", "SerialCaptureFilePath", model.SerialCaptureFilePath, iniFilePath);

    //--Flush .ini file
    WritePrivateProfileString(NULL, NULL, NULL, iniFilePath);
  }
}

extern "C" {
  __declspec(dllexport) ConfigModel __cdecl LoadConfiguration(char* iniFilePath) {
    ConfigModel model = ConfigModel();

    //[Version]
    //GetPrivateProfileString("Version", "Release", "", model.Release, MAX_LOADSTRING, iniFilePath);  //## Write-only ##//

    //[CPU]
    model.CPUMultiplier = GetPrivateProfileInt("CPU", "CPUMultiplier", 2, iniFilePath);
    model.FrameSkip = GetPrivateProfileInt("CPU", "FrameSkip", 1, iniFilePath);
    model.SpeedThrottle = GetPrivateProfileInt("CPU", "SpeedThrottle", 1, iniFilePath);
    model.CpuType = GetPrivateProfileInt("CPU", "CpuType", 0, iniFilePath);
    model.MaxOverclock = GetPrivateProfileInt("CPU", "MaxOverClock", 227, iniFilePath);

    //[Audio]
    model.AudioRate = GetPrivateProfileInt("Audio", "AudioRate", 3, iniFilePath);
    GetPrivateProfileString("Audio", "SoundCardName", "", model.SoundCardName, MAX_LOADSTRING, iniFilePath);

    //[Video]
    model.MonitorType = GetPrivateProfileInt("Video", "MonitorType", 1, iniFilePath);
    model.PaletteType = GetPrivateProfileInt("Video", "PaletteType", 1, iniFilePath);
    model.ScanLines = GetPrivateProfileInt("Video", "ScanLines", 0, iniFilePath);
    model.ForceAspect = GetPrivateProfileInt("Video", "ForceAspect", 0, iniFilePath);
    model.RememberSize = GetPrivateProfileInt("Video", "RememberSize", 0, iniFilePath);
    model.WindowSizeX = GetPrivateProfileInt("Video", "WindowSizeX", 640, iniFilePath);
    model.WindowSizeY = GetPrivateProfileInt("Video", "WindowSizeY", 480, iniFilePath);

    //[Memory]
    model.RamSize = GetPrivateProfileInt("Memory", "RamSize", 1, iniFilePath);
    GetPrivateProfileString("Memory", "ExternalBasicImage", "", model.ExternalBasicImage, MAX_PATH, iniFilePath);

    //[Misc]
    model.AutoStart = GetPrivateProfileInt("Misc", "AutoStart", 1, iniFilePath);
    model.CartAutoStart = GetPrivateProfileInt("Misc", "CartAutoStart", 1, iniFilePath);
    model.KeyMapIndex = GetPrivateProfileInt("Misc", "KeyMapIndex", 0, iniFilePath);

    //[Module]
    GetPrivateProfileString("Module", "ModulePath", "", model.ModulePath, MAX_PATH, iniFilePath);

    //[LeftJoyStick]
    model.Left.UseMouse = GetPrivateProfileInt("LeftJoyStick", "UseMouse", 1, iniFilePath);
    model.Left.Left = GetPrivateProfileInt("LeftJoyStick", "Left", 75, iniFilePath);
    model.Left.Right = GetPrivateProfileInt("LeftJoyStick", "Right", 77, iniFilePath);
    model.Left.Up = GetPrivateProfileInt("LeftJoyStick", "Up", 72, iniFilePath);
    model.Left.Down = GetPrivateProfileInt("LeftJoyStick", "Down", 80, iniFilePath);
    model.Left.Fire1 = GetPrivateProfileInt("LeftJoyStick", "Fire1", 59, iniFilePath);
    model.Left.Fire2 = GetPrivateProfileInt("LeftJoyStick", "Fire2", 60, iniFilePath);
    model.Left.DiDevice = GetPrivateProfileInt("LeftJoyStick", "DiDevice", 0, iniFilePath);
    model.Left.HiRes = GetPrivateProfileInt("LeftJoyStick", "HiResDevice", 0, iniFilePath);

    //[RightJoyStick]
    model.Right.UseMouse = GetPrivateProfileInt("RightJoyStick", "UseMouse", 1, iniFilePath);
    model.Right.Left = GetPrivateProfileInt("RightJoyStick", "Left", 75, iniFilePath);
    model.Right.Right = GetPrivateProfileInt("RightJoyStick", "Right", 77, iniFilePath);
    model.Right.Up = GetPrivateProfileInt("RightJoyStick", "Up", 72, iniFilePath);
    model.Right.Down = GetPrivateProfileInt("RightJoyStick", "Down", 80, iniFilePath);
    model.Right.Fire1 = GetPrivateProfileInt("RightJoyStick", "Fire1", 59, iniFilePath);
    model.Right.Fire2 = GetPrivateProfileInt("RightJoyStick", "Fire2", 60, iniFilePath);
    model.Right.DiDevice = GetPrivateProfileInt("RightJoyStick", "DiDevice", 0, iniFilePath);
    model.Right.HiRes = GetPrivateProfileInt("RightJoyStick", "HiResDevice", 0, iniFilePath);

    //[DefaultPaths]
    GetPrivateProfileString("DefaultPaths", "CassPath", "", model.CassPath, MAX_PATH, iniFilePath);
    GetPrivateProfileString("DefaultPaths", "FloppyPath", "", model.FloppyPath, MAX_PATH, iniFilePath);
    GetPrivateProfileString("DefaultPaths", "CoCoRomPath", "", model.CoCoRomPath, MAX_PATH, iniFilePath);
    GetPrivateProfileString("DefaultPaths", "SerialCaptureFilePath", "", model.SerialCaptureFilePath, MAX_PATH, iniFilePath);
    GetPrivateProfileString("DefaultPaths", "PakPath", "", model.PakPath, MAX_PATH, iniFilePath);

    return model;
  }
}
