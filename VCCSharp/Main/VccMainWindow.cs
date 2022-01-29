using System;
using System.Windows.Interop;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Models;

namespace VCCSharp.Main
{
    public interface IVccMainWindow
    {
        void Run(IMainWindow window);
    }

    public class VccMainWindow : IVccMainWindow
    {
        private readonly IFactory _factory;
        private readonly IVccThread _thread;
        private readonly ICommandLineParser _commandLineParser;

        public VccMainWindow(IFactory factory, IVccThread thread, ICommandLineParser commandLineParser)
        {
            _factory = factory;
            _thread = thread;
            _commandLineParser = commandLineParser;
        }

        public void Run(IMainWindow window)
        {
            var commands = _factory.Get<MainWindowCommands>();

            window.ViewModel = _factory.Get<IViewModelFactory>().CreateMainWindowViewModel(commands.MenuItems);

            _factory.Get<IModules>().Emu.TestIt = () =>
            {
                window.ViewModel.WindowHeight += 20;
                window.ViewModel.WindowWidth += 30;
            };

            _factory.Get<IMainWindowEvents>().Bind(window, commands);

            IntPtr hWnd = new WindowInteropHelper(window.Window).EnsureHandle(); //--Note: Still on UI thread

            CmdLineArguments args = _commandLineParser.Parse();

            _thread.Run(hWnd, args);
        }
    }
}
