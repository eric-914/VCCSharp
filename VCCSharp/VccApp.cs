using System.Diagnostics;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
using VCCSharp.Modules;

namespace VCCSharp;

public interface IVccApp
{
    void LoadConfiguration(string? iniFile);
    void Startup(string? qLoadFile);
    void Run();

    void SetWindow(IntPtr hWnd);
}

public class VccApp : IVccApp
{
    private readonly IModules _modules;

    public VccApp(IModules modules)
    {
        _modules = modules;

        //TODO: Not sure this is proper place.  Probably should have each module respond accordingly.
        _modules.ConfigurationManager.OnConfigurationSynch += ConfigurationSynchronize;
    }

    public void LoadConfiguration(string? iniFile)
    {
        Debug.WriteLine("LoadConfiguration(...)");

        if (iniFile == null) throw new ArgumentNullException(nameof(iniFile));

        _modules.ConfigurationManager.Load(iniFile);
    }

    public void Startup(string? qLoadFile)
    {
        Debug.WriteLine("Startup(...)");

        _modules.Reset();

        _modules.CoCo.SetAudioEventAudioOut();

        _modules.CoCo.OverClock = 1;  //Default clock speed .89 MHZ	

        _modules.Vcc.CreatePrimaryWindow();

        _modules.Draw.ClearScreen();

        _modules.Emu.ResetPending = ResetPendingStates.Cls;

        _modules.MenuCallbacks.RefreshCartridgeMenu();

        _modules.Emu.ResetPending = ResetPendingStates.Hard;

        _modules.Emu.EmulationRunning = _modules.ConfigurationManager.Model.Startup.AutoStart;

        _modules.Vcc.BinaryRunning = true;

        _modules.Throttle.CalibrateThrottle();

        if (!string.IsNullOrEmpty(qLoadFile))
        {
            if (_modules.QuickLoad.QuickStart(qLoadFile) == QuickStartStatuses.Ok)
            {
                _modules.Vcc.SetAppTitle(qLoadFile); //TODO: No app title if no quick load
            }

            _modules.Emu.EmulationRunning = true;
        }
    }

    public void Run()
    {
        Debug.WriteLine("Run(...)");

        _modules.Vcc.EmuLoop();
    }

    public void SetWindow(IntPtr hWnd)
    {
        Debug.WriteLine("SetWindow(...)");

        _modules.Emu.WindowHandle = hWnd;
    }

    private void ConfigurationSynchronize(SynchDirection direction, IConfiguration model)
    {
        Debug.WriteLine("ConfigurationSynchronize(...)");

        if (direction == SynchDirection.ConfigurationChanged)
        {
            _modules.Emu.SetWindowSize(model.Window);
            _modules.Joysticks.Configure(model.Joysticks);

            if (!string.IsNullOrEmpty(_modules.Emu.PakPath))
            {
                model.Accessories.MultiPak.FilePath = _modules.Emu.PakPath;
            }
        }
        else //--SynchDirection.SaveConfiguration
        {
            model.Window.Width = (short)_modules.Emu.WindowSize.X;
            model.Window.Height = (short)_modules.Emu.WindowSize.Y;

            string? modulePath = model.Accessories.ModulePath;

            if (string.IsNullOrEmpty(modulePath))
            {
                modulePath = _modules.PAKInterface.GetCurrentModule();
            }

            if (!string.IsNullOrEmpty(modulePath))
            {
                model.Accessories.ModulePath = modulePath;
            }
        }
    }
}
