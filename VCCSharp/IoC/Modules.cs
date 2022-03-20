﻿using VCCSharp.Models.Configuration;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules;
using VCCSharp.Modules.TC1014;

namespace VCCSharp.IoC;

// ReSharper disable InconsistentNaming
public interface IModules
{
    IAudio Audio { get; }
    ICPU CPU { get; }
    ICassette Cassette { get; }
    IClipboard Clipboard { get; }
    ICoCo CoCo { get; }
    IConfig Config { get; }
    IConfiguration Configuration { get; }
    IDraw Draw { get; }
    IEmu Emu { get; }
    IEvents Events { get; }
    IGraphics Graphics { get; }
    IIOBus IOBus { get; }
    IJoystick Joystick { get; }
    IKeyboard Keyboard { get; }
    IMC6821 MC6821 { get; }
    IMenuCallbacks MenuCallbacks { get; }
    IPAKInterface PAKInterface { get; }
    IQuickLoad QuickLoad { get; }
    ITC1014 TC1014 { get; }
    IThrottle Throttle { get; }
    IVcc Vcc { get; }

    IHD6309 HD6309 { get; }
    IMC6809 MC6809 { get; }
}
// ReSharper restore InconsistentNaming

public class Modules : IModules
{
    private readonly IFactory _factory;

    public IAudio Audio { get; private set; }= default!;
    public ICPU CPU { get; private set; }= default!;
    public ICassette Cassette { get; private set; }= default!;
    public IClipboard Clipboard { get; private set; }= default!;
    public ICoCo CoCo { get; private set; }= default!;
    public IConfig Config { get; private set; }= default!;
    public IConfiguration Configuration => Config.Model;
    public IDraw Draw { get; private set; }= default!;
    public IEmu Emu { get; private set; }= default!;
    public IEvents Events { get; private set; }= default!;
    public IGraphics Graphics { get; private set; }= default!;
    public IIOBus IOBus { get; private set; }= default!;
    public IJoystick Joystick { get; private set; }= default!;
    public IKeyboard Keyboard { get; private set; }= default!;
    public IMC6821 MC6821 { get; private set; }= default!;
    public IMenuCallbacks MenuCallbacks { get; private set; }= default!;
    public IPAKInterface PAKInterface { get; private set; }= default!;
    public IQuickLoad QuickLoad { get; private set; }= default!;
    public ITC1014 TC1014 { get; private set; }= default!;
    public IThrottle Throttle { get; private set; }= default!;
    public IVcc Vcc { get; private set; } = default!;

    public Modules(IFactory factory)
    {
        _factory = factory;
    }

    public void Initialize()
    {
        Audio = _factory.Get<IAudio>();
        CPU = _factory.Get<ICPU>();
        Cassette = _factory.Get<ICassette>();
        Clipboard = _factory.Get<IClipboard>();
        CoCo = _factory.Get<ICoCo>();
        Config = _factory.Get<IConfig>();
        Draw = _factory.Get<IDraw>();
        Emu = _factory.Get<IEmu>();
        Events = _factory.Get<IEvents>();
        Graphics = _factory.Get<IGraphics>();
        IOBus = _factory.Get<IIOBus>();
        Joystick = _factory.Get<IJoystick>();
        Keyboard = _factory.Get<IKeyboard>();
        MC6821 = _factory.Get<IMC6821>();
        MenuCallbacks = _factory.Get<IMenuCallbacks>();
        PAKInterface = _factory.Get<IPAKInterface>();
        QuickLoad = _factory.Get<IQuickLoad>();
        TC1014 = _factory.Get<ITC1014>();
        Throttle = _factory.Get<IThrottle>();
        Vcc = _factory.Get<IVcc>();
    }

    public IHD6309 HD6309 => _factory.Get<IHD6309>();
    public IMC6809 MC6809 => _factory.Get<IMC6809>();
}