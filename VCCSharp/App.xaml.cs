using System.Windows;
using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.DX8.Libraries;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Menu;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Modules;
using VCCSharp.Modules.TC1014;
using VCCSharp.TapePlayer;

namespace VCCSharp
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //ShutdownMode = ShutdownMode.OnExplicitShutdown;

            Factory.Instance
                .SelfBind()

                //--Modules
                .Singleton<IAudio, Audio>()
                .Singleton<ICPU, CPU>()
                .Singleton<ICassette, Cassette>()
                .Singleton<IClipboard, Modules.Clipboard>()
                .Singleton<ICoCo, CoCo>()
                .Singleton<IConfig, Config>()
                .Singleton<IDraw, Draw>()
                .Singleton<IEmu, Emu>()
                .Singleton<IEvents, Events>()
                .Singleton<IGraphics, Graphics>()
                .Singleton<IIOBus, IOBus>()
                .Singleton<IJoystick, Joystick>()
                .Singleton<IKeyboard, Keyboard>()
                .Singleton<IMC6821, MC6821>()
                .Singleton<IMenuCallbacks, MenuCallbacks>()
                .Singleton<IPAKInterface, PAKInterface>()
                .Singleton<IQuickLoad, QuickLoad>()
                .Singleton<ITC1014, TC1014>()
                .Singleton<IThrottle, Throttle>()
                .Singleton<IVcc, Vcc>()

                .Singleton<IHD6309, HD6309>()
                .Singleton<IMC6809, MC6809>()

                //--Modules container/accessor
                .Singleton<IModules, IoC.Modules>()

                //--Windows Libraries
                .Bind<IKernel, Kernel>()
                .Bind<IUser32, User32>()
                .Bind<IGdi32, Gdi32>()
                .Bind<IWinmm, Winmm>()
                .Bind<IDDraw, DDraw>()
                .Bind<IDSound, DSound>()
                .Bind<IDInput, DInput>()

                //--Main
                .Bind<ICommandLineParser, CommandLineParser>()
                .Bind<IVccApp, VccApp>()
                .Bind<IVccThread, VccThread>()

                //--Menu
                .Singleton<IMainMenu, MainMenu>()

                //--Options
                .Bind<ICartridge, MenuManager>()
                .Bind<IConfiguration, ConfigurationManager>()
                .Bind<ITapePlayer, TapePlayerManager>()
                .Bind<IBitBanger, BitBangerManager>()

                //--Status Bar
                .Singleton<IStatus, StatusManager>()

                //--Options container/accessor
                .Singleton<IOptions, Options>()

                .InitializeModules()
                ;
        }
    }
}
