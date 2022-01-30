using DX8;
using System.Windows;
using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.DX8;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Models.Keyboard;
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

                //--Specialized Factories
                .Singleton<IViewModelFactory, ViewModelFactory>()

                //--Utilities
                .Singleton<IKeyboardScanCodes, KeyboardScanCodes>()
                .Singleton<IKeyScanMapper, KeyScanMapper>()
                .Singleton<IMainWindowEvents, MainWindowEvents>()
                .Singleton<IConfigurationPersistence, ConfigurationPersistence>()
                .Singleton<Models.Configuration.IConfiguration, Root>()

                //--Modules
                .Singleton<IAudio, Modules.Audio>()
                .Singleton<ICPU, Modules.CPU>()
                .Singleton<ICassette, Cassette>()
                .Singleton<IClipboard, Modules.Clipboard>()
                .Singleton<ICoCo, CoCo>()
                .Singleton<IConfig, Config>()
                .Singleton<IDraw, Draw>()
                .Singleton<IEmu, Emu>()
                .Singleton<IEvents, Events>()
                .Singleton<IGraphics, Graphics>()
                .Singleton<IIOBus, IOBus>()
                .Singleton<IJoystick, Modules.Joystick>()
                .Singleton<IKeyboard, Modules.Keyboard>()
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

                //--Bind to DX8 library
                .Bind<IDxDraw, Dx>()
                .Bind<IDxSound, Dx>()
                .Bind<IDxInput, Dx>()

                //--Main
                .Bind<ICommandLineParser, CommandLineParser>()
                .Bind<IVccApp, VccApp>()
                .Bind<IVccThread, VccThread>()
                .Bind<IVccMainWindow, VccMainWindow>()

                //--Menu
                .Singleton<IMainMenu, MainMenu>()

                //--Options
                .Bind<ICartridge, MenuManager>()
                .Bind<Configuration.IConfiguration, ConfigurationManager>()
                .Bind<ITapePlayer, TapePlayerManager>()
                .Bind<IBitBanger, BitBangerManager>()

                //--Status Bar
                .Singleton<IStatus, StatusViewModel>()

                //--Options container/accessor
                .Singleton<IOptions, Options>()

                .InitializeModules()
                ;
        }
    }
}
