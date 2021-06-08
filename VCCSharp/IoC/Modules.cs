using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules;

namespace VCCSharp.IoC
{
    // ReSharper disable InconsistentNaming
    public interface IModules
    {
        IAudio Audio { get; }
        ICPU CPU { get; }
        ICassette Cassette { get; }
        IClipboard Clipboard { get; }
        ICoCo CoCo { get; }
        IConfig Config { get; }
        IDirectDraw DirectDraw { get; }
        IDirectSound DirectSound { get; }
        IEmu Emu { get; }
        IEvents Events { get; }
        IFileOperations FileOperations { get; }
        IGDI GDI { get; }
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

        public IAudio Audio { get; private set; }
        public ICPU CPU { get; private set; }
        public ICassette Cassette { get; private set; }
        public IClipboard Clipboard { get; private set; }
        public ICoCo CoCo { get; private set; }
        public IConfig Config { get; private set; }
        public IDirectDraw DirectDraw { get; private set; }
        public IDirectSound DirectSound { get; private set; }
        public IEmu Emu { get; private set; }
        public IEvents Events { get; private set; }
        public IFileOperations FileOperations { get; private set; }
        public IGDI GDI { get; private set; }
        public IGraphics Graphics { get; private set; }
        public IIOBus IOBus { get; private set; }
        public IJoystick Joystick { get; private set; }
        public IKeyboard Keyboard { get; private set; }
        public IMC6821 MC6821 { get; private set; }
        public IMenuCallbacks MenuCallbacks { get; private set; }
        public IPAKInterface PAKInterface { get; private set; }
        public IQuickLoad QuickLoad { get; private set; }
        public ITC1014 TC1014 { get; private set; }
        public IThrottle Throttle { get; private set; }
        public IVcc Vcc { get; private set; }

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
            DirectDraw = _factory.Get<IDirectDraw>();
            DirectSound = _factory.Get<IDirectSound>();
            Emu = _factory.Get<IEmu>();
            Events = _factory.Get<IEvents>();
            FileOperations = _factory.Get<IFileOperations>();
            GDI = _factory.Get<IGDI>();
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
}
