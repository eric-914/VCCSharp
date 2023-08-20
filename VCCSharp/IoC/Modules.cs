using System.Diagnostics;
using VCCSharp.Configuration;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules;
using VCCSharp.Modules.TCC1014;

namespace VCCSharp.IoC;

// ReSharper disable InconsistentNaming
public interface IModules
{
    IAudio Audio { get; }
    ICPU CPU { get; }
    ICassette Cassette { get; }
    IClipboard Clipboard { get; }
    ICoCo CoCo { get; }
    IConfiguration Configuration { get; }
    IDraw Draw { get; }
    IEmu Emu { get; }
    IGraphics Graphics { get; }
    IIOBus IOBus { get; }
    IJoysticks Joysticks { get; }
    IKeyboard Keyboard { get; }
    IMC6821 MC6821 { get; }
    IMenuCallbacks MenuCallbacks { get; }
    IPAKInterface PAKInterface { get; }
    IScreen Screen { get; }
    ITCC1014 TCC1014 { get; }
    IThrottle Throttle { get; }
    IVcc Vcc { get; }

    IHD6309 HD6309 { get; }
    IMC6809 MC6809 { get; }

    void Reset();
}
// ReSharper restore InconsistentNaming

public class Modules : IModules
{
    private readonly IFactory _factory;

    public IAudio Audio { get; private set; } = default!;
    public ICPU CPU { get; private set; } = default!;
    public ICassette Cassette { get; private set; } = default!;
    public IClipboard Clipboard { get; private set; } = default!;
    public ICoCo CoCo { get; private set; } = default!;
    public IDraw Draw { get; private set; } = default!;
    public IEmu Emu { get; private set; } = default!;
    public IGraphics Graphics { get; private set; } = default!;
    public IIOBus IOBus { get; private set; } = default!;
    public IJoysticks Joysticks { get; private set; } = default!;
    public IKeyboard Keyboard { get; private set; } = default!;
    public IMC6821 MC6821 { get; private set; } = default!;
    public IMenuCallbacks MenuCallbacks { get; private set; } = default!;
    public IPAKInterface PAKInterface { get; private set; } = default!;
    public IScreen Screen { get; private set; } = default!;
    public ITCC1014 TCC1014 { get; private set; } = default!;
    public IThrottle Throttle { get; private set; } = default!;
    public IVcc Vcc { get; private set; } = default!;

    //TODO: Model is null during Initialize() below.
    public IConfiguration Configuration => _configurationManager.Model;
    private IConfigurationManager _configurationManager = default!;

    public Modules(IFactory factory)
    {
        _factory = factory;
    }

    public void Initialize()
    {
        //TODO: _factory.Get<IConfiguration>() is mapped to ConfigurationRoot.
        _configurationManager = _factory.Get<IConfigurationManager>();

        Audio = _factory.Get<IAudio>();
        CPU = _factory.Get<ICPU>();
        Cassette = _factory.Get<ICassette>();
        Clipboard = _factory.Get<IClipboard>();
        CoCo = _factory.Get<ICoCo>();
        Draw = _factory.Get<IDraw>();
        Emu = _factory.Get<IEmu>();
        Graphics = _factory.Get<IGraphics>();
        IOBus = _factory.Get<IIOBus>();
        Joysticks = _factory.Get<IJoysticks>();
        Keyboard = _factory.Get<IKeyboard>();
        MC6821 = _factory.Get<IMC6821>();
        MenuCallbacks = _factory.Get<IMenuCallbacks>();
        PAKInterface = _factory.Get<IPAKInterface>();
        Screen = _factory.Get<IScreen>();
        TCC1014 = _factory.Get<ITCC1014>();
        Throttle = _factory.Get<IThrottle>();
        Vcc = _factory.Get<IVcc>();
    }

    public IHD6309 HD6309 => _factory.Get<IHD6309>();
    public IMC6809 MC6809 => _factory.Get<IMC6809>();

    /// <summary>
    /// Tell all the modules to do a full reset.  Happens when the configuration is loaded, or a Hard Reset occurs
    /// </summary>
    public void Reset()
    {
        Debug.WriteLine("Modules.Reset()");

        //Audio.ModuleReset();
        //CPU.ModuleReset();
        Cassette.ModuleReset();
        CoCo.ModuleReset();
        Draw.ModuleReset();
        Emu.ModuleReset();
        Graphics.ModuleReset();
        IOBus.ModuleReset();
        Joysticks.ModuleReset();
        Keyboard.ModuleReset();
        MC6821.ModuleReset();
        MenuCallbacks.ModuleReset();
        PAKInterface.ModuleReset();
        TCC1014.ModuleReset();
        Throttle.ModuleReset();
        Vcc.ModuleReset();
    }
}
