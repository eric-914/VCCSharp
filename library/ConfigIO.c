#include "ConfigModel.h"

#include "fileoperations.h"

extern "C" {
  __declspec(dllexport) void __cdecl SaveConfiguration(ConfigModel* model, char* iniFilePath) {
    //[Version]
    WritePrivateProfileString("Version", "Release", model->Release, iniFilePath); //## Write-only ##//

    //[CPU]
    FileWritePrivateProfileInt("CPU", "CPUMultiplier", model->CPUMultiplier, iniFilePath);
    FileWritePrivateProfileInt("CPU", "FrameSkip", model->FrameSkip, iniFilePath);
    FileWritePrivateProfileInt("CPU", "SpeedThrottle", model->SpeedThrottle, iniFilePath);
    FileWritePrivateProfileInt("CPU", "CpuType", model->CpuType, iniFilePath);
    FileWritePrivateProfileInt("CPU", "MaxOverClock", model->MaxOverclock, iniFilePath);

    //[Audio]
    WritePrivateProfileString("Audio", "SoundCardName", model->SoundCardName, iniFilePath);
    FileWritePrivateProfileInt("Audio", "AudioRate", model->AudioRate, iniFilePath);

    //[Video]
    FileWritePrivateProfileInt("Video", "MonitorType", model->MonitorType, iniFilePath);
    FileWritePrivateProfileInt("Video", "PaletteType", model->PaletteType, iniFilePath);
    FileWritePrivateProfileInt("Video", "ScanLines", model->ScanLines, iniFilePath);
    FileWritePrivateProfileInt("Video", "ForceAspect", model->ForceAspect, iniFilePath);
    FileWritePrivateProfileInt("Video", "RememberSize", model->RememberSize, iniFilePath);
    FileWritePrivateProfileInt("Video", "WindowSizeX", model->WindowSizeX, iniFilePath);
    FileWritePrivateProfileInt("Video", "WindowSizeY", model->WindowSizeY, iniFilePath);

    //[Memory]
    FileWritePrivateProfileInt("Memory", "RamSize", model->RamSize, iniFilePath);
    //WritePrivateProfileString("Memory", "ExternalBasicImage", model->ExternalBasicImage, iniFilePath); //## READ-ONLY ##//

    //[Misc]
    FileWritePrivateProfileInt("Misc", "AutoStart", model->AutoStart, iniFilePath);
    FileWritePrivateProfileInt("Misc", "CartAutoStart", model->CartAutoStart, iniFilePath);
    FileWritePrivateProfileInt("Misc", "KeyMapIndex", model->KeyMapIndex, iniFilePath);

    //[Module]
    WritePrivateProfileString("Module", "ModulePath", model->ModulePath, iniFilePath);

    //[LeftJoyStick]
    FileWritePrivateProfileInt("LeftJoyStick", "UseMouse", model->Left->UseMouse, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Left", model->Left->Left, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Right", model->Left->Right, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Up", model->Left->Up, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Down", model->Left->Down, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Fire1", model->Left->Fire1, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "Fire2", model->Left->Fire2, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "DiDevice", model->Left->DiDevice, iniFilePath);
    FileWritePrivateProfileInt("LeftJoyStick", "HiResDevice", model->Left->HiRes, iniFilePath);

    //[RightJoyStick]
    FileWritePrivateProfileInt("RightJoyStick", "UseMouse", model->Right->UseMouse, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Left", model->Right->Left, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Right", model->Right->Right, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Up", model->Right->Up, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Down", model->Right->Down, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Fire1", model->Right->Fire1, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "Fire2", model->Right->Fire2, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "DiDevice", model->Right->DiDevice, iniFilePath);
    FileWritePrivateProfileInt("RightJoyStick", "HiResDevice", model->Right->HiRes, iniFilePath);

    //[DefaultPaths]
    WritePrivateProfileString("DefaultPaths", "CassPath", model->CassPath, iniFilePath);
    WritePrivateProfileString("DefaultPaths", "PakPath", model->PakPath, iniFilePath);
    WritePrivateProfileString("DefaultPaths", "FloppyPath", model->FloppyPath, iniFilePath);
    //WritePrivateProfileString("DefaultPaths", "CoCoRomPath", model->CoCoRomPath, iniFilePath); //## READ-ONLY ##//
    WritePrivateProfileString("DefaultPaths", "SerialCaptureFilePath", model->SerialCaptureFilePath, iniFilePath);

    //--Flush .ini file
    WritePrivateProfileString(NULL, NULL, NULL, iniFilePath);
  }
}
