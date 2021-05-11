using System.Windows;
using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules;
using VCCSharp.TapePlayer;

namespace VCCSharp
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Factory.Instance
                .SelfBind()

                //--Modules
                .Singleton<IAudio, Audio>()
                .Singleton<ICallbacks, Callbacks>()
                .Singleton<ICassette, Cassette>()
                .Singleton<IClipboard, Modules.Clipboard>()
                .Singleton<ICoCo, CoCo>()
                .Singleton<IConfig, Config>()
                .Singleton<ICPU, CPU>()
                .Singleton<IDirectDraw, DirectDraw>()
                .Singleton<IDirectSound, DirectSound>()
                .Singleton<IEmu, Emu>()
                .Singleton<IEvents, Events>()
                .Singleton<IGDI, GDI>()
                .Singleton<IGraphics, Graphics>()
                .Singleton<IJoystick, Joystick>()
                .Singleton<IKeyboard, Keyboard>()
                .Singleton<IIOBus, IOBus>()
                .Singleton<IMenuCallbacks, MenuCallbacks>()
                .Singleton<IMC6821, MC6821>()
                .Singleton<IQuickLoad, QuickLoad>()
                .Singleton<IPAKInterface, PAKInterface>()
                .Singleton<IThrottle, Throttle>()
                .Singleton<ITC1014, TC1014>()
                .Singleton<IVcc, Vcc>()

                .Singleton<IHD6309, HD6309>()
                .Singleton<IMC6809, MC6809>()

                //--Modules container/accessor
                .Singleton<IModules, IoC.Modules>()

                //--Windows Libraries
                .Bind<IKernel, Kernel>()
                .Bind<IUser32, User32>()
                .Bind<IWinmm, Winmm>()

                //--Main
                .Bind<ICommandLineParser, CommandLineParser>()
                .Bind<IVccApp, VccApp>()
                .Bind<IVccThread, VccThread>()

                //--Options
                .Bind<IConfiguration, ConfigurationManager>()
                .Bind<ITapePlayer, TapePlayerManager>()
                .Bind<IBitBanger, BitBangerManager>()

                //--Options container/accessor
                .Singleton<IOptions, Options>()
                ;
        }
    }
}
