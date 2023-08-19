using System.Windows.Interop;
using VCCSharp.IoC;
using VCCSharp.Menu;
using VCCSharp.Models;
using VCCSharp.Modules;

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

            //TODO: Figure out how to get Actions instance and do this right.
            Actions.TestItAction = () => _factory.Get<IEvents>().ResetWindow(window);

            _factory.Get<IMainWindowEvents>().Bind(window, commands);

            IntPtr hWnd = new WindowInteropHelper(window.Window).EnsureHandle(); //--Note: Still on UI thread

            CmdLineArguments? args = _commandLineParser.Parse();

            if (args != null)
            {
                _thread.Run(hWnd, args);
            }
        }
    }
}
