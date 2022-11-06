using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models.Configuration;

namespace VCCSharp;

public interface IVccApp
{
    void LoadConfiguration(string? iniFile);
    void Startup(string? qLoadFile);
    void RunThreaded();

    void SetWindow(IntPtr hWnd);
}

public class VccApp : IVccApp
{
    private readonly IModules _modules;
    private readonly IUser32 _user32;

    private IConfiguration _configuration = default!;

    public VccApp(IModules modules, IUser32 user32)
    {
        _modules = modules;
        _user32 = user32;
    }

    public void LoadConfiguration(string? iniFile)
    {
        if (iniFile == null) throw new ArgumentNullException(nameof(iniFile));

        _modules.ConfigurationManager.Load(iniFile);

        _configuration = _modules.ConfigurationManager.Model;
    }

    public void Startup(string? qLoadFile)
    {
        _modules.Reset();

        _modules.CoCo.SetAudioEventAudioOut();

        _modules.CoCo.OverClock = 1;  //Default clock speed .89 MHZ	

        _modules.Vcc.CreatePrimaryWindow();

        _modules.Draw.ClearScreen();

        _modules.Emu.ResetPending = ResetPendingStates.Cls;

        _modules.MenuCallbacks.RefreshCartridgeMenu();

        _modules.Emu.ResetPending = ResetPendingStates.Hard;

        _modules.Emu.EmulationRunning = _configuration.Startup.AutoStart;

        _modules.Vcc.BinaryRunning = true;

        _modules.Throttle.CalibrateThrottle();

        if (!string.IsNullOrEmpty(qLoadFile))
        {
            if (_modules.QuickLoad.QuickStart(qLoadFile) == (int)QuickStartStatuses.Ok)
            {
                _modules.Vcc.SetAppTitle(qLoadFile); //TODO: No app title if no quick load
            }

            _modules.Emu.EmulationRunning = true;
        }
    }

    public void RunThreaded()
    {
        Task.Run(_modules.Vcc.EmuLoop);
    }

    public void SetWindow(IntPtr hWnd)
    {
        _modules.Emu.WindowHandle = hWnd;
    }
}
