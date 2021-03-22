using VCCSharp.Modules;

namespace VCCSharp.IoC
{
    public interface IModules
    {
        IAudio Audio { get; }
        ICassette Cassette { get; }
        IClipboard Clipboard { get; }
        ICoCo CoCo { get; }
        IConfig Config { get; }
        ICPU CPU { get; }
        IDirectDraw DirectDraw { get; }
        IDirectSound DirectSound { get; }
        IEmu Emu { get; }
        IEvents Events { get; }
        IGraphics Graphics { get; }
        IJoystick Joystick { get; }
        IKeyboard Keyboard { get; }
        IMenuCallbacks MenuCallbacks { get; }
        IMC6821 MC6821 { get; }
        IQuickLoad QuickLoad { get; }
        IPAKInterface PAKInterface { get; }
        IResource Resource { get; }
        IThrottle Throttle { get; }
        ITC1014 TC1014 { get; }
        IVcc Vcc { get; }
    }

    public class Modules : IModules
    {
        private readonly IFactory _factory;

        public Modules(IFactory factory)
        {
            _factory = factory;
        }

        public IAudio Audio => _factory.Get<IAudio>();
        public ICassette Cassette => _factory.Get<ICassette>();
        public IClipboard Clipboard => _factory.Get<IClipboard>();
        public ICoCo CoCo => _factory.Get<ICoCo>();
        public IConfig Config => _factory.Get<IConfig>();
        public ICPU CPU => _factory.Get<ICPU>();
        public IDirectDraw DirectDraw => _factory.Get<IDirectDraw>();
        public IDirectSound DirectSound => _factory.Get<IDirectSound>();
        public IEmu Emu => _factory.Get<IEmu>();
        public IEvents Events => _factory.Get<IEvents>();
        public IGraphics Graphics => _factory.Get<IGraphics>();
        public IJoystick Joystick => _factory.Get<IJoystick>();
        public IKeyboard Keyboard => _factory.Get<IKeyboard>();
        public IMenuCallbacks MenuCallbacks => _factory.Get<IMenuCallbacks>();
        public IMC6821 MC6821 => _factory.Get<IMC6821>();
        public IQuickLoad QuickLoad => _factory.Get<IQuickLoad>();
        public IPAKInterface PAKInterface => _factory.Get<IPAKInterface>();
        public IResource Resource => _factory.Get<IResource>();
        public IThrottle Throttle => _factory.Get<IThrottle>();
        public ITC1014 TC1014 => _factory.Get<ITC1014>();
        public IVcc Vcc => _factory.Get<IVcc>();
    }
}
