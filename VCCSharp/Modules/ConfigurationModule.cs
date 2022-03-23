using System;
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

namespace VCCSharp.Modules;

public class ConfigurationModule : IConfigurationModule
{
    private readonly IModules _modules;
    private readonly IUser32 _user32;
    private readonly IConfigurationModulePersistence _persistence;

    private readonly JoystickModel _left = new();
    private readonly JoystickModel _right = new();

    private string? _filePath;

    public int TapeCounter { get; set; }
    public TapeModes TapeMode { get; set; } = TapeModes.Stop;

    public string? TapeFileName { get; set; }

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

        //--Synch joysticks to configurationModule instance
        _modules.Joystick.SetLeftJoystick(_left);
        _modules.Joystick.SetRightJoystick(_right);

        SynchSystemWithConfig();

        ConfigureJoysticks();

        string device = Model.Audio.Device;
        int deviceIndex = _modules.Audio.FindSoundDevices().IndexOf(device);

        _modules.Audio.SoundInit(_modules.Emu.WindowHandle, deviceIndex, Model.Audio.Rate.Value);

        if (_persistence.IsNew(_filePath))
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

    // LoadFrom allows user to browse for an ini file and reloads the configurationModule from it.
    public void LoadFrom() => _persistence.LoadFrom(_filePath, LoadFrom);

    private void LoadFrom(string filePath)
    {
        Save();

        _filePath = filePath;

        Load();

        SynchSystemWithConfig();

        _modules.Emu.ResetPending = ResetPendingStates.Hard;
    }

    public void Load()
    {
        Model = _persistence.Load(_filePath);

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
            if (Model.Joysticks.Left.InputSource.Value == JoystickDevices.Joystick)
            {
                _left.InputSource = JoystickDevices.Mouse;
            }

            if (Model.Joysticks.Right.InputSource.Value == JoystickDevices.Joystick)
            {
                _right.InputSource = JoystickDevices.Mouse;
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

    private void SetCpuType(CPUTypes cpuType)
    {
        var cpu = new Dictionary<CPUTypes, string>
        {
            {CPUTypes.MC6809, "MC6809"},
            {CPUTypes.HD6309, "HD6309"}
        };

        _modules.Emu.CpuType = cpuType;
        _modules.Vcc.CpuName = cpu[cpuType];
    }

    private void SetWindowSize(short width, short height)
    {
        HWND handle = _user32.GetActiveWindow();

        SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

        _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
    }
}
