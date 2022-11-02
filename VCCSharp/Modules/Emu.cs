using System.Windows;
using VCCSharp.Enums;
using VCCSharp.IoC;
using VCCSharp.Models.Configuration;
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
    short FrameCounter { get; set; }
    short LineCounter { get; set; } //--Still used in text modes
    long SurfacePitch { get; set; }
    double CpuCurrentSpeed { get; set; }

    bool EmulationRunning { get; set; }
    ResetPendingStates ResetPending { get; set; }

    string? StatusLine { get; set; }

    string? PakPath { get; set; }

    Point WindowSize { get; set; }
}

public class Emu : IEmu
{
    private readonly IModules _modules;
    private readonly IConfiguration _configuration;

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

    public Emu(IModules modules, IConfiguration configuration)
    {
        _modules = modules;
        _configuration = configuration;
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
        _modules.TCC1014.MmuInit(_configuration.Memory.Ram.Value);
        _modules.TCC1014.ChipReset();  //Captures internal rom pointer for CPU Interrupt Vectors
        _modules.MC6821.ChipReset();

        _modules.CPU.Init(_configuration.CPU.Type.Value);
        _modules.CPU.ChipReset();   // Zero all CPU Registers and sets the PC to VRESET

        _modules.Graphics.ResetGraphicsState();
        _modules.Graphics.SetPaletteType();
        _modules.CoCo.CocoReset();
        _modules.Audio.ResetAudio();

        _modules.PAKInterface.UpdateBusPointer();
        _modules.PAKInterface.ResetBus();

        _modules.CoCo.OverClock = 1;
    }

    private void SetCpuMultiplier()
    {
        _doubleSpeedMultiplier = _configuration.CPU.CpuMultiplier;

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

    public void ModuleReset()
    {
        HardReset();
        SetCpuMultiplier();
    }
}
