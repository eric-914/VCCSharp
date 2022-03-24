using System;
using System.Diagnostics;
using System.IO;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using static System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules;

public class ConfigurationModule : IConfigurationModule
{
    private readonly IModules _modules;
    private readonly IUser32 _user32;
    private readonly IConfigurationModulePersistence _persistence;

    private string? _filePath;

    public IConfigurationRoot Model { get; private set; } = default!;

    public ConfigurationModule(IModules modules, IUser32 user32, IConfigurationModulePersistence persistence)
    {
        _modules = modules;
        _user32 = user32;
        _persistence = persistence;
    }

    public string? GetFilePath() => _filePath;

    public void Load(string filePath)
    {
        _filePath = GetConfigurationFilePath(filePath);  //--Use default if needed

        Load();

        SynchSystemWithConfig();

        ConfigureJoysticks();
        
        if (_persistence.IsNew(_filePath))
        {
            Save();
        }
    }

    // LoadFrom allows user to browse for an ini file and reloads the configurationModule from it.
    public void LoadFrom() => _persistence.LoadFrom(_filePath, LoadFrom);

    private void LoadFrom(string filePath)
    {
        Save();

        _filePath = filePath;

        Load();

        SynchSystemWithConfig();
    }

    public void Load()
    {
        Model = _persistence.Load(_filePath);

        if (!string.IsNullOrEmpty(_modules.Emu.PakPath))
        {
            Model.Accessories.MultiPak.FilePath = _modules.Emu.PakPath;
        }

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
        _persistence.SaveAs(_filePath, SaveAs);
    }

    private void SaveAs(string filePath)
    {
        _filePath = filePath;
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

        _persistence.Save(_filePath, Model);
    }

    private void ConfigureJoysticks()
    {
        var joystick = _modules.Joysticks;

        var joysticks = joystick.FindJoysticks();

        if (Model.Joysticks.Left.DeviceIndex >= joysticks.Count)
        {
            Model.Joysticks.Left.DeviceIndex = 0;
        }

        if (Model.Joysticks.Right.DeviceIndex >= joysticks.Count)
        {
            Model.Joysticks.Right.DeviceIndex = 0;
        }

        joystick.SetStickNumbers(Model.Joysticks.Left.DeviceIndex, Model.Joysticks.Right.DeviceIndex);

        if (joysticks.Count == 0)	//Use Mouse input if no Joysticks present
        {
            if (Model.Joysticks.Left.InputSource.Value == JoystickDevices.Joystick)
            {
                Model.Joysticks.Left.InputSource.Value = JoystickDevices.Mouse;
            }

            if (Model.Joysticks.Right.InputSource.Value == JoystickDevices.Joystick)
            {
                Model.Joysticks.Left.InputSource.Value = JoystickDevices.Mouse;
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

    private void SetWindowSize(short width, short height)
    {
        HWND handle = _user32.GetActiveWindow();

        SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

        _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
    }

    public void SynchSystemWithConfig()
    {
        _modules.Emu.SetCpuMultiplier();

        _modules.Graphics.SetMonitorType();
        _modules.Graphics.SetPaletteType();
        _modules.Graphics.SetScanLines();

        _modules.Draw.SetAspect();

        _modules.MC6821.SetCartAutoStart();

        //--Synch joysticks to configurationModule instance
        _modules.Joysticks.SetLeftJoystick();
        _modules.Joysticks.SetRightJoystick();

        _modules.Keyboard.KeyboardBuildRuntimeTable();

        _modules.PAKInterface.InsertModule();   // Should this be here?

        _modules.Audio.SoundInit(Model);
    }
}
