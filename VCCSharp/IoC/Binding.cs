using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.Configuration;
using VCCSharp.Models.Configuration.Support;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Models.Joystick;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;
using VCCSharp.TapePlayer;

namespace VCCSharp.IoC;

internal static class Binding
{
    public static void Initialize(IBinder binder)
    {
        binder
            .SelfBind()

            .WindowsLibraryBind()   //--Windows Libraries
            .DxBind()               //--Bind to DX8 library and the DxManager
            .ConfigurationBind()    //--Configuration
            .ModuleBind()           //--Modules
            
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
            
            .Singleton<IHD6309, HD6309>()
            .Singleton<IMC6809, MC6809>()

            //--Modules container/accessor
            .Singleton<IModules, Modules>()
            
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

            //--Wait until everything is defined before initializing
            .InitializeDxManager()
            .InitializeModules()
            ;
    }
}
