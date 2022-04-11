using System.Diagnostics;
using VCCSharp.BitBanger;
using VCCSharp.Configuration;
using VCCSharp.Main;
using VCCSharp.Menu;
using VCCSharp.Models.CPU.HD6309;
using VCCSharp.Models.CPU.MC6809;
using VCCSharp.Models.Joystick;
using VCCSharp.Models.Keyboard;
using VCCSharp.Modules;
using VCCSharp.TapePlayer;

namespace VCCSharp.IoC;

internal static class Binding
{
    /// <summary>
    /// Bind up everything to the IoC/Dependency Inject container (via Factory)
    /// </summary>
    /// <param name="binder">Factory Binding accessor</param>
    public static void Bind(IBinder binder)
    {
        var watch = Stopwatch.StartNew();

        binder
            .BindFactory()  //--Make the factory itself injectable

            //--These other binders help keep things more focused.
            .Include(WindowsLibraryBinding.Bind)     //--Windows Libraries
            .Include(DxBinding.Bind)                 //--Bind to DX8 library and the DxManager
            .Include(ConfigurationBinding.Bind)      //--Configuration
            .Include(ModuleBinding.Bind)             //--Modules
            
            //--Specialized Factories
            .Singleton<IViewModelFactory, ViewModelFactory>()

            //--Utilities
            .Singleton<IKeyScanMapper, KeyScanMapper>()
            .Singleton<IMainWindowEvents, MainWindowEvents>()

            //--Models
            .Bind<IKeyboardAsJoystick, KeyboardAsJoystick>()

            //--CPU's
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
            .Initialize(DxBinding.Initialize)
            .Initialize(ModuleBinding.Initialize)
            ;

        watch.Stop();
        Debug.WriteLine($"Binding finished. {watch.ElapsedMilliseconds}ms");
    }
}
