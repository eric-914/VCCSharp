using VCCSharp.Configuration;
using VCCSharp.IoC;
using VCCSharp.Modules.TCC1014;

namespace VCCSharp.Modules;

internal static class ModuleBinding
{
    public static void Bind(IBinder  binder)
    {
        binder
            .Singleton<IAudio, Audio>()
            .Singleton<ICPU, CPU>()
            .Singleton<ICassette, Cassette>()
            .Singleton<IClipboard, Clipboard>()
            .Singleton<ICoCo, CoCo>()
            .Singleton<IConfigurationManager, ConfigurationManager>()
            .Singleton<IDraw, Draw>()
            .Singleton<IEmu, Emu>()
            .Singleton<IEvents, Events>()
            .Singleton<IGraphics, Graphics>()
            .Singleton<IIOBus, IOBus>()
            .Singleton<IJoysticks, Joysticks>()
            .Singleton<IKeyboard, Keyboard>()
            .Singleton<IMC6821, MC6821>()
            .Singleton<IMenuCallbacks, MenuCallbacks>()
            .Singleton<IPAKInterface, PAKInterface>()
            .Singleton<IQuickLoad, QuickLoad>()
            .Singleton<ITCC1014, TCC1014.TCC1014>()
            .Singleton<IThrottle, Throttle>()
            .Singleton<IVcc, Vcc>()

            //TODO: _factory.Get<IConfiguration>() is mapped to ConfigurationRoot.
            //.Singleton(binder => binder.Get<IConfigurationManager>().Model)  //--IConfiguration
            ;
    }

    public static void Initialize(IFactory factory)
    {
        ((IoC.Modules)factory.Get<IModules>())
            .Initialize();
    }
}
