using DX8;
using System.Windows;
using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.DX8;
using VCCSharp.Libraries;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Models.Joystick;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;
using VCCSharp.Shared.Dx;
using VCCSharp.Shared.Threading;
using VCCSharp.TapePlayer;

namespace VCCSharp.IoC;

internal static class Binding
{
    public static void Initialize(IFactory factory)
    {
        factory
            .SelfBind()

            //--Specialized Factories
            .Singleton<IViewModelFactory, ViewModelFactory>()

            //--Utilities
            .Singleton<IKeyScanMapper, KeyScanMapper>()
            .Singleton<IMainWindowEvents, MainWindowEvents>()
            .Singleton<IConfigurationPersistence, ConfigurationPersistence>()
            .Singleton<IConfiguration, ConfigurationRoot>()
            .Singleton<IConfigurationPersistenceManager, ConfigurationPersistenceManager>()

            //--Models
            .Bind<IKeyboardAsJoystick, KeyboardAsJoystick>()

            //--Modules
            .ModuleBind()

            .Singleton<IHD6309, HD6309>()
            .Singleton<IMC6809, MC6809>()

            //--Modules container/accessor
            .Singleton<IModules, Modules>()

            //--Windows Libraries
            .Bind<IKernel, Kernel>()
            .Bind<IUser32, User32>()
            .Bind<IGdi32, Gdi32>()
            .Bind<IWinmm, Winmm>()

            //--Bind to DX8 library
            .Bind<IDxDraw, Dx>()
            .Bind<IDxSound, Dx>()
            .Bind<IDxInput, Dx>()

            .Bind<IDispatcher>(() => new DispatcherWrapper(Application.Current.Dispatcher))
            .Bind<IThreadRunner, ThreadRunner>()
            .Singleton<IDxManager, DxManager>()

            //--Main
            .Bind<ICommandLineParser, CommandLineParser>()
            .Bind<IVccApp, VccApp>()
            .Bind<IVccThread, VccThread>()
            .Bind<IVccMainWindow, VccMainWindow>()

            //--Menu
            .Singleton<IMainMenu, MainMenu>()

            //--Options
            .Bind<ICartridge, MenuManager>()
            .Bind<IConfigurationWindowManager, ConfigurationWindowManager>()
            .Bind<ITapePlayer, TapePlayerManager>()
            .Bind<IBitBanger, BitBangerManager>()

            //--Status Bar
            .Singleton<IStatus, StatusViewModel>()

            //--Options container/accessor
            .Singleton<IOptions, Options>()

            .InitializeDxManager()
            .InitializeModules()
            ;

    }
}
