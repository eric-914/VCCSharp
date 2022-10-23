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
    void SetTurboMode(byte data);

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

    public HWND WindowHandle { get; set; }

    public bool FullScreen { get; set; }

    public bool ScanLines { get; set; }

    public short FrameCounter { get; set; }
    public short LineCounter { get; set; }

    public long SurfacePitch { get; set; }

    public string? StatusLine { get; set; }

    public byte DoubleSpeedFlag;
    public int DoubleSpeedMultiplier = 2;

    public double CpuCurrentSpeed { get; set; } = .894;
    public byte TurboSpeedFlag = 1;

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
        _modules.TCC1014.MC6883Reset();
        _modules.MC6821.PiaReset();

        _modules.CPU.Reset();

        GimeReset();

        _modules.TCC1014.MmuReset();
        _modules.TCC1014.CopyRom();
        _modules.PAKInterface.ResetBus();

        TurboSpeedFlag = 1;
    }

    public void HardReset()
    {
        if (!_modules.TCC1014.MmuInit(_configuration.Memory.Ram.Value))
        {
            MessageBox.Show("Can't allocate enough RAM, out of memory", "Error");

            Environment.Exit(0);
        }

        if (_configuration.CPU.Type.Value == CPUTypes.HD6309)
        {
            _modules.CPU.SetHD6309();
        }
        else
        {
            _modules.CPU.SetMC6809();
        }

        _modules.TCC1014.MC6883Reset();  //Captures internal rom pointer for CPU Interrupt Vectors
        _modules.MC6821.PiaReset();

        _modules.CPU.Init();
        _modules.CPU.Reset();    // Zero all CPU Registers and sets the PC to VRESET

        GimeReset();

        _modules.PAKInterface.UpdateBusPointer();

        TurboSpeedFlag = 1;

        _modules.PAKInterface.ResetBus();

        _modules.CoCo.OverClock = 1;
    }

    private void GimeReset()
    {
        _modules.Graphics.ResetGraphicsState();

        _modules.Graphics.SetPaletteType();

        _modules.CoCo.CocoReset();

        _modules.Audio.ResetAudio();
    }

    public void SetCpuMultiplier()
    {
        DoubleSpeedMultiplier = _configuration.CPU.CpuMultiplier;

        SetCpuMultiplierFlag(DoubleSpeedFlag);
    }

    public void SetCpuMultiplierFlag(byte doubleSpeed)
    {
        _modules.CoCo.OverClock = 1;

        DoubleSpeedFlag = doubleSpeed;

        if (DoubleSpeedFlag != 0)
        {
            _modules.CoCo.OverClock = DoubleSpeedMultiplier * TurboSpeedFlag;
        }

        CpuCurrentSpeed = .894;

        if (DoubleSpeedFlag != 0)
        {
            CpuCurrentSpeed *= (DoubleSpeedMultiplier * (double)TurboSpeedFlag);
        }
    }

    public void SetTurboMode(byte data)
    {
        _modules.CoCo.OverClock = 1;

        TurboSpeedFlag = (byte)((data & 1) + 1);

        if (DoubleSpeedFlag != 0)
        {
            _modules.CoCo.OverClock = DoubleSpeedMultiplier * TurboSpeedFlag;
        }

        CpuCurrentSpeed = .894;

        if (DoubleSpeedFlag != 0)
        {
            CpuCurrentSpeed *= DoubleSpeedMultiplier * (double)TurboSpeedFlag;
        }
    }

    public void Reset()
    {
        HardReset();
        SetCpuMultiplier();
    }
}
