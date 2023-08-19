using System.Windows;
using VCCSharp.Configuration.Models;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using static System.IntPtr;
using HWND = System.IntPtr;

namespace VCCSharp.Modules;

public interface IEmu : IModule
{
    HWND WindowHandle { get; set; }

    void SoftReset();
    void HardReset();
    void SetCpuMultiplierFlag(byte doubleSpeed);

    bool FullScreen { get; set; }
    bool ScanLines { get; set; }
    short FrameCounter { get; }
    short LineCounter { get; set; } //--Still used in text modes
    long SurfacePitch { get; set; }
    double CpuCurrentSpeed { get; }

    bool EmulationRunning { get; set; }
    ResetPendingStates ResetPending { get; set; }

    string? StatusLine { get; set; }

    string? PakPath { get; set; }

    Point WindowSize { get; set; }
    void SetWindowSize(IWindowConfiguration window);
}

public class Emu : IEmu
{
    private readonly IModules _modules;
    private readonly IUser32 _user32;

    private IConfiguration Configuration => _modules.Configuration;

    private byte _doubleSpeedFlag;
    private int _doubleSpeedMultiplier = 2;

    public HWND WindowHandle { get; set; }

    public bool FullScreen { get; set; }

    public bool ScanLines { get; set; }

    public short FrameCounter { get; set; }
    public short LineCounter { get; set; }

    public long SurfacePitch { get; set; }

    public string? StatusLine { get; set; }

    public double CpuCurrentSpeed { get; set; } = .894;

    public Point WindowSize { get; set; }

    public bool EmulationRunning { get; set; }
    public ResetPendingStates ResetPending { get; set; } = ResetPendingStates.None;

    public string? PakPath { get; set; }

    public Emu(IModules modules, IUser32 user32)
    {
        _modules = modules;
        _user32 = user32;
    }

    public void SoftReset()
    {
        _modules.TCC1014.ChipReset();
        _modules.MC6821.ChipReset();

        _modules.CPU.ChipReset();

        _modules.Graphics.ResetGraphicsState();
        _modules.Graphics.SetPaletteType();
        _modules.CoCo.CocoReset();
        _modules.Audio.ResetAudio();

        _modules.PAKInterface.ResetBus();
    }

    public void HardReset()
    {
        _modules.TCC1014.ChipReset();  //Captures internal rom pointer for CPU Interrupt Vectors
        _modules.MC6821.ChipReset();
        _modules.CPU.ChipReset();      // Zero all CPU Registers and sets the PC to VRESET

        _modules.Graphics.ResetGraphicsState();
        _modules.Graphics.SetPaletteType();
        _modules.CoCo.CocoReset();
        _modules.Audio.ResetAudio();

        _modules.PAKInterface.UpdateBusPointer();
        _modules.PAKInterface.ResetBus();
    }

    private void SetCpuMultiplier()
    {
        _doubleSpeedMultiplier = Configuration.CPU.CpuMultiplier;

        SetCpuMultiplierFlag(_doubleSpeedFlag);
    }

    public void SetCpuMultiplierFlag(byte doubleSpeed)
    {
        _modules.CoCo.OverClock = 1;

        _doubleSpeedFlag = doubleSpeed;

        if (_doubleSpeedFlag != 0)
        {
            _modules.CoCo.OverClock = _doubleSpeedMultiplier;
        }

        CpuCurrentSpeed = .894;

        if (_doubleSpeedFlag != 0)
        {
            CpuCurrentSpeed *= _doubleSpeedMultiplier;
        }
    }

    public void SetWindowSize(IWindowConfiguration configuration)
    {
        if (configuration.RememberSize)
        {
            SetWindowSize((short)configuration.Width, (short)configuration.Height);
        }
        else
        {
            SetWindowSize(Define.DEFAULT_WIDTH, Define.DEFAULT_HEIGHT);
        }
    }

    private void SetWindowSize(short width, short height)
    {
        IntPtr handle = _user32.GetActiveWindow();

        SetWindowPosFlags flags = SetWindowPosFlags.NoMove | SetWindowPosFlags.NoOwnerZOrder | SetWindowPosFlags.NoZOrder;

        _user32.SetWindowPos(handle, Zero, 0, 0, width + 16, height + 81, (ushort)flags);
    }

    public void ModuleReset()
    {
        HardReset();
        SetCpuMultiplier();
    }
}
