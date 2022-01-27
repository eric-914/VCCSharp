using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using VCCSharp.Models;

namespace VCCSharp
{
    public interface IVccThread
    {
        void Run(Window window);
        void Run(IntPtr hWnd);
    }

    public class VccThread : IVccThread
    {
        private readonly ICommandLineParser _commandLineParser;
        private readonly IVccApp _vccApp;

        public VccThread(IVccApp vccApp, ICommandLineParser commandLineParser)
        {
            _vccApp = vccApp;
            _commandLineParser = commandLineParser;
        }

        public void Run(Window window)
        {
            IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle(); //--Note: Still on UI thread

            Task.Run(() => Run(hWnd));
        }

        public void Run(IntPtr hWnd)
        {
            CmdLineArguments args = _commandLineParser.Parse();
            if (args == null)
            {
                return;
            }

            _vccApp.SetWindow(hWnd);

            _vccApp.Startup(args);

            _vccApp.Threading();

            _vccApp.Run();

            _vccApp.Shutdown();
        }
    }
}
