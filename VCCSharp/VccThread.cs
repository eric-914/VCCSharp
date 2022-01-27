using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using VCCSharp.Models;

namespace VCCSharp
{
    public interface IVccThread
    {
        void Run(Window window, int surfaceHeight);
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

        public void Run(Window window, int surfaceHeight)
        {
            IntPtr hWnd = new WindowInteropHelper(window).EnsureHandle(); //--Note: Still on UI thread

            Task.Run(() => Run(hWnd, surfaceHeight));
        }

        public void Run(IntPtr hWnd, int surfaceHeight)
        {
            CmdLineArguments args = _commandLineParser.Parse();
            if (args == null)
            {
                return;
            }

            _vccApp.SetWindow(hWnd, surfaceHeight);

            _vccApp.Startup(args);

            //--The emulation runs on a different thread
            _vccApp.Threading();

            _vccApp.Run();
        }
    }
}
