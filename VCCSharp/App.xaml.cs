using System.Windows;
using VCCSharp.IoC;
using VCCSharp.Libraries;
using VCCSharp.Modules;

namespace VCCSharp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Factory.Instance
                .SelfBind()
                .Singleton<IAudio, Audio>()
                .Singleton<ICoCo, CoCo>()
                .Singleton<IConfig, Config>()
                .Singleton<IDirectDraw, DirectDraw>()
                .Singleton<IEmu, Emu>()
                .Singleton<IMenuCallbacks, MenuCallbacks>()
                .Singleton<IQuickLoad, QuickLoad>()
                .Singleton<IPAKInterface, PAKInterface>()
                .Singleton<IVcc, Vcc>()
                .Singleton<IModules, IoC.Modules>()

                .Bind<IKernel, Kernel>()
                .Bind<IUser32, User32>()

                .Bind<ICommandLineParser, CommandLineParser>()
                .Bind<IVccApp, VccApp>()
                .Bind<IVccThread, VccThread>()
                ;
        }
    }
}
